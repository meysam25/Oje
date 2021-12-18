using Oje.Infrastructure.Models;
using Oje.Section.Financial.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Financial.Interfaces
{
    public interface IBankService
    {
        ApiResult Create(CreateUpdateBankVM input, long? userId);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(CreateUpdateBankVM input, long? userId);
        GridResultVM<BankMainGridResultVM> GetList(BankMainGrid searchInput);
        object GetLightList();
    }
}
