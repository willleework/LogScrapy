using Config.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Entity
{
    /// <summary>
    /// 应用程序配置
    /// </summary>
    public class AppConfigParam: IConfigParam
    {
        /// <summary>
        /// 用户应用程序配置路径
        /// </summary>
        public string UserConfigPath { get; set; }
    }

    public class UserAppConfigParam : IConfigParam
    {
        public string ConfigPath { get; set; }
    }

    public class CacheLogConfigParam : IConfigParam
    {
        public string 公共缓存配置目录 { get; set; }
        public string 衍生品缓存配置目录 { get; set; }
        public string 权益缓存配置目录 { get; set; }
        public string 固收缓存配置目录 { get; set; }
    }
}
