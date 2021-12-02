using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models.PageForms
{
    public class step
    {
        public step()
        {
            panels = new List<panel>();
        }

        public int? order { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public List<panel> panels { get; set; }
    }
}
