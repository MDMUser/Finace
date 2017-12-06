using System;
using System.Web;
using System.Collections;
using System.IO;
using System.Data;
using Genersoft.Platform.AppFramework.Service;
using FinanceMs.Common;
using FinanceMs.Import;
using Newtonsoft.Json;

namespace FinanceMs.UploadServer.ImpExpWeb
{
    /// <summary>
    /// FinaceImporter 的摘要说明
    /// </summary>
    public class FinaceImporter : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            #region 构造GSPState
            string AppInstanceID = context.Request.Form["AppInstanceID"],
            userCode = HttpUtility.UrlDecode(context.Request.Form["UserCode"]),
            userID = context.Request.Form["UserId"],
            ProcessID = context.Request.Form["ProcessID"],
            bizDate = context.Request.Form["BizDate"],//业务日期
            LoginDate = context.Request.Form["LoginDate"],//登录日期
            ClientIP = context.Request.Form["ClientIP"].Replace('-', '.'),//客户端IP
            FrameType = context.Request.Form["FrameType"],//框架类型
            UserName = HttpUtility.UrlDecode(context.Request.Form["UserName"]);//用户名称
            if (string.IsNullOrEmpty(userID))
            {
                string s1 = System.Web.HttpUtility.UrlEncode("错误信息", System.Text.Encoding.UTF8);
                string errContent = System.Web.HttpUtility.UrlEncode("用户未登录或已掉线，请登录！", System.Text.Encoding.UTF8);
                context.Response.Write("{\"result\":\"1\",\"context\":\"" + s1 + ":" + errContent + "\"}");
                return;
            }
            if (string.IsNullOrEmpty(AppInstanceID))
            {
                string s1 = System.Web.HttpUtility.UrlEncode("错误信息", System.Text.Encoding.UTF8);
                string errContent = System.Web.HttpUtility.UrlEncode("AppInstanceID为空值！", System.Text.Encoding.UTF8);
                context.Response.Write("{\"result\":\"1\",\"context\":\"" + s1 + ":" + errContent + "\"}");
                return;
            }
            if (string.IsNullOrEmpty(userCode))
            {
                string s1 = System.Web.HttpUtility.UrlEncode("错误信息", System.Text.Encoding.UTF8);
                string errContent = System.Web.HttpUtility.UrlEncode("userCode为空值！", System.Text.Encoding.UTF8);
                context.Response.Write("{\"result\":\"1\",\"context\":\"" + s1 + ":" + errContent + "\"}");
                return;
            }

            //使用Hashtable，构建GSPState，避免GSPState各属性的大小写问题
            GSPState.IgnoreCheck();
            Hashtable hashTable = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            hashTable.Add("UserID", userID);
            hashTable.Add("UserCode", userCode);
            hashTable.Add("ProcessID", ProcessID);
            hashTable.Add("AppInstanceID", AppInstanceID);
            hashTable.Add("BizDate", bizDate);
            hashTable.Add("LoginDate", LoginDate);
            hashTable.Add("ClientIP", ClientIP);
            hashTable.Add("FrameType", FrameType);
            hashTable.Add("UserName", UserName);
            GSPState _newState = new GSPState(hashTable);
            GSPState.SetServerState(_newState);
            #endregion 构造GSPState

            string vsZdbh = context.Request.Form["MDMZdbh"]; //字典编号

            string tempFilePath = String.Empty;
            try
            {
                foreach (string file in context.Request.Files)
                {
                    HttpPostedFile hpf = context.Request.Files[file] as HttpPostedFile;
                    string FileName = string.Empty;
                    if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
                    {
                        string[] files = hpf.FileName.Split(new char[] { '\\' });
                        FileName = files[files.Length - 1];
                    }
                    else
                    {
                        FileName = hpf.FileName;
                    }
                    string extension = System.IO.Path.GetExtension(FileName);//扩展名

                    string basePath = AppDomain.CurrentDomain.BaseDirectory + @"MDMWeb\MDMTempFile";
                    tempFilePath = basePath + @"\temp" + extension;
                    if (Directory.Exists(basePath) == false)//判断安装目录下的MDM文件夹是否存在     
                    {
                        try
                        {
                            Directory.CreateDirectory(basePath);//创建安装目录下的MDM文件夹     
                        }
                        catch
                        {
                            //throw ex;
                        }
                    }
                    hpf.SaveAs(tempFilePath);

                    ConvertsData excelOper = new ConvertsData();
                    DataSet dsData = excelOper.ExcelToDataSet(tempFilePath);

                    if (dsData.Tables.Count == 0 || dsData.Tables[0].Rows.Count == 0)
                    {
                        string s1 = System.Web.HttpUtility.UrlEncode("错误信息", System.Text.Encoding.UTF8);
                        string errContent = System.Web.HttpUtility.UrlEncode("导入的Excel中不存在数据。", System.Text.Encoding.UTF8);
                        context.Response.Write("{\"result\":\"1\",\"context\":\"" + s1 + ":" + errContent + "\"}");
                        return;
                    }
                    string vsMsg = String.Empty;
                    ImportManage importManage = new ImportManage();
                    importManage.ImportData(vsZdbh, dsData, ref vsMsg);
                    if (!string.IsNullOrWhiteSpace(vsMsg))
                    {
                        // vsMsg = System.Web.HttpUtility.UrlEncode("导入出错，请检查数据是否正确。", System.Text.Encoding.UTF8);
                        var result = new
                        {
                            result = "1",
                            context = HttpUtility.UrlEncode(vsMsg, System.Text.Encoding.UTF8)
                        };

                        context.Response.Write(JsonConvert.SerializeObject(result));
                        return;
                    }
                    else
                    {
                        context.Response.Write("{\"result\":\"0\",\"context\":\"\"}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                string errMessage = System.Web.HttpUtility.UrlEncode("导入出错，请检查数据是否正确。", System.Text.Encoding.UTF8);
                context.Response.Write("{\"result\":\"1\",\"context\":\"" + errMessage + "\"}");
            }
            finally
            {
                if (tempFilePath != String.Empty)
                {
                    File.Delete(tempFilePath);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}