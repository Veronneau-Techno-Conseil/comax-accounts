using CommunAxiom.Accounts.BusinessLayer.Apps;
using DatabaseFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer.Profiles
{
    public class ApplicationsProfiles: AutoMapper.Profile
    {
        public ApplicationsProfiles() 
        {
            this.CreateMap<AppConfiguration, AppConfigurationDetails>()
                .ForMember(x => x.AppConfigurationKey, a => a.MapFrom(y=>y.AppVersionConfiguration.ConfigurationKey))
                .ForMember(x => x.FromAppDefault, a => a.Ignore());

            this.CreateMap<AppSecret, AppSecret>()
                .ForMember(x => x.Id, a => a.Ignore())
                .ForMember(x => x.Application, a => a.Ignore());

        }
    }
}
