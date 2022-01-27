using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IFooterExteraLinkService
    {
        ApiResult Create(FooterExteraLinkCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(FooterExteraLinkCreateUpdateVM input, int? siteSettingId);
        GridResultVM<FooterExteraLinkMainGridResultVM> GetList(FooterExteraLinkMainGrid searchInput, int? siteSettingId);
        object GetLightList(int? siteSettingId);
    }
}
