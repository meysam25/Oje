using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class AutoAnswerOnlineChatMessageCreateUpdateVM
    {
        public int? id { get; set; }
        public int? pKey { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }
        public bool? isMessage { get; set; }
        public bool? hasLike { get; set; }
    }
}
