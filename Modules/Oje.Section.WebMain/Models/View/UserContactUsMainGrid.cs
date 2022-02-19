using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class UserContactUsMainGrid: GlobalGrid
    {
        public string createDate { get; set; }
        public string fullname { get; set; }
        public string tell { get; set; }
        public string email { get; set; }

    }
}
