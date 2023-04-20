using Microsoft.AspNetCore.Http;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Infrastructure.Models.PageForms
{
    public class ctrl
    {
        public ctrl()
        {
        }

        public string id { get; set; }
        public string @class { get; set; }
        public string parentCL { get; set; }
        public string css { get; set; }
        public ctrlType? type { get; set; }
        public string textfield { get; set; }
        public string valuefield { get; set; }
        public string dataurl { get; set; }
        public string url { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public List<ctrlShowHideCondation> showHideCondation { get; set; }
        public bool? isRequired { get; set; }
        public string acceptEx { get; set; }
        public ctrlSchema schema { get; set; }
        public bool? nationalCodeValidation { get; set; }
        public bool? disabled { get; set; }
        public bool? seperator { get; set; }
        public string defValue { get; set; }
        public string ph { get; set; }
        public string bankUrl { get; set; }
        public string bindFormUrl { get; set; }
        public int? minDateValidation { get; set; }
        public int? maxDateValidation { get; set; }
        public int? maxLengh { get; set; }
        public string mask { get; set; }
        public bool? ltr { get; set; }
        public bool? hideOnPrint { get; set; }
        public MapName names { get; set; }
        public List<ctrl> ctrls { get; set; }
        public List<IdTitle> values { get; set; }
        public List<validation> validations { get; set; }
        public List<string> exteraParameterIds { get; set; }
        public string childId { get; set; }
        public List<string> callChangeEvents { get; set; }
        public List<string> multiPlay { get; set; }
        public List<cTemplateMaps> fieldMaps { get; set; }
        public List<string> calcDate { get; set; }

        [JsonIgnore]
        public string defV { get; set; }


        public static void requiredValidationForCtrl(ctrl ctrl, IFormCollection form)
        {
            if (ctrl.isRequired == true)
                if (!form.Keys.Contains(ctrl.name) || string.IsNullOrEmpty(form.GetStringIfExist(ctrl.name)))
                    if (!needToBeIgnore(ctrl.name))
                        throw BException.GenerateNewException
                                (
                                    string.Format
                                        (
                                            ctrl.type == ctrlType.text ? BMessages.Please_Enter_X.GetEnumDisplayName() : BMessages.Please_Select_X.GetEnumDisplayName()
                                            , ctrl.label
                                        )
                                );

            if (ctrl.maxLengh != null && ctrl.maxLengh > 0)
                if (form.Keys.Contains(ctrl.name) && !string.IsNullOrEmpty(form.GetStringIfExist(ctrl.name)) && form.GetStringIfExist(ctrl.name).Length > ctrl.maxLengh)
                    throw BException.GenerateNewException(BMessages.Validation_Error.GetEnumDisplayName() + " (" + ctrl.label + ")");


        }

        public static void validateAndUpdateCtrl(ctrl ctrl, IFormCollection form, List<ctrl> ctrls)
        {
            if (ctrl.type == ctrlType.checkBox)
            {
                string currValue = form.GetStringIfExist(ctrl.name);
                if (!string.IsNullOrEmpty(currValue) && !string.IsNullOrEmpty(ctrl.defValue) && currValue != ctrl.defValue)
                    throw BException.GenerateNewException(BMessages.Validation_Error.GetEnumDisplayName() + " (" + ctrl.name + ")");
                else if (!string.IsNullOrEmpty(currValue) && currValue == ctrl.defValue)
                    ctrl.defV = ctrl.defValue;
            }
            else if (ctrl.type == ctrlType.radio)
            {
                string currValue = form.GetStringIfExist(ctrl.name);
                if (!string.IsNullOrEmpty(currValue))
                {
                    if (ctrl.values != null)
                    {
                        if (!ctrl.values.Any(tt => tt.id == currValue))
                            throw BException.GenerateNewException(BMessages.Validation_Error.GetEnumDisplayName() + " (" + ctrl.name + ")");
                        else
                            ctrl.defV = currValue;
                    }
                }
            }
            else if (ctrl.disabled == true && ctrl.multiPlay != null && ctrl.multiPlay.Count > 0)
            {
                long resultM = 0;
                for (var i = 0; i < ctrl.multiPlay.Count; i++)
                {
                    var foundCtrl = ctrls.Where(t => t.id == ctrl.multiPlay[i]).FirstOrDefault();
                    if (foundCtrl != null)
                    {
                        var currValue = form.GetStringIfExist(foundCtrl.name).ToLongReturnZiro();
                        if (resultM == 0)
                            resultM = 1;
                        resultM = resultM * currValue;
                    }
                }
                ctrl.defV = resultM.ToString("###,###");
            }
            else if (ctrl.type == ctrlType.carPlaque)
            {
                string currValue = form.GetStringIfExist(ctrl.name + "_1") + " " + form.GetStringIfExist(ctrl.name + "_2") + " " + form.GetStringIfExist(ctrl.name + "_3") + " " + form.GetStringIfExist(ctrl.name + "_4");
                if (!string.IsNullOrEmpty(currValue))
                    ctrl.defV = currValue;
            }
        }

        public static void navionalCodeValidation(ctrl ctrl, IFormCollection form)
        {
            if (ctrl.nationalCodeValidation == true && !form.GetStringIfExist(ctrl.name).IsCodeMeli())
                throw BException.GenerateNewException(BMessages.Invalid_NationaCode);
        }

        public static void validateAndUpdateValuesOfDS(ctrl ctrl, IFormCollection form)
        {
            if (ctrl.type == ctrlType.dropDown || ctrl.type == ctrlType.dropDown2 || ctrl.type == ctrlType.radio)
            {
                if (ctrl.values != null && ctrl.values.Count > 0)
                {
                    var inputValue = form.GetStringIfExist(ctrl.name);
                    if (!string.IsNullOrEmpty(inputValue))
                    {
                        var foundDs = ctrl.values.Where(t => t.id == inputValue).FirstOrDefault();
                        if (foundDs == null)
                            throw BException.GenerateNewException(BMessages.Validation_Error.GetEnumDisplayName() + " (" + ctrl.name + ")");
                        ctrl.defV = foundDs.title;
                    }
                }
            }
        }

        public static void validateBaseDataEnums(ctrl ctrl, IFormCollection form)
        {
            if (ctrl.type == ctrlType.dropDown && !string.IsNullOrEmpty(ctrl.dataurl))
            {
                if (ctrl.dataurl.StartsWith("/Core/BaseData/Get/"))
                {
                    string selectValue = form.GetStringIfExist(ctrl.name);
                    if (!string.IsNullOrEmpty(selectValue))
                    {
                        string enumName = ctrl.dataurl.Replace("/Core/BaseData/Get/", "");
                        var foundEnum = EnumService.GetEnum(enumName);

                        if (foundEnum == null || foundEnum.Count == 0)
                            throw BException.GenerateNewException(String.Format(BMessages.Invalid_BaseData.GetEnumDisplayName(), ctrl.dataurl));

                        if (!foundEnum.Any(tt => tt.id == selectValue || tt.title == selectValue))
                            throw BException.GenerateNewException(String.Format(BMessages.Invalid_BaseData.GetEnumDisplayName(), ctrl.dataurl));
                        ctrl.defV = foundEnum.Where(tt => tt.id == selectValue || tt.title == selectValue).Select(tt => tt.title).FirstOrDefault();
                    }
                }
            }
        }

        public static void reqularExperssionValidationCtrl(ctrl ctrl, IFormCollection form)
        {
            if (ctrl.validations != null && ctrl.validations.Count > 0)
            {
                var curCTRLValue = form.Keys.Any(t => t == ctrl.name) ? form.GetStringIfExist(ctrl.name) : "";
                if (!string.IsNullOrEmpty(curCTRLValue))
                {
                    foreach (var validation in ctrl.validations)
                    {
                        if (!RegexCache.getRegexInstance(validation.reg).IsMatch(curCTRLValue))
                            throw BException.GenerateNewException(ctrl.label + ": " + validation.msg);
                    }
                }
            }
        }

        public static void validateAndUpdateMultiRowInputCtrl(ctrl ctrl, IFormCollection form, PageForm ppfObj)
        {
            if (ctrl.type == ctrlType.multiRowInput && ctrl.ctrls != null && ctrl.ctrls.Count > 0)
            {
                string baseName = ctrl.name;
                var allBaseCTRLs = ctrl.ctrls.ToList();
                int maxIndex = getMaxIndexByForm(baseName, form);
                if (maxIndex > 0)
                {
                    for (var startIndex = maxIndex; startIndex > 0; startIndex--)
                    {
                        var newClones = allBaseCTRLs.Select(t => JsonConvert.DeserializeObject<ctrl>(JsonConvert.SerializeObject(t))).ToList();
                        for (var i = 0; i < newClones.Count; i++)
                        {
                            var curChildCTrl = newClones[i];
                            curChildCTrl.name = baseName + "[" + (startIndex - 1) + "]." + curChildCTrl.name;
                            curChildCTrl.defV = !string.IsNullOrEmpty(form.GetStringIfExist(curChildCTrl.name + "_Title")) ? form.GetStringIfExist(curChildCTrl.name + "_Title") : form.GetStringIfExist(curChildCTrl.name);
                            if (ppfObj.panels.FirstOrDefault().ctrls == null)
                                ppfObj.panels.FirstOrDefault().ctrls = new List<ctrl>();
                            ppfObj.panels.FirstOrDefault().ctrls.Add(curChildCTrl);
                            ctrl.requiredValidationForCtrl(curChildCTrl, form);
                            ctrl.navionalCodeValidation(ctrl, form);
                        }
                    }
                }
            }
        }

        static int getMaxIndexByForm(string baseName, IFormCollection form)
        {
            int result = 0;
            for (var i = 0; i < 20; i++)
            {
                if (form.Keys.Any(t => t.Contains(baseName + "[" + i + "]")))
                    result = i + 1;
                else
                    break;
            }
            return result;
        }

        static bool needToBeIgnore(string name)
        {
            switch (name)
            {
                case "payCondation":
                    return true;
                default:
                    return false;
            }
        }

        public static List<IdTitle> dublicateMapValueIfNeeded(ctrl ctrl, PageForm ppfObj, IFormCollection form)
        {
            if (ctrl.type == ctrlType.map && ctrl.names != null && ppfObj != null && ppfObj.panels.Count > 0)
            {
                if (ppfObj.panels[0].ctrls == null)
                    ppfObj.panels[0].ctrls = new();

                double? lat = form.GetStringIfExist(ctrl.names.lat).ToDoubleReturnNull();
                double? lon = form.GetStringIfExist(ctrl.names.lon).ToDoubleReturnNull();
                int? zoom = form.GetStringIfExist(ctrl.names.zoom).ToIntReturnZiro();

                if (lat != null && lon != null && zoom > 0)
                {
                    var tempPoint = new Point(lat.Value, lon.Value);
                    if (!tempPoint.IsValid)
                        throw BException.GenerateNewException(BMessages.Validation_Error.GetEnumDisplayName());
                    return new List<IdTitle>() { new IdTitle() { id = ctrl.names.lat, title = "نقشه لت" }, new IdTitle() { id = ctrl.names.lon, title = "نقشه لان" }, new IdTitle() { id = ctrl.names.zoom, title = "نقشه زوم" } };
                }
            }
            return new List<IdTitle>();
        }

        public static void validateMinAndMaxDayForDateInput(ctrl ctrl, IFormCollection form)
        {
            if (ctrl.type == ctrlType.persianDateTime && ctrl.minDateValidation != null)
            {
                var curCTRLValue = form.Keys.Any(t => t == ctrl.name) ? form.GetStringIfExist(ctrl.name) : "";
                if (!string.IsNullOrEmpty(curCTRLValue))
                {
                    var enDate = curCTRLValue.ToEnDate();
                    if (enDate == null)
                        throw BException.GenerateNewException(ctrl.label + " مجاز نمی باشد");
                    var curDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    if (enDate.Value <= curDate.AddDays(ctrl.minDateValidation.Value).AddDays(-1))
                        throw BException.GenerateNewException(ctrl.label + " مجاز نمی باشد");
                }
            }
            else if (ctrl.type == ctrlType.persianDateTime && ctrl.maxDateValidation != null)
            {
                var curCTRLValue = form.Keys.Any(t => t == ctrl.name) ? form.GetStringIfExist(ctrl.name) : "";
                if (!string.IsNullOrEmpty(curCTRLValue))
                {
                    var enDate = curCTRLValue.ToEnDate();
                    if (enDate == null)
                        throw BException.GenerateNewException(ctrl.label + " مجاز نمی باشد");
                    var curDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    if (enDate.Value >= curDate.AddDays(ctrl.maxDateValidation.Value).AddDays(1))
                        throw BException.GenerateNewException(ctrl.label + " مجاز نمی باشد");
                }
            }
        }

        public void validateSumDate(ctrl ctrl, List<ctrl> allCtrls, IFormCollection form)
        {
            if (ctrl != null && allCtrls != null && allCtrls.Count > 0 && form != null && ctrl.calcDate != null && ctrl.calcDate.Count > 0)
            {
                var tempStr = form.GetStringIfExist(ctrl.name);
                if (!string.IsNullOrEmpty(tempStr))
                {
                    var curValue = tempStr.ToEnDate();
                    if (curValue == null)
                        throw BException.GenerateNewException(BMessages.Invalid_Date);

                    long sumMS = 0;
                    foreach(var ctrlId in ctrl.calcDate)
                    {
                        var foundCurCtrl = allCtrls.Where(t => t.id == ctrlId).FirstOrDefault();
                        if (foundCurCtrl == null)
                            throw BException.GenerateNewException(BMessages.Validation_Error);

                        if (foundCurCtrl.type != ctrlType.persianDateTime)
                        {
                            var targetValue = form.GetStringIfExist(foundCurCtrl.name).ToIntReturnZiro();
                            if (targetValue > 0)
                                sumMS += (targetValue * 864000000000);
                        } else
                        {
                            var targetDate = form.GetStringIfExist(foundCurCtrl.name).ToEnDate();
                            if (targetDate != null)
                            {
                                sumMS += targetDate.Value.Ticks;
                            }
                            
                        }
                    }
                    if (sumMS > 0)
                    {
                        var newDate = new DateTime(sumMS);
                        if (newDate.ToString("yyyy/MM/dd") != curValue.Value.ToString("yyyy/MM/dd"))
                            throw BException.GenerateNewException(BMessages.Invalid_Date);

                        ctrl.defV = newDate.ToFaDate();
                    }
                }
            }
        }
    }
}
