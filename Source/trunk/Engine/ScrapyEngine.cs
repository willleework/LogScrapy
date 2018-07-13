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
            builder.RegisterType<CachePool>().As<ICache>().SingleInstance();
            builder.RegisterType<LogUtility.LogUtility>().As<ILogUtility>().SingleInstance();

            context = builder.Build();
        }

        /// <summary>
        /// 启动引擎
        /// </summary>
        public void BootEngine(EngineParam engineParam)
        {
            //配置服务初始化
            IAppConfigManage appConfig = Get<IAppConfigManage>();
            AppConfigParam appConfigParam = new AppConfigParam()
            {
                UserConfigPath = engineParam.AppConfigPath
            };
            appConfig.LoadConfigs(appConfigParam);
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

    }

    /// <summary>
    /// 引擎参数
    /// </summary>
    public class EngineParam
    {
        public string AppConfigPath { get; set; }
    }
}
