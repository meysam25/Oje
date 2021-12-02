using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models
{
    public class MyHtmlString
    {
        public string innerText { get; set; }
        public MyHtmlString(string innerText)
        {
            this.innerText = innerText;
        }

        public int Length { get { return (innerText + "").Length; } }

        public static implicit operator string(MyHtmlString v) { return v?.innerText; }
        public static implicit operator MyHtmlString(string v) { return new MyHtmlString(v); }
    }
}
