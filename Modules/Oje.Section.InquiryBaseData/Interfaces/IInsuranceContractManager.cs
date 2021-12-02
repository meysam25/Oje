using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Interfaces
{
    public interface IInsuranceContractManager
    {
        object GetLightList();
        bool Exist(int id);
    }
}
