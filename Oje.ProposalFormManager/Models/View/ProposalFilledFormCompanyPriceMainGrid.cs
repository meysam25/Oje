using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.View
{
    public class ProposalFilledFormCompanyPriceMainGrid: GlobalGrid
    {
        public long? pKey { get; set; }
        public int? cId { get; set; }
        public long? price { get; set; }
        public string createUser { get; set; }
        public string createDate { get; set; }
        public string updateUser { get; set; }
        public string updateDate { get; set; }
        public bool? isSelected { get; set; }
    }
}
