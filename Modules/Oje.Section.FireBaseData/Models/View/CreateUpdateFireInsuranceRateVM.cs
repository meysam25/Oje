using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.View
{
    public class CreateUpdateFireInsuranceRateVM
    {
        public int? id { get; set; }
        public List<int> cIds { get; set; }
        public int? bTypeId { get; set; }
        public int? bBodyId { get; set; }
        public string title { get; set; }
        public long? minValue { get; set; }
        public long? maxValue { get; set; }
        public decimal? rate { get; set; }
        public bool? isActive { get; set; }
    }
}
