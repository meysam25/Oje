using Oje.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Vereyon.Web;
using Oje.Infrastructure.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using Oje.Infrastructure.Models.PageForms;
using System.Net;

namespace Oje.Infrastructure.Services
{
    public static class RushCode
    {
        public static string GetStringIfExist(this IFormCollection input, string key)
        {
            string result = "";

            if (input != null && input.ContainsKey(key))
            {
                var currValue = input[key];
                result = string.Join(",", currValue);
            }

            return result;
        }

        public static bool isCtrlVisible(this ctrl ctrl, IFormCollection form, List<ctrl> allCtrls)
        {
            bool result = true;
            var allCtrlExceptCurrentCTRL = allCtrls.Where(t => t != ctrl).ToList();
            var currentClass = ctrl.@class;
            var currentClassArr = string.IsNullOrEmpty(currentClass) ? new List<string>() : currentClass.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var foundCtrlsThatHaveThisClass = allCtrlExceptCurrentCTRL
                .Where(t => t.showHideCondation != null && t.showHideCondation.Any(tt => (tt.classShow != null && tt.classShow.Any(ttt => currentClassArr.Contains(ttt))) || (tt.classHide != null && tt.classHide.Any(ttt => currentClassArr.Contains(ttt)))))
                .ToList();

            foreach (var actorCtrl in foundCtrlsThatHaveThisClass)
            {
                string ctrlSelectValue = form.GetStringIfExist(actorCtrl.name);
                var defValue = actorCtrl.showHideCondation.Where(t => t.isDefault == true).FirstOrDefault();
                var selectedValue = actorCtrl.showHideCondation.Where(t => t.value == ctrlSelectValue).FirstOrDefault();
                var notSelectedValue = actorCtrl.showHideCondation.Where(t => !string.IsNullOrEmpty(t.value) && t.value.Substring(0, 1) == "!" && t.value.Replace("!", "") != ctrlSelectValue).FirstOrDefault();

                if (selectedValue != null && currentClassArr.Any(t => selectedValue.classHide != null && selectedValue.classHide.Contains(t)))
                    result = false;
                else if (selectedValue != null && currentClassArr.Any(t => selectedValue.classShow != null && selectedValue.classShow.Contains(t)))
                {

                }
                else if (notSelectedValue != null && currentClassArr.Any(t => notSelectedValue.classHide.Contains(t)))
                    result = false;
                else if (notSelectedValue != null && currentClassArr.Any(t => notSelectedValue.classShow.Contains(t)))
                {

                }
                else if (defValue != null && currentClassArr.Any(t => defValue.classHide.Contains(t)))
                    result = false;
                else if (defValue != null && currentClassArr.Any(t => defValue.classShow != null && defValue.classShow.Contains(t)))
                {

                }
            }

            return result;
        }

        public static List<T> GetAllListOf<T>(this object input)
        {
            var result = new List<T>();

            if (input != null)
            {
                var allProps = input.GetType().GetProperties();
                foreach (var prop in allProps)
                {
                    if (prop.PropertyType == result.GetType() && prop.GetValue(input) != null)
                        result.AddRange(prop.GetValue(input) as List<T>);
                    else
                    {
                        var inputType = input.GetType();
                        if (inputType != typeof(int) && inputType != typeof(string) && inputType != typeof(bool?) && inputType != typeof(int?) && inputType != typeof(bool))
                        {
                            var tempObj = prop.GetValue(input);
                            if (tempObj != null)
                                if (tempObj is IList)
                                {
                                    IList tempList = tempObj as IList;
                                    foreach (var item in tempList)
                                        result.AddRange(item.GetAllListOf<T>());
                                }
                                else
                                    result.AddRange(tempObj.GetAllListOf<T>());
                        }
                    }
                }
            }

            return result;
        }


        public static string Encrypt(this string input)
        {
            return Convert.ToBase64String((new C_EDSecure()).Encrypt(input));
        }

        public static string Decrypt(this string input)
        {
            return (new C_EDSecure()).Decrypt(Convert.FromBase64String(input));
        }

        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidExtension(this IFormFile input, string extension)
        {
            try
            {
                if (extension == null)
                    extension = "";
                if (input == null || input.Length <= 20)
                    return false;
                FileInfo fi = new FileInfo(input.FileName);
                var allExtension = extension.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                var foundExtensionStr = allExtension.Where(t => t != null && t.ToLower() == fi.Extension.ToLower()).FirstOrDefault();
                if (string.IsNullOrEmpty(foundExtensionStr))
                    return false;
                byte[] foundFileExtension = MyFileExtensions.allExtensions.Keys.Any(t => t.ToLower() == foundExtensionStr) ? MyFileExtensions.allExtensions[foundExtensionStr] : new byte[] { };

                if (foundFileExtension.Length == 0)
                    return false;
                using (var ms = new MemoryStream())
                {
                    input.CopyTo(ms);
                    byte[] fileBinary = ms.ToArray();

                    if (fileBinary.Take(foundFileExtension.Length).SequenceEqual(foundFileExtension))
                        return true;
                }


                return false;
            }
            catch
            {
                return false;
            }
        }

        public static string GetEnumDisplayName(this Enum input)
        {
            return input.GetAttribute<DisplayAttribute>()?.Name + "";
        }

        public static byte ToByteReturnZiro(this object input)
        {
            try
            {
                if (input == null)
                    return 0;
                return Convert.ToByte(input);
            }
            catch
            {
                return 0;
            }
        }

        public static bool IsWeekPassword(this object input)
        {
            try
            {
                return !(new Regex(@"^(?=.{10,})(((?=.*[A-Z])(?=.*[a-z]))|((?=.*[A-Z])(?=.*[0-9]))|((?=.*[a-z])(?=.*[0-9]))).*$").IsMatch(input + ""));
            }
            catch
            {
                return true;
            }
        }

        public static TAttribute GetAttribute<TAttribute>(this object enumValue)
            where TAttribute : Attribute
        {
            if (enumValue == null)
                return null;

            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .FirstOrDefault()
                            ?.GetCustomAttribute<TAttribute>();
        }

        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static byte[] Zip(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public static string Unzip(this byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static IpSections GetIpAddress(this IHttpContextAccessor input)
        {
            try
            {
                var apAddress = input?.HttpContext?.Request?.HttpContext?.Connection?.RemoteIpAddress.ToString();
                if (!string.IsNullOrEmpty(apAddress))
                {
                    if (apAddress == "::1")
                        apAddress = "127.0.0.1";
                    return apAddress.GetIpSections();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static IpSections GetIpAddress(this HttpContext input)
        {
            try
            {
                var apAddress = input?.Request?.HttpContext?.Connection?.RemoteIpAddress.ToString();
                if (!string.IsNullOrEmpty(apAddress))
                {
                    if (apAddress == "::1")
                        apAddress = "127.0.0.1";
                    return apAddress.GetIpSections();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static string ConvertEnglishNumberToPersianNumber(this string input)
        {
            if (!string.IsNullOrEmpty(input) && input.Length > 0)
            {
                input = input.Replace("0", "۰");
                input = input.Replace("1", "۱");
                input = input.Replace("2", "۲");
                input = input.Replace("3", "۳");
                input = input.Replace("4", "۴");
                input = input.Replace("5", "۵");
                input = input.Replace("6", "۶");
                input = input.Replace("7", "۷");
                input = input.Replace("8", "۸");
                input = input.Replace("9", "۹");
            }
            return input;
        }

        public static string ConvertPersianNumberToEnglishNumber(this string input)
        {
            if (!string.IsNullOrEmpty(input) && input.Length > 0)
            {
                input = input.Replace("۰", "0");
                input = input.Replace("۱", "1");
                input = input.Replace("۲", "2");
                input = input.Replace("۳", "3");
                input = input.Replace("۴", "4");
                input = input.Replace("۵", "5");
                input = input.Replace("۶", "6");
                input = input.Replace("۷", "7");
                input = input.Replace("۸", "8");
                input = input.Replace("۹", "9");
            }
            return input;
        }

        public static bool IsCodeMeli(this string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length != 10)
            {
                return false;
            }
            try
            {
                var englishNumber = input.ConvertPersianNumberToEnglishNumber();

                var number1Text = englishNumber.Substring(0, 1);
                var number1 = int.Parse(number1Text);
                var number1PositionFactor = 10;
                var number1MultiplicationResult = number1 * number1PositionFactor;
                var number2Text = englishNumber.Substring(1, 1);
                var number2 = int.Parse(number2Text);
                var number2PositionFactor = 9;
                var number2MultiplicationResult = number2 * number2PositionFactor;
                var number3Text = englishNumber.Substring(2, 1);
                var number3 = int.Parse(number3Text);
                var number3PositionFactor = 8;
                var number3MultiplicationResult = number3 * number3PositionFactor;
                var number4Text = englishNumber.Substring(3, 1);
                var number4 = int.Parse(number4Text);
                var number4PositionFactor = 7;
                var number4MultiplicationResult = number4 * number4PositionFactor;
                var number5Text = englishNumber.Substring(4, 1);
                var number5 = int.Parse(number5Text);
                var number5PositionFactor = 6;
                var number5MultiplicationResult = number5 * number5PositionFactor;
                var number6Text = englishNumber.Substring(5, 1);
                var number6 = int.Parse(number6Text);
                var number6PositionFactor = 5;
                var number6MultiplicationResult = number6 * number6PositionFactor;
                var number7Text = englishNumber.Substring(6, 1);
                var number7 = int.Parse(number7Text);
                var number7PositionFactor = 4;
                var number7MultiplicationResult = number7 * number7PositionFactor;
                var number8Text = englishNumber.Substring(7, 1);
                var number8 = int.Parse(number8Text);
                var number8PositionFactor = 3;
                var number8MultiplicationResult = number8 * number8PositionFactor;
                var number9Text = englishNumber.Substring(8, 1);
                var number9 = int.Parse(number9Text);
                var number9PositionFactor = 2;
                var number9MultiplicationResult = number9 * number9PositionFactor;
                var number10ControlText = englishNumber.Substring(9, 1);
                var number10Control = int.Parse(number10ControlText);


                var total = number1MultiplicationResult + number2MultiplicationResult + number3MultiplicationResult
                                    + number4MultiplicationResult + number5MultiplicationResult + number6MultiplicationResult
                                    + number7MultiplicationResult + number8MultiplicationResult + number9MultiplicationResult;

                var quotient = 11;
                var remainder = total % quotient;
                var devidend = Math.Floor(Convert.ToDecimal(total / quotient));
                if (remainder < 2)
                {
                    if (remainder == number10Control)
                    {
                        return true;
                    }
                }
                else
                {
                    if ((quotient - remainder) == number10Control)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public static bool IsMobile(this string mobile)
        {
            bool result = true;
            try
            {
                if (mobile == null)
                    return false;
                if (mobile.Length != 11)
                    result = false;

                if (!mobile.StartsWith("09"))
                    result = false;

                return result;
            }
            catch
            {
                return false;
            }
        }

        public static string ToEnDateStr(this object input)
        {
            DateTime? dt = null;
            PersianCalendar pc = new PersianCalendar();

            try
            {
                if (input == null)
                    return "";
                string Input = input.ToString();
                if (Input != null && Input.Length == 4)
                {
                    Input = Input + "/01/01";
                }
                int year = Input.Split('/')[0].ToIntReturnZiro();
                int month = Input.Split('/')[1].ToIntReturnZiro();
                int day = Input.Split('/')[2].ToIntReturnZiro();

                dt = pc.ToDateTime(year, month, day, 0, 0, 0, 0);
                string tempResult = dt.Value.ToString("yyyy/MM/dd");
                return tempResult;
            }
            catch
            {
            }


            return "";
        }

        public static DateTime? ToEnDate(this object input)
        {
            DateTime? dt = null;
            PersianCalendar pc = new PersianCalendar();

            try
            {
                if (input == null)
                    return null;
                string Input = input.ToString();
                if (Input != null && Input.Length == 4)
                {
                    Input = Input + "/01/01";
                }
                int year = Input.Split('/')[0].ToIntReturnZiro();
                int month = Input.Split('/')[1].ToIntReturnZiro();
                int day = Input.Split('/')[2].ToIntReturnZiro();

                dt = pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
            catch
            {
            }

            return dt;
        }

        public static DateTime ToEnDate(this DateTime input)
        {
            PersianCalendar pc = new PersianCalendar();

            DateTime dt = new DateTime(input.Year, input.Month, input.Day, pc);

            return dt;
        }

        public static string ToFaDate(this object input)
        {
            string result = "";
            try
            {
                if (input == null)
                    return "";
                DateTime d = Convert.ToDateTime(input);
                PersianCalendar pc = new PersianCalendar();
                string dayStr = pc.GetDayOfMonth(d) + "";
                if (dayStr.Length == 1)
                    dayStr = "0" + dayStr;
                string monthStr = pc.GetMonth(d) + "";
                if (monthStr.Length == 1)
                    monthStr = "0" + monthStr;
                result = string.Format("{0}/{1}/{2}", pc.GetYear(d), monthStr, dayStr);
            }
            catch
            {

            }


            return result;
        }

        public static string ToFaDate(this object input, string seperator = "/")
        {
            string result = "";
            try
            {
                if (input == null)
                    return "";
                DateTime d = Convert.ToDateTime(input);
                PersianCalendar pc = new PersianCalendar();
                string dayStr = pc.GetDayOfMonth(d) + "";
                if (dayStr.Length == 1)
                    dayStr = "0" + dayStr;
                string monthStr = pc.GetMonth(d) + "";
                if (monthStr.Length == 1)
                    monthStr = "0" + monthStr;
                result = string.Format("{0}{3}{1}{3}{2}", pc.GetYear(d), monthStr, dayStr, seperator);
            }
            catch
            {

            }


            return result;
        }

        public static List<IdTitle> FromYear(this DateTime input, int prevYear)
        {
            List<IdTitle> result = new List<IdTitle>();
            result.Add(new IdTitle { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name });
            if (prevYear <= 0)
                prevYear = 10;

            for (var i = 0; i < prevYear; i++)
            {
                string persianDate = input.ToString("yyyy/MM/dd").ToFaDate();
                string persianYear = "";
                if (!string.IsNullOrEmpty(persianDate))
                    persianYear = persianDate.Split('/')[0];
                result.Add(new IdTitle { id = input.Year + "", title = (input.Year + "-" + persianYear) });
                input = input.AddYears(-1);
            }

            return result;
        }

        public static int ToIntReturnZiro(this object input)
        {
            try
            {
                if (input == null)
                    return 0;
                return Convert.ToInt32(input);
            }
            catch
            {
                return 0;
            }
        }

        public static double? ToDoubleReturnNull(this object input)
        {
            try
            {
                if (input == null)
                    return null;
                return Convert.ToDouble(input);
            }
            catch
            {
                return null;
            }
        }

        public static bool IsLighthouse(this HttpRequest input)
        {
            try
            {
                if (input.Headers["User-Agent"][0].ToLower().IndexOf("lighthouse") > 0)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static string GetTargetAreaByRefferForInquiry(this HttpRequest input)
        {
            try
            {
                string currPath = new Uri(input.Headers["Referer"][0]).LocalPath;
                if (currPath == "/")
                    return "";

                return "/ProposalFilledForm";
            }
            catch
            {
                return "/ProposalFilledForm";
            }
        }

        public static string GetRefererUrl(this HttpRequest input)
        {
            try
            {
                var  currPath = new Uri(input.Headers["Referer"][0]);

                return currPath.Host ;
            }
            catch
            {
                return null;
            }
        }

        public static string GetTargetAreaByRefferForPPFDetailes(this HttpRequest input)
        {
            try
            {
                string currPath = new Uri(input.Headers["Referer"][0]).LocalPath;
                if (currPath == "/Proposal/Form")
                    return "/Proposal/Detaile";

                return "";
            }
            catch
            {
                return "";
            }
        }

        public static string FormatShaba(this long input)
        {
            try
            {
                return input.ToString("IR##-####-####-####-####-####-##");
            }
            catch
            {
                return "";
            }
        }

        public static string FormatCardNo(this long input)
        {
            try
            {
                return input.ToString("####-####-####-####");
            }
            catch
            {
                return "";
            }
        }

        public static IpSections ToIp(this string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input))
                    return null;
                if (input == "::1")
                    return new IpSections() { Ip1 = 127, Ip2 = 0, Ip3 = 0, Ip4 = 1 };
                var allParts = input.Split('.');
                if (allParts.Length != 4)
                    return null;
                var result = new IpSections() { Ip1 = allParts[0].ToByteReturnZiro(), Ip2 = allParts[1].ToByteReturnZiro(), Ip3 = allParts[2].ToByteReturnZiro(), Ip4 = allParts[3].ToByteReturnZiro() };
                if (result.Ip1 > 255 || result.Ip2 > 255 || result.Ip3 > 255 || result.Ip4 > 255)
                    return null;
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static DateTime? ToDateTime(this object input)
        {
            try
            {
                if (input == null)
                    return null;
                return Convert.ToDateTime(input);
            }
            catch
            {
                return null;
            }
        }

        public static DateTime? ToDateTimeFromTick(this object input)
        {
            try
            {
                if (input == null)
                    return null;
                return new DateTime(input.ToLongReturnZiro());
            }
            catch
            {
                return null;
            }
        }

        public static PaymentFactorVM GetPayModel(this string input)
        {
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    string dCryptStatus = input.Decrypt2();

                    if (dCryptStatus.IndexOf(",") > 0)
                    {
                        string[] allParamsArr = dCryptStatus.Split(',');
                        if (allParamsArr.Length == 5)
                            return new PaymentFactorVM() { type = (Enums.BankAccountFactorType)allParamsArr[0].ToIntReturnZiro(), objectId = allParamsArr[1].ToLongReturnZiro(), price = allParamsArr[2].ToLongReturnZiro(), returnUrl = allParamsArr[3], userId = allParamsArr[4].ToLongReturnZiro() };
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public static string GetUserFrindlyDate(this DateTime input)
        {
            try
            {
                string result = "";
                string suffix = "";
                var defDate = DateTime.Now - input;
                if (defDate.TotalMilliseconds < 0)
                {
                    suffix = "دیگه";
                    defDate = input - DateTime.Now;
                }
                else
                    suffix = "پیش";

                if (defDate.TotalMinutes <= 10)
                    result = "لحضات " + suffix;
                if (defDate.TotalMinutes > 10 && defDate.TotalMinutes < 60)
                    result = Math.Floor(defDate.TotalMinutes).ToIntReturnZiro() + " دقیقه " + suffix;
                else if (defDate.TotalHours >= 1 && defDate.TotalHours < 24)
                    result = Math.Floor(defDate.TotalHours) + " ساعت " + suffix;
                else if (defDate.TotalDays >= 1 && defDate.TotalDays < 32)
                    result = Math.Floor(defDate.TotalDays) + " روز " + suffix;
                else if (defDate.TotalDays >= 32 && defDate.TotalDays < 367)
                    result = Math.Floor(defDate.TotalDays / Convert.ToDouble(31)) + " ماه " + suffix;
                else 
                    result = Math.Floor(defDate.TotalDays / Convert.ToDouble(365)) + " سال " + suffix;

                if (string.IsNullOrWhiteSpace(result))
                    result = input.ToFaDate();

                return result;
            }
            catch
            {
                return input.ToFaDate() + " " + input.ToString("HH:mm:ss");
             }
        }

        public static long ToLongReturnZiro(this object input)
        {
            try
            {
                if (input == null)
                    return 0;
                return Convert.ToInt64(input);
            }
            catch
            {
                return 0;
            }
        }

        public static string GetValueExcel(this ICell input)
        {
            string result = "";
            try
            {
                if (input == null)
                    return result;
                result = input.NumericCellValue + "";
            }
            catch
            {
                try { result = input.BooleanCellValue + ""; }
                catch
                {
                    try
                    {
                        result = input.BooleanCellValue + "";
                    }
                    catch
                    {
                        try
                        {
                            result = input.StringCellValue + "";
                        }
                        catch
                        {

                        }

                    }

                }

            }

            return result;
        }

        public static bool ToBooleanReturnFalse(this object input)
        {
            try
            {
                if (input == null)
                    return false;
                return Convert.ToBoolean(input);
            }
            catch
            {
                return false;
            }
        }

        public static decimal ToDecimalZiro(this object input)
        {
            try
            {
                if (input == null)
                    return 0;
                return Convert.ToDecimal(input);
            }
            catch
            {
                return 0;
            }
        }

        public static string removeDangerusTags(this object input)
        {
            try
            {
                var sanitizer = new HtmlSanitizer();
                sanitizer.Tag("h1");
                sanitizer.Tag("h2");
                sanitizer.Tag("h3");
                sanitizer.Tag("h4");
                sanitizer.Tag("p");
                sanitizer.Tag("ul");
                sanitizer.Tag("li");
                sanitizer.Tag("figure");
                sanitizer.Tag("img").AllowAttributes("src");
                sanitizer.Tag("blockquote");
                sanitizer.Tag("table");
                sanitizer.Tag("tbody");
                sanitizer.Tag("tr");
                sanitizer.Tag("td");
                sanitizer.Tag("ol");
                sanitizer.Tag("thead");
                sanitizer.Tag("a").AllowAttributes("href").AllowAttributes("target");
                return sanitizer.Sanitize(input + "");
            }
            catch
            {
                return "";
            }
        }

        public static string Slugify(this string phrase)
        {
            try
            {
                if (phrase == null)
                    return "";
                string str = phrase.RemoveAccent().ToLower();
                // invalid chars           
                str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
                // convert multiple spaces into one space   
                str = Regex.Replace(str, @"\s+", " ").Trim();
                // cut and trim 
                str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
                str = Regex.Replace(str, @"\s", "-"); // hyphens   
                return str;
            }
            catch
            {
                return "";
            }
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        private static List<byte> GetNewKey(int Count)
        {
            List<byte> result = new List<byte>();

            var random = new Random();

            for (var i = 0; i < Count; i++)
            {
                result.Add(random.Next(0, 255).ToByteReturnZiro());

            }

            return result;
        }

        private static List<EKeyVM> GetTodayKeysValue()
        {
            List<EKeyVM> result = new List<EKeyVM>();
            string rootPath = GlobalConfig.WebHostEnvironment.ContentRootPath + "\\KeyValues";
            string todayKeyValue = rootPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            if (!File.Exists(todayKeyValue))
            {
                File.WriteAllText(todayKeyValue,
                        (
                            string.Join(',', GetNewKey(32)) + "&:-:&" + string.Join(',', GetNewKey(16)) + Environment.NewLine +
                            string.Join(',', GetNewKey(32)) + "&:-:&" + string.Join(',', GetNewKey(16)) + Environment.NewLine +
                            string.Join(',', GetNewKey(32)) + "&:-:&" + string.Join(',', GetNewKey(16)) + Environment.NewLine +
                            string.Join(',', GetNewKey(32)) + "&:-:&" + string.Join(',', GetNewKey(16)) + Environment.NewLine
                        ).Encrypt()
                    );
            }

            string tString = File.ReadAllText(todayKeyValue).Decrypt();
            string[] tStringArr = tString.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < tStringArr.Length; i++)
            {
                var allSections = tStringArr[i].Split(new string[] { "&:-:&" }, StringSplitOptions.RemoveEmptyEntries);
                EKeyVM newResult = new EKeyVM();
                newResult.key = allSections[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.ToByteReturnZiro()).ToArray();
                newResult.value = allSections[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.ToByteReturnZiro()).ToArray();
                result.Add(newResult);
            }

            return result;
        }

        public static string Encrypt2(this string input)
        {
            var resultKeys = GetTodayKeysValue();
            string result = input;
            foreach (var item in resultKeys)
            {
                result = Convert.ToBase64String((new C_EDSecure(item.key, item.value)).Encrypt(result));
            }

            return result;
        }

        public static string Decrypt2(this string input)
        {
            var resultKeys = GetTodayKeysValue();
            resultKeys.Reverse();
            string result = input;
            foreach (var item in resultKeys)
            {
                result = (new C_EDSecure(item.key, item.value)).Decrypt(Convert.FromBase64String(result));
            }
            return result;
        }

        public static LoginUserVM Decrypt2AndGetUserVM(this string input)
        {
            LoginUserVM result = null;

            try
            {
                if (string.IsNullOrEmpty(input))
                    return result;
                string dycriptText = input.Decrypt2();
                string[] dycriptTextArr = dycriptText.Split(',');
                result = new LoginUserVM();

                result.UserId = Convert.ToInt64(dycriptTextArr[0]);
                result.Username = dycriptTextArr[1];
                result.Fullname = dycriptTextArr[2];
                result.Ip = dycriptTextArr[3];
                result.siteSettingId = string.IsNullOrEmpty(dycriptTextArr[4]) ? null : dycriptTextArr[4].ToIntReturnZiro();
                result.sessionFileName = dycriptTextArr[5];
                string roles = dycriptTextArr[6];
                result.roles = roles.Split('-').ToList();
                if (string.IsNullOrEmpty(result.sessionFileName))
                    return null;
                if (!MySession.IsFileExist(result.sessionFileName))
                    return null;
            }
            catch
            {
                return null;
            }

            return result;
        }

        public static IpSections GetIpSections(this string input)
        {
            try
            {
                if (!string.IsNullOrEmpty(input) && input.IndexOf(".") > 0)
                {
                    return new IpSections()
                    {
                        Ip1 = input.Split('.')[0].ToByteReturnZiro(),
                        Ip2 = input.Split('.')[1].ToByteReturnZiro(),
                        Ip3 = input.Split('.')[2].ToByteReturnZiro(),
                        Ip4 = input.Split('.')[3].ToByteReturnZiro()
                    };
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public static LoginUserVM GetLoginUser(this HttpContext input)
        {
            try
            {
                LoginUserVM loginUser = null;
                if (input.Request.Cookies.ContainsKey("login"))
                {
                    loginUser = input.Request.Cookies["login"].Decrypt2AndGetUserVM();
                }
                return loginUser;
            }
            catch
            {
                return null;
            }
        }
    }
}
