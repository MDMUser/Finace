using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FinanceMs.Common.Models
{
    public  class MDMIndustry
    {
        /// <summary>
        /// GUID
        /// </summary>
        public string NM { get; set; }

        /// <summary>
        /// 行业编码
        /// </summary>
        [Description("行业编码")]
        public string Code { get; set; }

        /// <summary>
        /// 行业名称
        /// </summary>
        [Description("行业名称")]
        public string Name { get; set; }

        /// <summary>
        /// 行业类别
        /// </summary>
        [Description("行业类别")]
        public string Type { get; set; }

        
        /// <summary>
        /// 上级区划内码NM
        /// </summary>
        public string ParentNM { get; set; }

        /// <summary>
        /// 上级区划编号
        /// </summary>
        [Description("上级区划编号")]
        public string ParentCode { get; set; }      

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
