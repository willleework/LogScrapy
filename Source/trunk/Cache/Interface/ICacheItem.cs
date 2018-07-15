using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    /// <summary>
    /// 缓存实体
    /// </summary>
    public interface ICacheItem
    {
        /// <summary>
        /// 拷贝数据到指定引用
        /// </summary>
        /// <param name="item"></param>
        void Copy(ICacheItem des);
    }
}
