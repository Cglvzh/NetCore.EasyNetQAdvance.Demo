using Common.RabbitMQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Message.Client.MessageManage.Consume
{
    public class TopicMessageConsume : IMessageConsume
    {
        public void Consume(string message)
        {
            var dto = JsonConvert.DeserializeObject<TestDto>(message);
            Console.WriteLine(dto.Var1 + ";" + dto.Var2 + ";" + dto.Var3);
        }
    }
}
