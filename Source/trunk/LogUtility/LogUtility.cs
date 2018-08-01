﻿using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogDecode
{
    /// <summary>
    /// 日志解析辅助类
    /// </summary>
    public class LogDecodeUtility : ILogUtility
    {
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string ReadLogFile(string filePath)
        {
            string log = string.Empty;
            log = FileOperate.ReadFile(filePath);
            return log;
        }

        /// <summary>
        /// 解码日志内容
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public List<LogEntityBase> DecodeLog(string log, string rowPattern, string timePattern)
        {
            if (string.IsNullOrWhiteSpace(log))
            {
                return new List<LogEntityBase>();
            }

            List<LogEntityBase> datas = new List<LogEntityBase>();

            List<string> timeStamps = new List<string>();
            List<string> dataLists = new List<string>();

            Regex re = new Regex(@timePattern);
            MatchCollection imgreg = re.Matches(log);

            string[] rlsts = Regex.Split(log, @rowPattern);
            dataLists = new List<string>(rlsts);

            StringBuilder time = new StringBuilder();
            int timeLength = imgreg.Count;
            int i = 1;
            while (i * 2 < rlsts.Length)
            {
                int index = i - 1;
                if (index < imgreg.Count)
                {
                    time.Clear().Append(imgreg[index].ToString());
                }
                else
                {
                    time.Clear();
                }
                datas.Add(new LogEntityBase()
                {
                    TimeStamp = time.ToString(),
                    Level = i < rlsts.Length ? rlsts[i * 2 - 1] : "",
                    DataInfo = i < rlsts.Length ? rlsts[i * 2] : ""
                });
                i++;
            }
            return datas;
        }

        /// <summary>
        /// 解码日志内容
        /// </summary>
        /// <param name="log"></param>
        /// <param name="rowPattern"></param>
        /// <param name="timeaPattern"></param>
        /// <returns></returns>
        public List<LogEntityBase> DecodeLog1(string log, string rowPattern, string timeaPattern)
        {
            List<LogEntityBase> datas = new List<LogEntityBase>();
            if (string.IsNullOrWhiteSpace(log))
            {
                return datas;
            }
            List<string> dataLists = new List<string>();
            string[] rlsts = Regex.Split(log, @rowPattern);
            dataLists = new List<string>(rlsts);

            return datas;
        }
    }
}
