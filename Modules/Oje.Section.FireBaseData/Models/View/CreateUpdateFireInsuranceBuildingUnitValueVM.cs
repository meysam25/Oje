using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.View
{
    public class CreateUpdateFireInsuranceBuildingUnitValueVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public long? value { get; set; }
        public bool? isActive { get; set; }
    }
}
