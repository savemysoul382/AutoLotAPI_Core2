using System;
using AutoLotDAL_Core2.DataInitialization;
using AutoLotDAL_Core2.EF;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AutoLotAPI_Core2
{
    public class Program
    {
        public static void Main(String[] args)
        {
            var web_host = BuildWebHost(args);
            using (var scope = web_host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AutoLotContext>();
                MyDataInitializer.RecreateDatabase(context);
                //MyDataInitializer.ClearData(context);
                MyDataInitializer.InitializeData(context);
            }
            web_host.Run();
        }

        public static IWebHost BuildWebHost(String[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
