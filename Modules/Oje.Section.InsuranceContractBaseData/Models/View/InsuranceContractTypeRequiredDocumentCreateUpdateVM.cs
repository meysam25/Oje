using Microsoft.AspNetCore.Http;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractTypeRequiredDocumentCreateUpdateVM
    {
        public int? id { get; set; }
        public int? cid { get; set; }
        public int? ctId { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
        public bool? isRequired { get; set; }
        public IFormFile downloadFile { get; set; }
        public string downloadFile_address { get; set; }
    }
}
