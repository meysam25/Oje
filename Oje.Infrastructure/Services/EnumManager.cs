using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Services
{
    public static class EnumManager
    {
        public static List<IdTitle> GetEnum(string enumName)
        {
            List<IdTitle> result = new List<IdTitle>();
            result.Add(new IdTitle { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name });

            var assembly = Assembly.GetExecutingAssembly();
            var enumType = assembly.GetType("Oje.Infrastructure.Enums." + enumName);
            if (enumType == null)
                throw BException.GenerateNewException(BMessages.Invalid_BaseData);
            var allValues = Enum.GetValues(enumType);

            foreach (var value in allValues)
            {
                string displayName = value.GetAttribute<DisplayAttribute>()?.Name;
                var propValue = value.GetAttribute<DisplayAttribute>()?.Prompt;

                string id = ((int)value) + "";
                if (!string.IsNullOrEmpty(propValue))
                    id = propValue;

                result.Add(new IdTitle { id = id, title = (!string.IsNullOrEmpty(displayName) ? displayName : ((propValue ?? value) + "")) });
            }

            return result;
        }

    }
}
