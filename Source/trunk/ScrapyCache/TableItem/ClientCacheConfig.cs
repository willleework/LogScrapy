using Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapyCache
{
    /// <summary>
    /// 客户端缓存信息配置
    /// </summary>
    public class ClientCacheConfig : CacheItem, IClientCacheConfig
    {
        public string 英文名 { get; set; }
        public string 表名 { get; set; }
        public string 更新消息主题 { get; set; }
        public int 端口号 { get; set; }
        public string 后台组 { get; set; }
        public string 数据库表名 { get; set; }

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <param name="des"></param>
        public override void Copy(ICacheItem des)
        {
            base.Copy(des);
            ClientCacheConfig model = des as ClientCacheConfig;
            model.英文名 = this.英文名;
            model.表名 = this.表名;
            model.更新消息主题 = this.更新消息主题;
            model.端口号 = this.端口号;
            model.后台组 = this.后台组;
            model.数据库表名 = this.数据库表名;
        }

        /// <summary>
        /// 类信息
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[ClientCacheConfig]英文名称:{0}, 表名:{1}", this.英文名, this.表名);
        }
    }
}
