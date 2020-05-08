using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AutoLotMVC_Core2
{
    public class Program
    {
        public static void Main(String[] args)
        {
            var web_host = BuildWebHost(args: args);
            web_host.Run();
        }

        public static IWebHost BuildWebHost(String[] args) =>
            WebHost.CreateDefaultBuilder(args: args)
                .UseStartup<Startup>()
                .Build();
    }
}
