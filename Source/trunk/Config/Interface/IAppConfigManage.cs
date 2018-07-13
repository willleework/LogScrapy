using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Interface
{
    /// <summary>
    /// 应用程序参数配置管理类
    /// </summary>
    public interface IAppConfigManage
    {
        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="path"></param>
        void LoadConfigs(string path);

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="path"></param>
        void SaveConfigs(string path);
    }
}
