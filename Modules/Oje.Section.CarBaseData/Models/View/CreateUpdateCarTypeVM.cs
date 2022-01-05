using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class CreateUpdateCarTypeVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
        public int? order { get; set; }
    }
}
