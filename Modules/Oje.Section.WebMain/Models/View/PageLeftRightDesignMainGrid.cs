using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class PageLeftRightDesignMainGrid : GlobalGrid
    {
        public string pageTitle { get; set; }
        public bool? isActive { get; set; }
    }
}
