using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.View
{
    public class CreateUpdateFireInsuranceBuildingAgeVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int? order { get; set; }
        public int? year { get; set; }
        public int? percent { get; set; }
        public bool? isActive { get; set; }
    }
}
