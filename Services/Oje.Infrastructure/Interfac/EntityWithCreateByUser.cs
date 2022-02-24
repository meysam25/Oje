using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Interfac
{
    public interface EntityWithCreateByUser<User, CUT> where User : EntityWithParent<User> where CUT : struct
    {
        public CUT? CreateByUserId { get; set; }
        public User CreateByUser { get; set; }
    }
}
