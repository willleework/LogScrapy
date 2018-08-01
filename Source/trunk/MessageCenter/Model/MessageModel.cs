using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogScrapy
{
    /// <summary>
    /// 消息
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 消息接收时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool Read { get; set; }
    }
}
