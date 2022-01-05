using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.View
{
    public class RequiredQuestionVM
    {
        public int? vehicleTypeId { get; set; }
        public int? havePrevInsurance { get; set; }
        public bool? isNewCar { get; set; }
    }
}
