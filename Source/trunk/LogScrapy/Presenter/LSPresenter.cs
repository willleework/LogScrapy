using Cache;
using Config.Entity;
using Config.Interface;
using Engine;
using Log;
using ScrapyCache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
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
        /// 客户端配置文件路径
        /// </summary>
        string appConfigFile = string.Format(@"{0}\Config\logscrapyconfig.xml", Directory.GetCurrentDirectory());
        string logConfigFile = string.Format(@"{0}\Config\ScrapyLog.config", Directory.GetCurrentDirectory());

        public string AppConfigFile { get => appConfigFile; }
        public string LogConfigFile { get => logConfigFile; }

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
        public LSPresenter(MainWindow view)
        {
            ScrapyEngine engine = new ScrapyEngine();
            EngineParam param = new EngineParam()
            {
                AppConfigPath = AppConfigFile,
                LogConfigPath = LogConfigFile,
            };
            //耗时操作
            engine.BootEngine(param);
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
            CachePattern pattern = Engine.Get<IAppConfigManage>().UserConfig.缓存匹配策略列表.Find(p => p.CacheType!=null?p.CacheType.Equals(cacheType):false);
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
            try
            {
                foreach (ICacheItem row in Engine.Get<ICachePool>().Get<ClientCacheConfigTable>().Get())
                {
                    ClientCacheConfig config = row as ClientCacheConfig;
                    types.Add(config.英文名);
                }
            }
            catch (Exception ex)
            {
                Engine.Get<ILogContext>().LogForCommon.LogError(string.Format("获取缓存表信息失败：{0};StackTrace:{1}", ex.Message, ex.StackTrace));
            }
            return types;
        }

        /// <summary>
        /// 获取缓存包含的列
        /// </summary>
        /// <returns></returns>
        public List<string> GetCacheColumn(string cacheName)
        {
            List<string> columns = new List<string>();
            try
            {
                List<ICacheItem> tables = Engine.Get<ICachePool>().Get<ClientCacheConfigTable>().Get(cacheName);
                if (tables.Count <= 0)
                {
                    return columns;
                }
                ClientCacheConfig table = tables[0] as ClientCacheConfig;
                foreach (ICacheItem row in Engine.Get<ICachePool>().Get<ClientCacheConfigColumnTable>().Get(table.表名, ClientCacheConfigColumnTable.IndexByTable))
                {
                    ClientCacheConfigColumn column = row as ClientCacheConfigColumn;
                    columns.Add(column.标准字段);
                }
            }
            catch (Exception ex)
            {
                Engine.Get<ILogContext>().LogForCommon.LogError(string.Format("获取缓存表列信息失败：{0};StackTrace:{1}", ex.Message, ex.StackTrace));
            }
            return columns;
        }

        /// <summary>
        /// 根据匹配模式获取动态类型日志信息列表
        /// </summary>
        /// <param name="regexs"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public List<dynamic> GetLogInfoRowsByRegFilters(List<Regex> regexs, string cacheType)
        {
            List<ICacheItem> logs = new List<ICacheItem>();
            List<LogInfoRow> infoRows = new List<LogInfoRow>();
            List<dynamic> results = new List<dynamic>();
            try
            {
                logs = Engine.Get<ICachePool>().Get<LogInfoRowTable>().Get();
                List<string> columns = GetCacheColumn(cacheType);
                //regexs.Clear();
                logs.AsParallel().ForAll(p =>
                {
                    LogInfoRow row = p as LogInfoRow;
                    if (regexs.Count <= 0 || CheckPattern(regexs, row.DataInfo))
                    {
                        dynamic obj = new ExpandoObject();
                        ((IDictionary<string, object>)obj).Add("TimeStamp", row.TimeStamp);
                        ((IDictionary<string, object>)obj).Add("Level", row.Level);
                        foreach (string column in columns)
                        {
                            string value = string.Empty;
                            string reg = string.Format("{0}={1}", column, @"\w+\,");
                            Regex r = new Regex(@reg);
                            MatchCollection m = r.Matches(row.DataInfo);
                            if (m.Count > 0)
                            {
                                value = m[0].Value.Remove(0, column.Length + 1).TrimEnd(',');
                            }
                            ((IDictionary<string, object>)obj).Add(column, value);
                        }
                        results.Add(obj);
                    }
                });
            }
            catch (Exception ex)
            {
                Engine.Get<ILogContext>().LogForCommon.LogError(string.Format("筛选日志信息失败：{0};StackTrace:{1}", ex.Message, ex.StackTrace));
            }
            return results;
        }
    }
}
