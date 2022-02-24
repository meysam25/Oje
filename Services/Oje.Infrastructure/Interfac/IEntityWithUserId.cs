using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Interfac
{
    public interface IEntityWithUserId<TUser, CUT> where TUser : EntityWithParent<TUser> where CUT : struct
    {
        public CUT UserId { get; set; }
        public TUser User { get; set; }
    }
}
