using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microphone.Consul;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microphone;

namespace MicroService1
{
    public class Program
    {
        private static string ConsulIP = "consul";
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            var options = new ConsulOptions();
            options.Host = ConsulIP;
            options.HealthCheckPath = "/api/values";
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger("logger");
            var provider = new ConsulProvider(loggerFactory, Options.Create(options));
            Cluster.RegisterService(new Uri(string.Format("http://{0}",Environment.MachineName)), provider, "environment", "v1", logger);
            Console.WriteLine("Registré environment pour la machine {0}", Environment.MachineName);
            logger.LogError("Registré environment pour la machine {0}", Environment.MachineName);
            host.Run();
           
        }
    }
}
