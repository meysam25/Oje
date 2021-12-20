using Oje.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Services
{
    public static class MyValidations
    {
        public static void SiteSettingValidation(int? userSiteSettingId, int? siteSettingId)
        {
            if (userSiteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.User_SiteSetting_Can_Not_Be_Founded);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (userSiteSettingId.ToIntReturnZiro() != siteSettingId.ToIntReturnZiro())
                throw BException.GenerateNewException(BMessages.Confilict_In_SiteSetting);
        }
    }
}
