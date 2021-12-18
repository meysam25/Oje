using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class AccessTreeViewUser
    {
        public AccessTreeViewUser()
        {
        }

        public long id { get; set; }
        public string title { get; set; }
        public bool selected { get; set; }
        public List<AccessTreeViewUser> childs { get; set; }
    }
}
