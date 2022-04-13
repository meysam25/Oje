using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Hubs.Models
{
    public class MapObj
    {
        public bool isAdmin { get; set; }
        public string type { get; set; }
        public string cTime { get; set; }
        public string mapLat { get; set; }
        public string mapLon { get; set; }
        public string mapZoom { get; set; }
    }
}
