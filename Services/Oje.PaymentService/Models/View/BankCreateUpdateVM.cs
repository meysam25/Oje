using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.View
{
    public class BankCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
        public IFormFile minPic { get; set; }
    }
}
