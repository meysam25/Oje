using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.View
{
    public class ProposalFilledFormDocumentCreateUpdateVM
    {
        public long? id { get; set; }
        public long? pKey { get; set; }
        public ProposalFilledFormDocumentType? type { get; set; }
        public long? price { get; set; }
        public string arriveDate { get; set; }
        public string cashDate { get; set; }
        public string code { get; set; }
        public int? bankId { get; set; }
        public IFormFile mainFile { get; set; }
        public string description { get; set; }
    }
}
