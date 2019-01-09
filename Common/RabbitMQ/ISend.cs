using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.RabbitMQ
{
    internal interface ISend
    {
        Task SendMsgAsync(PushMsg pushMsg, IBus bus);

        void SendMsg(PushMsg pushMsg, IBus bus);
    }
}
