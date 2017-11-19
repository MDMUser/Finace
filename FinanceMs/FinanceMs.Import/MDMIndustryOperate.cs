using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genersoft.MDM.Pub.Server.Com;
using System.Data;
using FinanceMs.Common;
using FinanceMs.Common.Models;

namespace FinanceMs.Import
{
    public class MDMIndustryOperate
    {
        private readonly DataBaseEx db = new DataBaseEx();

        #region 导入
        /// <summary>
        /// 行业导入
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
                string[] list = { "行业编码", "行业名称", "级数", "是否明细" };
                msg = Verification.ImportColumns(data.Tables[0].Columns, list);
                if (!string.IsNullOrWhiteSpace(msg))
                    return msg;

                // ②将dataTable转为list
                var hyList = ConvertsData.DataTableToListByProperties<MDMIndustry>(dtData);

                // ③筛选出添加、修改、无效数据
                // 声明(添加、修改)和 无效list
                IList<MDMIndustry> editList = new List<MDMIndustry>(),
                            invalidList = new List<MDMIndustry>();

                if (hyList != null && hyList.Count > 0)
                {
                    invalidList = hyList.Where(g => ConvertsData.ValidNullString(g.Code, "") == ""
                                                   || ConvertsData.ValidNullString(g.Name, "") == ""
                                                   || g.Layer <= 0
                                                   || !Verification.CharRangeOut(g.IsDetail, typeof(EnumIsDetail))
                                                ).ToArray();

                    editList = hyList.Where(g => ConvertsData.ValidNullString(g.Code, "") != ""
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
        /// 处理行业数据
        /// </summary>
        /// <param name="cList"></param>
        /// <returns></returns>
        private string EditListOperate(IList<MDMIndustry> cList)
        {
            string result = "";
            for (int i = 0; i < cList.Count; i++)
            {
                MDMIndustry addInfo = cList[i];
                var resModel = DBUtility.GetNewFJMByDict(db, "MDMIndustry", addInfo.Code, addInfo.Layer, addInfo.ParentCode);
                if (resModel != null && (!string.IsNullOrWhiteSpace(resModel.NewFJM) || !string.IsNullOrWhiteSpace(resModel.NM)))
                {
                    if (!string.IsNullOrWhiteSpace(resModel.NM))
                    {
                        // 修改该条数据基本信息
                        StringBuilder sqledit = new StringBuilder();

                        sqledit.AppendFormat("UPDATE MDMIndustry SET Name='{0}', IsDetail='{1}', ", addInfo.Name, addInfo.IsDetail);
                        sqledit.AppendFormat(" Note='{0}', ", addInfo.Note);
                        if (Enum.IsDefined(typeof(EnumIndustryType), ConvertsData.ValidNullString(addInfo.Type, "")))
                        {
                            sqledit.AppendFormat(" Type= '{0}',", (int)Enum.Parse(typeof(EnumIndustryType), addInfo.Type));
                        }

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
                        addSql.AppendLine(" INSERT INTO MDMIndustry ( NM, Code, Name, Type, ParentNM,ParentCode, ");
                        addSql.AppendLine("  Note,FJM, Layer, IsDetail, AuditState, TYBZ, CreateUser, CreateTime ) VALUES  (  ");
                        addSql.AppendFormat("'{0}','{1}','{2}', ", System.Guid.NewGuid().ToString(), addInfo.Code, addInfo.Name);
                        if (Enum.IsDefined(typeof(EnumIndustryType), ConvertsData.ValidNullString(addInfo.Type, "")))
                        {
                            addSql.AppendFormat(" '{0}', ", (int)Enum.Parse(typeof(EnumIndustryType), addInfo.Type));
                        }
                        else
                        {
                            addSql.Append("  '', ");
                        }
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
        private string InvalidOperate(IList<MDMIndustry> cList)
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
        /// 行业导出
        /// </summary>
        /// <param name="where"></param>，
        /// <param name="message"></param>
        /// <returns></returns>
        public DataSet ExportData(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT a.Code 行业编码,a.Name 行业名称,case  a.Type when '0' then '门' when '1' then '大' when '2' then '中' END 行业类别, b.Name 上级行业名称, ");
            sb.AppendLine(" a.Note 备注,a.Layer 级数, a.IsDetail 是否明细 ");
            sb.AppendLine(" FROM MDMIndustry a left join MDMIndustry b on a.parentNM = b.NM  WHERE a.TYBZ='0' and a.AuditState='2' ");
            sb.AppendLine(where);
            sb.AppendLine(" order by a.layer ");
            DataSet ds = db.ExecuteSQL(sb.ToString());
            return ds;
        }
        #endregion
    }
}
