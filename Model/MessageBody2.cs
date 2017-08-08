using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MessageBody2
    {
        public string app_key { get; set; }
        public string userName { get; set; }
        public string passWord { get; set; }
        public string timestamp { get; set; }
        public string NoticeId { get; set; }
        public List<string> Account { get; set; }
        public string Message { get; set; }
    }
}
