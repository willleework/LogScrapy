﻿using Config.Entity;
using Config.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Config
{
    /// <summary>
    /// 用户配置文件
    /// </summary>
    public class AppConfigManage : IAppConfigManage
    {
        #region 属性
        private UserAppConfig _appConfig;
        /// <summary>
        /// 用户配置
        /// </summary>
        public UserAppConfig UserConfig => _appConfig;

        private CacheLogConfig _cacheLogConfig;
        /// <summary>
        /// 缓存日志配置
        /// </summary>
        public CacheLogConfig CacheLogConfig => _cacheLogConfig;
        
        /// <summary>
        /// Debug级别日志 
        /// </summary>
        public static Action<string> LogDebugEvent;
        /// <summary>
        /// Info级别日志
        /// </summary>
        public static Action<string> LogInfoEvent;
        /// <summary>
        /// Error级别日志
        /// </summary>
        public static Action<string> LogErrorEvent;
        #endregion

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="path"></param>
        public void LoadConfigs(IConfigParam param)
        {
            AppConfigParam configParam = param as AppConfigParam;

            //用户应用程序配置加载
            _appConfig = new UserAppConfig();
            _appConfig.LoadConfigs(new UserAppConfigParam() { ConfigPath = configParam.UserConfigPath});

            //缓存配置加载
            _cacheLogConfig = new CacheLogConfig();
            _cacheLogConfig.LoadConfigs(new CacheLogConfigParam()
            {
                公共缓存配置目录 = _appConfig.公共缓存配置目录,
                衍生品缓存配置目录 = _appConfig.衍生品缓存配置目录,
                权益缓存配置目录 = _appConfig.权益缓存配置目录,
                固收缓存配置目录 = _appConfig.固收缓存配置目录
            });
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="path"></param>
        public void SaveConfigs(IConfigParam param)
        {
        }

        /// <summary>
        /// Debug级别日志
        /// </summary>
        /// <param name="message"></param>
        internal static void LogDebug(string message)
        {
            LogDebugEvent?.Invoke(message);
        }

        /// <summary>
        /// Info级别日志
        /// </summary>
        /// <param name="message"></param>
        internal static void LogInfo(string message)
        {
            LogInfoEvent?.Invoke(message);
        }

        /// <summary>
        /// Error级别日志
        /// </summary>
        /// <param name="message"></param>
        internal static void LogError(string message)
        {
            LogErrorEvent?.Invoke(message);
        }
    }
}
