using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API.DataAnnotation
{
    public class PascalCasingPropertiesFilter : ISchemaFilter
    {
        public PascalCasingPropertiesFilter()
        {

        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Properties =
                schema.Properties.ToDictionary(d => d.Key.Substring(0, 1).ToUpper() + d.Key.Substring(1), d => d.Value);
        }
    }
}
