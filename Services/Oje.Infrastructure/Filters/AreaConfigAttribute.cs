using System;

namespace Oje.Infrastructure.Filters
{
    public class AreaConfigAttribute : Attribute
    {
        public string ModualTitle { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool IsMainMenuItem { get; set; }
        public bool HasFormGenerator { get; set; }
        public int Order { get; set; }

        public AreaConfigAttribute()
        {
            Title = "";
            Icon = "";
            ModualTitle = "";
            IsMainMenuItem = false;
            HasFormGenerator = false;
            Order = 1;
        }
    }
}
