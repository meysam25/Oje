using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class CreateUpdateCarExteraDiscountValueVM
    {
        public int? id { get; set; }
        public int? ceId { get; set; }
        public string ceId_Title { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
    }
}
