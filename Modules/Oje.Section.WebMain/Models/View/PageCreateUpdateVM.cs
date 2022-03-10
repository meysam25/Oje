using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class PageCreateUpdateVM
    {
        public long? id { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public string stColor { get; set; }
        public string summery { get; set; }
        public IFormFile mainImage { get; set; }
        public IFormFile mainImageSmall { get; set; }
        public string bTitle { get; set; }
        public string bLink { get; set; }
    }
}
