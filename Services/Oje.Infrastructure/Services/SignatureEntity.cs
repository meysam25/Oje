﻿using Oje.Infrastructure.Interfac;
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
        public byte[] Signature { get; set; }
        static readonly string equalValue = "%*‎#*";
        static readonly string seperatorValue = "(#‎*(";


        public void FilledSignature()
        {
            Signature = ConvertToString(GetCurrentValues()).EncryptSignature();
        }

        public void UpdateSignature()
        {
            var curObj = GetCurrentValues();
            var prevObj = GetCurrentValuesFromSignature();

            foreach (var v in curObj)
            {
                var foundInPrevValue = prevObj.Where(t => t.key == v.key).FirstOrDefault();
                if (foundInPrevValue != null)
                    foundInPrevValue.value = v.value;
                else
                    prevObj.Add(v);
            }

            Signature = ConvertToString(prevObj).EncryptSignature();

            curObj?.Clear();
            prevObj?.Clear();
        }

        public bool IsSignature()
        {
            var curObj = GetCurrentValues();
            var prevObj = GetCurrentValuesFromSignature();

            if (prevObj == null || prevObj.Count == 0)
            {
                curObj?.Clear();
                prevObj?.Clear();
                return false;
            }
            var tempResult = false;

            if (prevObj.Count < curObj.Count)
            {
                tempResult = prevObj.Count(t => curObj.Any(tt => tt.key == t.key && tt.value == t.value)) == prevObj.Count;
                curObj?.Clear();
                prevObj?.Clear();
                return tempResult;

            }

            tempResult = curObj.Count(t => prevObj.Any(tt => tt.key == t.key && tt.value == t.value)) == curObj.Count;
            curObj?.Clear();
            prevObj?.Clear();

            return tempResult;
        }

        public string GetSignatureChanges()
        {
            string result = "";
            var curObj = GetCurrentValues();
            var prevObj = GetCurrentValuesFromSignature();

            foreach (var item in curObj)
            {
                var foundItem = prevObj.Where(t => t.key == item.key).FirstOrDefault();
                if (foundItem != null)
                {
                    if (foundItem.value != item.value)
                        result += "--- " + item.key + " been change from " + item.value + " to " + foundItem.value + Environment.NewLine;
                }
                else
                    result += "--- " + item.key + " been change from " + item.value + " to not exist" + Environment.NewLine;
            }

            curObj.Clear();
            prevObj.Clear();

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
                    object propValue = prop.GetValue(this, null);
                    if (
                        propType == typeof(int?) || propType == typeof(int) ||
                        propType == typeof(long?) || propType == typeof(long) ||
                        propType == typeof(string) || propType == typeof(string) ||
                        propType == typeof(bool?) || propType == typeof(bool) ||
                        propType.IsEnum
                        )
                    {
                        if (propName != "Signature")
                        {
                            string curValue = propValue + "";
                            if (propType.IsEnum && propValue != null)
                                curValue = ((int)propValue) + "";
                            result.Add(new KeyValue() { key = propName, value = curValue });

                        }
                    }
                }
            }

            return result;
        }

        List<KeyValue> GetCurrentValuesFromSignature()
        {
            List<KeyValue> result = new List<KeyValue>();

            if (Signature != null && Signature.Length > 0)
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
