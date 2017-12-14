using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FinanceMs.Common.Models.Extend;
using FinanceMs.Common.Models;
using System.Reflection;
using Genersoft.MDM.Pub.Server.Com;
using FinanceMs.Common;

namespace FinaceMs.WebDictHelper.WebDAL
{
    public class SearchManage
    {
        private readonly DataBaseEx db = new DataBaseEx();
        /// <summary>
        /// 查询功能
        /// </summary>
        /// <param name="dictName"></param>
        /// <param name="where"></param>
        public string GetSearchData(string dictName, List<WebFilter> listFilter)
        {
            IList<Identify> result = null;
            string paraNM = "";
            switch (dictName)
            {
                case "MDMXZQH":
                    MDMXZQH xzqh = new MDMXZQH();
                    result = GetDictData<MDMXZQH>(dictName, listFilter, xzqh);
                    break;
                case "MDMIndustry":
                    MDMIndustry industry = new MDMIndustry();
                    result = GetDictData<MDMIndustry>(dictName, listFilter, industry);
                    break;
                case "MDMAgency":
                    MDMAgency agency = new MDMAgency();
                    result = GetDictData<MDMAgency>(dictName, listFilter, agency);
                    break;
                case "MDMCSZD":
                    MDMCSZD cs = new MDMCSZD();
                    result = GetDictData<MDMCSZD>(dictName, listFilter, cs);
                    break;
                case "MDMZGBM":
                    MDMZGBM zgbm = new MDMZGBM();
                    result = GetDictData<MDMZGBM>(dictName, listFilter, zgbm);
                    break;
            }
            if (result != null && result.Count > 0)
            {
                for (int i = 0; i < result.Count; i++)
                {

                    if (i == result.Count - 1)
                    {
                        paraNM += "'" + result[i].NM + "'";
                    }
                    else
                    {
                        paraNM += "'" + result[i].NM + "',";
                    }
                }
            }
            return paraNM;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T">model</typeparam>
        /// <param name="dictName">字典名称</param>
        /// <param name="listFilter">条件列表</param>
        /// <returns></returns>
        private IList<Identify> GetDictData<T>(string dictName, List<WebFilter> listFilter, T model)
        {
            IList<Identify> NMList = null;
            List<string> whereList = new List<string>();
            for (int i = 0; i < listFilter.Count; i++)
            {
                var filter = listFilter[i];
                PropertyInfo _info = model.GetType().GetProperty(filter.field);
                if (_info != null)
                {
                    string where = filter.field + " like " + "'%" + filter.value + "%'";
                    whereList.Add(where);
                }
            }
            string filterWhere = string.Join(" AND ", whereList.ToArray());
            DataSet ds = db.ExecuteSQL(GetSQLByFilters(dictName, filterWhere));
            NMList = ConvertsData.DataTableToList<Identify>(ds.Tables[0]);
            return NMList;
        }

        /// <summary>
        /// 获取某字典的查询sql
        /// </summary>
        /// <param name="dictName">字典名</param>
        /// <param name="filters">条件</param>
        /// <returns></returns>
        private string GetSQLByFilters(string dictName, string filters)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" WITH Emp(NM,ParentNM) AS ");
            sb.AppendLine("  (select NM, ParentNM from mdmagency WHERE ");
            sb.AppendLine(filters);
            sb.AppendLine(" UNION ALL ");
            sb.AppendLine(" SELECT d.NM, d.ParentNM ");
            sb.AppendFormat(" FROM Emp INNER JOIN {0} d  ON d.NM = Emp.ParentNM) ", dictName);
            sb.AppendLine(" SELECT distinct NM FROM Emp ");
            return sb.ToString();
        }
    }
}
