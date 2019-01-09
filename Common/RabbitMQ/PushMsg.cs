using System;
using System.Collections.Generic;
using System.Text;

namespace Common.RabbitMQ
{
    public enum SendEnum
    {
        订阅模式 = 1,
        推送模式 = 2,
        主题路由模式 = 3
    }
    public class PushMsg
    {
        /// <summary>
        /// 发送JSON
        /// </summary>
        public object sendMsg { get; set; }

        /// <summary>
        /// 消息推送的模式
        /// 现在支持：订阅模式,推送模式,主题路由模式
        /// </summary>
        public SendEnum sendEnum { get; set; }

        /// <summary>
        /// 管道名称
        /// </summary>
        public string exchangeName { get; set; }

        /// <summary>
        /// 路由名称
        /// </summary>
        public string routeName { get; set; } = "";
    }
}
