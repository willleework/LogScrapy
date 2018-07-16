using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapyCache
{
    public interface IClientCacheConfig
    {
        string 表名 { get; set; }
        string 英文名 { get; set; }
        string 更新消息主题 { get; set; }
        int 端口号 { get; set; }
        string 后台组 { get; set; }
        string 数据库表名 { get; set; }
    }
}
