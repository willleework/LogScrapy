using Config.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Interface
{
    /// <summary>
    /// 应用程序参数配置管理
    /// </summary>
    public interface IAppConfigManage: IConfigBase
    {
        /// <summary>
        /// 用户软件配置
        /// </summary>
        UserAppConfig UserConfig { get; }
        /// <summary>
        /// 缓存日志配置
        /// </summary>
        CacheLogConfig CacheLogConfig { get; }
    }
}
