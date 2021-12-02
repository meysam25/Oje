using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Models.View
{
    public class CreateUpdateThirdPartyPassengerRateVM
    {
        public int? id { get; set; }
        public List<int> cIds { get; set; }
        public int? carSpecId { get; set; }
        public string title { get; set; }
        public int? year { get; set; }
        public decimal? rate { get; set; }
        public bool? isActive { get; set; }
        public string carSpecId_Title { get; internal set; }
    }
}
