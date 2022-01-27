using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class AboutUsMainPageVM: AboutUsMainPageCreateUpdateVM
    {
        public string rightFile_address { get; set; }
        public string centerFile_address { get; set; }
        public string leftFile_address { get; set; }
    }
}
