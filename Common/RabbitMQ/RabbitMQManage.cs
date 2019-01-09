using Common.Config;
using EasyNetQ;
using EasyNetQ.Topology;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.RabbitMQ
{
    public class RabbitMQManage
    {

        private volatile static IBus bus = null;

        private static readonly object lockHelper = new object();

        /// <summary>
        /// 创建服务总线
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IBus CreateEventBus()
        {
            var config = SystemJsonConfigManage.GetInstance().AppSettings["MeessageService"];
            if (string.IsNullOrEmpty(config))
                throw new Exception("消息地址未配置");
            if (bus == null && !string.IsNullOrEmpty(config))
            {
                lock (lockHelper)
                {
                    if (bus == null)
                        bus = RabbitHutch.CreateBus(config);
                }
            }
            return bus;
        }
        /// <summary>
        /// 释放服务总线
        /// </summary>
        public static void DisposeBus()
        {
            bus?.Dispose();
        }
        /// <summary>
        ///  消息同步投递
        /// </summary>
        /// <param name="listMsg"></param>
        /// <returns></returns>
        public static bool PushMessage(PushMsg pushMsg)
        {
            bool b = true;
            try
            {
                if (bus == null)
                    CreateEventBus();
                new SendMessageMange().SendMsg(pushMsg, bus);
                b = true;
            }
            catch (Exception ex)
            {
               
                b = false;
            }
            return b;
        }
        /// <summary>
        /// 消息异步投递
        /// </summary>
        /// <param name="listMsg"></param>
        public static async Task PushMessageAsync(PushMsg pushMsg)
        {
            try
            {
                if (bus == null)
                    CreateEventBus();
               await new SendMessageMange().SendMsgAsync(pushMsg, bus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 消息订阅
        /// </summary>
        public static void Subscribe<TConsum>(MesArgs args)
            where TConsum : IMessageConsume, new()
        {
            if (bus == null)
                CreateEventBus();
            if (string.IsNullOrEmpty(args.exchangeName))
                return;
            Expression<Action<TConsum>> methodCall;
            IExchange ex = null;
            //判断推送模式
            if (args.sendEnum == SendEnum.推送模式)
            {
                ex = bus.Advanced.ExchangeDeclare(args.exchangeName, ExchangeType.Direct);

            }
            if (args.sendEnum == SendEnum.订阅模式)
            {
                //广播订阅模式
                ex = bus.Advanced.ExchangeDeclare(args.exchangeName, ExchangeType.Fanout);
            }
            if (args.sendEnum == SendEnum.主题路由模式)
            {
                //主题路由模式
                ex = bus.Advanced.ExchangeDeclare(args.exchangeName, ExchangeType.Topic);
            }
            IQueue qu;
            if (string.IsNullOrEmpty(args.rabbitQueeName))
            {
                qu = bus.Advanced.QueueDeclare();
            }
            else
                qu = bus.Advanced.QueueDeclare(args.rabbitQueeName);
            bus.Advanced.Bind(ex, qu, args.routeName);
            bus.Advanced.Consume(qu, (body, properties, info) => Task.Factory.StartNew(() =>
            {
                try
                {
                    lock (lockHelper)
                    {

                        var message = Encoding.UTF8.GetString(body);
                        //处理消息
                        methodCall = job => job.Consume(message);
                        methodCall.Compile()(new TConsum());
                    }

                }
                catch (Exception e)
                {
                   
                    throw e;

                }


            }));
        }
    }
}
