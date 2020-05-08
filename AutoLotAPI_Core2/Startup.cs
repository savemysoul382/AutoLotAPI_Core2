using AutoLotAPI_Core2.Filters;
using AutoLotDAL_Core2.EF;
using AutoLotDAL_Core2.Repos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace AutoLotAPI_Core2
{
    public class Startup
    {

        private readonly IHostingEnvironment env;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.env = env;
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(setupAction: config =>
                {
                    config.Filters.Add(item: new AutoLotExceptionFilter(env: this.env));
                })
                .AddJsonOptions(setupAction: options =>
                {
                    //Revert to PascalCasing for JSON handling
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });
            services.AddDbContextPool<AutoLotContext>(
                optionsAction: options => options.UseSqlServer(connectionString: Configuration.GetConnectionString(name: "AutoLot"),
                        sqlServerOptionsAction: o => o.EnableRetryOnFailure())
                    .ConfigureWarnings(warningsConfigurationBuilderAction: warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)));
            services.AddScoped<IInventoryRepo, InventoryRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
