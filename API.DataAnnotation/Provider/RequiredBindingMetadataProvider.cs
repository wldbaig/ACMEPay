using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace API.DataAnnotation
{
    public class RequiredBindingMetadataProvider : IBindingMetadataProvider
    {
        public void CreateBindingMetadata(BindingMetadataProviderContext context)
        {
            if (context.PropertyAttributes != null && context.PropertyAttributes.OfType<RequiredAttribute>().Any())
            {
                context.BindingMetadata.IsBindingRequired = true;
            } 
        }
    }
}
