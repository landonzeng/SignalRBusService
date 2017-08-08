using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerConsole
{
    public class UserInfo
    {
        public string ConnectionId { get; set; }
        //public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime LastLoginTime { get; set; }
    }
}
