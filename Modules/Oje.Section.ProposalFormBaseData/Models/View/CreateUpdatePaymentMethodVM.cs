using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class CreateUpdatePaymentMethodVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public List<int> comIds { get; set; }
        public string title { get; set; }
        public int? order { get; set; }
        public PaymentMethodType? type { get; set; }
        public int? formId { get; set; }
        public bool? isActive { get; set; }
        public bool? isDefault { get; set; }
        public bool? isCheck { get; set; }
        public int? prePayPercent { get; set; }
        public int? debitCount { get; set; }
    }
}
