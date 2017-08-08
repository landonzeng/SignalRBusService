using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerConsole
{
    [HubName("IMHub")]
    public class ChatHub : Hub
    {
        // 静态属性
        public static List<UserInfo> OnlineUsers = new List<UserInfo>(); // 在线用户列表

        /// <summary>
        /// 登录连线
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userName">用户名</param>
        public void Register(string userName)
        {
            var connnectId = Context.ConnectionId;

            if (OnlineUsers.Count(x => x.ConnectionId == connnectId) == 0)
            {
                if (OnlineUsers.Any(x => x.UserName == userName))
                {
                    var items = OnlineUsers.Where(x => x.UserName == userName).ToList();
                    foreach (var item in items)
                    {
                        Clients.AllExcept(connnectId).onUserDisconnected(item.ConnectionId, item.UserName);
                    }
                    OnlineUsers.RemoveAll(x => x.UserName == userName);
                }

                //添加在线人员
                OnlineUsers.Add(new UserInfo
                {
                    ConnectionId = connnectId,
                    UserName = userName,
                    LastLoginTime = DateTime.Now
                });
            }

            // 所有客户端同步在线用户
            Clients.All.onConnected(connnectId, userName, OnlineUsers);
        }

        /// <summary>
        /// 发送私聊
        /// </summary>
        /// <param name="toUserId">接收方用户连接ID</param>
        /// <param name="message">内容</param>
        public void SendPrivateMessage(string toUserName, string message)
        {
            var fromConnectionId = Context.ConnectionId;

            var toUser = OnlineUsers.FirstOrDefault(x => x.UserName == toUserName);
            var fromUser = OnlineUsers.FirstOrDefault(x => x.ConnectionId == fromConnectionId);

            if (toUser != null )
            {
                Clients.Client(toUser.ConnectionId).receivePrivateMessage(fromUser.UserName, message);
            }
            else
            {
                //表示对方不在线
                Clients.Caller.absentSubscriber();
            }
        }

        public void Send(string name, string message)
        {
            //Clients.All { get; } // 代表所有客户端
            //Clients.AllExcept(params string[] excludeConnectionIds); // 除了参数中的所有客户端
            //Clients.Client(string connectionId); // 特定的客户端，这个方法也就是我们实现端对端聊天的关键
            //Clients.Clients(IList<string> connectionIds); // 参数中的客户端
            //Clients.Group(string groupName, params string[] excludeConnectionIds); // 指定客户端组，这个也是实现群聊的关键所在
            //Clients.Groups(IList<string> groupNames, params string[] excludeConnectionIds);参数中的客户端组
            //Clients.User(string userId);  // 特定的用户
            //Clients.Users(IList<string> userIds); // 参数中的用户

            Console.WriteLine("ConnectionId:{0}, InvokeMethod:{1}", Context.ConnectionId, "Send");
            Clients.All.addMessage(name, message);
        }

        /// <summary>
        /// 连线时调用
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            Console.WriteLine("客户端连接，连接ID是:{0},当前在线人数为{1}", Context.ConnectionId, OnlineUsers.Count);
            return base.OnConnected();
        }


        /// <summary>
        /// 断线时调用
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            var user = OnlineUsers.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);

            // 判断用户是否存在,存在则删除
            if (user == null)
            {
                return base.OnDisconnected(stopCalled);
            }

            Clients.All.onUserDisconnected(user.ConnectionId, user.UserName);   //调用客户端用户离线通知
            // 删除用户
            OnlineUsers.Remove(user);
            Console.WriteLine("客户端断线，连接ID是:{0},当前在线人数为{1}", Context.ConnectionId, OnlineUsers.Count);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            //Console.WriteLine("客户端重新连接，连接ID是:{0},当前在线人数为{1}", Context.ConnectionId, OnlineUsers.Count);
            return base.OnReconnected();
        }
    }
}
