using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microphone.Consul;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microphone;

namespace MicroService2
{
    public class Program
    {
        private static string ConsulIP = "consul";
        private static string ThisServiceAlias = "microservice2";
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
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger("logger");
            var provider = new ConsulProvider(loggerFactory, Options.Create(options));
            Cluster.RegisterService(new Uri(string.Format("http://{0}", ThisServiceAlias)), provider, "integers", "v1", logger);

            host.Run();
        }
    }
}
