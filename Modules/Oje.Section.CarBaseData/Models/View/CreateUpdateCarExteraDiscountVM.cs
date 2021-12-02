using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class CreateUpdateCarExteraDiscountVM
    {
        public int? id { get; set; }
        public int? formId { get; set; }
        public string title { get; set; }
        public bool? isOption { get; set; }
        public CarExteraDiscountType? type { get; set; }
        public int? cTypeId { get; set; }
        public CarExteraDiscountCalculateType? calcType { get; set; }
        public bool? isActive { get; set; }
        public bool? hasPrevInsurance { get; set; }
        public string description { get; set; }
        public int? catId { get; set; }
        public int? order { get; set; }
        public bool? dotRemove { get; set; }

        public string formId_Title { get; set; }
    }
}
