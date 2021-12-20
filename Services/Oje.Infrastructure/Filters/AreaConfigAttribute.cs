using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Filters
{
    public class AreaConfigAttribute : Attribute
    {
        public string ModualTitle { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool IsMainMenuItem { get; set; }
        public bool HasFormGenerator { get; set; }

        public AreaConfigAttribute()
        {
            Title = "";
            Icon = "";
            ModualTitle = "";
            IsMainMenuItem = false;
            HasFormGenerator = false;
        }
    }
}
