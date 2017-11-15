using System;
using System.Web;
using System.IO;
using System.Data;
using Genersoft.Platform.AppFramework.Service;
using System.Collections;
using FinanceMs.Common;
using FinanceMs.Import;

namespace FinanceMs.UploadServer.ImpExpWeb
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class FinaceExporter : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            #region 构造GSPState
            string AppInstanceID = context.Request.Form["AppInstanceID"],
            userCode = context.Request.Form["UserCode"],
            userID = context.Request.Form["UserId"],
            ProcessID = context.Request.Form["ProcessID"],
            bizDate = context.Request.Form["BizDate"],//业务日期
            LoginDate = context.Request.Form["LoginDate"],//登录日期
            ClientIP = context.Request.Form["ClientIP"].Replace('-', '.'),//客户端IP
            FrameType = context.Request.Form["FrameType"],//框架类型
            UserName = context.Request.Form["UserName"];//用户名称
            if (string.IsNullOrEmpty(userID))
            {
                context.Response.Write("<script> alert('错误信息:用户未登录或已掉线，请登录！');window.close();</script>");
                return;
            }
            if (string.IsNullOrEmpty(AppInstanceID))
            {
                context.Response.Write("<script> alert('错误信息:AppInstanceID为空值！');window.close();</script>");
                return;
            }
            if (string.IsNullOrEmpty(userCode))
            {
                context.Response.Write("<script> alert('错误信息:userCode为空值！');window.close();</script>");
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

            string vsZdbh = context.Request.Form["zdbh"],
            vsWhere = context.Request.Form["vsWhere"];
            
            string filePath = String.Empty;
            string vsMsg = String.Empty;
            string fileName = SaveExcel(vsZdbh, vsWhere, ref filePath, ref vsMsg);

            if (vsMsg != String.Empty)
            {
                context.Response.Write("<script> alert('错误信息:" + vsMsg + "');window.close();</script>");
                return;
            }

            DownloadExcel(context, fileName);

            DeleteExcel(filePath);
        }

      

        /// <summary>
        /// 将数据保存到excel中
        /// </summary>
        /// <param name="psZdbh">字典名称</param>
        /// <param name="psWhere">查询条件</param>
        /// <param name="filePath">返回的文件路径</param>
        /// <param name="psMsg">返回的信息</param>
        /// <returns></returns>
        private string SaveExcel(string psZdbh, string psWhere, ref string filePath, ref string psMsg)
        {
            string vsMsg = String.Empty;
            OutManage export = new OutManage();
            DataSet dsData = export.GetExportData(psZdbh, psWhere, ref vsMsg);
            if (vsMsg != String.Empty)
            {
                psMsg += vsMsg;
            }
            if (dsData == null || dsData.Tables.Count == 0)
            {
                psMsg += "获取数据库数据失败！";
                return String.Empty;
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            time = time.Replace("-", "").Replace(":", "").Replace(" ", "");
            string fileName = psZdbh + time + ".xlsx";
            string basePath = AppDomain.CurrentDomain.BaseDirectory + @"MDMWeb\MDMTempFile";
            filePath = basePath + @"\" + fileName;
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
            try
            {
                ConvertsData excelOper = new ConvertsData();
                excelOper.DataSetToExcel(dsData, filePath);
            }
            catch
            {
                psMsg += "导出Excel文件失败！";
                return String.Empty;
            }

            return fileName;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        private void DeleteExcel(string filePath)
        {
            File.Delete(filePath);
        }

        /// <summary>
        /// 读取并下载Excel文件
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fileName"></param>
        private void DownloadExcel(HttpContext context,string fileName)
        {
            string url = "/cwbase/MDMWeb/MDMTempFile/" + fileName;
            //判断服务端是否生成Excel文件
            if (File.Exists(context.Server.MapPath(url)))
            {
                //获取文件路径和文件名           
                string filePath = context.Server.MapPath(url);
                //将文件转换为字节流
                FileStream fs = new FileStream(filePath, FileMode.Open);
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();

                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    //文件导出到客户端(Web)
                    ResponseFile(context.Request, context.Response, fileName, stream, 10240);
                }
            }
            else
            {
                context.Response.Write("<script> alert('错误信息:未导出Excel文件');window.close();</script>");
            }
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="_Request">Request对象</param>
        /// <param name="_Response">Response对象</param>
        /// <param name="_fileName">文件名</param>
        /// <param name="stream">发送的流</param>
        /// <param name="block">每次读取的字节数</param>
        /// <returns></returns>
        private bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, Stream stream, int block)
        {
            try
            {
                _Response.Clear();
                using (var br = new BinaryReader(stream))
                {

                    _Response.AddHeader("Accept-Ranges", "bytes");
                    _Response.Buffer = false;
                    long fileLength = stream.Length;
                    long startBytes = 0;

                    int pack = block; //每次读取的字节数
                    if (_Request.Headers["Range"] != null)
                    {
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        _Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }
                    _Response.AddHeader("Connection", "Keep-Alive");
                    _Response.ContentType = "application/octet-stream";

                    _Response.AddHeader("Content-Disposition", "attachment;filename=" + Uri.EscapeUriString(_fileName));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((fileLength - startBytes) / (double)pack) + 1;

                    for (int i = 0; i < maxCount; i++)
                    {
                        if (_Response.IsClientConnected)
                        {
                            _Response.BinaryWrite(br.ReadBytes(pack));
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                    br.Close();
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                _Response.Write(e.Message + e.StackTrace);
                _Response.Flush();
            }
            return true;
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