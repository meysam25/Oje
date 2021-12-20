using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Services
{
    public static class GlobalServices
    {
        public static string replaceKeyword(string input, long? objectId, string title, string userFullname)
        {
            return (input + "").Replace("{{datetime}}", DateTime.Now.ToFaDate()).Replace("{{objectId}}", objectId + "").Replace("{{fromUser}}", userFullname).Replace("{{title}}", title);
        }

        public static int MaxForNotify { get { return 1000; } }
    }
}
