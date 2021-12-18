using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFormCategoryService
    {
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
