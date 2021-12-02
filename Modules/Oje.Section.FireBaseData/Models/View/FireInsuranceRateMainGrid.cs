using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.View
{
    public class FireInsuranceRateMainGrid : GlobalGrid
    {
        public int? company { get; set; }
        public int? bBody { get; set; }
        public int? bType { get; set; }
        public string title { get; set; }
        public long? maxValue { get; set; }
        public long? minValue { get; set; }
        public decimal? rate { get; set; }
        public bool? isActive { get; set; }
    }
}
