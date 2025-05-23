﻿using Newtonsoft.Json;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Oje.Infrastructure.Services
{
    public static class EnumService
    {
        public static List<IdTitle> GetEnum(string enumName, bool ignoreEmpty = false)
        {
            if (string.IsNullOrEmpty(enumName))
                return new List<IdTitle>();
            List<IdTitle> result = new List<IdTitle>();
            if (ignoreEmpty == false)
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

                string id = "";
                try
                {
                    id = ((int)value) + "";
                }
                catch
                {
                    id = ((byte)value) + "";
                }
                if (!string.IsNullOrEmpty(propValue))
                    id = propValue;

                result.Add(new IdTitle { id = id, title = (!string.IsNullOrEmpty(displayName) ? displayName : ((propValue ?? value) + "")) });
            }

            return result;
        }

        public static JsonSerializerSettings EnumConverterSetting
        {
            get
            {
                var jsonSetting = new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };
                jsonSetting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                return jsonSetting;
            }
        }

    }
}
