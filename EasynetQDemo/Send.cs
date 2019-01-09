using Common.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasynetQDemo
{
    public class Send
    {


        /// <summary>
        /// 发送消息
        /// </summary>
        public static void SendMessage()
        {
            //需要注意一点儿，如果发送的时候，在该管道下找不到相匹配的队列框架将默认丢弃该消息




            //推送模式
            //推送模式下，需指定管道名称和路由键值名称
            //消息只会被发送到指定的队列中去
            var directdto = new PushMsg()
            {
                sendMsg = new TestDto()
                {
                    Var1 = "这是推送模式"
                },
                exchangeName = "message.directdemo",
                routeName = "routekey",
                sendEnum = SendEnum.推送模式
            };
            //同步发送 ，返回true或fasle true 发送成功，消息已存储到Rabbitmq中，false表示发送失败
            var b = RabbitMQManage.PushMessage(directdto);
            //异步发送，如果失败，失败的消息会被写入数据库，会有后台线程轮询数据库进行重新发送
            //RabbitMQManage.PushMessageAsync(directdto);


            //订阅模式
            //订阅模式只需要指定管道名称
            //消息会被发送到该管道下的所有队列中
            var fanoutdto = new PushMsg()
            {
                sendMsg = new TestDto()
                {
                    Var1 = "这是订阅模式"
                },
                exchangeName = "message.fanoutdemo",
                sendEnum = SendEnum.订阅模式
            };
            //同步发送 
            var fb = RabbitMQManage.PushMessage(fanoutdto);
            //异步发送
            // RabbitMQManage.PushMessageAsync(fanoutdto);

            //主题路由模式
            //路由模式下需指定 管道名称和路由值
            //消息会被发送到该管道下，和路由值匹配的队列中去
            var routedto = new PushMsg()
            {
                sendMsg = new TestDto()
                {
                    Var1 = "这是主题路由模式1",
                },
                exchangeName = "message.topicdemo",
                routeName = "a.log",
                sendEnum = SendEnum.主题路由模式

            };
            var routedto2 = new PushMsg()
            {
                sendMsg = new TestDto()
                {
                    Var1 = "这是主题路由模式2",
                },
                exchangeName = "message.topicdemo",
                routeName = "a.log.a.b",
                sendEnum = SendEnum.主题路由模式

            };
            //同步发送 
            var rb = RabbitMQManage.PushMessage(routedto);
            var rb2 = RabbitMQManage.PushMessage(routedto2);
            //异步发送
            //RabbitMQManage.PushMessageAsync(routedto);
        }
    }
}
