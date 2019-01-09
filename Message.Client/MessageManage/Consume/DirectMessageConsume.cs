using Common.RabbitMQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Message.Client.MessageManage.Consume
{
    public class DirectMessageConsume : IMessageConsume
    {
        //消息的处理方法中最好不要进行try catch操作
        //如果发送异常，框架底层会自动捕获异常并将消息放入错误队列，然后尝试重现发送
        //如果在Consume方法体中捕获了异常，框架底层会默认消息处理成功不会在重新发送
        //消息的幂等性需业务方自行处理,也就是说同一条消息可能会接收到两次
        //（比如说第一次正在处理消息的时候服务挂掉，服务重启后这条消息又会重新推送过来）
        public void Consume(string message)
        {
            var dto = JsonConvert.DeserializeObject<TestDto>(message);
            Console.WriteLine(dto.Var1 + ";" + dto.Var2 + ";" + dto.Var3);
        }
    }
}
