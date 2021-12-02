using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Models.View
{
    public class CreateUpdateCompanyVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public bool? isActive { get; set; }
        public IFormFile minPic { get; set; }
    }
}
