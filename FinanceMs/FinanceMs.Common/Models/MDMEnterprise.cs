using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FinanceMs.Common.Models
{
    public class MDMEnterprise
    {
        /// <summary>
        /// GUID
        /// </summary>
        public string NM { get; set; }

        /// <summary>
        /// 企业代码
        /// </summary>
        [Description("企业代码")]
        public string Code { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        [Description("企业名称")]
        public string Name { get; set; }

        /// <summary>
        /// 企业简称
        /// </summary>
        [Description("企业简称")]
        public string ShortName { get; set; }

        /// <summary>
        /// 注册地
        /// </summary>
        [Description("注册地")]
        public string RegistAddr { get; set; }

        /// <summary>
        /// 登记机关
        /// </summary>
        [Description("登记机关")]
        public string RegistOrg { get; set; }

        /// <summary>
        /// 行业代码
        /// </summary>
        public string IndustryNM { get; set; }

        /// <summary>
        /// 行业名称
        /// </summary>
        [Description("行业名称")]
        public string IndustryName { get; set; }

        /// <summary>
        /// 行政区划内码
        /// </summary>
        public string XZQHNM { get; set; }
        /// <summary>
        /// 行政区划名称
        /// </summary>
        [Description("行政区划名称")]
        public string XZQHName { get; set; }

        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string CreditCode { get; set; }

        /// <summary>
        /// 税号
        /// </summary>
        public string TaxNumber { get; set; }

        /// <summary>
        /// 所有者类型
        /// </summary>

        public string OwnerType { get; set; }
        /// <summary>
        /// 所有者类型编码
        /// </summary>
        public string OwnerTypeCode { get; set; }
        /// <summary>
        /// 所有者类型名称
        /// </summary>
        [Description("所有者类型名称")]
        public string OwnerTypeName { get; set; }

        /// <summary>
        /// 上级单位内码
        /// </summary>
        public string ParentNM { get; set; }

        /// <summary>
        /// 上级单位代码
        /// </summary>
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
        public int Layer { get; set; }

        /// <summary>
        /// 是否明细
        /// </summary>
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
