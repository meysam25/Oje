using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class UserAccessCache
    {
        public UserAccessCache()
        {
            CreateDate = DateTime.Now;
        }
        public DateTime CreateDate { get; set; }
        public long UserId { get; set; }
        public List<DB.Action> Actions { get; set; }
    }
}
