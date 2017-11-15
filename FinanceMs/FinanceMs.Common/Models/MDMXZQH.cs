using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FinanceMs.Common.Models
{
    public class MDMXZQH
    {
        /// <summary>
        /// GUID
        /// </summary>
        public string NM { get; set; }

        /// <summary>
        /// 区划代码
        /// </summary>
        [Description("区划代码")]
        public string Code { get; set; }

        /// <summary>
        /// 区划名称
        /// </summary>
        [Description("区划名称")]
        public string Name { get; set; }

        /// <summary>
        /// 财政管理级次代码
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// 财政管理级次名称
        /// </summary>
        [Description("财政管理级次")]
        public string LevelName { get; set; }

        /// <summary>
        /// 财政管理级次标识代码
        /// </summary>
        public string MarkCode { get; set; }

        /// <summary>
        /// 财政管理级次标识名称
        /// </summary>
        [Description("财政管理级次标识")]
        public string MarkName { get; set; }

        /// <summary>
        /// 上级区划ID
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// 上级区划代码
        /// </summary>
        [Description("上级区划代码")]
        public string ParentCode { get; set; }

        /// <summary>
        /// 字母拼音
        /// </summary>
        [Description("字母拼音")]
        public string PinYin { get; set; }

        /// <summary>
        /// 拼音缩写
        /// </summary>
        [Description("拼音缩写")]
        public string JianPin { get; set; }

        /// <summary>
        /// 东中西部代码
        /// </summary>
        public string DZXBCode { get; set; }

        /// <summary>
        /// 东中西部名称
        /// </summary>
        [Description("东中西部")]
        public string DZXBName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Note { get; set; }

        /// <summary>
        /// 分级码
        /// </summary>
        public string FJM { get; set; }

        /// <summary>
        /// 级数
        /// </summary>
        [Description("级数")]
        public int Layer { get; set; }

        /// <summary>
        /// 是否明细
        /// </summary>
        [Description("是否明细")]
        public string IsDetail { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public string AuditState { get; set; }

        /// <summary>
        /// 停用标志
        /// </summary>
        public string TYBZ { get; set; }

        /// <summary>
        /// 停用年度
        /// </summary>
        public string TYND { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string LastModifiedUser { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModifiedTime { get; set; }
    }
}
