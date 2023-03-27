using CommunAxiom.Accounts.Contracts;
using DatabaseFramework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace CommunAxiom.Accounts.BusinessLayer.Apps.Factory
{
    public partial class AppFactory
    {
        public async Task<AppCreationResult> CreateCommons(string displayName, string redirectUri, string postLogout)
        {
            //Create the RandomWord
            var RandomWord = RandomizerFactory.GetRandomizer(new FieldOptionsTextWords { Min = 4, Max = 5 }).Generate().ToString().Replace(" ", "_");

            //Validate its uniqueness in the database
            var apps = _applicationManager.FindByClientIdAsync(RandomWord).Result;
            if (apps != null)
            {
                do
                    RandomWord = RandomizerFactory.GetRandomizer(new FieldOptionsTextWords { Min = 4, Max = 5 }).Generate().ToString().Replace(" ", "_");
                while (_applicationManager.FindByClientIdAsync(RandomWord).Result == null);
            }

            var permissions = new List<string>
                {
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.GrantTypes.DeviceCode,
                    Permissions.GrantTypes.ClientCredentials,

                    Permissions.Endpoints.Device,
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Introspection,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,

                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,

                    Permissions.ResponseTypes.CodeIdTokenToken,
                    Permissions.ResponseTypes.Code
                };


            if (string.IsNullOrWhiteSpace(_configuration["Prod"]) || !bool.TryParse(_configuration["Prod"], out var prod) || !prod)
                permissions.Add(Permissions.GrantTypes.Password);

            //Create the application
            //var application = new OpenIddictApplicationDescriptor
            var application = new Application
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = RandomWord,
                Deleted = false,
                DeletedDate = null,
                DisplayName = displayName,
                DisplayNames = Newtonsoft.Json.JsonConvert.SerializeObject(new System.Collections.Generic.Dictionary<string, string>
                {
                    { "en-CA", displayName }
                }),
                Type = ClientTypes.Confidential,
                ConsentType = ConsentTypes.Explicit,
                Permissions = JsonSerializer.Serialize(permissions.ToArray()),
                PostLogoutRedirectUris = JsonSerializer.Serialize(new[]
                {
                    postLogout
                }),
                RedirectUris = JsonSerializer.Serialize(new[]
                {
                    redirectUri
                }),
                Requirements = JsonSerializer.Serialize(new[]
                {
                    Requirements.Features.ProofKeyForCodeExchange
                })
            };
            var secret = Guid.NewGuid().ToString();
            await _applicationManager.CreateAsync(application, secret);

            var createdApplication = _applicationManager.FindByClientIdAsync(RandomWord).Result;

            var CommonsApp = _context.Set<ApplicationType>().AsQueryable().Where(x => x.Name == ApplicationType.COMMONS).FirstOrDefault();

            var ApplicationTypeMap = new ApplicationTypeMap
            {
                ApplicationId = createdApplication.Id,
                ApplicationTypeId = CommonsApp.Id
            };

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user != null)
            {
                var userApplicationMap = new UserApplicationMap
                {
                    UserId = user.Id,
                    ApplicationId = createdApplication.Id,
                    HostingType = UserApplicationMap.HostingTypes.None
                };
                await _context.Set<UserApplicationMap>().AddAsync(userApplicationMap);
            }

            await _context.Set<ApplicationTypeMap>().AddAsync(ApplicationTypeMap);
            await _context.SaveChangesAsync();

            return new AppCreationResult
            {
                ApplicationId = createdApplication.Id,
                ClientId = createdApplication.ClientId,
                Secret = secret
            };
        }

        public async Task<OperationResult<MissingConfigs>> ConfigureHostedCommons(string applicationId, int applicationVerisonTagId, string baseUrl)
        {
            var rt = new RandomDataGenerator.Randomizers.RandomizerText(new FieldOptionsText
            {
                UseLetter = true,
                UseLowercase = true,
                UseNullValues = false,
                UseNumber = true,
                UseUppercase = false,
                UseSpace = false,
                UseSpecial = false,
                ValueAsString = true,
                Min = 12,
                Max = 12
            });

            var hash = rt.Generate();
            var url = baseUrl.Replace("[HASH]", hash);

            var app = await _applicationManager.FindByIdAsync(applicationId);
            var newSecret = await this.RefreshSecret(applicationId);

            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add(AppConfiguration.APP_URI, url);
            values.Add(AppConfiguration.OIDC_AUTHORITY, _authorityInfo.Authority);
            values.Add(AppConfiguration.OIDC_CLIENT_ID, app.ClientId);
            values.Add(AppConfiguration.OIDC_SECRET, newSecret);
            values.Add(AppConfiguration.APP_HASH, hash);

            var tag = _context.Set<AppVersionTag>();
            var vertag = await tag
                .Include(x => x.AppVersionConfigurations)
                .FirstOrDefaultAsync(x => x.Id == applicationVerisonTagId);

            var confs = vertag.AppVersionConfigurations.Select(x => new Checklist<AppVersionConfiguration> { Value = x }).ToList();

            foreach (var val in values)
            {
                var entry = confs.FirstOrDefault(x => x.Value.ConfigurationKey == val.Key);
                if (entry != null)
                {
                    await _appConfigurations.UpsertConfiguration(new AppConfigurationDetails
                    {
                        AppConfigurationKey = val.Key,
                        ApplicationId = app.Id,
                        AppVersionConfigurationId = entry.Value.Id,
                        FromAppDefault = false,
                        FromSecret = entry.Value.Sensitive,
                        Value = val.Value
                    });
                    entry.Done = true;
                }
            }

            var leftover = confs.Where(x => !x.Done).ToList();
            foreach (var entry in leftover)
            {
                if (entry.Value.UserValueMandatory && string.IsNullOrEmpty(entry.Value.ValueGenerator))
                    continue;

                try
                {
                    await _appConfigurations.UpsertConfiguration(new AppConfigurationDetails
                    {
                        AppConfigurationKey = entry.Value.ConfigurationKey,
                        ApplicationId = app.Id,
                        AppVersionConfigurationId = entry.Value.Id,
                        FromAppDefault = false,
                        FromSecret = entry.Value.Sensitive,
                        Value = !string.IsNullOrWhiteSpace(entry.Value.ValueGenerator) ?
                                        ValueGenFactory.Generate(entry.Value.ValueGenerator, entry.Value.ValueGenParameter) :
                                        entry.Value.DefaultValue
                    });
                }
                catch (Exception ex)
                {
                    throw;
                }
                entry.Done = true;
            }

            var maps = await _context.Set<UserApplicationMap>().Where(x => x.ApplicationId == applicationId).ToListAsync();
            foreach (var map in maps)
            {
                map.HostingType = UserApplicationMap.HostingTypes.Managed;
            }
            await _context.SaveChangesAsync();

            return new OperationResult<MissingConfigs>
            {
                Result = new MissingConfigs
                {
                    Keys = leftover.Where(x => !x.Done).Select(x => x.Value.ConfigurationKey).ToArray()
                }
            };
        }
    }
}
