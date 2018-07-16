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
        #region 属性
        /// <summary>
        /// 业务逻辑层
        /// </summary>
        LSPresenter Presenter { get; set; }
        /// <summary>
        /// 日志文件路径
        /// </summary>
        string LogFile { get; set; }
        /// <summary>
        /// 客户端配置文件路径
        /// </summary>
        string configFile = string.Format(@"{0}\Config\logscrapyconfig.xml", Directory.GetCurrentDirectory());
        #endregion


        List<LogEntityBase> datas = new List<LogEntityBase>();

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 界面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScrapyEngine engine = new ScrapyEngine();
            EngineParam param = new EngineParam()
            {
                AppConfigPath = @configFile
            };
            //耗时操作
            engine.BootEngine(param);
            Presenter = new LSPresenter(this, engine);
            QueryPageInit();
            SettingInit();
        }

        /// <summary>
        /// 添加过滤条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFilterCondition_Click(object sender, RoutedEventArgs e)
        {
            string condition = string.Empty;
            string column = cmb_CacheColumn.Text;
            string value = tb_FilterText.Text;
            TextRange documentTextRange = new TextRange(rtx_FilterPattern.Document.ContentStart, rtx_FilterPattern.Document.ContentEnd);
            if (!string.IsNullOrWhiteSpace(documentTextRange.Text.Trim()))
            {
                condition = string.Format("{0},{1}={2}", documentTextRange.Text.Trim(), column, value);
            }
            else
            {
                condition = string.Format("{0}={1}", column, value);
            }
            Paragraph p = new Paragraph();
            Run r = new Run(condition);
            p.Inlines.Add(r);
            rtx_FilterPattern.Document.Blocks.Clear();
            rtx_FilterPattern.Document.Blocks.Add(p);
        }
    }

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// 主界面
    /// </summary>
    public partial class MainWindow
    {
        #region 属性
        public static DependencyProperty CacheTypesProperty = DependencyProperty.Register("CacheTypes", typeof(IEnumerable<string>), typeof(ComboBox));
        /// <summary>
        /// 缓存类型
        /// </summary>
        public List<string> CacheTypes
        {
            get { return (List<string>)GetValue(CacheModeProperty); }
            set { SetValue(CacheModeProperty, value); }
        }
        #endregion

        private void QueryPageInit()
        {
            cmb_CacheType.ItemsSource = new ObservableCollection<string>(Presenter.GetCacheType());
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
            List<LogEntityBase> logs = logUtility.DecodeLog(log, Presenter.Engine.Get<IAppConfigManage>().UserConfig.分行策略, Presenter.Engine.Get<IAppConfigManage>().UserConfig.时间戳提取策略);
            if (logs == null || logs.Count <= 0)
            {
                return;
            }
            datas = logs;

            #region 获取过滤模式
            List<Regex> regexs = new List<Regex>();
            string cachePattern = Presenter.GetCachePatternByType(cmb_CacheType.Text);
            if (!string.IsNullOrWhiteSpace(cachePattern))
            {
                Regex regCachePattern = new Regex(@cachePattern);
                regexs.Add(regCachePattern);
            }
            else
            {
                MessageBox.Show("没有配置当前缓存对应的匹配策略");
                return;
            }

            TextRange textRange = new TextRange(rtx_FilterPattern.Document.ContentStart, rtx_FilterPattern.Document.ContentEnd);
            string filterPattern = textRange.Text.TrimEnd();
            if (!string.IsNullOrWhiteSpace(filterPattern))
            {
                string[] patterns = filterPattern.Split(new char[] { ',', '，' });
                foreach (string pat in patterns)
                {
                    Regex re = new Regex(@pat);
                    regexs.Add(re);
                }
            }
            #endregion

            ObservableCollection<LogEntityBase> logEntities = new ObservableCollection<LogEntityBase>(datas.Where(p => Presenter.CheckPattern(regexs, p.DataInfo)));
            dataGrid.ItemsSource = logEntities;
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
            ofd.Filter = "All files (*.*)|*.*|log file|*.log";
            if (ofd.ShowDialog() == true)
            {
                tb_LogFileDir.Text = ofd.FileName;
                LogFile = ofd.FileName;
            }
        }

        /// <summary>
        /// 缓存类型选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_CacheType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObservableCollection<string> columns;
            string cacheType = (string)cmb_CacheType.SelectedItem;
            
            if (string.IsNullOrWhiteSpace(cacheType))
            {
                columns = new ObservableCollection<string>();
            }
            else
            {
                columns = new ObservableCollection<string>(Presenter.GetCacheColumn(cacheType));
            }
            cmb_CacheColumn.ItemsSource = columns;
        }
    }

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// 设置界面
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// 设置初始化
        /// </summary>
        private void SettingInit()
        {
            tb_baseCacheSettingFile.Text = Presenter.Engine.Get<IAppConfigManage>().UserConfig.公共缓存配置目录;
            tb_derCacheSettingFile.Text = Presenter.Engine.Get<IAppConfigManage>().UserConfig.衍生品缓存配置目录;
            txt_RawRowPattern.Text = Presenter.Engine.Get<IAppConfigManage>().UserConfig.分行策略;
            txt_TimeStrapPattern.Text = Presenter.Engine.Get<IAppConfigManage>().UserConfig.时间戳提取策略;
            if (Presenter.Engine.Get<IAppConfigManage>().UserConfig.缓存匹配策略列表 != null && Presenter.Engine.Get<IAppConfigManage>().UserConfig.缓存匹配策略列表.Count > 0)
            {
                grid_CachePattern.ItemsSource = new ObservableCollection<CachePattern>(Presenter.Engine.Get<IAppConfigManage>().UserConfig.缓存匹配策略列表);
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <returns></returns>
        private void SaveSetting()
        {
            Presenter.Engine.Get<IAppConfigManage>().UserConfig.公共缓存配置目录 = tb_baseCacheSettingFile.Text;
            Presenter.Engine.Get<IAppConfigManage>().UserConfig.衍生品缓存配置目录 = tb_derCacheSettingFile.Text;
            Presenter.Engine.Get<IAppConfigManage>().UserConfig.分行策略 = txt_RawRowPattern.Text;
            Presenter.Engine.Get<IAppConfigManage>().UserConfig.时间戳提取策略 = txt_TimeStrapPattern.Text;

            ObservableCollection<CachePattern> patterns = grid_CachePattern.ItemsSource as ObservableCollection<CachePattern>;
            if (patterns != null)
            {
                Presenter.Engine.Get<IAppConfigManage>().UserConfig.缓存匹配策略列表 = new List<CachePattern>(patterns.ToList());
            }
            else
            {
                Presenter.Engine.Get<IAppConfigManage>().UserConfig.缓存匹配策略列表.Clear();
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSetting_Click(object sender, RoutedEventArgs e)
        {
            SaveSetting();
            UserAppConfigParam config = new UserAppConfigParam() { ConfigPath = @configFile };
            Presenter.Engine.Get<IAppConfigManage>().UserConfig.SaveConfigs(config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommonCacheConfigFileSelect_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".xlsm";
            ofd.Filter = "Excel File|*.xlsm|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                tb_baseCacheSettingFile.Text = ofd.FileName;
                Presenter.Engine.Get<IAppConfigManage>().UserConfig.公共缓存配置目录 = ofd.FileName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DerCacheConfigFileSelect_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".xlsm";
            ofd.Filter = "Excel File|*.xlsm|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                tb_derCacheSettingFile.Text = ofd.FileName;
                Presenter.Engine.Get<IAppConfigManage>().UserConfig.衍生品缓存配置目录 = ofd.FileName;
            }
        }
    }
}
