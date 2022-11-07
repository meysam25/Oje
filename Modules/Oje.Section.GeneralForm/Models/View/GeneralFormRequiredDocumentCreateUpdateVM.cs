using Microsoft.AspNetCore.Http;

namespace Oje.Section.GlobalForms.Models.View
{
    public class GeneralFormRequiredDocumentCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public long? fId { get; set; }
        public bool? isActive { get; set; }
        public bool? isRequired { get; set; }
        public IFormFile mainFile { get; set; }
    }
}
