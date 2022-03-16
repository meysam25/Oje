using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum DashboardSectionType
    {
        [Display(Name = "دکمه میانبور")]
        Shortcut = 1,
        [Display(Name = "محتوی")]
        Content = 2,
        [Display(Name = "کلید تب")]
        TabContent = 3,
        [Display(Name = "تب")]
        Tab = 4
    }
}
