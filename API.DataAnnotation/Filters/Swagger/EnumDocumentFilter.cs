using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API.DataAnnotation
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                var enumValues = schema.Enum.ToArray();
                var i = 0;
                var enumSize = Enum.GetNames(context.Type).ToList().Count;
               
                foreach (var n in Enum.GetNames(context.Type).ToList())
                {
                    if (enumSize <= enumValues.Length)
                    {
                        schema.Description = (schema.Description ?? "") + (n + $" = {((OpenApiPrimitive<int>)enumValues[i]).Value}" + ", ");
                        
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
