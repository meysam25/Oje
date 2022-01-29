using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Models.View
{
    public class FileAccessRoleMainGrid: GlobalGrid
    {
        public int? role { get; set; }
        public FileType? fType { get; set; }
    }
}
