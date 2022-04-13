using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Oje.Section.WebMain.Hubs.Models
{
    public class UserMessage
    {
        public string connectionId { get; set; }
        public bool IsAdmin { get; set; }

        string _message;
        public string Message { set { _message = HttpUtility.HtmlEncode(value); } get { return _message; } }

        private string _link;
        public string Link
        {
            get { return HttpUtility.HtmlEncode(_link); }
            set { _link = HttpUtility.HtmlEncode(value); }
        }

        private string _voiceLink;

        public string voiceLink
        {
            get { return HttpUtility.HtmlEncode(_voiceLink); }
            set { _voiceLink = HttpUtility.HtmlEncode(value); }
        }

        public DateTime CreateDate { get; set; }
        public IpSections ClientIp { get; set; }
        public MapObj MapObj { get; set; }
    }
}
