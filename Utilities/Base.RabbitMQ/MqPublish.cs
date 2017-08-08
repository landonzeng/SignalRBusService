using RabbitMQ.Client;
using RabbitMQ.Client.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class MqPublish
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="exchange">路由名</param>
        /// <param name="type">路由类型</param>
        /// <param name="queue">队列名</param>
        /// <param name="routingKey"></param>
        /// <param name="message"></param>
        public static int AddQueue(string exchange, string type, string queue, string routingKey, string message)
        {
           return Send(exchange, type, queue, routingKey, message);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="exchange">要发出的交换机名字</param>
        /// <param name="routingKey">路由关键字</param>
        /// <param name="message">消息主体</param>
        private static int Send(string exchange, string type, string queue, string routingKey, string message)
        {
            using (var channel = MqHelper.GetNewConnection().CreateModel())
            {
                try
                {
                    var body = Encoding.UTF8.GetBytes(message);
                    //交换机持久化
                    channel.ExchangeDeclare(exchange, type, true);
                    //队列持久化
                    channel.QueueDeclare(queue, true, false, false, null);
                    channel.QueueBind(queue, exchange, routingKey, null);
                    channel.BasicAcks += channel_BasicAcks;
                    channel.ConfirmSelect();

                    var props = channel.CreateBasicProperties();

                    ////数据持久化
                    //props.DeliveryMode = 2;

                    //数据强持久化
                    props.SetPersistent(true);

                    channel.BasicPublish(exchange, routingKey, props, body);
                    channel.WaitForConfirmsOrDie();
                    return 1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private static void channel_BasicAcks(object sender, RabbitMQ.Client.Events.BasicAckEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}

