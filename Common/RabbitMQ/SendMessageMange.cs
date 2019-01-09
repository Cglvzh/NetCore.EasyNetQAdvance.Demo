using EasyNetQ;
using EasyNetQ.Topology;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.RabbitMQ
{
    internal class SendMessageMange : ISend
    {
        public async Task SendMsgAsync(PushMsg pushMsg, IBus bus)
        {
            //一对一推送
            var message = new Message<object>(pushMsg.sendMsg);
            IExchange ex = null;
            //判断推送模式
            if (pushMsg.sendEnum == SendEnum.推送模式)
            {
                ex = bus.Advanced.ExchangeDeclare(pushMsg.exchangeName, ExchangeType.Direct);
            }
            if (pushMsg.sendEnum == SendEnum.订阅模式)
            {
                //广播订阅模式
                ex = bus.Advanced.ExchangeDeclare(pushMsg.exchangeName, ExchangeType.Fanout);
            }
            if (pushMsg.sendEnum == SendEnum.主题路由模式)
            {
                //主题路由模式
                ex = bus.Advanced.ExchangeDeclare(pushMsg.exchangeName, ExchangeType.Topic);
            }
            await bus.Advanced.PublishAsync(ex, pushMsg.routeName, false, message)
            .ContinueWith(task =>
            {
                if (!task.IsCompleted && task.IsFaulted)//消息投递失败
                    {
                        //记录投递失败的消息信息

                    }
            });


        }

        public void SendMsg(PushMsg pushMsg, IBus bus)
        {

            //一对一推送

            var message = new Message<object>(pushMsg.sendMsg);
            IExchange ex = null;
            //判断推送模式
            if (pushMsg.sendEnum == SendEnum.推送模式)
            {
                ex = bus.Advanced.ExchangeDeclare(pushMsg.exchangeName, ExchangeType.Direct);
            }
            if (pushMsg.sendEnum == SendEnum.订阅模式)
            {
                //广播订阅模式
                ex = bus.Advanced.ExchangeDeclare(pushMsg.exchangeName, ExchangeType.Fanout);
            }
            if (pushMsg.sendEnum == SendEnum.主题路由模式)
            {
                //主题路由模式
                ex = bus.Advanced.ExchangeDeclare(pushMsg.exchangeName, ExchangeType.Topic);
            }
            bus.Advanced.Publish(ex, pushMsg.routeName, false, message);

        }

    }
}
