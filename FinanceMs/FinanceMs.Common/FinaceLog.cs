using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genersoft.MDM.NetLog.Server;

namespace FinanceMs.Common
{
    /// <summary>
    /// 调用主数据共同方法写日志
    /// </summary>
    public class FinaceLog
    {
        private ILog log = LogManager.CreateLogger("", "FinanceMs");
        /// <summary>
        /// 枚举值为2
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            log.Debug(message);
        }
        /// <summary>
        /// 枚举值为3
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            log.Info(message);
        }
        /// <summary>
        /// 枚举值为4
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            log.Error(message);
        } 
    }
}
