using Config.Entity;
using Config.Interface;
using Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogScrapy
{
    /// <summary>
    /// 事物层
    /// </summary>
    public class LSPresenter
    {
        /// <summary>
        /// 框架引擎
        /// </summary>
        public ScrapyEngine Engine { get; set; }

        /// <summary>
        /// 视图层
        /// </summary>
        MainWindow View { get; set; }

        /// <summary>
        /// 事物层
        /// </summary>
        /// <param name="view"></param>
        /// <param name="engine"></param>
        public LSPresenter(MainWindow view, ScrapyEngine engine)
        {
            Engine = engine;
            View = view;
        }

        /// <summary>
        /// 获取缓存过滤模式
        /// </summary>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public string GetCachePatternByType(string cacheType)
        {
            CachePattern pattern = Engine.Get<IAppConfigManage>().UserConfig.缓存匹配策略列表.Find(p => p.CacheType.Equals(cacheType));
            if (pattern != null)
            {
                return pattern.Pattern;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 检查是否符合模式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="regexs"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool CheckPattern(List<Regex> regexs, string str)
        {
            foreach (Regex reg in regexs)
            {
                if (!reg.IsMatch(str))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取缓存类型
        /// </summary>
        /// <returns></returns>
        public List<string> GetCacheType()
        {
            List<string> types = new List<string>();
            foreach (DataRow row in Engine.Get<IAppConfigManage>().CacheLogConfig.衍生品缓存表.Rows)
            {
                types.Add(row["英文名"].ToString());
            }
            return types;
        }
    }
}
