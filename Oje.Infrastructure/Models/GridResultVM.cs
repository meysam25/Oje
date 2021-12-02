using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models
{
    public class GridResultVM<T>
    {
        public int total { get; set; }
        public List<T> data { get; set; }
    }
}
