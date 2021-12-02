using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class CreateUpdatePaymentMethodFileVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int? payId { get; set; }
        public IFormFile minPic { get; set; }
        public bool? isRequired { get; set; }
    }
}
