using Config.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Config.Entity
{
    /// <summary>
    /// 用户配置文件
    /// </summary>
    public class AppConfig : IAppConfigManage
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
        #endregion

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="path"></param>
        public void LoadConfigs(string path)
        {
            _appConfig = new UserAppConfig();
            _appConfig.LoadConfigs(path);
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="path"></param>
        public void SaveConfigs(string path)
        {
        }
    }
}
