using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.View
{
    public class CreateUpdateFireInsuranceCoverageTitleVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public FireInsuranceCoverageEffectOn? effect { get; set; }
        public string description { get; set; }
        public bool? isActive { get; set; }
    }
}
