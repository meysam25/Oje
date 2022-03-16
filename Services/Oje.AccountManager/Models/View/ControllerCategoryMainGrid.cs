using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class ControllerCategoryMainGrid: GlobalGrid
    {
        
        public string title { get; set; }
        public string actionTitle { get; set; }
        public bool? isActive { get; set; }
    }
}
