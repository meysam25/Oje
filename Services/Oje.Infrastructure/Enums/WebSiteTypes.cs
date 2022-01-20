using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum WebSiteTypes
    {
        [Display(Name = "وبسایت")]
        website = 1,
        [Display(Name = "مقاله")]
        article = 2
    }
}
