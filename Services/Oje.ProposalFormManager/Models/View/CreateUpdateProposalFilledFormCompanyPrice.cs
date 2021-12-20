using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.View
{
    public class CreateUpdateProposalFilledFormCompanyPrice
    {
        public string id { get; set; }
        public long? pKey { get; set; }
        public int? companyId { get; set; }
        public long? price { get; set; }
        public IFormFile mainFile { get; set; }
    }
}
