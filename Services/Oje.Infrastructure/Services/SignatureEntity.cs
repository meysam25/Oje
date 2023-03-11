using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Oje.Infrastructure.Services
{
    public abstract class SignatureEntity : ISignatureEntity
    {
        public string Signature { get; set; }
        static string equalValue = "%*~#@=$#*)*";
        static string seperatorValue = "(#~#@,$#*(!";
        

        public void FilledSignature()
        {
            Signature = ConvertToString(GetCurrentValues()).EncryptSignature();
        }

        public bool IsSignature()
        {
            var curObj = GetCurrentValues();
            var prevObj = GetCurrentValuesFromSignature();

            return prevObj.Count(t => curObj.Any(tt => tt.key == t.key && tt.value == t.value)) == prevObj.Count;
        }

        public string GetSignatureChanges()
        {
            string result = "";
            var curObj = GetCurrentValues();
            var prevObj = GetCurrentValuesFromSignature();

            foreach(var item in prevObj)
            {
                var foundItem = curObj.Where(t => t.key == item.key).FirstOrDefault();
                if (foundItem != null)
                {
                    if (foundItem.value != item.value)
                        result += "--- " + item.key + " been change from " + item.value + " to " + foundItem.value + Environment.NewLine;
                }
                else
                    result += "--- " + item.key + " been change from " + item.value + " to not exist" + Environment.NewLine;
            }

            return result;
        }

        List<KeyValue> GetCurrentValues()
        {
            List<KeyValue> result = new List<KeyValue>();
            var allProps = GetType().GetProperties();
            foreach (var prop in allProps)
            {
                var hasNotMapped = prop.GetCustomAttributes<NotMappedAttribute>(false).FirstOrDefault();
                if (hasNotMapped == null) 
                {
                    var propType = prop.PropertyType;
                    string propName = prop.Name;
                    string propValue = prop.GetValue(this, null) + "";
                    if (
                        propType == typeof(int?) || propType == typeof(int) ||
                        propType == typeof(long?) || propType == typeof(long) ||
                        propType == typeof(string) || propType == typeof(string) ||
                        propType == typeof(bool?) || propType == typeof(bool) ||
                        propType.IsEnum
                        )
                    {
                        if (propName != "Signature")
                            result.Add(new KeyValue() { key = propName, value = propValue });
                    }
                }
            }

            return result;
        }

        List<KeyValue> GetCurrentValuesFromSignature()
        {
            List<KeyValue> result = new List<KeyValue>();

            if (!string.IsNullOrEmpty(Signature))
            {
                string decriptValue = "";
                try { decriptValue = Signature.DecryptSignature(); } catch { }
                var arrProps = decriptValue.Split(new string[] { seperatorValue }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (var prop in arrProps)
                {
                    string[] kvArr = prop.Split(new string[] { equalValue }, System.StringSplitOptions.None);
                    if (kvArr != null && kvArr.Length >= 2)
                    {
                        result.Add(new KeyValue()
                        {
                            key = kvArr[0],
                            value = kvArr[1]
                        });
                    }

                }
            }
            else 
                result.Add(new KeyValue() { key = "entity", value = "no signature" });

            return result;
        }

        string ConvertToString(List<KeyValue> input)
        {
            string result = "";

            if (input != null)
                foreach (KeyValue keyValue in input)
                {
                    if (!string.IsNullOrEmpty(result))
                        result += seperatorValue;
                    result += keyValue.key + equalValue + keyValue.value;
                }

            return result;
        }

    }
}
