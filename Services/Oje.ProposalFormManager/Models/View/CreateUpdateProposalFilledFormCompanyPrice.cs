using Microsoft.AspNetCore.Http;

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
