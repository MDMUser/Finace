using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Genersoft.Platform.Engine.DataAccess;
using Genersoft.Platform.Engine.DataAccess.ClientSPI;
using Genersoft.MDM.Pub.Server.Com;
using FinanceMs.Common.Models;
using Genersoft.Platform.Core.DataAccess;
using Genersoft.Platform.AppFramework.Service;

namespace FinanceMs.Common
{
    public static class DBUtility
    {
        /// <summary>
        /// 通过导入的一条数据的code、layer、parentCode 获取该条数据生成的FJM、正确级数、父级内码
        /// </summary>
        /// <param name="db">数据库操作类</param>
        /// <param name="dictName">导入的表名</param>
        /// <param name="codeValue">导入数据的code</param>
        /// <param name="layer">导入数据的layer</param>
        /// <param name="parentCode">导入数据的父级code</param>
        /// <returns></returns>
        public static ResNewFJM GetNewFJMByDict(DataBaseEx db, string dictName, string codeValue, int layer, string parentCode)
        {
            ResNewFJM resModel = new ResNewFJM();
            // 内码字典名称、编号字段名称、级数字段名称
            string nmField = "", codeField = "", layerField = "";
            // 后期改为从系统字典表中获取
            switch (dictName)
            {
                case "MDMXZQH":
                case "MDMIndustry":
                case "MDMCSZD":
                case "MDMAgency":
                case "MDMZGBM":
                    nmField = "NM";
                    codeField = "code";
                    layerField = "layer";
                    break;
                case "":
                    break;
            }
            if (!string.IsNullOrWhiteSpace(nmField) && !string.IsNullOrWhiteSpace(codeField))
            {
                db.ResultNum = 1;
                DataSet ds = new DataSet();
                IDbDataParameter[] param = new IDbDataParameter[7];
                db.MakeInParam("@DictName", dictName, out param[0]);
                db.MakeInParam("@NMName", nmField, out param[1]);
                db.MakeInParam("@CodeName", codeField, out param[2]);
                db.MakeInParam("@LayerName", layerField, out param[3]);
                db.MakeInParam("@OurCode", codeValue, out param[4]);
                db.MakeInParam("@OurLayer", layer, out param[5]);
                db.MakeInParam("@ParentCode", ConvertsData.ValidNullString(parentCode, ""), out param[6]);
                db.RunProc("MDM_FinanceDict_GetNewFJM", param, out ds);
                if (ds != null && ds.Tables.Count > 0)
                {
                    var modelList = ConvertsData.DataTableToList<ResNewFJM>(ds.Tables[0]);
                    if (modelList != null && modelList.Count > 0)
                    {
                        resModel = modelList[0];
                    }
                }
            }
            return resModel;
        }

        /// <summary>
        /// 获取操作时间
        /// </summary>
        /// <returns></returns>
        public static string GetOperateDate()
        {
            IGSPDatabase dbGSP = GSPContext.Current.Database;
            string datetime = string.Empty;
            if (dbGSP.DbType == GSPDbType.Oracle)
                datetime = " to_date('" + System.DateTime.Now.ToString() + "','yyyy-mm-dd HH24:MI:SS')";
            else
                datetime = "'" + System.DateTime.Now.ToString() + "'";
            return datetime;
        }

        /// <summary>
        /// 获取操作员
        /// </summary>
        /// <returns></returns>
        public static string GetOperateUser()
        {
            //操作员
            string userName = GSPContext.Current.Session.UserName;
            return userName;
        }

    }
}
