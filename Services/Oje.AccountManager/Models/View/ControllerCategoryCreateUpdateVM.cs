using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class ControllerCategoryCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }
        public string icon { get; set; }
        public List<int> controllerIds { get; set; }
    }
}
