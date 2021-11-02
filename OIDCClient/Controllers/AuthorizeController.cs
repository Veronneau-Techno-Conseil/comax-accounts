using Microsoft.AspNetCore.Mvc;
using OIDCClient.ViewModels.Auhtenticate;
using OIDCClient.ViewModels.Authorize;
using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OIDCClient.Controllers
{
    public class AuthorizeController : Controller
    {
        [HttpGet]
        public IActionResult Authorize()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authorize(AuthorizeViewModel model)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };


            string url = string.Format(
                       "https://localhost:5001/connect/authorize?" +
                       "client_id={0}&" +
                       "response_type={1}&" +
                       "scope={2}&" +
                       "redirect_uri={3}&" +
                       "nonce={4}",
                       Uri.EscapeDataString(model.ClientId),
                       Uri.EscapeDataString("code"),
                       Uri.EscapeDataString("openid"),
                       Uri.EscapeDataString("https://localhost:44363/Authorize/Token"),
                       Uri.EscapeDataString("abcabc"));

            var client = new HttpClient(httpClientHandler) { BaseAddress = new Uri(url) };
            var result = await client.GetAsync(url);
            return Redirect(url);
        }

        [HttpGet]
        public IActionResult Token(string code)
        {
            var model = new TokenViewModel
            {
                code = code
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Token(TokenViewModel model)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            var client = new HttpClient(httpClientHandler);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5001/connect/token");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["client_id"] = model.clientId,
                ["client_secret"] = model.secret,
                ["redirect_uri"] = "https://localhost:44363/Authorize/Token",
                ["code"] = model.code
            });

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            var payload = await response.Content.ReadFromJsonAsync<OpenIddictResponse>();

            if (!string.IsNullOrEmpty(payload.Error))
            {
                throw new InvalidOperationException("An error occurred while retrieving an access token.");
            }

            var Token = payload.AccessToken;

            return RedirectToAction("Display", new { token = Token });
        }

        [HttpGet]
        public IActionResult Display(string token)
        {

            var display = new DisplayViewModel();

            ViewBag.Message = token;
            return View(display);
        }
    }
}
