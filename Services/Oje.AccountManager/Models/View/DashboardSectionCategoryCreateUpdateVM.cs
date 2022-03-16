using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class DashboardSectionCategoryCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string cssClass { get; set; }
        public DashboardSectionCategoryType? type { get; set; }
        public int? order { get; set; }
    }
}
