using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.View
{
    public class CreateUpdateFireInsuranceCoverageVM
    {
        public int? id { get; set; }
        public List<int> cIds { get; set; }
        public int? titleId { get; set; }
        public int? pfId { get; set; }
        public string title { get; set; }
        public decimal? rate { get; set; }
        public bool? isActive { get; set; }
        public string pfId_Title { get; set; }
        public string titleId_Title { get; set; }

    }
}
