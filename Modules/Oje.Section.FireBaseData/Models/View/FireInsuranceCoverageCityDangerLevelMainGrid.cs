using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.View
{
    public class FireInsuranceCoverageCityDangerLevelMainGrid: GlobalGrid
    {
        public string cover { get; set; }
        public decimal? rate { get; set; }
        public int? danger { get; set; }
        public bool? isActive { get; set; }
    }
}
