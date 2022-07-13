using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Oje.Infrastructure.Enums;
using System;

namespace Oje.Infrastructure.Services
{
    public static class GlobalServices
    {
        public static string replaceKeyword(string input, long? objectId, string title, string userFullname, object exteraParameter)
        {
            if(exteraParameter != null)
            {
                var allProps = exteraParameter.GetType().GetProperties();
                foreach(var prop in allProps)
                    input = input.Replace("{{" + prop.Name + "}}", prop.GetValue(exteraParameter) + "");
            }
            return (input + "").Replace("{{datetime}}", DateTime.Now.ToFaDate() + " " + DateTime.Now.ToString("HH:mm")).Replace("{{objectId}}", objectId + "").Replace("{{fromUser}}", userFullname).Replace("{{title}}", title);
        }

        public static int MaxForNotify { get { return 1000; } }

        public static void FillSeoInfo(ViewDataDictionary viewData, string pageTitle, string pageDescription, string pageLink, string pageShortLink, WebSiteTypes website, string imageUrl, DateTime? createDate)
        {
            viewData["Title"] = pageTitle;
            viewData["metaDescription"] = pageDescription;
            viewData["canonical"] = pageLink;
            viewData["shortLink"] = pageShortLink;
            viewData["type"] = website.ToString();
            viewData["imageUrl"] = imageUrl;
            if (createDate != null)
                viewData["createDate"] = createDate.Value.ToUniversalTime().ToString();
        }
    }
}
