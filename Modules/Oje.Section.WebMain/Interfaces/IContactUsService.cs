using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IContactUsService
    {
        ApiResult Create(int? siteSettingId, IpSections ipSections, ContactUsWebVM input);
        object GetBy(string id, int? siteSettingId);
        GridResultVM<UserContactUsMainGridResultVM> GetList(UserContactUsMainGrid searchInput, int? siteSettingId);
    }
}
