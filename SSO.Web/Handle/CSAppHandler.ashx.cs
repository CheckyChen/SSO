using SSO.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using SSO.Data;
using System.Data;
using System.Web.SessionState;

namespace SSO.Web.Handle
{
    /// <summary>
    /// CSAppHandler 的摘要说明
    /// </summary>
    [HandlerLogin]
    public class CSAppHandler : IHttpHandler, IRequiresSessionState
    {
        Log log = LogFactory.GetLogger(typeof(CSAppHandler));
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string op = context.Request.QueryString["op"];
            string applicationId = string.Empty;

            //if (OperatorProvider.Provider.GetCurrent() == null)
            //{
            //    context.Response.Write("<script>top.location.href = '/Login/Index';</script>");
            //    HttpContext.Current.ApplicationInstance.CompleteRequest();

            //}
            //else
            //{
            switch (op)
            {
                //提交应用程序表单
                case "SubmitForm":

                    string postData_app = context.Request["postData"];
                    applicationId = context.Request["ApplicationId"];
                    context.Response.Write(SubmitForm(postData_app, applicationId).ToJson());
                    break;

                //获取应用程序表单
                case "GetApplicationEntity":

                    applicationId = context.Request["ApplicationId"];
                    context.Response.Write(GetApplicationEntity(applicationId).ToJson());
                    break;

                //删除应用程序
                case "DeleteApp":
                    applicationId = context.Request["ApplicationId"];
                    context.Response.Write(new CSApplication_BLL().Delete(applicationId).ToJson());
                    break;
                //获取应用列表
                case "GetApplicationList":

                    string keywords = HttpContext.Current.Server.UrlDecode(context.Request["keywords"]);
                    int rows = Convert.ToInt32(context.Request["rows"]);//每页的大小
                    int page = Convert.ToInt32(context.Request["page"]);//第几页
                    string sidx = context.Request["sidx"];//排序
                    string sord = context.Request["sord"];//排序方式

                    Pagination pagination = new Pagination();
                    pagination.page = page;
                    pagination.rows = rows;
                    pagination.sidx = sidx;
                    pagination.sord = sord;
                    DataTable dt = new CSApplication_BLL().FindList(keywords, pagination);
                    var data = new
                    {
                        rows = dt,
                        records = pagination.records,
                        page = pagination.page,
                        total = pagination.total
                    };
                    context.Response.Write(data.ToJson());
                    break;

                //上传文件
                case "UploadFile":
                    UploadFile(context);
                    break;

                //合并文件
                case "MergeFile":
                    MergeFile(context);
                    break;

                //没有合并时的保存
                case "AppVersionAdd":
                    AppVersionAdd(context);
                    break;

                //删除版本
                case "DeleleVersion":
                    string versionId = context.Request["AppVersionId"];
                    context.Response.Write(DeleleVersion(versionId, context).ToJson());
                    break;
                case "GetPreviVersion":
                    applicationId = context.Request["ApplicationId"];
                    context.Response.Write(new CSApplication_BLL().GetPreviVersion(applicationId));
                    break;

                //新增版本文件
                case "VersionFileAdd":
                    VersionFileAdd(context);
                    break;

                //获取日志                    
                case "GetLogList":

                    string keywords_1 = HttpContext.Current.Server.UrlDecode(context.Request["keywords"]);
                    int rows_1 = Convert.ToInt32(context.Request["rows"]);//每页的大小
                    int page_1 = Convert.ToInt32(context.Request["page"]);//第几页
                    string sidx_1 = context.Request["sidx"];//排序
                    string sord_1 = context.Request["sord"];//排序方式

                    Pagination pagination_1 = new Pagination();
                    pagination_1.page = page_1;
                    pagination_1.rows = rows_1;
                    pagination_1.sidx = sidx_1;
                    pagination_1.sord = sord_1;
                    DataTable dt_1 = new CSLog_BLL().FindList(keywords_1, pagination_1);
                    var data_1 = new
                    {
                        rows = dt_1,
                        records = pagination_1.records,
                        page = pagination_1.page,
                        total = pagination_1.total
                    };
                    context.Response.Write(data_1.ToJson());
                    break;

                //获取WCF版本文件列表
                case "GetWCFVersionList":

                    string keywords_2 = HttpContext.Current.Server.UrlDecode(context.Request["keywords"]);
                    int rows_2 = Convert.ToInt32(context.Request["rows"]);//每页的大小
                    int page_2 = Convert.ToInt32(context.Request["page"]);//第几页
                    string sidx_2 = context.Request["sidx"];//排序
                    string sord_2 = context.Request["sord"];//排序方式

                    Pagination pagination_2 = new Pagination();
                    pagination_2.page = page_2;
                    pagination_2.rows = rows_2;
                    pagination_2.sidx = sidx_2;
                    pagination_2.sord = sord_2;
                    DataTable dt_2 = new CSWCFVersionInfo_BLL().FindList(keywords_2, pagination_2);
                    var data_2 = new
                    {
                        rows = dt_2,
                        records = pagination_2.records,
                        page = pagination_2.page,
                        total = pagination_2.total
                    };
                    context.Response.Write(data_2.ToJson());
                    break;

                //删除WCF版本
                case "DeleteWCFVersion":

                    string wcfVersionInfoId = context.Request["WCFVersionInfoId"].ToString();
                    context.Response.Write(DeleleWCFVersion(wcfVersionInfoId, context).ToJson());
                    break;

                //上传WCF文件
                case "WCFUploadFile":
                    WCFUploadFile(context);
                    break;

                //合并WCF文件
                case "WCFMergeFile":
                    WCFMergeFile(context);
                    break;

                //没有合并WCF时的保存
                case "WCFVersionAdd":
                    WCFVersionAdd(context);
                    break;

                //WCF文件上传
                case "WCFVersionFileAdd":
                    WCFVersionFileAdd(context);
                    break;
                default:
                    break;
            }
            //}

        }


        #region 第三方CS端更新文件上传
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="context"></param>
        private void UploadFile(HttpContext context)
        {

            string appVersionId = context.Request["AppVersionId"];

            //如果进行了分片
            if (context.Request.Form.AllKeys.Any(m => m == "chunk"))
            {
                //取得chunk和chunks
                int chunk = Convert.ToInt32(context.Request.Form["chunk"]);//当前分片在上传分片中的顺序（从0开始）
                int chunks = Convert.ToInt32(context.Request.Form["chunks"]);//总分片数
                string name = HttpContext.Current.Server.UrlDecode(context.Request.Form["name"]);//文件名
                //根据GUID创建用该GUID命名的临时文件夹
                string folder = context.Server.MapPath("~/CSAppFile/" + context.Request["guid"] + "/");
                string path = folder + chunk;

                //建立临时传输文件夹
                if (!Directory.Exists(Path.GetDirectoryName(folder)))
                {
                    Directory.CreateDirectory(folder);
                }

                FileStream addFile = new FileStream(path, FileMode.Append, FileAccess.Write);
                BinaryWriter AddWriter = new BinaryWriter(addFile);
                //获得上传的分片数据流
                HttpPostedFile file = context.Request.Files[0];
                Stream stream = file.InputStream;

                BinaryReader TempReader = new BinaryReader(stream);
                //将上传的分片追加到临时文件末尾
                AddWriter.Write(TempReader.ReadBytes((int)stream.Length));

                TempReader.Close();
                stream.Close();
                AddWriter.Close();
                addFile.Close();

                TempReader.Dispose();
                stream.Dispose();
                AddWriter.Dispose();
                addFile.Dispose();

                context.Response.Write("{\"chunked\" : true, \"hasError\" : false,\"f_name\":\"" + file.FileName + "\"}");
            }
            else//没有分片直接保存
            {
                string path = context.Server.MapPath("~/CSAppFile/" + appVersionId);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = context.Request.Files[0].FileName;
                // string ext = Path.GetExtension(context.Request.Files[0].FileName);
                string filePath = path + "\\" + fileName;
                string responPath = filePath.Replace(@"\", @"\\");

                Uri uri = context.Request.Url;
                string serverFilePath = uri.Scheme + "://" + uri.Authority + "/CSAppFile/" + appVersionId + "/" + fileName;

                context.Request.Files[0].SaveAs(filePath);
                context.Response.Write("{\"chunked\" : false, \"hasError\" : false,\"f_path\" : \"" + serverFilePath + "\"}");
            }
        }

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="context"></param>
        public void MergeFile(HttpContext context)
        {
            try
            {
                string guid = context.Request["guid"];
                string fileName = HttpContext.Current.Server.UrlDecode(context.Request["FileName"]);
                string appVersionId = context.Request["AppVersionId"];
                string root = context.Server.MapPath("~/CSAppFile/");
                string sourcePath = Path.Combine(root, guid + "/");//源数据文件夹

                string targetRoot = context.Server.MapPath("~/CSAppFile/" + appVersionId);
                string targetPath = Path.Combine(targetRoot, fileName);//合并后的文件

                DirectoryInfo dicInfo = new DirectoryInfo(sourcePath);
                if (Directory.Exists(Path.GetDirectoryName(sourcePath)))
                {
                    FileInfo[] files = dicInfo.GetFiles();
                    foreach (FileInfo file1 in files.OrderBy(f => int.Parse(f.Name)))
                    {

                        if (!Directory.Exists(targetRoot))
                        {
                            Directory.CreateDirectory(targetRoot);
                        }

                        FileStream addFile1 = null;

                        if (File.Exists(targetPath))
                        {
                            addFile1 = new FileStream(targetPath, FileMode.Append, FileAccess.Write);
                        }
                        else
                        {
                            addFile1 = new FileStream(targetPath, FileMode.Create, FileAccess.Write);
                        }

                        BinaryWriter AddWriter1 = new BinaryWriter(addFile1);

                        //获得上传的分片数据流
                        FileStream stream1 = file1.Open(FileMode.Open);
                        BinaryReader TempReader1 = new BinaryReader(stream1);
                        //将上传的分片追加到临时文件末尾
                        AddWriter1.Write(TempReader1.ReadBytes((int)stream1.Length));
                        //关闭BinaryReader文件阅读器
                        TempReader1.Close();
                        stream1.Close();
                        AddWriter1.Close();
                        addFile1.Close();

                        TempReader1.Dispose();
                        stream1.Dispose();
                        AddWriter1.Dispose();
                        addFile1.Dispose();
                    }
                    Uri uri = context.Request.Url;
                    string serverFilePath = uri.Scheme + "://" + uri.Authority + "/CSAppFile/" + appVersionId + "/" + fileName;
                    C_S_VersionFile entity = new C_S_VersionFile();
                    entity.VersionFileId = Guid.NewGuid().ToString();
                    entity.AppVersionId = context.Request["AppVersionId"];
                    entity.FileVersionCode = context.Request["FileVersionCode"];
                    entity.FileName = fileName;
                    entity.FileSize = context.Request["FileSize"];
                    entity.VirtualPath = serverFilePath;
                    entity.CreateUserId = OperatorProvider.Provider.GetCurrent().UserId;

                    AjaxResult msg = new CSVersionFile_BLL().Add(entity);

                    //合并完成后需要删除被分片的临时文件
                    DeleteFolder(sourcePath);
                    context.Response.Write("{\"chunked\" : true, \"hasError\" : false, \"savePath\" :\"" + System.Web.HttpUtility.UrlEncode(targetPath) + "\"}");
                }
                else
                    context.Response.Write("{\"hasError\" : true}");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                context.Response.Write("{\"hasError\" : true}");
            }
        }


        /// <summary>
        /// 添加应用App
        /// </summary>
        /// <param name="context"></param>
        public void AppVersionAdd(HttpContext context)
        {
            AjaxResult msg = new AjaxResult();
            try
            {
                C_S_AppVersion entity = new C_S_AppVersion();
                entity.Create();
                entity.AppVersionId = context.Request["AppVersionId"];
                entity.ApplicationId = context.Request["ApplicationId"];
                entity.VersionCode = context.Request["VersionCode"];
                entity.AppFileName = " ";
                entity.AppDirectory = " ";
                entity.AppSize = context.Request["AppSize"];
                entity.Remark = context.Request["Remark"];
                msg = new CSAppVersion_BLL().Add(entity);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                msg.message = "程序发生异常";
                msg.state = "error";
                msg.data = "";
            }

            context.Response.Write(msg.ToJson());
        }



        /// <summary>
        /// 新增版本文件
        /// </summary>
        /// <param name="context"></param>
        public void VersionFileAdd(HttpContext context)
        {

            AjaxResult msg = new AjaxResult();
            try
            {

                string fileName = HttpContext.Current.Server.UrlDecode(context.Request["FileName"]);
                Uri uri = context.Request.Url;
                string serverFilePath = uri.Scheme + "://" + uri.Authority + "/CSAppFile/" + context.Request["AppVersionId"] + "/" + fileName;

                C_S_VersionFile entity = new C_S_VersionFile();
                entity.VersionFileId = Guid.NewGuid().ToString();
                entity.AppVersionId = context.Request["AppVersionId"];
                entity.FileVersionCode = context.Request["FileVersionCode"];
                entity.FileName = fileName;
                entity.FileSize = context.Request["FileSize"];
                entity.VirtualPath = serverFilePath;
                entity.CreateUserId = OperatorProvider.Provider.GetCurrent().UserId;
                msg = new CSVersionFile_BLL().Add(entity);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                msg.message = "程序发生异常";
                msg.state = "error";
                msg.data = "";
            }

            context.Response.Write(msg.ToJson());
        }
        #endregion


        #region WCF本地服务文件上传

        /// <summary>
        /// 上传WCF版本文件
        /// </summary>
        /// <param name="context"></param>
        private void WCFUploadFile(HttpContext context)
        {
            string wcfVersionInfoId = context.Request["WCFVersionInfoId"];

            //如果进行了分片
            if (context.Request.Form.AllKeys.Any(m => m == "chunk"))
            {
                //取得chunk和chunks
                int chunk = Convert.ToInt32(context.Request.Form["chunk"]);//当前分片在上传分片中的顺序（从0开始）
                int chunks = Convert.ToInt32(context.Request.Form["chunks"]);//总分片数
                string name = HttpContext.Current.Server.UrlDecode(context.Request.Form["name"]);//文件名
                //根据GUID创建用该GUID命名的临时文件夹
                string folder = context.Server.MapPath("~/CSAppFile/WCF/" + context.Request["guid"] + "/");
                string path = folder + chunk;

                //建立临时传输文件夹
                if (!Directory.Exists(Path.GetDirectoryName(folder)))
                {
                    Directory.CreateDirectory(folder);
                }

                FileStream addFile = new FileStream(path, FileMode.Append, FileAccess.Write);
                BinaryWriter AddWriter = new BinaryWriter(addFile);
                //获得上传的分片数据流
                HttpPostedFile file = context.Request.Files[0];
                Stream stream = file.InputStream;

                BinaryReader TempReader = new BinaryReader(stream);
                //将上传的分片追加到临时文件末尾
                AddWriter.Write(TempReader.ReadBytes((int)stream.Length));

                TempReader.Close();
                stream.Close();
                AddWriter.Close();
                addFile.Close();

                TempReader.Dispose();
                stream.Dispose();
                AddWriter.Dispose();
                addFile.Dispose();

                context.Response.Write("{\"chunked\" : true, \"hasError\" : false,\"f_name\":\"" + file.FileName + "\"}");
            }
            else//没有分片直接保存
            {
                string path = context.Server.MapPath("~/CSAppFile/WCF/" + wcfVersionInfoId);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = context.Request.Files[0].FileName;
                // string ext = Path.GetExtension(context.Request.Files[0].FileName);
                string filePath = path + "\\" + fileName;
                string responPath = filePath.Replace(@"\", @"\\");

                Uri uri = context.Request.Url;
                string serverFilePath = uri.Scheme + "://" + uri.Authority + "/CSAppFile/WCF/" + wcfVersionInfoId + "/" + fileName;

                context.Request.Files[0].SaveAs(filePath);
                context.Response.Write("{\"chunked\" : false, \"hasError\" : false,\"f_path\" : \"" + serverFilePath + "\"}");
            }
        }
        /// <summary>
        /// 合并WCF文件
        /// </summary>
        /// <param name="context"></param>
        public void WCFMergeFile(HttpContext context)
        {
            try
            {
                string guid = context.Request["guid"];
                string fileName = HttpContext.Current.Server.UrlDecode(context.Request["FileName"]);
                string wcfVersionInfoId = context.Request["WCFVersionInfoId"];
                string root = context.Server.MapPath("~/CSAppFile/WCF/");
                string sourcePath = Path.Combine(root, guid + "/");//源数据文件夹

                string targetRoot = context.Server.MapPath("~/CSAppFile/WCF/" + wcfVersionInfoId);
                string targetPath = Path.Combine(targetRoot, fileName);//合并后的文件

                DirectoryInfo dicInfo = new DirectoryInfo(sourcePath);
                if (Directory.Exists(Path.GetDirectoryName(sourcePath)))
                {
                    FileInfo[] files = dicInfo.GetFiles();
                    foreach (FileInfo file1 in files.OrderBy(f => int.Parse(f.Name)))
                    {

                        if (!Directory.Exists(targetRoot))
                        {
                            Directory.CreateDirectory(targetRoot);
                        }

                        FileStream addFile1 = null;

                        if (File.Exists(targetPath))
                        {
                            addFile1 = new FileStream(targetPath, FileMode.Append, FileAccess.Write);
                        }
                        else
                        {
                            addFile1 = new FileStream(targetPath, FileMode.Create, FileAccess.Write);
                        }

                        BinaryWriter AddWriter1 = new BinaryWriter(addFile1);

                        //获得上传的分片数据流
                        FileStream stream1 = file1.Open(FileMode.Open);
                        BinaryReader TempReader1 = new BinaryReader(stream1);
                        //将上传的分片追加到临时文件末尾
                        AddWriter1.Write(TempReader1.ReadBytes((int)stream1.Length));
                        //关闭BinaryReader文件阅读器
                        TempReader1.Close();
                        stream1.Close();
                        AddWriter1.Close();
                        addFile1.Close();

                        TempReader1.Dispose();
                        stream1.Dispose();
                        AddWriter1.Dispose();
                        addFile1.Dispose();
                    }
                    Uri uri = context.Request.Url;
                    string serverFilePath = uri.Scheme + "://" + uri.Authority + "/CSAppFile/WCF/" + wcfVersionInfoId + "/" + fileName;
                    C_S_WCFVersionFiles entity = new C_S_WCFVersionFiles();
                    entity.WCFVersionFileId = Guid.NewGuid().ToString();
                    entity.WCFVersionInfoId = context.Request["WCFVersionInfoId"];
                    entity.FileName = fileName;
                    entity.FileSize = context.Request["FileSize"];
                    entity.FilePath = serverFilePath;
                    entity.CreateUserId = OperatorProvider.Provider.GetCurrent().UserId;
                    entity.CreateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    AjaxResult msg = new CSWCFVersionFiles_BLL().Add(entity);

                    //合并完成后需要删除被分片的临时文件
                    DeleteFolder(sourcePath);
                    context.Response.Write("{\"chunked\" : true, \"hasError\" : false, \"savePath\" :\"" + System.Web.HttpUtility.UrlEncode(targetPath) + "\"}");
                }
                else
                    context.Response.Write("{\"hasError\" : true}");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                context.Response.Write("{\"hasError\" : true}");
            }
        }

        /// <summary>
        /// 新增WCF版本文件
        /// </summary>
        /// <param name="context"></param>
        public void WCFVersionAdd(HttpContext context)
        {
            AjaxResult msg = new AjaxResult();
            try
            {
                C_S_WCFVersionInfo entity = new C_S_WCFVersionInfo();
                entity.WCFVersionInfoId = context.Request["WCFVersionInfoId"];
                entity.VersionCode = context.Request["VersionCode"];
                entity.CreateUserId = OperatorProvider.Provider.GetCurrent().UserName;
                entity.CreateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                entity.FileSize = context.Request["FileSize"];
                entity.Remark = context.Request["Remark"];
                msg = new CSWCFVersionInfo_BLL().Add(entity);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                msg.message = "程序发生异常";
                msg.state = "error";
                msg.data = "";
            }

            context.Response.Write(msg.ToJson());
        }

        /// <summary>
        /// 新增WCF版本文件
        /// </summary>
        /// <param name="context"></param>
        public void WCFVersionFileAdd(HttpContext context)
        {

            AjaxResult msg = new AjaxResult();
            try
            {
                string fileName = HttpContext.Current.Server.UrlDecode(context.Request["FileName"]);
                Uri uri = context.Request.Url;
                string serverFilePath = uri.Scheme + "://" + uri.Authority + "/CSAppFile/WCF/" + context.Request["WCFVersionInfoId"] + "/" + fileName;
                C_S_WCFVersionFiles entity = new C_S_WCFVersionFiles();
                entity.WCFVersionFileId = Guid.NewGuid().ToString();
                entity.WCFVersionInfoId = context.Request["WCFVersionInfoId"];
                entity.FileName = fileName;
                entity.FileSize = context.Request["FileSize"];
                entity.FilePath = serverFilePath;
                entity.CreateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                entity.CreateUserId = OperatorProvider.Provider.GetCurrent().UserId;
                msg = new CSWCFVersionFiles_BLL().Add(entity);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                msg.message = "程序发生异常";
                msg.state = "error";
                msg.data = "";
            }

            context.Response.Write(msg.ToJson());
        }
        #endregion


        /// <summary>
        /// 提交Application表单
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="ApplicationId"></param>
        /// <returns></returns>
        private AjaxResult SubmitForm(string postData, string ApplicationId)
        {
            AjaxResult msg = new AjaxResult();
            try
            {
                if (!string.IsNullOrEmpty(postData))
                {
                    C_S_Application entity = postData.ToObject<C_S_Application>();
                    if (string.IsNullOrEmpty(ApplicationId))
                    {
                        entity.Create();
                        msg = new CSApplication_BLL().Add(entity);
                    }
                    else
                    {
                        entity.Update(ApplicationId);
                        msg = new CSApplication_BLL().Update(entity, ApplicationId);
                    }
                }
                else
                {
                    msg.message = "参数错误！";
                    msg.state = "error";
                    msg.data = "";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                msg.message = "程序发生异常！";
                msg.state = "error";
                msg.data = "";
            }
            return msg;
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        private AjaxResult GetApplicationEntity(string applicationId)
        {
            AjaxResult msg = new AjaxResult();
            if (!string.IsNullOrEmpty(applicationId))
            {
                DataTable dt = new CSApplication_BLL().FindOne(applicationId);
                msg.data = dt;
                msg.state = "success";
                msg.message = "获取成功！";
            }
            else
            {
                msg.message = "参数错误！";
                msg.state = "error";
                msg.data = "";
            }

            return msg;
        }

        /// <summary>
        /// 删除应用版本
        /// </summary>
        /// <param name="verionId"></param>
        /// <returns></returns>
        public AjaxResult DeleleVersion(string verionId, HttpContext context)
        {
            AjaxResult msg = new AjaxResult();
            if (string.IsNullOrEmpty(verionId))
            {
                msg.state = "error";
                msg.message = "参数无效";
            }
            else
            {
                msg = new CSAppVersion_BLL().Delete(verionId);

                if (msg.state.ToString() == "success")
                {
                    try
                    {
                        string filePath = context.Server.MapPath("~/CSAppFile/" + verionId);
                        //删除了数据库数据  当然上传的版本物理文件也要删除啦
                        if (Directory.Exists(filePath))
                        {
                            Directory.Delete(filePath, true);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                }
            }

            return msg;
        }


        /// <summary>
        /// 删除WCF版本更新文件
        /// </summary>
        /// <param name="wcfVersionInfoId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public AjaxResult DeleleWCFVersion(string wcfVersionInfoId, HttpContext context)
        {
            AjaxResult msg = new AjaxResult();
            if (string.IsNullOrEmpty(wcfVersionInfoId))
            {
                msg.state = "error";
                msg.message = "参数无效";
            }
            else
            {
                msg = new CSWCFVersionInfo_BLL().Delete(wcfVersionInfoId);

                if (msg.state.ToString() == "success")
                {
                    try
                    {
                        string filePath = context.Server.MapPath("~/CSAppFile/WCF/" + wcfVersionInfoId);
                        //删除了数据库数据  当然上传的版本物理文件也要删除啦
                        if (Directory.Exists(filePath))
                        {
                            Directory.Delete(filePath, true);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                }
            }

            return msg;
        }

        /// <summary>
        /// 删除文件夹及其内容
        /// </summary>
        /// <param name="dir"></param>
        private static void DeleteFolder(string strPath)
        {
            //删除这个目录下的所有子目录
            if (Directory.GetDirectories(strPath).Length > 0)
            {
                foreach (string fl in Directory.GetDirectories(strPath))
                {
                    Directory.Delete(fl, true);
                }
            }
            //删除这个目录下的所有文件
            if (Directory.GetFiles(strPath).Length > 0)
            {
                foreach (string f in Directory.GetFiles(strPath))
                {
                    System.IO.File.Delete(f);
                }
            }
            Directory.Delete(strPath, true);
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