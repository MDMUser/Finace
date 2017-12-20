using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FinanceMs.Common;
using FinanceMs.Common.Models;
using Genersoft.MDM.Pub.Server.Com;

namespace FinanceMs.Import
{
    public class EnterpriseOperate
    {
        private readonly DataBaseEx db = new DataBaseEx();
        #region 导入
        /// <summary>
        /// 企业导入
        /// </summary>
        /// <param name="data">导入数据源</param>
        /// <param name="schema">该字典的数据模型</param>
        /// <returns></returns>
        public string ImportData(DataSet data)
        {
            // 应返回的信息
            string msg = string.Empty;

            string invalidResult = "", editResult = "";
            for (int tableCount = 0; tableCount < data.Tables.Count; tableCount++)
            {
                DataTable dtData = data.Tables[tableCount];
                // ①判断excel表格的合理性
                string[] list = { "企业代码", "企业名称" };
                msg = Verification.ImportColumns(data.Tables[0].Columns, list);
                if (!string.IsNullOrWhiteSpace(msg))
                    return msg;
                // ②将dataTable转为list
                var enList = ConvertsData.DataTableToListByProperties<MDMEnterprise>(dtData);
                // ③筛选出添加、修改、无效数据
                // 声明(添加、修改)和 无效list
                IList<MDMEnterprise> editList = new List<MDMEnterprise>(),
                            invalidList = new List<MDMEnterprise>();
                if (enList != null && enList.Count > 0)
                {
                    invalidList = enList.Where(g => ConvertsData.ValidNullString(g.Code, "") == ""
                                                   || ConvertsData.ValidNullString(g.Name, "") == ""
                                                ).ToArray();

                    editList = enList.Where(g => ConvertsData.ValidNullString(g.Code, "") != ""
                                                   && ConvertsData.ValidNullString(g.Name, "") != ""
                                                ).OrderBy(g => g.Code).ToArray();

                }
                try
                {
                    // ④分别对数据进行增、改功能
                    if (editList != null && editList.Count > 0)
                    {
                        editResult += EditListOperate(editList);
                    }
                    if (invalidList != null && invalidList.Count > 0)
                    {
                        invalidResult += InvalidOperate(invalidList);
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                // ⑤返回处理结果
                if (!string.IsNullOrWhiteSpace(editResult) || !string.IsNullOrWhiteSpace(invalidResult))
                {
                    msg += "有部分数据导入存在问题如下： <br/> "
                        + editResult + "<br/>"
                        + invalidResult;
                }
            }
            return msg;
        }
        #region 导入数据
        /// <summary>
        /// 处理企业字典数据
        /// </summary>
        /// <param name="cList"></param>
        /// <returns></returns>
        private string EditListOperate(IList<MDMEnterprise> cList)
        {
            string result = "";
            for (int i = 0; i < cList.Count; i++)
            {
                MDMEnterprise addInfo = cList[i];

                bool isUpdate = DBUtility.CheackIsUpdate(db, "MDMEnterprise", addInfo.Code);
                if (isUpdate)
                {
                    // 修改该条数据基本信息
                    StringBuilder sqledit = new StringBuilder();
                    sqledit.AppendFormat("UPDATE MDMEnterprise SET Code='{0}', Name='{1}',ShortName='{2}',RegistAddr='{3}',RegistOrg='{4}', ", addInfo.Code, addInfo.Name, addInfo.ShortName, addInfo.RegistAddr, addInfo.RegistOrg);


                    // 行业关联表对接
                    if (!string.IsNullOrWhiteSpace(addInfo.IndName))
                    {
                        sqledit.AppendFormat(" IndNM=(select NM from MDMIndustry where Name = '{0}') , IndName='{0}', ", addInfo.IndName.Trim());
                    }
                    //  行政区划关联表对接
                    if (!string.IsNullOrWhiteSpace(addInfo.XZQHName))
                    {
                        sqledit.AppendFormat(" XZQHNM = (select NM from MDMXZQH where Name = '{0}') , XZQHName='{0}', ", addInfo.XZQHName.Trim());

                    }

                    sqledit.AppendFormat("CreditCode='{0}',TaxNumber='{1}',", addInfo.CreditCode, addInfo.TaxNumber);
                    // 所有者类型码表对接
                    if (!string.IsNullOrWhiteSpace(addInfo.OwnerTypeName))
                    {
                        sqledit.AppendFormat(" OwnerTypeCode={0}, OwnerTypeName='{1}', ", ConvertsData.GetCodeByName("OwnerType", addInfo.OwnerTypeName.Trim()), addInfo.OwnerTypeName.Trim());
                    }

                    sqledit.AppendFormat(" TYBZ='{0}',TYND='{1}', AuditState='{2}',isTrans='{3}',Note='{4}', ", addInfo.TYBZ, addInfo.TYND, addInfo.AuditState, addInfo.IsDetail, addInfo.Note);
                    sqledit.AppendFormat(" LastModifiedUser='{0}',LastModifiedTime={1} ", DBUtility.GetOperateUser() + "导入", DBUtility.GetOperateDate());
                    sqledit.AppendFormat(" Where Code = '{0}' ", addInfo.Code);
                    db.ExecuteSQL(sqledit.ToString());
                }
                else
                {
                    // 添加该条数据
                    StringBuilder addSql = new StringBuilder();
                    addSql.AppendLine(" INSERT INTO MDMEnterprise (NM, Code, Name, ShortName,RegistAddr, RegistOrg, IndustryNM ,XZQHNM,");
                    addSql.AppendLine(" CreditCode,TaxNumber,OwnerType,Note,TYND,isTrans,AuditState,TYBZ, CreateUser, CreateTime, LastModifiedUser, LastModifiedTime ) VALUES  (  ");
                    addSql.AppendFormat("'{0}','{1}','{2}',", System.Guid.NewGuid().ToString(), addInfo.Code, addInfo.Name);
                    addSql.AppendFormat("'{0}','{1}','{2}',", addInfo.ShortName, addInfo.RegistAddr, addInfo.RegistOrg);

                    if (!string.IsNullOrWhiteSpace(addInfo.IndName))
                    {
                        // 行业关联
                        addSql.AppendFormat(" (select NM from MDMIndustry where Name = '{0}') ,'{0}', ", addInfo.IndName.Trim());
                    }
                    else
                    {
                        addSql.Append(" '','',");
                    }
                    if (!string.IsNullOrWhiteSpace(addInfo.XZQHName))
                    {
                        // 企业关联
                        addSql.AppendFormat(" (select XZQHNM from MDMXZQH where Name = '{0}') ,'{0}', ", addInfo.XZQHName.Trim());
                    }
                    else
                    {
                        addSql.Append(" '','',");
                    }
                    addSql.AppendFormat("'{0}',", addInfo.CreditCode);
                    //addSql.AppendFormat(" (select code from gscodeitems where Name = '{0}' and codesetnm = 'OwnerType' ),");
                    addSql.AppendFormat(" {0},", ConvertsData.GetCodeByName("OwnerType", addInfo.OwnerTypeName), addInfo.OwnerTypeName);
                    addSql.AppendFormat("'{0}',", addInfo.Note);
                    addSql.AppendFormat("'{0}','{1}',", (int)EnumAuditState.pass, (int)EnumTYBZ.enabled);
                    addSql.AppendFormat("'{0}',{1},", DBUtility.GetOperateUser() + "导入", DBUtility.GetOperateDate());
                    addSql.AppendFormat("'{0}',{1})", DBUtility.GetOperateUser() + "导入", DBUtility.GetOperateDate());
                    db.ExecuteSQL(addSql.ToString());
                }

            }
            return result;
        }
        #endregion

        #region 无效数据处理
        private string InvalidOperate(IList<MDMEnterprise> cList)
        {
            string info = "";
            for (int i = 0; i < cList.Count; i++)
            {
                info += "编号 " + cList[i].Code + "，名称 " + cList[i].Name + "： 信息编号、名称、级数、是否明细不全无法导入；<br/>";
            }
            return info;
        }
        #endregion
        #endregion
        #region 导出
        /// <summary>
        /// 企业导出
        /// </summary>
        /// <param name="where"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public DataSet ExportData(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT a.Code 企业代码,a.Name 企业名称,a.ShortName  企业简称,a.registaddr 注册地,b.name 行业名称,c.name 行政区划名称,a.CreditCode 统一社会信用代码,");
            sb.AppendLine("a.taxnumber  税号,a.RegistOrg  登记机关,a.ownertype 所有者类型编号,a.name 所有者类型,a.Note 备注");
            sb.AppendLine("FROM MDMEnterprise a left join mdmindustry b   on a.industrynm= b.nm  left join mdmxzqh  c   on a.xzqhnm=c.nm  left join gscodeitems  d   on a.ownertype=d.code and codesetnm = 'OwnerType'  WHERE a.TYBZ = '0' and a.AuditState = '2'");
            sb.AppendLine(where);
            DataSet ds = db.ExecuteSQL(sb.ToString());
            return ds;
        }
        #endregion
    }
}