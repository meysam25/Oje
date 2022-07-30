using System.Collections.Generic;

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
        public string printTitle { get; set; }
        public bool? hideOnPrint { get; set; }
        public List<panel> panels { get; set; }
    }
}
