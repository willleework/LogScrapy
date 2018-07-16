using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public class CacheItem : ICacheItem
    {
        /// <summary>
        /// 拷贝数据到指定引用
        /// </summary>
        /// <param name="des"></param>
        public virtual void Copy(ICacheItem des)
        {
        }
    }
}
