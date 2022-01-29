using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Models.View
{
    public class CreateUpdateFileAccessRoleVM
    {
        public int? id { get; set; }
        public int? roleId { get; set; }
        public FileType? fType { get; set; }
    }
}
