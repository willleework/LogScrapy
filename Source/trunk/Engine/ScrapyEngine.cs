using Autofac;
using Cache;
using Cache.Interface;
using Config.Entity;
using Config.Interface;
using LogUtility;
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
        IContainer context;

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
            var builder = new ContainerBuilder();
            builder.RegisterType<UserAppConfig>().As<IAppConfigManage>().SingleInstance();
            builder.RegisterType<CachePool>().As<ICache>().SingleInstance();
            builder.RegisterType<LogUtility.LogUtility>().As<ILogUtility>().SingleInstance();

            context = builder.Build();
        }

        /// <summary>
        /// 启动引擎
        /// </summary>
        public void BootEngine(EngineParam engineParam)
        {
            IAppConfigManage appConfig = Get<IAppConfigManage>();

            appConfig.LoadConfigs(engineParam.AppConfigPath);
        }

        #region 服务获取
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>()
        {
            return context.Resolve<T>();
        } 
        #endregion

        public void Dispose()
        {
        }
    }

    /// <summary>
    /// 引擎参数
    /// </summary>
    public class EngineParam
    {
        public string AppConfigPath { get; set; }
    }
}
