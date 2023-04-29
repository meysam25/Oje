using Microsoft.AspNetCore.Http;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class TreatmentMainSliderCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public IFormFile mainFile { get; set; }
        public string link { get; set; }
        public bool? isActive { get; set; }
    }
}
