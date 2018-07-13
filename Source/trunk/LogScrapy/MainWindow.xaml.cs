using Config.Entity;
using Config.Interface;
using Engine;
using LogUtility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LogScrapy
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        LSPresenter Presenter { get; set; }
        UserAppConfig UserConfig { get; set; }
        string LogFile { get; set; }


        List<LogEntityBase> datas = new List<LogEntityBase>();

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ILogUtility logUtility = Presenter.Engine.Get<ILogUtility>();
            if (!File.Exists(LogFile))
            {
                MessageBox.Show("日志文件不存在");
                return;
            }
            string log = logUtility.ReadLogFile(@LogFile);
            if (string.IsNullOrWhiteSpace(log))
            {
                return;
            }
            List<LogEntityBase> logs = logUtility.DecodeLog(log, UserConfig.分行策略, UserConfig.时间戳提取策略);
            if (logs == null || logs.Count <= 0)
            {
                return;
            }
            datas = logs;

            #region 获取过滤模式
            List<Regex> regexs = new List<Regex>();
            string cachePattern = Presenter.GetCachePatternByType(cmbCacheType.Text);
            if (!string.IsNullOrWhiteSpace(cachePattern))
            {
                Regex regCachePattern = new Regex(@cachePattern);
                regexs.Add(regCachePattern);
            }

            TextRange textRange = new TextRange(rtxFilterPattern.Document.ContentStart, rtxFilterPattern.Document.ContentEnd);
            string filterPattern = textRange.Text.TrimEnd();
            if (!string.IsNullOrWhiteSpace(filterPattern))
            {
                Regex re2 = new Regex(@filterPattern);
                regexs.Add(re2);
            }
            #endregion

            ObservableCollection<LogEntityBase> logEntities = new ObservableCollection<LogEntityBase>(logs);
            logEntities = new ObservableCollection<LogEntityBase>(datas.Where(p => Presenter.CheckPattern(regexs, p.DataInfo)));
            dataGrid.ItemsSource = logEntities;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string path = string.Format(@"{0}\Config\logscrapyconfig.xml", Directory.GetCurrentDirectory());
            ScrapyEngine engine = new ScrapyEngine();
            EngineParam param = new EngineParam()
            {
                AppConfigPath = path
            };
            engine.BootEngine(param);
            Presenter = new LSPresenter(this, engine);
            UserConfig = Presenter.Engine.Get<IAppConfigManage>() as UserAppConfig;
            SettingInit();
        }

        /// <summary>
        /// 设置初始化
        /// </summary>
        private void SettingInit()
        {
            tb_baseCacheSettingFile.Text = UserConfig.公共缓存配置目录;
            tb_derCacheSettingFile.Text = UserConfig.衍生品缓存配置目录;
            txt_RawRowPattern.Text = UserConfig.分行策略;
            txt_TimeStrapPattern.Text = UserConfig.时间戳提取策略;
            if (UserConfig.缓存匹配策略列表 != null && UserConfig.缓存匹配策略列表.Count > 0)
            {
                grid_CachePattern.ItemsSource = new ObservableCollection<CachePattern>(UserConfig.缓存匹配策略列表);
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <returns></returns>
        private void SaveSetting()
        {
            UserConfig.公共缓存配置目录 = tb_baseCacheSettingFile.Text;
            UserConfig.衍生品缓存配置目录 = tb_derCacheSettingFile.Text;
            UserConfig.分行策略 = txt_RawRowPattern.Text;
            UserConfig.时间戳提取策略 = txt_TimeStrapPattern.Text;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSetting_Click(object sender, RoutedEventArgs e)
        {
            string path = string.Format(@"{0}\Config\logscrapyconfig.xml", Directory.GetCurrentDirectory());
            UserConfig.SaveConfigs(@path);
        }

        /// <summary>
        /// 选择日志文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogFileSelect_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".log";
            ofd.Filter = "log file|*.log|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                tb_LogFileDir.Text = ofd.FileName;
                LogFile = ofd.FileName;
            }
        }
    }

    public class CacheLogEntity
    {
        public string Level { get; set; }

        public string TimeStamp { get; set; }

        public string DataInfo { get; set; }
    }
}
