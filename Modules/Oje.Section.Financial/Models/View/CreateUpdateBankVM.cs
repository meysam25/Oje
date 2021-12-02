using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Financial.Models.View
{
    public class CreateUpdateBankVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
        public IFormFile minPic { get; set; }
    }
}
