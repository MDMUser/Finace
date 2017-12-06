using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FinanceMs.Import
{
    /// <summary>
    /// 导出类
    /// </summary>
    public class OutManage
    {
        /// <summary>
        /// 执行导出
        /// </summary>
        /// <param name="psZdbh"></param>
        /// <param name="psWhere"></param>
        /// <param name="psMsg"></param>
        public DataSet GetExportData(string psZdbh, string psWhere, ref string psMsg)
        {
            DataSet result = null;
            switch (psZdbh)
            {
                case "MDMXZQH":
                    result = new XZQHOperate().ExportData(psWhere);
                    break;
                case "MDMIndustry":
                    result = new MDMIndustryOperate().ExportData(psWhere);
                    break;
                case "MDMAgency":
                    result = new MDMAgencyOperate().ExportData(psWhere);
                    break;
                case "MDMCSZD":
                    result = new CSZDOperate().ExportData(psWhere);
                    break;
                case "MDMZGBM":
                    result = new ZGBMOperate().ExportData(psWhere);
                    break;
                default:
                    psMsg += "尚未配置字典【" + psZdbh + "】的导出程序，请联系系统管理员！";
                    break;
            }
            return result;
        }
    }
}
