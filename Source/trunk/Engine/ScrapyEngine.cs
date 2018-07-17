using Autofac;
using Cache;
using Config;
using Config.Entity;
using Config.Interface;
using Log;
using LogDecode;
using ScrapyCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// 系统引擎
    /// </summary>
    public class ScrapyEngine: IDisposable
    {
        /// <summary>
        /// 容器上下文
        /// </summary>
        IContainer context;

        #region 初始化及注销
        /// <summary>
        /// 系统引擎
        /// </summary>
        public ScrapyEngine()
        {
            InitEngine();
        }

        /// <summary>
        /// 初始化引擎
        /// </summary>
        private void InitEngine()
        {
            //注册服务
            var builder = new ContainerBuilder();
            builder.RegisterType<AppConfigManage>().As<IAppConfigManage>().SingleInstance();
            builder.RegisterType<ScrapyCachePool>().As<ICachePool>().SingleInstance();
            builder.RegisterType<LogDecodeUtility>().As<ILogUtility>().SingleInstance();
            builder.RegisterType<LogContext>().As<ILogContext>().SingleInstance();

            context = builder.Build();
        }

        /// <summary>
        /// 启动引擎
        /// </summary>
        public void BootEngine(EngineParam engineParam)
        {
            #region 配置服务初始化
            IAppConfigManage appConfig = Get<IAppConfigManage>();
            AppConfigParam appConfigParam = new AppConfigParam()
            {
                UserConfigPath = engineParam.AppConfigPath
            };
            appConfig.LoadConfigs(appConfigParam);
            #endregion

            #region 日志服务初始化
            ILogContext log = Get<ILogContext>();
            log.Init(@engineParam.LogConfigPath);
            #endregion

            #region 缓存服务初始化
            ScrapyCachePool cache = Get<ICachePool>() as ScrapyCachePool;
            cache.Init();
            ScrapyCachePool.LogDebugEvent += ScrapyCacheLogForDebug;
            ScrapyCachePool.LogInfoEvent += ScrapyCacheLogForInfo;
            ScrapyCachePool.LogErrorEvent += ScrapyCacheLogForError;
            //客户端缓存配置表
            cache.Get<ClientCacheConfigTable>().LoadDatas(appConfig.CacheLogConfig.基础组缓存表);
            cache.Get<ClientCacheConfigTable>().LoadDatas(appConfig.CacheLogConfig.衍生品缓存表);
            cache.Get<ClientCacheConfigTable>().LoadDatas(appConfig.CacheLogConfig.权益缓存表);
            cache.Get<ClientCacheConfigTable>().LoadDatas(appConfig.CacheLogConfig.固收缓存表);
            //客户端缓存列配置表
            cache.Get<ClientCacheConfigColumnTable>().LoadDatas(appConfig.CacheLogConfig.基础组缓存字段表);
            cache.Get<ClientCacheConfigColumnTable>().LoadDatas(appConfig.CacheLogConfig.衍生品缓存字段表);
            cache.Get<ClientCacheConfigColumnTable>().LoadDatas(appConfig.CacheLogConfig.权益缓存字段表);
            cache.Get<ClientCacheConfigColumnTable>().LoadDatas(appConfig.CacheLogConfig.固收缓存字段表);
            #endregion
        }

        /// <summary>
        /// 注销引擎
        /// </summary>
        public void Dispose()
        {
        } 
        #endregion

        #region 服务获取
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T">服务接口类型</typeparam>
        /// <returns></returns>
        public T Get<T>()
        {
            return context.Resolve<T>();
        }
        #endregion

        #region 缓存日志函数
        /// <summary>
        /// Debug日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="log"></param>
        private void ScrapyCacheLogForDebug(string log)
        {
            Get<ILogContext>().LogForCache.LogDebug(log);
        }
        /// <summary>
        /// Info日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="log"></param>
        private void ScrapyCacheLogForInfo(string log)
        {
            Get<ILogContext>().LogForCache.LogInfo(log);
        }
        /// <summary>
        /// Error日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="log"></param>
        private void ScrapyCacheLogForError(string log)
        {
            Get<ILogContext>().LogForCache.LogError(log);
        }
        #endregion
    }

    /// <summary>
    /// 引擎参数
    /// </summary>
    public class EngineParam
    {
        /// <summary>
        /// 应用程序配置文件路径
        /// </summary>
        public string AppConfigPath { get; set; }
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string LogConfigPath { get; set; }
    }
}
