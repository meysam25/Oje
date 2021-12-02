using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models.PageForms
{
    public class PageForm
    {
        public PageForm()
        {
            panels = new List<panel>();
        }

        public List<panel> panels { get; set; }
    }
}
