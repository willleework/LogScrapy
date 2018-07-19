using Cache;
using Config.Entity;
using Config.Interface;
using Engine;
using Log;
using LogDecode;
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
                    types.Add(config.表名);
                }
            }
            catch (Exception ex)
            {
                Engine.Get<ILogContext>().LogForCommon.LogError(string.Format("获取缓存表信息失败：{0};StackTrace:{1}", ex.Message, ex.StackTrace));
            }
            return types;
        }

        /// <summary>
        /// 获取缓存类型
        /// </summary>
        /// <returns></returns>
        public List<ClientCacheConfig> GetCacheTypes()
        {
            List<ClientCacheConfig> types = new List<ClientCacheConfig>();
            try
            {
                foreach (ICacheItem row in Engine.Get<ICachePool>().Get<ClientCacheConfigTable>().Get())
                {
                    ClientCacheConfig config = row as ClientCacheConfig;
                    types.Add(config);
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
        public List<ClientCacheConfigColumn> GetCacheColumn(string cacheName)
        {
            List<ClientCacheConfigColumn> columns = new List<ClientCacheConfigColumn>();
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
                    columns.Add(column);
                }
            }
            catch (Exception ex)
            {
                Engine.Get<ILogContext>().LogForCommon.LogError(string.Format("获取缓存表列信息失败：{0};StackTrace:{1}", ex.Message, ex.StackTrace));
            }
            return columns;
        }

        /// <summary>
        /// 通过列中文名称获取英文名
        /// </summary>
        /// <param name="chnName"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public string GetColumnEngNameByChnName(string chnName, string cacheType)
        {
            List<ICacheItem>columns = Engine.Get<ICachePool>().Get<ClientCacheConfigColumnTable>().Get(cacheType, ClientCacheConfigColumnTable.IndexByTable);
            if (columns.Count > 0)
            {
                ICacheItem item = columns.Find(p =>
                {
                    ClientCacheConfigColumn col = p as ClientCacheConfigColumn;
                    return col.标准字段名称.Equals(chnName);
                });
                if (item != null)
                {
                    return ((ClientCacheConfigColumn)item).标准字段;
                }
            }
            return string.Empty;
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
                List<ClientCacheConfigColumn> columns = GetCacheColumn(cacheType);
                //regexs.Clear();
                //logs.AsParallel().ForAll(p =>
                //{
                foreach (LogInfoRow p in logs)
                {
                    LogInfoRow row = p as LogInfoRow;
                    if (regexs.Count <= 0 || CheckPattern(regexs, row.DataInfo))
                    {
                        dynamic obj = new ExpandoObject();
                        ((IDictionary<string, object>)obj).Add("TimeStamp", row.TimeStamp);
                        ((IDictionary<string, object>)obj).Add("Level", row.Level);
                        foreach (ClientCacheConfigColumn column in columns)
                        {
                            string value = string.Empty;
                            string reg = string.Format("{0}={1}", column.标准字段, @"\w+\,");
                            Regex r = new Regex(@reg);
                            MatchCollection m = r.Matches(row.DataInfo);
                            if (m.Count > 0)
                            {
                                value = m[0].Value.Remove(0, column.标准字段.Length + 1).TrimEnd(',');
                            }
                            ((IDictionary<string, object>)obj).Add(column.标准字段, value);
                        }
                        results.Add(obj);
                    }
                }
                //});
            }
            catch (Exception ex)
            {
                Engine.Get<ILogContext>().LogForCommon.LogError(string.Format("筛选日志信息失败：{0};StackTrace:{1}", ex.Message, ex.StackTrace));
            }
            return results;
        }

        /// <summary>
        /// 根据匹配模式获取动态类型日志信息列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<dynamic> GetLogInfoRowsByRegFilters(dynamic param)
        {
            return GetLogInfoRowsByRegFilters(param.Regexs, param.CacheType);
        }

        /// <summary>
        /// 加载日志到缓存中
        /// </summary>
        /// <param name="path"></param>
        public void LoadLogDatasToCache(string path)
        {
            ILogUtility logUtility = Engine.Get<ILogUtility>();
            string logInfo = logUtility.ReadLogFile(@path);
            if (string.IsNullOrWhiteSpace(logInfo))
            {
                return;
            }
            List<LogEntityBase> logs = logUtility.DecodeLog(logInfo, Engine.Get<IAppConfigManage>().UserConfig.分行策略, Engine.Get<IAppConfigManage>().UserConfig.时间戳提取策略);
            if (logs == null || logs.Count <= 0)
            {
                return;
            }
            List<LogInfoRow> logRows = new List<LogInfoRow>();
            foreach (LogEntityBase log in logs)
            {
                logRows.Add(new LogInfoRow()
                {
                    Level = log.Level,
                    TimeStamp = log.TimeStamp,
                    DataInfo = log.DataInfo,
                });
            }
            Engine.Get<ICachePool>().Get<LogInfoRowTable>().LoadDatas(logRows);
        }
    }
}
