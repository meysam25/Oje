using Newtonsoft.Json;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace Oje.Infrastructure.Services
{
    public static class SignatureManager
    {
        public static string Create(object input)
        {
            List<KeyValue> result = new List<KeyValue>();

            if (input != null)
            {
                var allProps = input.GetType().GetProperties();
                foreach (var prop in allProps)
                {
                    if (
                        prop.PropertyType == typeof(string) ||
                        prop.PropertyType == typeof(bool) ||
                        prop.PropertyType == typeof(bool?) ||
                        prop.PropertyType == typeof(int) ||
                        prop.PropertyType == typeof(int?) ||
                        prop.PropertyType == typeof(long) ||
                        prop.PropertyType == typeof(long?) ||
                        prop.PropertyType == typeof(DateTime) ||
                        prop.PropertyType == typeof(DateTime?) ||
                        prop.PropertyType.IsEnum
                        )
                    {
                        if (prop.Name != "Signature")
                            result.Add(new KeyValue() { key = prop.Name, value = prop.GetValue(input) + "" });
                    }
                }
            }

            return JsonConvert.SerializeObject(result).Encrypt();
        }
    }
}
