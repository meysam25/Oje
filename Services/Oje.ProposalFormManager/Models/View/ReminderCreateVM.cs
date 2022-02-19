using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.View
{
    public class ReminderCreateVM
    {
        public string id { get; set; }
        public int? fid { get; set; }
        public string targetDate { get; set; }
        public string mobile { get; set; }
        public string summery { get; set; }
        public IFormFile insuranceImage { get; set; }
        public IFormFile nationalCard { get; set; }
    }
}
