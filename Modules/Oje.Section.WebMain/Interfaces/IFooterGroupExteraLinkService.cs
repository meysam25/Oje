using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IFooterGroupExteraLinkService
    {
        ApiResult Create(FooterGroupExteraLinkCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(FooterGroupExteraLinkCreateUpdateVM input, int? siteSettingId);
        GridResultVM<FooterGroupExteraLinkMainGridResultVM> GetList(FooterGroupExteraLinkMainGrid searchInput, int? siteSettingId);
        object GetLightList(int? siteSettingId);
    }
}
