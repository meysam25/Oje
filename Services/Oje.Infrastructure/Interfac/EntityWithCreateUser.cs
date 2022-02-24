using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Interfac
{
    public interface EntityWithCreateUser<User, CUT> where User : EntityWithParent<User> where CUT : struct
    {
        public CUT CreateUserId { get; set; }
        public User CreateUser { get; set; }
    }
}
