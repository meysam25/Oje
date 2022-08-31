using Microsoft.AspNetCore.Http;

namespace Oje.ProposalFormService.Models.View
{
    public class ProposalFilledFormChangeStatusFileVM
    {
        public string fileType { get; set; }
        public IFormFile mainFile { get; set; }
    }
}
