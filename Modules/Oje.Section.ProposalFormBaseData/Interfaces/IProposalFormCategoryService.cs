﻿using Oje.Infrastructure.Models;
using Oje.Section.ProposalFormBaseData.Models;
using Oje.Section.ProposalFormBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Interfaces
{
    public interface IProposalFormCategoryService
    {
        ApiResult Create(CreateUpdateProposalFormCategoryVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(CreateUpdateProposalFormCategoryVM input);
        GridResultVM<ProposalFormCategoryMainGridResultVM> GetList(ProposalFormCategoryMainGrid searchInput);
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
