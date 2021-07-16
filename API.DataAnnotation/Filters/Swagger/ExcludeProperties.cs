using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace API.DataAnnotation
{
    public class SwaggerExcludeFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context.Type == null)
                return;

            var excludedProperties = context.Type.GetProperties()
                                         .Where(t =>
                                                t.GetCustomAttribute<IgnoreDataMemberAttribute>()
                                                != null);

            foreach (var excludedProperty in excludedProperties)
            {
                var propertyNameInCamelCasing = char.ToLowerInvariant(excludedProperty.Name[0]) + excludedProperty.Name.Substring(1);

                if (schema.Properties.ContainsKey(propertyNameInCamelCasing))
                {
                    schema.Properties.Remove(propertyNameInCamelCasing);
                }
            }
        }
    }
}