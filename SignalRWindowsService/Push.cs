using Microsoft.AspNet.SignalR;
using System;
using System.Linq;
using System.Text;
using System.Web.Http;
using Utilities;
namespace SignalRWindowsService
{
    public class PushController : ApiController
    {
        [HttpPost]
        public string Send(dynamic a)
        {
            MessageBody model = JsonHelper.JsonToEntity<MessageBody>(a.ToString());
            var hub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            string toUserName = model.Account;
            string message = model.Message;

            var toUser = ChatHub.OnlineUsers.FirstOrDefault(x => x.UserName == toUserName);
            if (toUser != null)
            {
                hub.Clients.Client(toUser.ConnectionId).receivePrivateMessage(toUserName, message);
                return "成功";
            }
            else
            {
                return "用户不在线";
            }
        }
    }
}
