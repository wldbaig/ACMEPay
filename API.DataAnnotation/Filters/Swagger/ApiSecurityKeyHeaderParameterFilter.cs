using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Any;
using API.Common;

namespace API.DataAnnotation
{
    public class ApiSecurityKeyHeaderParameterFilter : IOperationFilter
    {
        protected readonly SettingConfig settingConfig;
        public ApiSecurityKeyHeaderParameterFilter(SettingConfig settingConfig)
        {
            this.settingConfig = settingConfig;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);
            var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);
            var apiSecurityKey =settingConfig.App.ShowWebAPISecurityKeyInDoc ? settingConfig.App.WebAPISecurityKey : "";

            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "api-security-key",
                In = ParameterLocation.Header,
                Description = "Api Security Key",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString(apiSecurityKey)
                }
            });
        }
    }
}
