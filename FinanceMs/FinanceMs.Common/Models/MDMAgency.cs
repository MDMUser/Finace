using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FinanceMs.Common.Models
{
   public  class MDMAgency
    {
        /// <summary>
        /// GUID
        /// </summary>
        public string NM { get; set; }

        /// <summary>
        /// 单位层次代码
        /// </summary>
        [Description("单位层次代码")]
        public string Code { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [Description("单位名称")]
        public string Name { get; set; }

        /// <summary>
        /// 组织机构代码
        /// </summary>
        [Description("组织机构代码")]
        public string OrgCode { get; set; }

        /// <summary>
        /// 单位类型代码
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 单位类型名称
        /// </summary>
        [Description("单位类型名称")]
        public string TypeName { get; set; }

        /// <summary>
        /// 预算单位级次代码
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// 预算单位级次名称
        /// </summary>
        [Description("预算单位级次名称")]
        public string LevelName { get; set; }

        /// <summary>
        /// 行业代码
        /// </summary>
        public string IndCode { get; set; }

        /// <summary>
        /// 行业名称
        /// </summary>
        [Description("行业名称")]
        public string IndName { get; set; }

        /// <summary>
        /// 人员情况代码
        /// </summary>
        public string PerKindCode { get; set; }

        /// <summary>
        /// 人员情况名称
        /// </summary>
        [Description("行业名称")]
        public string PerKindName { get; set; }

        /// <summary>
        /// 财政部内部机构代码
        /// </summary>
        public string MOFDepCode { get; set; }

        /// <summary>
        /// 财政部内部机构名称
        /// </summary>
        [Description("财政部内部机构名称")]
        public string MOFDepName { get; set; }

        /// <summary>
        /// 部门标识代码
        /// </summary>
        public string SupDepCode { get; set; }

        /// <summary>
        /// 部门标识名称
        /// </summary>
        [Description("部门标识名称")]
        public string SupDepName { get; set; }

        /// <summary>
        /// 单位行政级别代码
        /// </summary>
        public string AdmLevelCode { get; set; }

        /// <summary>
        /// 单位行政级别名称
        /// </summary>
        [Description("单位行政级别名称")]
        public string AdmLevelName { get; set; }

        /// <summary>
        /// 性质分类代码
        /// </summary>
        public string XZTypeCode { get; set; }

        /// <summary>
        /// 性质分类名称
        /// </summary>
        [Description("性质分类名称")]
        public string XZTypeName { get; set; }

        /// <summary>
        /// 经费供给方式代码
        /// </summary>
        public string FundSupCode { get; set; }

        /// <summary>
        /// 经费供给方式名称
        /// </summary>
        [Description("经费供给方式名称")]
        public string FundsupName { get; set; }

        /// <summary>
        /// 上级单位内码
        /// </summary>
        public string ParentNM { get; set; }

        /// <summary>
        /// 上级单位代码
        /// </summary>
        [Description("上级单位代码")]
        public string ParentCode { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [Description("传真")]
        public string Fax { get; set; }

        /// <summary>
        /// 单位电话
        /// </summary>
        [Description("单位电话")]
        public string Tel { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Description("地址")]
        public string Address { get; set; }

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
