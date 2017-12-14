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
                    result = GetAgencyData(listFilter);
                    break;
                case "MDMIndustry":
                    result = GetAgencyData(listFilter);
                    break;
                case "MDMAgency":
                    result = GetAgencyData(listFilter);
                    break;
                case "MDMCSZD":
                    result = GetAgencyData(listFilter);
                    break;
                case "MDMZGBM":
                    result = GetAgencyData(listFilter);
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
        /// <param name="listFilter">条件列表</param>
        /// <returns></returns>
        public IList<Identify> GetAgencyData(List<WebFilter> listFilter)
        {
            IList<Identify> NMList = null;
            List<string> whereList = new List<string>();
            MDMAgency model = new MDMAgency();
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
            string fliterWhere = string.Join(" AND ", whereList.ToArray());
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" WITH Emp(NM,ParentNM) AS ");
            sb.AppendLine("  (select NM, ParentNM from mdmagency WHERE ");
            sb.AppendLine(fliterWhere);
            sb.AppendLine(" UNION ALL ");
            sb.AppendLine(" SELECT d.NM, d.ParentNM ");
            sb.AppendLine(" FROM Emp INNER JOIN mdmagency d  ON d.NM = Emp.ParentNM) ");
            sb.AppendLine(" SELECT distinct NM FROM Emp ");
            DataSet ds = db.ExecuteSQL(sb.ToString());
            NMList = ConvertsData.DataTableToList<Identify>(ds.Tables[0]);
            return NMList;
        }
    }
}
