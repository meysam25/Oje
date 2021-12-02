using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class CreateUpdateVehicleUsageVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public decimal? bodyPercent { get; set; }
        public decimal? thirdPartyPercent { get; set; }
        public bool isActive { get; set; }
        public List<int> carTypeIds { get; set; }
    }
}
