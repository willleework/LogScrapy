using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapyCache
{
    public interface IScrapyCacheTable
    {
        /// <summary>
        /// 缓存表初始化
        /// </summary>
        void Init();
    }
}
