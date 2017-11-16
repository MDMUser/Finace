using FinanceMs.Common;
using Genersoft.Platform.BizComponent.BasicLib;
using Genersoft.Platform.Resource.Metadata.Component.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinaceMs.WebDictHelper
{
    /// <summary>
    /// web端行政区划引用类
    /// </summary>
    public class WebXZQHComponent : BaseBizComponent
    {
        /// <summary>
        /// 行政区划调整级次
        /// </summary>
        /// <param name="curentNM">当前需要调整的内码</param>
        /// <param name="newParentNM">待调整到的新的父级内码</param>
        /// <returns></returns>
        [BizComponentMethod(PropertyCommit = "行政区划调整级次")]
        public string AdjustLevels(string curentNM, string newParentNM)
        {
            var result = new
            {
                code = (int)EnumAdjustState.ok,
                context = ""
            };
            //var res= 
            return "";
        }
    }
}
