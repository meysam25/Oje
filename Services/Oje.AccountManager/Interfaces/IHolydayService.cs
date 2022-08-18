using Oje.AccountService.Models.View;
using Oje.Infrastructure.Models;
using System;

namespace Oje.AccountService.Interfaces
{
    public interface IHolydayService
    {
        ApiResult Create(HolydayCreateUpdateVM input);
        ApiResult Delete(long? id);
        object GetById(long? id);
        ApiResult Update(HolydayCreateUpdateVM input);
        GridResultVM<HolydayMainGridResultVM> GetList(HolydayMainGrid searchInput);
        bool IsHolyday(DateTime now);
    }
}
