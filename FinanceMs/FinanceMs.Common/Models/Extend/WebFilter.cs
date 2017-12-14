using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanceMs.Common.Models.Extend
{
    /// <summary>
    /// web端查询参数对象
    /// </summary>
    public class WebFilter
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string field { get; set; }
        /// <summary>
        /// 查询方式
        /// </summary>
        public string op { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string value { get; set; }
    }
}
