using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.View
{
    public class FireInsuranceCoverageMainGrid: GlobalGrid
    {
        public int? company { get; set; }
        public string titleId { get; set; }
        public string ppfTitle { get; set; }
        public string title { get; set; }
        public decimal? rate { get; set; }
        public bool? isActive { get; set; }
    }
}
