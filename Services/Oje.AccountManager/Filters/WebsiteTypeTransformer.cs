using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Enums;
using System.Threading.Tasks;

namespace Oje.AccountService.Filters
{
    public class WebsiteTypeTransformer : DynamicRouteValueTransformer
    {
        readonly ISiteSettingService SiteSettingService = null;
        public WebsiteTypeTransformer(ISiteSettingService SiteSettingService)
        {
           this.SiteSettingService = SiteSettingService;
        }

        public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            var curSetting = SiteSettingService.GetSiteSetting();
            if (curSetting != null)
            {
                switch (curSetting.WebsiteType)
                {
                    case WebsiteType.Tender:
                        await Task.Delay(0);
                        if (values == null)
                            values = new RouteValueDictionary();
                        values["area"] = null;
                        values["controller"] = "TenderWeb";
                        values["action"] = "Index";
                        return values;
                }
            }

            if (values == null)
                values = new RouteValueDictionary();
            values["area"] = null;
            values["controller"] = "Home";
            values["action"] = "Index";

            return values;
        }
    }
}
