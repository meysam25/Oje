using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class ContactUsVM
    {
        public string title { get; set; }
        public string subTitle { get; set; }
        public string mapLat { get; set; }
        public string mapLon { get; set; }
        public string mapZoom { get; set; }
        public string description { get; set; }
    }
}
