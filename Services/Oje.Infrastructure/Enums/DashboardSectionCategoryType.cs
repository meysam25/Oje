using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum DashboardSectionCategoryType: byte
    {
        [Display(Name = "کنترول تب")]
        TabCtrl = 1
    }
}
