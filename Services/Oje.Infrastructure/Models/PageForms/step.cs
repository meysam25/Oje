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
        }

        public decimal? order { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string showUrl { get; set; }
        public List<panel> panels { get; set; }
    }
}
