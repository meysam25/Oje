using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IPageWebItemVM
    {
        public int order { get; set; }
        public PageWebItemType type { get; set; }
    }
}
