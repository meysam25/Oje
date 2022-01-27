using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class FooterSymbolCreateUpdateVM
    {
        [Display(Name = "نماد الکترونیکی")]
        [IgnoreStringEncode]
        public MyHtmlString enamad { get; set; }
        [Display(Name = "نماد ساماندهی")]
        [IgnoreStringEncode]
        public MyHtmlString samandehi { get; set; }
    }
}
