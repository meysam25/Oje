using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.View
{
    public class ProposalFilledFormDocumentMainGrid: GlobalGridParentLong
    {
        public ProposalFilledFormDocumentType? type { get; set; }
        public int? bankId { get; set; }
        public long? price { get; set; }
        public string createDate { get; set; }
        public string arriveDate { get; set; }
        public string cashDate { get; set; }
        public CreditStatus? status { get; set; }
    }
}
