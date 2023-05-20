using Oje.Infrastructure.Models;

namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatLetterCategoryMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public bool? isActive { get; set; }
        public string code { get; set; }
    }
}
