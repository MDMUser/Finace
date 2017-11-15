using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanceMs.Common.Models
{
    /// <summary>
    /// 执行存储过程获取分级码等信息
    /// </summary>
    public class ResNewFJM
    {
        /// <summary>
        /// 导入某条数据的NM（存在则走修改，不存在则添加）
        /// </summary>
        public string NM { get; set; }
        /// <summary>
        /// 给导入数据生成的fjm
        /// </summary>
        public string NewFJM { get; set; }
        /// <summary>
        /// 父级内码
        /// </summary>
        public string ParentNM { get; set; }
        /// <summary>
        /// 导入信息正确的级数
        /// </summary>
        public int NewLayer { get; set; }
        /// <summary>
        /// 该导入数据存在的问题
        /// </summary>
        public int QuestionType { get; set; }
    }
}
