using Common.RabbitMQ;
using Message.Client.MessageManage.Consume;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Message.Client
{
    public class MessageManService
    {
        public static void Subsribe()
        {
            Task.Run(() =>
            {
                //概念  一个管道下面可以绑定多个队列。
                //发送消息 是指将消息发送到管道中，然后由rabbitmq根据发送规则在将消息具体的转发到对应到管道下面的队列中
                //消费消息 是指消费者（即服务）从管道下面的队列中获取消息
                //同一个队列 可以有多个消费者（即不同的服务，都可以连接到同一个队列去获取消息）
                //但注意 当一个队列有多个消费者的时候，消息会被依次分发到不同的消费者中。比如第一条消息给第一个消费者，第二条消息给第二个消费者(框架内部有一个公平分发的机制)


                //推送模式时 需指定管道名称和路由值
                //队列名称可自己指定
                //注意 ，管道名称和路由名称一定要和发送方的管道名称和路由名称一致
                //无论这个管道下面挂靠有多少个队列，只有路由名称和此处指定的路由名称完全一致的队列，才会收到这条消息。
                var dirarg = new MesArgs()
                {
                    sendEnum = SendEnum.推送模式,
                    exchangeName = "message.directdemo",
                    rabbitQueeName = "meesage.directmessagequene",
                    routeName = "routekey"
                };
                RabbitMQManage.Subscribe<DirectMessageConsume>(dirarg);

                //订阅模式时需指定管道名称，并且管道名称要和发送方管道名称一致
                //队列名称可自己指定
                //所有这个管道下面的队列，都将收到该条消息
                var fanoutrg = new MesArgs()
                {
                    sendEnum = SendEnum.订阅模式,
                    exchangeName = "message.fanoutdemo",
                    rabbitQueeName = "meesage.fanoutmessagequene"
                };
                RabbitMQManage.Subscribe<FanoutMessageConsume>(fanoutrg);

                //路由模式时需指定管道名称，路由关键字并且管道名称，路由关键字要和发送方的一致
                //队列名称可自己指定
                //消息将被发送到管道下面的能匹配路由关键字的队列中
                //也就是说 路由模式时，有多少队列能收到消息，取决于该队列的路由关键字是否匹配，只要匹配就能收到消息
                //符号“#”匹配一个或多个词，符号“*”匹配不多不少一个词
                var topicrg = new MesArgs()
                {
                    sendEnum = SendEnum.主题路由模式,
                    exchangeName = "message.topicdemo",
                    rabbitQueeName = "message.topicmessagequene",
                    routeName = "#.log.#"
                };

                RabbitMQManage.Subscribe<TopicMessageConsume>(topicrg);
            });
        }
    }
}
