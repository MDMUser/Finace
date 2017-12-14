using System.Collections.Generic;
using Genersoft.Platform.Resource.Metadata.Component.Attributes;
using Genersoft.Platform.BizComponent.BasicLib;
using FinanceMs.Common.Models.Extend;
using Newtonsoft.Json;
using System.Data;
using FinaceMs.WebDictHelper.WebDAL;

namespace FinaceMs.WebDictHelper
{
    /// <summary>
    /// 查询功能
    /// </summary>
    public class WebSearch : BaseBizComponent
    {
        [BizComponentMethod(PropertyCommit = "查询数据")]
        public string SearchDataByFilter(string dictName, string list)
        {
            SearchManage manage=new SearchManage();
            string result = string.Empty;
            List<WebFilter> listFilter = JsonConvert.DeserializeObject<List<WebFilter>>(list);
            return manage.GetSearchData(dictName, listFilter);
        }
    }
}
