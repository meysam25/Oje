using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.View
{
    public class CreateUpdateFireInsuranceCoverageCityDangerLevelVM
    {
        public int? id { get; set; }
        public int? coverId { get; set; }
        public int? danger { get; set; }
        public decimal? rate { get; set; }
        public bool? isActive { get; set; }
        public string coverId_Title { get; internal set; }
    }
}
