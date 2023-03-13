using Oje.Infrastructure.Models;

namespace Oje.ValidatedSignature.Models.View
{
    public class BankAccountMainGrid: GlobalGrid
    {
        public int id { get; set; }
        public string title { get; set; }
        public long? cardno { get; set; }
        public string shabaNo { get; set; }
        public long? hesabNo { get; set; }
        public string user { get; set; }
        public bool? isActive { get; set; }
        public string website { get; set; }
    }
}
