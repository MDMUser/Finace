using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genersoft.MDM.WEB.Server.Com;
using System.Data;
using FinanceMs.Common;

namespace FinanceMs.Import
{
    public class ImportManage
    {
        /// <summary>
        /// 执行导入
        /// </summary>
        /// <param name="psZdbh"></param>
        /// <param name="dsData"></param>
        /// <param name="psMsg"></param>
        public void ImportData(string psZdbh, DataSet dsData, ref string psMsg)
        {
            switch (psZdbh)
            {
                case "MDMXZQH":
                    psMsg = new XZQHOperate().ImportData(dsData);
                    break;
                case "MDMIndustry":
                    psMsg = new MDMIndustryOperate().ImportData(dsData);
                    break;
                case "MDMAgency":
                    psMsg = new MDMAgencyOperate().ImportData(dsData);
                case "MDMCSZD":
                    psMsg = new CSZDOperate().ImportData(dsData);
                    break;
                case "MDMZGBM":
                    psMsg = new ZGBMOperate().ImportData(dsData);
                    break;
            }
        }
    }
}
