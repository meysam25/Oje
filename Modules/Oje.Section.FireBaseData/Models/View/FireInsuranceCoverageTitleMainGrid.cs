using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.View
{
    public class FireInsuranceCoverageTitleMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public FireInsuranceCoverageEffectOn? effect { get; set; }
        public bool? isActive { get; set; }
    }
}
