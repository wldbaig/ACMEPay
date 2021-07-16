using API.DataAnnotation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Generic;
namespace API.WEB
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services, IWebHostEnvironment env)
        {
            //Swagger - Enable this line and the related lines in Configure method to enable swagger UI

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "API Doc", Version = "v1" }); 
                options.OperationFilter<ApiSecurityKeyHeaderParameterFilter>();
                
                options.SchemaFilter<SwaggerExcludeFilter>(); 
                options.SchemaFilter<EnumSchemaFilter>();
                options.SchemaFilter<PascalCasingPropertiesFilter>();
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
            // Add the detail information for the API.
            //services.ConfigureSwaggerGen(options =>
            //{
            //    //Determine base path for the application.
            //    string xmlPath = env.ContentRootFileProvider.GetFileInfo("API.xml")?.PhysicalPath;
            //    //Set the comments path for the swagger json and ui.
            //    options.IncludeXmlComments(xmlPath);
            //});

            return services;
        }

        public static void UseSwaggerMiddleware(this IApplicationBuilder app)
        {

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            //To serve the Swagger UI at the app's root (http://localhost:<port>/), set the RoutePrefix property to an empty string:
            app.UseSwaggerUI(options =>
            {
                //Allow to add addition attribute info on doc. like [MaxLength(50)]
                options.ConfigObject = new ConfigObject
                {
                    ShowCommonExtensions = true
                };
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"); 
            });
        }
    }
}
