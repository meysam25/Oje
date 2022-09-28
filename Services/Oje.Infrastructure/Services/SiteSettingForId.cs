using Oje.Infrastructure.Interfac;
using System.Linq;

namespace Oje.Infrastructure.Services
{
    public static class SiteSettingForId
    {
        public static IQueryable<T> getSiteSettingQuiry<T>(this IQueryable<T> input, bool? canSeeAllWebSites, int? siteSettingId) where T : IEntityWithSiteSettingId
        {
            if (canSeeAllWebSites == true)
                return input;

            return input.Where(t => t.SiteSettingId == siteSettingId);
        }

        public static IQueryable<T> getSiteSettingQuiryNullable<T>(this IQueryable<T> input, bool? canSeeAllWebSites, int? siteSettingId) where T : IEntityWithSiteSettingIdNullable
        {
            if (canSeeAllWebSites == true)
                return input;

            return input.Where(t => t.SiteSettingId == siteSettingId);
        }
    }
}
