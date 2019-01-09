using System;
using System.Collections.Generic;
using System.Text;

namespace Common.RabbitMQ
{
    public interface IMessageConsume
    {
        void Consume(string message);
    }
}
