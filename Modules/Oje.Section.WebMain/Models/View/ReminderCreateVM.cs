using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class ReminderCreateVM
    {
        public int? fid { get; set; }
        public string targetDate { get; set; }
        public string mobile { get; set; }
        public string summery { get; set; }
    }
}
