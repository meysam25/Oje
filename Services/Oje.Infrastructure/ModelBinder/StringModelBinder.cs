using Oje.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Oje.Infrastructure.Services.ModelBinder
{
    public class StringModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // ...
            // implement it based on your actual requirement
            // code logic here
            // ...

            var modelName = bindingContext.ModelName;

            // Try to fetch the value of the argument by name
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            var holderType = bindingContext.ModelMetadata.ContainerType;
            if (holderType != null)
            {
                var propertyType = holderType.GetProperty(bindingContext.ModelMetadata.PropertyName);
                var attributes = propertyType.GetCustomAttributes(true);
                var hasAttribute = attributes
                  .Cast<Attribute>()
                  .Any(a => a.GetType().IsEquivalentTo(typeof(IgnoreStringEncodeAttribute)));
                if(hasAttribute)
                {
                    bindingContext.Result = ModelBindingResult.Success(value);
                    return Task.CompletedTask;
                }
            }

            bindingContext.Result = ModelBindingResult.Success(HttpUtility.HtmlEncode(value));
            return Task.CompletedTask;
        }
    }
}
