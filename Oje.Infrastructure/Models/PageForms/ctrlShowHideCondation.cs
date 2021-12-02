using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models.PageForms
{
    public class ctrlShowHideCondation
    {
        public ctrlShowHideCondation()
        {
            classShow = new List<string>();
            classHide = new List<string>();
        }

        public string value { get; set; }
        public List<string> classShow { get; set; }
        public List<string> classHide { get; set; }
        public bool? isDefault { get; set; }
    }
}
