using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapyCache
{
    public interface IClientCacheConfigColumn
    {
        string 表名 { get; set; }
        string 标准字段 { get; set; }
        string 标准字段名称 { get; set; }
        string 数据类型 { get; set; }
        bool 主键 { get; set; }
    }
}
