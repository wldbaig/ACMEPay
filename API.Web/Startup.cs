using API.Common;
using API.DataAnnotation;
using API.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace API.WEB
{
    public class Startup
    {
        private readonly IWebHostEnvironment env; 
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
             
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
            });

            services.AddMvc(options =>
            { 
                options.ModelMetadataDetailsProviders.Add(new RequiredBindingMetadataProvider());
            })
            .AddNewtonsoftJson(options =>
            {
                // Use the default property (Pascal) casing
                options.SerializerSettings.ContractResolver = new DefaultContractResolver(); 
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            });

            services.AddHttpContextAccessor();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlServerOptions => sqlServerOptions.CommandTimeout(3600))
                        .UseLazyLoadingProxies();
            }, ServiceLifetime.Transient);
            services.AddAppConfigurations(Configuration); 
            services.AddBusinessLogicServices();
            services.AddSwaggerService(env);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        { 
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                var logger = context.RequestServices.GetRequiredService<IServiceLogger<Startup>>();
                logger.Error(exception.Message, exception);

                var result = JsonConvert.SerializeObject(new { error = exception.Message });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));
             
        //    app.UseCors(AllowAllOrigin);
            app.UseSwaggerMiddleware();
            app.UseHttpsRedirection();
            app.UseRouting(); 
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "api/{controller}/{action}/{id?}");
            });
        }
    }
}
