using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CommunAxiom.Accounts
{
    public class Program
    {
        public static void Main(string[] args)
        {
                CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(opts=>
                    {
                        if (webBuilder.GetSetting("Urls").StartsWith("https"))
                        {
                            opts.ConfigureHttpsDefaults(def =>
                            {
                                var certPem = File.ReadAllText("cert.pem");
                                var eccPem = File.ReadAllText("key.pem");

                                var cert = X509Certificate2.CreateFromPem(certPem, eccPem);
                                def.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(cert.Export(System.Security.Cryptography.X509Certificates.X509ContentType.Pkcs12));
                            });
                        }
                    })
                    .UseStartup<Startup>();
                });
    }
}
