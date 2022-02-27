using Oje.AccountService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Interfaces
{
    public interface ICompanyService
    {
        object GetightList();
        int GetIdBy(string companyTitle);
        Company GetBy(string companyTitle);
        string GetTitle(int id);
    }
}
