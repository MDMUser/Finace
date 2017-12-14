using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FinanceMs.Common.Models;
using Genersoft.MDM.Pub.Server.Com;
using System.Data;
using FinanceMs.Common;

namespace FinanceMs.Import
{
    /// <summary>
    /// 单位字典
    /// </summary>
    public class MDMAgencyOperate
    {

        private readonly DataBaseEx db = new DataBaseEx();

        #region 导入
        /// <summary>
        /// 单位导入
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
                string[] list = { "单位层次代码", "单位名称", "级数", "是否明细" };
                msg = Verification.ImportColumns(data.Tables[0].Columns, list);
                if (!string.IsNullOrWhiteSpace(msg))
                    return msg;

                // ②将dataTable转为list
                var dwList = ConvertsData.DataTableToListByProperties<MDMAgency>(dtData);

                // ③筛选出添加、修改、无效数据
                // 声明(添加、修改)和 无效list
                IList<MDMAgency> editList = new List<MDMAgency>(),
                            invalidList = new List<MDMAgency>();

                if (dwList != null && dwList.Count > 0)
                {
                    invalidList = dwList.Where(g => ConvertsData.ValidNullString(g.Code, "") == ""
                                                   || ConvertsData.ValidNullString(g.Name, "") == ""
                                                   || g.Layer <= 0
                                                   || !Verification.CharRangeOut(g.IsDetail, typeof(EnumIsDetail))
                                                ).ToArray();

                    editList = dwList.Where(g => ConvertsData.ValidNullString(g.Code, "") != ""
                                                   && ConvertsData.ValidNullString(g.Name, "") != ""
                                                   && g.Layer > 0
                                                   && Verification.CharRangeOut(g.IsDetail, typeof(EnumIsDetail))
                                                ).OrderBy(g => g.Layer).ToArray();

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
                    msg += "有部分数据导入存在问题如下： <br/>	"
                        + editResult + "<br/>"
                        + invalidResult;
                }
            }
            return msg;
        }

        #region 导入数据
        /// <summary>
        /// 处理单位数据
        /// </summary>
        /// <param name="cList"></param>
        /// <returns></returns>
        private string EditListOperate(IList<MDMAgency> cList)
        {
            string result = "";
            for (int i = 0; i < cList.Count; i++)
            {
                MDMAgency addInfo = cList[i];
                var resModel = DBUtility.GetNewFJMByDict(db, "MDMAgency", addInfo.Code, addInfo.Layer, addInfo.ParentCode);
                if (resModel != null && (!string.IsNullOrWhiteSpace(resModel.NewFJM) || !string.IsNullOrWhiteSpace(resModel.NM)))
                {
                    if (!string.IsNullOrWhiteSpace(resModel.NM))
                    {
                        // 修改该条数据基本信息
                        StringBuilder sqledit = new StringBuilder();
                        sqledit.AppendFormat("UPDATE MDMAgency SET Name='{0}', IsDetail='{1}',OrgCode='{2}',CreditCode = '{3}', ", addInfo.Name, addInfo.IsDetail, addInfo.OrgCode, addInfo.CreditCode);
                        // 单位类型码表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.TypeName))
                        {
                            sqledit.AppendFormat(" TypeCode={0}, TypeName='{1}', ", ConvertsData.GetCodeByName("AgencyType", addInfo.TypeName.Trim()), addInfo.TypeName.Trim());
                        }
                        // 预算单位级次码表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.LevelName))
                        {
                            sqledit.AppendFormat(" LevelCode={0}, LevelName='{1}', ", ConvertsData.GetCodeByName("AgencyLevel", addInfo.LevelName.Trim()), addInfo.LevelName.Trim());
                        }
                        // 人员情况码表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.PerKindName))
                        {
                            sqledit.AppendFormat(" PerKindCode={0} , PerKindName='{1}', ", ConvertsData.GetCodeByName("PersonKind", addInfo.PerKindName.Trim()), addInfo.PerKindName.Trim());
                        }
                        // 财政部内部机构码表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.MOFDepName))
                        {
                            sqledit.AppendFormat(" MOFDepCode={0} , MOFDepName='{1}', ", ConvertsData.GetCodeByName("MOFDept", addInfo.MOFDepName.Trim()), addInfo.MOFDepName.Trim());
                        }
                        // 单位行政级别码表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.AdmLevelName))
                        {
                            sqledit.AppendFormat(" AdmLevelCode={0} , AdmLevelName='{1}', ", ConvertsData.GetCodeByName("AdmLevel", addInfo.AdmLevelName.Trim()), addInfo.AdmLevelName.Trim());
                        }
                        // 性质分类码表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.PerKindName))
                        {
                            sqledit.AppendFormat(" XZTypeCode={0} , XZTypeName='{1}', ", ConvertsData.GetCodeByName("PropertyType", addInfo.XZTypeName.Trim()), addInfo.XZTypeName.Trim());
                        }
                        // 经费供给方式
                        if (!string.IsNullOrWhiteSpace(addInfo.FundsupName))
                        {
                            sqledit.AppendFormat(" FundsupCode={0} , FundsupName='{1}', ", ConvertsData.GetCodeByName("FundSupply", addInfo.FundsupName.Trim()), addInfo.FundsupName.Trim());
                        }
                        // 行业关联表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.IndName))
                        {
                            sqledit.AppendFormat(" IndNM=(select NM from MDMIndustry where Name = '{0}') , IndName='{0}', ", addInfo.IndName.Trim());
                        }
                        // 部门表示关联表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.FundsupName))
                        {
                            sqledit.AppendFormat(" SupDepCode= (select NM from MDMZGBM where Name = '{0}') , SupDepName='{0}', ", addInfo.SupDepName.Trim());
                        }
                        sqledit.AppendFormat(" Address='{0}', Note='{1}', ", addInfo.Address, addInfo.Note);
                        sqledit.AppendFormat(" Fax='{0}',Tel='{1}', ", addInfo.Fax, addInfo.Tel);
                        sqledit.AppendFormat(" LastModifiedUser='{0}',LastModifiedTime={1} ", DBUtility.GetOperateUser() + "导入", DBUtility.GetOperateDate());
                        sqledit.AppendFormat(" Where NM = '{0}' ", resModel.NM);
                        db.ExecuteSQL(sqledit.ToString());
                    }
                    else
                    {
                        // 走添加
                        switch (resModel.QuestionType)
                        {
                            case (int)ResInfoState.parentCodeEmpty:
                                result += "编号 " + addInfo.Code + "： 父级编号未维护，直接存为一级区划；<br/>";
                                break;
                            case (int)ResInfoState.parentCodeFail:
                                result += "编号 " + addInfo.Code + "： 父级编号在数据库中不存在，直接存为一级区划；<br/>";
                                break;
                            case (int)ResInfoState.layerFail:
                                result += "编号 " + addInfo.Code + "： 级数和父级级数相差不是1，故根据父级级数调整该数据的级数；<br/>";
                                break;
                        }
                        // 添加该条数据
                        StringBuilder addSql = new StringBuilder();
                        addSql.AppendLine(" INSERT INTO MDMAgency ( NM, Code, Name, OrgCode,CreditCode,TypeCode,TypeName,LevelCode, LevelName,");
                        addSql.AppendLine("  IndNM,IndName,PerKindCode, PerKindName, MOFDepCode, MOFDepName, ");
                        addSql.AppendLine(" SupDepNM, SupDepName, AdmLevelCode,AdmLevelName, XZTypeCode,XZTypeName, ");
                        addSql.AppendLine(" ParentNM,ParentCode,FundSupCode,FundsupName,Fax,Tel,Address, ");
                        addSql.AppendLine("  Note,FJM, Layer, IsDetail, TYBZ, AuditState, Createuser, Createtime, LastModifiedUser, LastModifiedTime ) VALUES  (  ");
                        addSql.AppendFormat("'{0}','{1}','{2}','{3}','{4}', ", System.Guid.NewGuid().ToString(), addInfo.Code, addInfo.Name, addInfo.OrgCode, addInfo.CreditCode);
                        // 单位类型
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("AgencyType", addInfo.TypeName), addInfo.TypeName);
                        // 单位级次
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("AgencyLevel", addInfo.LevelName), addInfo.LevelName);
                        // 行业关联表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.IndName))
                        {
                            // 行业关联
                            addSql.AppendFormat(" (select NM from MDMIndustry where Name = '{0}') ,'{0}', ", addInfo.IndName.Trim());
                        }
                        else
                        {
                            addSql.Append(" '','',");
                        }
                        // 人员情况
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("PersonKind", addInfo.PerKindName), addInfo.PerKindName);
                        // 财政部内部机构
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("MOFDept", addInfo.MOFDepName), addInfo.MOFDepName);

                        // 部门关联
                        if (!string.IsNullOrWhiteSpace(addInfo.FundsupName))
                        {
                            addSql.AppendFormat(" (select NM from MDMZGBM where Name = '{0}') , '{0}', ", addInfo.SupDepName.Trim());
                        }
                        else
                        {
                            addSql.Append(" '', '', ");
                        }

                        // 单位行政级别
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("AdmLevel", addInfo.AdmLevelName), addInfo.AdmLevelName);
                        // 性质分类
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("PropertyType", addInfo.XZTypeName), addInfo.XZTypeName);


                        // 父级信息
                        if (resModel.NewLayer != 1)
                        {
                            addSql.AppendFormat(" '{0}','{1}', ", resModel.ParentNM, addInfo.ParentCode);
                        }
                        else
                        {
                            addSql.AppendFormat(" '', '', ");
                        }
                        // 经费供给方式
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("FundSupply", addInfo.FundsupName), addInfo.FundsupName);
                        addSql.AppendFormat("'{0}','{1}', '{2}' , ", addInfo.Fax, addInfo.Tel, addInfo.Address);
                        addSql.AppendFormat("'{0}','{1}', {2} ,'{3}',  ", addInfo.Note, resModel.NewFJM, resModel.NewLayer, addInfo.IsDetail);
                        addSql.AppendFormat("'{0}','{1}',  ", (int)EnumTYBZ.enabled, (int)EnumAuditState.pass);
                        addSql.AppendFormat("'{0}',{1}, ", DBUtility.GetOperateUser() + "导入", DBUtility.GetOperateDate());
                        addSql.AppendFormat("'{0}',{1}) ", DBUtility.GetOperateUser() + "导入", DBUtility.GetOperateDate());
                        db.ExecuteSQL(addSql.ToString());
                    }
                }
            }
            return result;
        }

        #endregion

        #region 无效数据处理
        private string InvalidOperate(IList<MDMAgency> cList)
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
        /// 单位导出
        /// </summary>
        /// <param name="where"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public DataSet ExportData(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT  a.Code 单位层次代码, a.Name 单位名称, a.Layer 级数, a.CreditCode 统一社会信用代码, a.OrgCode 组织机构代码,CreditCodea.TypeName 单位类型, a.LevelName 单位级次, ");
            sb.AppendLine(" a.IndName 行业名称, a.PerKindName 人员情况, a.MOFDepName 财政部内部机构, a.SupDepName 部门名称, a.AdmLevelName  单位行政级别名称, ");
            sb.AppendLine(" a.XZTypeName  性质分类, b.Name 上级单位名称, a.FundsupName 经费供给方式, a.Fax 传真, a.Tel 电话, a.Address 地址, ");
            sb.AppendLine(" a.Note 备注, a.IsDetail 是否明细 ");
            sb.AppendLine(" FROM MDMAgency a LEFT JOIN MDMAgency b ON a.ParentNM = b.NM WHERE a.TYBZ='0' and a.AuditState='2'");
            sb.AppendLine(where);
            DataSet ds = db.ExecuteSQL(sb.ToString());
            return ds;
        }
        #endregion
    }
}
