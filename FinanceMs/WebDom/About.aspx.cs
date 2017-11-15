using System;
using WebDom.DBUtility;
using System.Data;
using System.Text;
using FinanceMs.Import;

namespace WebDom
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string testapi = "http://api.qida.yunxuetang.com.cn/";
            string api = "http://api.yunxuetang.cn/";

            XZQHOperate o = new XZQHOperate();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT TOP 10 *  ");
            sb.AppendLine(" FROM MDMOrgInfo ");
            DataSet ds = DbHelperSQL.Query(sb.ToString());
            o.ImportData(ds);
        }
    }
}
