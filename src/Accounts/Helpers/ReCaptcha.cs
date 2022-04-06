using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Helpers
{
    public class ReCaptcha
    {
        private readonly HttpClient captchaClient;
        private readonly IConfiguration configuration;
        private readonly ILogger<ReCaptcha> logger;

        public ReCaptcha(HttpClient captchaClient, IConfiguration configuration, ILogger<ReCaptcha> logger)
        {
            this.captchaClient = captchaClient;
            this.configuration = configuration;
            this.logger = logger;
        }

        public bool UseCaptcha
        {
            get
            {
                return !string.IsNullOrWhiteSpace(PubCaptcha);
            }
        }

        public string PubCaptcha
        {
            get
            {
                return configuration["PubCaptcha"];
            }
        }

        public string CaptchaSecret
        {
            get
            {
                return configuration["CaptchaSecret"];
            }
        }

        public async Task<bool> IsValid(string captcha)
        {
            try
            {
                var postTask = await captchaClient
                    .PostAsync($"?secret={ CaptchaSecret }&response={captcha}", new StringContent(""));
                var result = await postTask.Content.ReadAsStringAsync();
                var resultObject = JObject.Parse(result);
                dynamic success = resultObject["success"];
                return (bool)success;
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Error validating captcha");
                return false;
            }
        }
    }
}
