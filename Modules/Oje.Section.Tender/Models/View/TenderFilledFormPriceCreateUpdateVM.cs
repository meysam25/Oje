using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Tender.Models.View
{
    public class TenderFilledFormPriceCreateUpdateVM: GlobalGridParentLong
    {
        public long? id { get; set; }
        public int? cid { get; set; }
        public int? pfId { get; set; }
        public long? price { get; set; }
        public IFormFile minPic { get; set; }
        public string description { get; set; }
    }
}
