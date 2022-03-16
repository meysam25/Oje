using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class OurCustomerCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public IFormFile mainImage { get; set; }
        public string url { get; set; }
        public bool? isActive { get; set; }
    }
}
