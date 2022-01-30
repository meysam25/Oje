using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Security.Models.DB;
using Oje.Security.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Interfaces
{
    public interface IBlockClientConfigService
    {
        ApiResult Create(BlockClientConfigCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(BlockClientConfigCreateUpdateVM input, int? siteSettingId);
        GridResultVM<BlockClientConfigMainGridResultVM> GetList(BlockClientConfigMainGrid searchInput, int? siteSettingId);
        BlockClientConfig GetBy(BlockClientConfigType type, int? siteSettingId);
    }
}
