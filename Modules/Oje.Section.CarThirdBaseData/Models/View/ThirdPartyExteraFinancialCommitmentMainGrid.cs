using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Models.View
{
    public class ThirdPartyExteraFinancialCommitmentMainGrid: GlobalGrid
    {
        public int? company { get; set; }
        public string specTitle { get; set; }
        public string commitmentTitle { get; set; }
        public string title { get; set; }
        public decimal? rate { get; set; }
        public int? year { get; set; }
        public bool? isActive { get; set; }
    }
}
