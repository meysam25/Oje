using Oje.Infrastructure.Models;

namespace Oje.Section.GlobalForms.Models.View
{
    public class GeneralFormRequiredDocumentMainGrid: GlobalGrid
    {
        public string fId { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public bool? isActive { get; set; }
    }
}
