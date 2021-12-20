using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models.PageForms
{
    public class stepWizard
    {
        public stepWizard()
        {
        }

        public bool? isEdit { get; set; }
        public string lastStepButtonTitle { get; set; }
        public string lastStepButtonSubbmitUrl { get; set; }
        public List<actionOnLastStep> actionOnLastStep { get; set; }
        public List<step> steps { get; set; }
    }
}
