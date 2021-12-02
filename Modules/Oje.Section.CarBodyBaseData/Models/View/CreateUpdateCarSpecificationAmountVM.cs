using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBodyBaseData.Models.View
{
    public class CreateUpdateCarSpecificationAmountVM
    {
        public int? id { get; set; }
        public List<int> cIds { get; set; }
        public int? carSpecId { get; set; }
        public string carSpecId_Title { get; set; }
        public long? minVaue { get; set; }
        public long? maxValue { get; set; }
        public decimal? rate { get; set; }
        public long? price { get; set; }
        public bool? isActive { get; set; }
    }
}
