using NPOI.OpenXmlFormats.Dml.Chart;
using NPOI.SS.Formula.Functions;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Infrastructure.Services
{
    public static class LdJsonService
    {
        public static Dictionary<string, object> GetAboutUsJSObject(string url, string logo, string title, string description, Dictionary<string, object> input = null, string type = "Organization")
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            var curHeader = GetMain();
            if (curHeader != null)
                foreach (var item in curHeader)
                    result.Add(item.Key, curHeader[item.Key]);


            result.Add("@type", type);

            result.Add("url", url);
            result.Add("logo", logo);
            result.Add("name", title);
            result.Add("description", description);

            if (input != null)
                foreach (var item in input)
                    result.Add(item.Key, input[item.Key]);

            return result;
        }

        public static Dictionary<string, object> GetNews(string title, string mainImage_address, string createDateEn, string publishDateEn, string user, string catTitle, string url, string summery)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            var curHeader = GetMain();
            if (curHeader != null)
                foreach (var item in curHeader)
                    result.Add(item.Key, curHeader[item.Key]);

            result.Add("@type", "NewsArticle");
            result.Add("headline", title);
            result.Add("description", summery);
            result.Add("image", mainImage_address);
            result.Add("datePublished", publishDateEn);
            result.Add("dateCreated", createDateEn);
            result.Add("url", url);
            result.Add("mainEntityOfPage", catTitle);
            List<object> tempAuthor = new List<object>();
            tempAuthor.Add(new Dictionary<string, object>() { { "@type", "Person" }, { "name", user } });
            result.Add("author", tempAuthor);

            return result;
        }

        public static object GetNews2(string title, string mainImage, string mainImageSmall, string url, string summery, DateTime createDate)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            var curHeader = GetMain();
            if (curHeader != null)
                foreach (var item in curHeader)
                    result.Add(item.Key, curHeader[item.Key]);

            result.Add("@type", "Article");
            result.Add("headline", title);
            result.Add("datePublished", createDate.ToString("yyyy-MM-dd"));
            result.Add("dateCreated", createDate.ToString("yyyy-MM-dd"));
            result.Add("description", summery);
            result.Add("image", new List<string>() { mainImage, mainImageSmall });
            result.Add("url", url);

            return result;
        }

        public static object GetBreadcrumb(List<KeyValue> breadcrumbList)
        {
            if (breadcrumbList == null || breadcrumbList.Count == 0)
                return null;
            Dictionary<string, object> result = new Dictionary<string, object>();

            var curHeader = GetMain();
            if (curHeader != null)
                foreach (var item in curHeader)
                    result.Add(item.Key, curHeader[item.Key]);

            result.Add("@type", "BreadcrumbList");

            List<object> arrItems = new List<object>();
            for (var i = 0; i < breadcrumbList.Count; i++)
            {
                Dictionary<string, object> resultItems = new Dictionary<string, object>();
                resultItems.Add("@type", "ListItem");
                resultItems.Add("position", (i + 1) + "");
                resultItems.Add("name", breadcrumbList[i].key);
                if (!string.IsNullOrEmpty(breadcrumbList[i].value))
                    resultItems.Add("item", breadcrumbList[i].value);
                arrItems.Add(resultItems);
            }

            result.Add("itemListElement", arrItems);

            return result;
        }

        public static Dictionary<string, object> GetCorporation(string title, string alterTitle, string url, string logo, string email, List<Dictionary<string, object>> input = null)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            var curHeader = GetMain();
            if (curHeader != null)
                foreach (var item in curHeader)
                    result.Add(item.Key, curHeader[item.Key]);

            result.Add("@type", "Corporation");

            result.Add("name", title);
            result.Add("alternateName", new List<string>() { title, alterTitle });
            result.Add("url", url);
            result.Add("email", email);
            result.Add("logo", logo);

            if (input != null)
                foreach (var arrItem in input)
                {
                    if (arrItem == null)
                        continue;
                    foreach (var item in arrItem)
                    {
                        if (!arrItem.Keys.Any(t => t == item.Key))
                            continue;
                        result.Add(item.Key, arrItem[item.Key]);
                    }

                }


            return result;
        }

        public static Dictionary<string, object> GetAddress(string address, string provinceCity, string postalCode, Dictionary<string, object> geo)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(address))
            {
                var addResDic = new Dictionary<string, object>();
                addResDic.Add("@type", "PostalAddress");
                addResDic.Add("streetAddress", address);
                if (!string.IsNullOrEmpty(provinceCity))
                    addResDic.Add("addressLocality", provinceCity);
                if (!string.IsNullOrEmpty(postalCode))
                    addResDic.Add("postalCode", postalCode);



                if (geo != null)
                {
                    var servcePlace = new Dictionary<string, object>();
                    servcePlace.Add("@context", "https://schema.org");
                    servcePlace.Add("@type", "Place");
                    foreach (var geoItem in geo)
                        servcePlace.Add(geoItem.Key, geo[geoItem.Key]);

                    addResDic.Add("areaServed", servcePlace);
                }

                addResDic.Add("addressCountry", new Dictionary<string, object>() { { "@type", "Country" }, { "name", "Iran" } });

                result.Add("address", addResDic);
            }


            return result;
        }

        public static Dictionary<string, object> GetContactTell(string tell)
        {
            if (!string.IsNullOrEmpty(tell))
                return new() { { "contactPoint", new Dictionary<string, object>() { { "@type", "ContactPoint" }, { "telephone", tell }, { "contactType", "customer service" }, { "areaServed", "IR" }, { "availableLanguage", "Persian" } } } };

            return null;
        }

        public static Dictionary<string, object> GetFounder(string fullname, string image)
        {
            if (!string.IsNullOrEmpty(fullname))
            {
                var founder = new Dictionary<string, object>();

                founder.Add("@context", "https://schema.org");
                founder.Add("@type", "Person");
                founder.Add("jobTitle", "Owner");
                founder.Add("name", fullname);
                if (!string.IsNullOrEmpty(image))
                    founder.Add("image", image);

                return new() { { "founders", new List<Dictionary<string, object>>() { founder } } };

            }

            return null;
        }

        public static Dictionary<string, object> GetGEO(double? lat, double? lon)
        {
            if (lat != null && lon != null)
                return new() { { "geo", new Dictionary<string, object>() { { "@type", "GeoCoordinates" }, { "latitude", lat }, { "longitude", lon } } } };

            return null;
        }

        static Dictionary<string, string> GetMain()
        {
            return new Dictionary<string, string>() { { "@context", "https://schema.org" } };
        }
    }
}
