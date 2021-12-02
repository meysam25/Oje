using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models
{
    public class GlobalGrid
    {
        int? Skip;
        public int skip { get
            {
                if (Skip == null)
                    return 0;
                if (Skip <= 0)
                    return 0;
                return Skip.Value;
            }
            set
            {
                Skip = value;
            }
        }
        int? Take;
        public int take
        {
            get
            {
                if (Take == null)
                    return 10;
                if (Take <= 0)
                    return 10;
                if (Take > 1000)
                    return 1000;
                return Take.Value;
            }
            set
            {
                Take = value;
            }
        }
    }
}
