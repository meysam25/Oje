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
            actionOnLastSteps = new List<actionOnLastStep>();
            steps = new List<step>();
        }

        public string lastStepButtonTitle { get; set; }
        public List<actionOnLastStep> actionOnLastSteps { get; set; }
        public List<step> steps { get; set; }
    }
}
