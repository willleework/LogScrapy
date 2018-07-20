﻿using Cache;
using Config.Entity;
using Config.Interface;
using Engine;
using LogDecode;
using ScrapyCache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using Task;

namespace LogScrapy
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        WaitingDlg dlg = new WaitingDlg();
        enum DialogMode { Async, Sync};
        #region 属性
        /// <summary>
        /// 业务逻辑层
        /// </summary>
        LSPresenter Presenter { get; set; }
        /// <summary>
        /// 日志文件路径
        /// </summary>
        string LogFile { get; set; }
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
            dlg.Owner = this;
            dlg.Topmost = true;
            dlg.ShowInTaskbar = false;

            Presenter = new LSPresenter(this);
            ScrapyEngine.Get<ITaskMange>().Init(SynchronizationContext.Current);
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
            string column = (string)cmb_CacheColumn.SelectedValue;
            string cacheType = (string)cmb_CacheType.SelectedValue;
            string colEng = Presenter.GetColumnEngNameByChnName(column, cacheType);
            string value = tb_FilterText.Text;
            TextRange documentTextRange = new TextRange(rtx_FilterPattern.Document.ContentStart, rtx_FilterPattern.Document.ContentEnd);
            if (!string.IsNullOrWhiteSpace(documentTextRange.Text.Trim()))
            {
                condition = string.Format("{0},{1}={2}", documentTextRange.Text.Trim(), colEng, value);
            }
            else
            {
                condition = string.Format("{0}={1}", colEng, value);
            }
            Paragraph p = new Paragraph();
            Run r = new Run(condition);
            p.Inlines.Add(r);
            rtx_FilterPattern.Document.Blocks.Clear();
            rtx_FilterPattern.Document.Blocks.Add(p);
        }

        /// <summary>
        /// 生成列事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="datas"></param>
        private void GenerateColumns(object sender, IEnumerable datas)
        {
            List<ClientCacheConfigColumn> columns = Presenter.GetCacheColumn(cmb_CacheType.Text);
            DataGrid grid = (DataGrid)sender;
            grid.ItemsSource = null;
            grid.Columns.Clear();
            grid.Columns.Add(new DataGridTextColumn()
            {
                Header = "时间戳",
                Binding = new Binding("TimeStamp"),
            });
            grid.Columns.Add(new DataGridTextColumn()
            {
                Header = "日志级别",
                Binding = new Binding("Level"),
            });
            foreach (ClientCacheConfigColumn column in columns)
            {
                DataGridTextColumn gridcolumn = new DataGridTextColumn();
                gridcolumn.Header = column.标准字段名称;
                gridcolumn.Binding = new Binding(column.标准字段);
                grid.Columns.Add(gridcolumn);
            }
        }

        /// <summary>
        /// 显示等待窗口
        /// </summary>
        private void ShowWaitingDialog(DialogMode mode = DialogMode.Sync)
        {
            if (mode == DialogMode.Sync)
            {
                dlg.ShowDialog();
            }
            else
            {
                dlg.Show();
            }
        }

        /// <summary>
        /// 隐藏等待窗口
        /// </summary>
        private void CloseWaitingDialog()
        {
            if (dlg != null)
            {
                dlg.Hide();
            }
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
            cmb_DomainType_SelectionChanged(null, null);
            dataGrid.GeneratingColumnsEvent += GenerateColumns;
        }

        /// <summary>
        /// 查询日志数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Presenter == null)
            {
                return;
            }
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

            //dataGrid.DataSource = Presenter.GetLogInfoRowsByRegFilters(regexs, cmb_CacheType.Text);
            dynamic param = new ExpandoObject();
            param.Regexs = regexs;
            param.CacheType = cmb_CacheType.Text;
            ScrapyEngine.Get<ITaskMange>().AsyncRunWithCallBack<dynamic, List<dynamic>>(Presenter.GetLogInfoRowsByRegFilters, BingdingLogs, param as object);
            ShowWaitingDialog();
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="datas"></param>
        private void BingdingLogs(List<dynamic> datas)
        {
            CloseWaitingDialog();
            dataGrid.DataSource = datas;
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
        /// 业务选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_DomainType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Presenter == null)
            {
                return;
            }
            ObservableCollection<string> cacheTyps;
            ComboBoxItem domainItem = (ComboBoxItem)cmb_DomainType.SelectedItem;
            string domainType = string.Empty;
            if (domainItem.Content == null)
            {
                return;
            }
            domainType = (string)domainItem.Content;
            if (string.IsNullOrWhiteSpace(domainType))
            {
                cacheTyps = new ObservableCollection<string>();
            }
            else
            {
                List<ClientCacheConfig> cacheColumns = Presenter.GetCacheTypes(domainType);
                cacheTyps = new ObservableCollection<string>(cacheColumns.Select(p => p.表名));
            }
            cmb_CacheType.ItemsSource = cacheTyps;
        }

        /// <summary>
        /// 缓存类型选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_CacheType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Presenter == null)
            {
                return;
            }
            ObservableCollection<string> columns;
            string cacheType = (string)cmb_CacheType.SelectedItem;

            if (string.IsNullOrWhiteSpace(cacheType))
            {
                columns = new ObservableCollection<string>();
            }
            else
            {
                List<ClientCacheConfigColumn> cacheColumns = Presenter.GetCacheColumn(cacheType);
                columns = new ObservableCollection<string>(cacheColumns.Select(p=>p.标准字段名称));
            }
            cmb_CacheColumn.ItemsSource = columns;
        }

        /// <summary>
        /// 加载日志信息到本地缓存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadLogsToCache_Click(object sender, RoutedEventArgs e)
        {
            if (Presenter == null)
            {
                return;
            }
            if (!File.Exists(@LogFile))
            {
                MessageBox.Show("日志文件不存在");
                return;
            }
            ScrapyEngine.Get<ITaskMange>().AsyncRunWithCallBack<string>(Presenter.LoadLogDatasToCache, CloseWaitingDialog, LogFile);
            ShowWaitingDialog();
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
            tb_baseCacheSettingFile.Text = ScrapyEngine.Get<IAppConfigManage>().UserConfig.公共缓存配置目录;
            tb_derCacheSettingFile.Text = ScrapyEngine.Get<IAppConfigManage>().UserConfig.衍生品缓存配置目录;
            tb_xhCacheSettingFile.Text = ScrapyEngine.Get<IAppConfigManage>().UserConfig.权益缓存配置目录;
            tb_gsCacheSettingFile.Text = ScrapyEngine.Get<IAppConfigManage>().UserConfig.固收缓存配置目录;
            txt_RawRowPattern.Text = ScrapyEngine.Get<IAppConfigManage>().UserConfig.分行策略;
            txt_TimeStrapPattern.Text = ScrapyEngine.Get<IAppConfigManage>().UserConfig.时间戳提取策略;
            if (ScrapyEngine.Get<IAppConfigManage>().UserConfig.缓存匹配策略列表 != null && ScrapyEngine.Get<IAppConfigManage>().UserConfig.缓存匹配策略列表.Count > 0)
            {
                grid_CachePattern.ItemsSource = new ObservableCollection<CachePattern>(ScrapyEngine.Get<IAppConfigManage>().UserConfig.缓存匹配策略列表);
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <returns></returns>
        private void SaveSetting()
        {
            ScrapyEngine.Get<IAppConfigManage>().UserConfig.公共缓存配置目录 = tb_baseCacheSettingFile.Text;
            ScrapyEngine.Get<IAppConfigManage>().UserConfig.衍生品缓存配置目录 = tb_derCacheSettingFile.Text;
            ScrapyEngine.Get<IAppConfigManage>().UserConfig.分行策略 = txt_RawRowPattern.Text;
            ScrapyEngine.Get<IAppConfigManage>().UserConfig.时间戳提取策略 = txt_TimeStrapPattern.Text;

            ObservableCollection<CachePattern> patterns = grid_CachePattern.ItemsSource as ObservableCollection<CachePattern>;
            if (patterns != null)
            {
                ScrapyEngine.Get<IAppConfigManage>().UserConfig.缓存匹配策略列表 = new List<CachePattern>(patterns.ToList());
            }
            else
            {
                ScrapyEngine.Get<IAppConfigManage>().UserConfig.缓存匹配策略列表.Clear();
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
            UserAppConfigParam config = new UserAppConfigParam() { ConfigPath = @Presenter.AppConfigFile };
            ScrapyEngine.Get<IAppConfigManage>().UserConfig.SaveConfigs(config);
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
                ScrapyEngine.Get<IAppConfigManage>().UserConfig.公共缓存配置目录 = ofd.FileName;
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
                ScrapyEngine.Get<IAppConfigManage>().UserConfig.衍生品缓存配置目录 = ofd.FileName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XHCacheConfigFileSelect_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".xlsm";
            ofd.Filter = "Excel File|*.xlsm|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                tb_xhCacheSettingFile.Text = ofd.FileName;
                ScrapyEngine.Get<IAppConfigManage>().UserConfig.权益缓存配置目录 = ofd.FileName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GSCacheConfigFileSelect_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".xlsm";
            ofd.Filter = "Excel File|*.xlsm|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                tb_gsCacheSettingFile.Text = ofd.FileName;
                ScrapyEngine.Get<IAppConfigManage>().UserConfig.固收缓存配置目录 = ofd.FileName;
            }
        }
    }

    /// <summary>
    /// 测试界面
    /// </summary>
    public partial class MainWindow
    {
        private void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            this.gridTest.GeneratingColumnsEvent += GenerateColumns;
        }


        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            //#region 获取过滤模式
            //List<Regex> regexs = new List<Regex>();
            //string cachePattern = Presenter.GetCachePatternByType(cmb_CacheType.Text);
            //if (!string.IsNullOrWhiteSpace(cachePattern))
            //{
            //    Regex regCachePattern = new Regex(@cachePattern);
            //    regexs.Add(regCachePattern);
            //}
            //else
            //{
            //    MessageBox.Show("没有配置当前缓存对应的匹配策略");
            //    return;
            //}

            //TextRange textRange = new TextRange(rtx_FilterPattern.Document.ContentStart, rtx_FilterPattern.Document.ContentEnd);
            //string filterPattern = textRange.Text.TrimEnd();
            //if (!string.IsNullOrWhiteSpace(filterPattern))
            //{
            //    string[] patterns = filterPattern.Split(new char[] { ',', '，' });
            //    foreach (string pat in patterns)
            //    {
            //        Regex re = new Regex(@pat);
            //        regexs.Add(re);
            //    }
            //}
            //#endregion

            ////#region 解析日志
            ////ILogUtility logUtility = ScrapyEngine.Get<ILogUtility>();
            ////if (!File.Exists(LogFile))
            ////{
            ////    MessageBox.Show("日志文件不存在");
            ////    return;
            ////}
            ////string log = logUtility.ReadLogFile(@LogFile);
            ////if (string.IsNullOrWhiteSpace(log))
            ////{
            ////    return;
            ////}
            ////List<LogEntityBase> logs = logUtility.DecodeLog(log, ScrapyEngine.Get<IAppConfigManage>().UserConfig.分行策略, ScrapyEngine.Get<IAppConfigManage>().UserConfig.时间戳提取策略);
            ////if (logs == null || logs.Count <= 0)
            ////{
            ////    return;
            ////}
            ////datas = logs;
            ////#endregion
            //List<ICacheItem> logs = new List<ICacheItem>();
            //List<LogInfoRow> infoRows = new List<LogInfoRow>();
            //logs = ScrapyEngine.Get<ICachePool>().Get<LogInfoRowTable>().Get();
            //logs.AsParallel().ForAll(p => 
            //{
            //    LogInfoRow row = p as LogInfoRow;
            //    if (Presenter.CheckPattern(regexs, row.DataInfo))
            //    {
            //        infoRows.Add(row);
            //    }
            //});

            //List<dynamic> results = new List<dynamic>();
            //List<string> columns = Presenter.GetCacheColumn(cmb_CacheType.Text);
            //foreach (LogInfoRow entity in logs)
            //{
            //    dynamic obj = new ExpandoObject();
            //    foreach (string column in columns)
            //    {
            //        string value = string.Empty;
            //        string reg = string.Format("{0}={1}", column, @"\w+\,");
            //        Regex r = new Regex(@reg);
            //        MatchCollection m = r.Matches(entity.DataInfo);
            //        if (m.Count > 0)
            //        {
            //            value = m[0].Value.Remove(0, column.Length + 1).TrimEnd(',');
            //        }
            //        ((IDictionary<string, object>)obj).Add(column, value);
            //    }
            //    results.Add(obj);
            //}
            //gridTest.DataSource = results;
        }
    }
}
