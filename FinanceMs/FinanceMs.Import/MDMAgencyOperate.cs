﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FinanceMs.Common.Models;
using Genersoft.MDM.Pub.Server.Com;
using System.Data;
using FinanceMs.Common;

namespace FinanceMs.Import
{
    public  class MDMAgencyOperate
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
                string[] list = { "单位层次代码", "单位名称", "单位层次","级数", "是否明细" };
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
                                                   || ConvertsData.ValidNullString(g.LevelName, "") == ""
                                                   || g.Layer <= 0
                                                   || !Verification.CharRangeOut(g.IsDetail, typeof(EnumIsDetail))
                                                ).ToArray();

                    editList = dwList.Where(g => ConvertsData.ValidNullString(g.Code, "") != ""
                                                   && ConvertsData.ValidNullString(g.Name, "") != ""
                                                   && ConvertsData.ValidNullString(g.LevelName, "") != ""
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
        /// 处理行政区划数据
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
                        sqledit.AppendFormat("UPDATE MDMAgency SET Name='{0}', IsDetail='{1}', ", addInfo.Name, addInfo.IsDetail);
                        // 单位类型码表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.TypeName))
                        {
                            sqledit.AppendFormat(" TypeCode={0}, TypeName='{1}', ", ConvertsData.GetCodeByName("", addInfo.TypeName.Trim()), addInfo.TypeName.Trim());
                        }
                        // 单位级次码表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.LevelName))
                        {
                            sqledit.AppendFormat(" LevelCode={0}, LevelName='{1}', ", ConvertsData.GetCodeByName("", addInfo.LevelName.Trim()), addInfo.LevelName.Trim());
                        }
                        // 人员情况码表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.PerKindName))
                        {
                            sqledit.AppendFormat(" PerKindCode={0} , PerKindName='{1}', ", ConvertsData.GetCodeByName("", addInfo.PerKindName.Trim()), addInfo.PerKindName.Trim());
                        }
                        // 财政部内部机构码表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.MOFDepName))
                        {
                            sqledit.AppendFormat(" MOFDepCode={0} , MOFDepName='{1}', ", ConvertsData.GetCodeByName("", addInfo.MOFDepName.Trim()), addInfo.MOFDepName.Trim());
                        }


                        // 单位行政级别码表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.AdmLevelName))
                        {
                            sqledit.AppendFormat(" AdmLevelCode={0} , AdmLevelName='{1}', ", ConvertsData.GetCodeByName("", addInfo.AdmLevelName.Trim()), addInfo.AdmLevelName.Trim());
                        }

                        // 性质分类码表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.PerKindName))
                        {
                            sqledit.AppendFormat(" XZTypeCode={0} , XZTypeName='{1}', ", ConvertsData.GetCodeByName("", addInfo.XZTypeName.Trim()), addInfo.XZTypeName.Trim());
                        }

                        // 人员情况码表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.FundsupName))
                        {
                            sqledit.AppendFormat(" FundsupCode={0} , FundsupName='{1}', ", ConvertsData.GetCodeByName("", addInfo.FundsupName.Trim()), addInfo.FundsupName.Trim());
                        }


                        // 行业关联表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.IndName))
                        {
                            sqledit.AppendFormat(" IndCode={0} , IndName='{1}', ", ConvertsData.GetCodeByName("", addInfo.IndName.Trim()), addInfo.IndName.Trim());
                        }


                        // 部门表示关联表对接
                        if (!string.IsNullOrWhiteSpace(addInfo.FundsupName))
                        {
                            sqledit.AppendFormat(" SupDepCode={0} , SupDepName='{1}', ", ConvertsData.GetCodeByName("", addInfo.SupDepName.Trim()), addInfo.SupDepName.Trim());
                        }

                        sqledit.AppendFormat(" ParentNM='{0}',ParentCode='{1}', ", resModel.ParentNM, addInfo.ParentCode);
                        sqledit.AppendFormat(" Address='{0}',OrgCode='{1}', Note='{2}', ", addInfo.Address, addInfo.OrgCode, addInfo.Note);
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
                        addSql.AppendLine(" INSERT INTO MDMAgency ( NM, Code, Name, OrgCode,TypeName,TypeCode,LevelName,LevelCode, ");
                        addSql.AppendLine(" IndName,IndCode,PerKindName,PerKindCode,MOFDepName,MOFDepCode, ");
                        addSql.AppendLine(" SupDepName,SupDepCode,AdmLevelName,AdmLevelCode,XZTypeName,XZTypeCode, ");
                        addSql.AppendLine(" ParentNM,ParentCode,FundSupCode,FundsupName,Fax,Tel,Address, ");
                        addSql.AppendLine("  Note,FJM, Layer, IsDetail, TYBZ,TYND,AuditState,isTrans,Transcope,Createuser, Createtime ) VALUES  (  ");
                        addSql.AppendFormat("'{0}','{1}','{2}', ", System.Guid.NewGuid().ToString(), addInfo.Code, addInfo.Name);
                        // 单位类型
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("", addInfo.TypeName), addInfo.TypeName);
                        // 单位级次
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("", addInfo.LevelName), addInfo.LevelName);
                        // 行业关联
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("", addInfo.IndName), addInfo.IndName);
                        // 人员情况
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("", addInfo.PerKindName), addInfo.PerKindName);
                        // 财政部内部机构
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("", addInfo.MOFDepName), addInfo.MOFDepName);
                        // 部门关联
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("", addInfo.SupDepName), addInfo.SupDepName);
                        // 单位行政级别
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("", addInfo.AdmLevelName), addInfo.AdmLevelName);
                        // 性质分类
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("", addInfo.XZTypeName), addInfo.XZTypeName);
                        // 经费供给方式
                        addSql.AppendFormat(" {0},'{1}', ", ConvertsData.GetCodeByName("", addInfo.FundsupName), addInfo.FundsupName);
                       
                       

                        // 父级信息
                        if (resModel.NewLayer != 1)
                        {
                            addSql.AppendFormat(" '{0}','{1}', ", resModel.ParentNM, addInfo.ParentCode);
                        }
                        else
                        {
                            addSql.AppendFormat(" '', '', ");
                        }
                        
                        

                        addSql.AppendFormat("'{0}','{1}', {2} ,'{3}',  ", addInfo.Note, resModel.NewFJM, resModel.NewLayer, addInfo.IsDetail);
                        addSql.AppendFormat("'{0}','{1}',  ", (int)EnumAuditState.pass, (int)EnumTYBZ.enabled);
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
                info += "编号 " + cList[i].Code + "，名称 " + cList[i].Name + "： 信息编号、名称、级数、是否明细不全无法导入；" + System.Environment.NewLine;
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
            sb.AppendLine(" SELECT a.NM 单位内码, a.Code 单位编号, a.Name 单位名称, a.OrgCode 组织机构代码,a.TypeName 单位类型, a.LevelName 单位级次, ");
            sb.AppendLine(" a.IndName 行业名称, a.PerKindName 人员情况, a.MOFDepName 财政部内部机构, a.SupDepName 部门名称, a.AdmLevelName  单位行政级别名称, ");
            sb.AppendLine(" a.XZTypeName  性质分类, b.Name 上级单位名称, a.FundsupName 经费供给方式, a.Fax 传真, a.Tel 电话, a.Address 地址, ");
            sb.AppendLine(" a.Note 备注, a.IsDetail 是否明细, a.TYBZ 停用标志, a.TYND 停用年度, a.AuditState 审批状态, ");
            sb.AppendLine(" a.Createuser 创建人, a.Createtime 创建时间, a.LastModifiedUser 最后修改人, a.LastModifiedTime 最后修改时间");
            sb.AppendLine(" FROM MDMAgency a LEFT JOIN MDMAgency b ON a.ParentNM = b.NM ");                            
            sb.AppendLine(where);
            DataSet ds = db.ExecuteSQL(sb.ToString());
            return ds;
        }
        #endregion
    }
}