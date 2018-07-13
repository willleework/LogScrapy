using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Interface
{
    public interface IConfigBase
    {
        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="path"></param>
        void LoadConfigs(IConfigParam param);

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="path"></param>
        void SaveConfigs(IConfigParam param);
    }
}
