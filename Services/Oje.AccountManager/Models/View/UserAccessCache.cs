using System;
using System.Collections.Generic;

namespace Oje.AccountService.Models.View
{
    public class UserAccessCache
    {
        public UserAccessCache()
        {
            CreateDate = DateTime.Now;
        }
        public DateTime CreateDate { get; set; }
        public DateTime LastActiveTime { get; set; }
        public long UserId { get; set; }
        public List<DB.Action> Actions { get; set; }
    }
}
