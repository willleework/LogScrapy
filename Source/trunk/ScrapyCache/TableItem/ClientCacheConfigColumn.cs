using Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapyCache
{
    /// <summary>
    /// 客户端缓存配置列信息
    /// </summary>
    public class ClientCacheConfigColumn : CacheItem, IClientCacheConfigColumn
    {
        public string 表名 { get; set; }
        public string 标准字段 { get; set; }
        public string 标准字段名称 { get; set; }
        public string 数据类型 { get; set; }
        public bool 主键 { get; set; }

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <param name="des"></param>
        public override void Copy(ICacheItem des)
        {
            base.Copy(des);
            ClientCacheConfigColumn model = des as ClientCacheConfigColumn;
            model.表名 = this.表名;
            model.标准字段 = this.标准字段;
            model.标准字段名称 = this.标准字段名称;
            model.数据类型 = this.数据类型;
            model.主键 = this.主键;
        }

        /// <summary>
        /// 类信息
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[ClientCacheConfigColumn]表名:{0}, 标准字段:{1}", this.表名, this.标准字段);
        }
    }
}
