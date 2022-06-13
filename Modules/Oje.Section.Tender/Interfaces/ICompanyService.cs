using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Tender.Interfaces
{
    public interface ICompanyService
    {
        object GetLightList(long? userId);
        object GetLightList();
    }
}
