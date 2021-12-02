using Oje.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Services.ModelBinder
{
    public class MyHtmlStringBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // specify the parameter your binder operates on
            if (context.Metadata.ModelType == typeof(MyHtmlString))
            {
                return new BinderTypeModelBinder(typeof(MyHtmlStringBinder));
            }

            return null;
        }
    }
}
