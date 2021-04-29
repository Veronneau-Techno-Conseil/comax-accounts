using System;
using System.Collections.Generic;
using System.Linq;
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
                        opts.ConfigureHttpsDefaults(def =>
                        {
                            def.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(System.IO.Path.GetFullPath("./certs/localhost.pfx"), "tester123");
                        });
                    })
                        .UseUrls("https://localhost:5001")
                        .UseStartup<Startup>();
                });
    }
}
