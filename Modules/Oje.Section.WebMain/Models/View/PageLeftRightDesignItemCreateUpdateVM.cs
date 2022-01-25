using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class PageLeftRightDesignItemCreateUpdateVM
    {
        public long? id { get; set; }
        public long? dId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public IFormFile mainImage { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }
        public string bTitle { get; set; }
        public string bLink { get; set; }
    }
}
