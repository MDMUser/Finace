using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FinanceMs.Common.Models
{
    /// <summary>
    /// 主管部门
    /// </summary>
    public class MDMZGBM
    {
        /// <summary>
        /// GUID
        /// </summary>
        public string NM { get; set; }

        /// <summary>
        /// 部门代码
        /// </summary>
        [Description("部门代码")]
        public string Code { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Description("部门名称")]
        public string Name { get; set; }

        /// <summary>
        /// 部门地址
        /// </summary>
        [Description("部门地址")]
        public string Address { get; set; }

        /// <summary>
        /// 部门联系电话
        /// </summary>
        [Description("部门联系电话")]
        public string Phone { get; set; }

        /// <summary>
        /// 上级部门NM
        /// </summary>
        public string ParentNM { get; set; }

        /// <summary>
        /// 上级部门代码
        /// </summary>
        [Description("上级部门代码")]
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


