using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class MqHelper
    {
        private static IConnection _connection;

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <returns></returns>
        public static IConnection GetConnection()
        {
            if (_connection != null) return _connection;
            _connection = GetNewConnection();
            return _connection;
        }

        public static IConnection GetNewConnection()
        {

            //从工厂中拿到实例 本地host、用户admin
            var factory = new ConnectionFactory()
            {
                HostName = ConfigurationManager.AppSettings["MQHostName"].ToString(),
                UserName = ConfigurationManager.AppSettings["MQUserName"].ToString(),
                Password = ConfigurationManager.AppSettings["MQPassword"].ToString()
            };

            _connection = factory.CreateConnection();
            return _connection;
        }
    }
}
