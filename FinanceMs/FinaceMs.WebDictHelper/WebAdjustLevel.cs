﻿using FinanceMs.Common;
using Genersoft.Platform.BizComponent.BasicLib;
using Genersoft.Platform.Resource.Metadata.Component.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genersoft.MDM.Pub.Server.Com;
using Newtonsoft.Json;

namespace FinaceMs.WebDictHelper
{
    /// <summary>
    /// web端行政区划引用类
    /// </summary>
    public class WebAdjustLevel : BaseBizComponent
    {
        private readonly DataBaseEx db = new DataBaseEx();
        private readonly FinaceLog log = new FinaceLog();
        /// <summary>
        /// 调整级次
        /// </summary>
        /// <param name="curentNM">当前需要调整的内码</param>
        /// <param name="newParentNM">待调整到的新的父级内码</param>
        /// <returns></returns>
        [BizComponentMethod(PropertyCommit = "调整级次")]
        public string AdjustLevels(string dictName, string curentNM, string newParentNM)
        {
            int code;
            string context = "";
            if (!string.IsNullOrWhiteSpace(curentNM))
            {
                // 调整级次
                log.Info(DBUtility.GetOperateUserCode() + "进行级次调整:将 " + curentNM + " 调整到 " + newParentNM);
                code = DBUtility.ChangeParentInfoByDict(db, dictName, curentNM, newParentNM);
            }
            else
            {
                code = (int)EnumAdjustState.infoFail;
            }
            switch (code)
            {
                case (int)EnumAdjustState.failure:
                    context = "参数传递有误";
                    break;
                case (int)EnumAdjustState.ok:
                    context = "成功";
                    break;
                case (int)EnumAdjustState.infoFail:
                    context = "所传内码无效";
                    break;
                case (int)EnumAdjustState.busIssue:
                    context = "不允许将父级节点调整到子集节点下";
                    break;
                case (int)EnumAdjustState.unchanged:
                    context = "父级节点未改变，无需调整";
                    break;
            }
            var obj = new
            {
                code = code,
                context = context
            };
            return JsonConvert.SerializeObject(obj);
        }
    }
}
