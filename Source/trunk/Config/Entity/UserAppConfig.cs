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
    /// 用户配置
    /// </summary>
    public class UserAppConfig: IConfigBase
    {
        #region 属性
        public string 公共缓存配置目录 { get; set; }
        public string 衍生品缓存配置目录 { get; set; }
        public string 权益缓存配置目录 { get; set; }
        public string 固收缓存配置目录 { get; set; }

        public string 分行策略 { get; set; }
        public string 时间戳提取策略 { get; set; }
        public List<CachePattern> 缓存匹配策略列表 { get; set; }

        #endregion

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="path"></param>
        public void LoadConfigs(IConfigParam param)
        {
            try
            {
                UserAppConfigParam appConfig = param as UserAppConfigParam;
                if (File.Exists(appConfig.ConfigPath))
                {
                    UserAppConfig config = SerializeHelper.Load(typeof(UserAppConfig), appConfig.ConfigPath) as UserAppConfig;
                    if (config != null)
                    {
                        this.公共缓存配置目录 = config.公共缓存配置目录;
                        this.衍生品缓存配置目录 = config.衍生品缓存配置目录;
                        this.权益缓存配置目录 = config.权益缓存配置目录;
                        this.固收缓存配置目录 = config.固收缓存配置目录;

                        this.分行策略 = config.分行策略;
                        this.时间戳提取策略 = config.时间戳提取策略;
                        this.缓存匹配策略列表 = new List<CachePattern>(config.缓存匹配策略列表);
                    }
                    else
                    {
                        Init();
                    }
                }
                else
                {
                    Init();
                }
            }
            catch (Exception ex)
            {
                //TODO: 错误需要日志
                Init();
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="path"></param>
        public void SaveConfigs(IConfigParam param)
        {
            UserAppConfigParam appConfig = param as UserAppConfigParam;
            SerializeHelper.Save(this, appConfig.ConfigPath);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            公共缓存配置目录 = string.Empty;
            衍生品缓存配置目录 = string.Empty;
            权益缓存配置目录 = string.Empty;
            固收缓存配置目录 = string.Empty;

            分行策略 = string.Empty;
            时间戳提取策略 = string.Empty;
            缓存匹配策略列表 = new List<CachePattern>();
        }
    }


    /// <summary>
    /// 缓存匹配策略
    /// </summary>
    public class CachePattern
    {
        /// <summary>
        /// 缓存类型
        /// </summary>
        public string CacheType { get; set; }
        /// <summary>
        /// 匹配模式
        /// </summary>
        public string Pattern { get; set; }
    }
}
