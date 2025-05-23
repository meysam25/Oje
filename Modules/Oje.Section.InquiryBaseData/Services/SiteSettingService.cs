﻿using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Services.EContext;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Oje.Section.InquiryBaseData.Services
{
    public class SiteSettingService: ISiteSettingService
    {
        readonly InquiryBaseDataDBContext db = null;
        public SiteSettingService(InquiryBaseDataDBContext db)
        {
            this.db = db;
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.SiteSettings.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
