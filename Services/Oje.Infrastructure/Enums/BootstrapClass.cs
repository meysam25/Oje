using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum BootstrapClass
    {
        [Display(Name = "col-md-12 col-sm-12 col-lg-12 col-xl-12 col-xs-12", Prompt = "col-md-12 col-sm-12 col-lg-12 col-xl-12 col-xs-12")]
        all_12 = 1,
        [Display(Name = "col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12", Prompt = "col-xl-4 col-lg-4 col-md-4 col-sm-6 col-xs-12")]
        col_4_6_12 = 2,
        [Display(Name = "col-xl-3 col-lg-3 col-md-3 col-sm-6 col-xs-12", Prompt = "col-xl-3 col-lg-3 col-md-3 col-sm-6 col-xs-12")]
        col_3_6_12 = 3,
        [Display(Name = "col-xl-2 col-lg-2 col-md-2 col-sm-6 col-xs-12", Prompt = "col-xl-2 col-lg-2 col-md-2 col-sm-6 col-xs-12")]
        col_2_6_12 = 4
    }
}
