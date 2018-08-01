using Common.Utility;
using Config.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Entity
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public class CacheLogConfig : IConfigBase
    {
        #region 属性
        #region 缓存表信息
        public DataTable 基础组缓存表 { get; set; }
        public DataTable 衍生品缓存表 { get; set; }
        public DataTable 权益缓存表 { get; set; }
        public DataTable 固收缓存表 { get; set; }
        #endregion

        #region 缓存表字段信息
        public DataTable 基础组缓存字段表 { get; set; }
        public DataTable 衍生品缓存字段表 { get; set; }
        public DataTable 权益缓存字段表 { get; set; }
        public DataTable 固收缓存字段表 { get; set; }
        #endregion 
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            基础组缓存表 = new DataTable();
            衍生品缓存表 = new DataTable();
            权益缓存表 = new DataTable();
            固收缓存表 = new DataTable();

            基础组缓存字段表 = new DataTable();
            衍生品缓存字段表 = new DataTable();
            权益缓存字段表 = new DataTable();
            固收缓存字段表 = new DataTable();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="param"></param>
        public void LoadConfigs(IConfigParam param)
        {
            Init();
            //读取缓存配置信息
            CacheLogConfigParam configParam = param as CacheLogConfigParam;
            if (File.Exists(configParam.公共缓存配置目录))
            {
                try
                {
                    基础组缓存表 = ExcelUtility.ExcelToDataTable(configParam.公共缓存配置目录, "缓存表");
                    基础组缓存字段表 = ExcelUtility.ExcelToDataTable(configParam.公共缓存配置目录, "缓存表字段");
                }
                catch (Exception ex)
                {
                    AppConfigManage.LogError(string.Format("加载公共缓存配置出错：配合文件【{0}】，错误信息【{1}】", configParam.公共缓存配置目录, ex.Message));
                }
            }
            if (File.Exists(configParam.衍生品缓存配置目录))
            {
                try
                {
                    衍生品缓存表 = ExcelUtility.ExcelToDataTable(configParam.衍生品缓存配置目录, "缓存表");
                    衍生品缓存字段表 = ExcelUtility.ExcelToDataTable(configParam.衍生品缓存配置目录, "缓存表字段");
                }
                catch (Exception ex)
                {
                    AppConfigManage.LogError(string.Format("加载衍生品缓存配置出错：配合文件【{0}】，错误信息【{1}】", configParam.衍生品缓存配置目录, ex.Message));
                }
            }
            if (File.Exists(configParam.固收缓存配置目录))
            {
                try
                {
                    固收缓存表 = ExcelUtility.ExcelToDataTable(configParam.固收缓存配置目录, "缓存表");
                    固收缓存字段表 = ExcelUtility.ExcelToDataTable(configParam.固收缓存配置目录, "缓存表字段");
                }
                catch (Exception ex)
                {
                    AppConfigManage.LogError(string.Format("加载固收缓存配置出错：配合文件【{0}】，错误信息【{1}】", configParam.固收缓存配置目录, ex.Message));
                }
            }
            if (File.Exists(configParam.权益缓存配置目录))
            {
                try
                {
                    权益缓存表 = ExcelUtility.ExcelToDataTable(configParam.权益缓存配置目录, "缓存表");
                    权益缓存字段表 = ExcelUtility.ExcelToDataTable(configParam.权益缓存配置目录, "缓存表字段");
                }
                catch (Exception ex)
                {
                    AppConfigManage.LogError(string.Format("加载权益缓存配置出错：配合文件【{0}】，错误信息【{1}】", configParam.权益缓存配置目录, ex.Message));
                }
            }
        }

        public void SaveConfigs(IConfigParam param)
        {
            throw new NotImplementedException("此配置文件不支持保存");
        }
    }
}
