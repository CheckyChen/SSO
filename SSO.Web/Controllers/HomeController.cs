using SSO.Code;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    [HandlerLogin]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (OperatorProvider.Provider.GetCurrent() != null)
            {
                ViewBag.UserName = OperatorProvider.Provider.GetCurrent().UserName;
                ViewBag.UserCode = SSO.Code.OperatorProvider.Provider.GetCurrent().UserCode;
            }
            else
            {
                return View("../Login/Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Default()
        {
            return View();
        }
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// 单点登录C/S端主进程文件
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public FileResult DownLoadHostFile()
        //{
        //    string filePath = Server.MapPath("~/CSHostFile/SSOHostPackageService.exe");//路径
        //    string s = MimeMapping.GetMimeMapping("SSOHostPackageService.exe");
        //    return File(filePath, s, "恺恩泰单点登录服务.exe"); //恺恩泰单点登录服务.exe是客户端保存的名字
        //}

        //[HttpGet]
        //public FileStreamResult DownLoadHostFile()
        //{
        //    string absoluFilePath = Server.MapPath("~/CSHostFile/SSOHostPackageService.exe");
        //    return File(new FileStream(absoluFilePath, FileMode.Open), "application/octet-stream");
        //}


        [HttpGet]
        public ActionResult DownLoadHostFile()
        {
            string filePath = Server.MapPath("~/CSHostFile/SSOHostPackageService.zip");
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode("SSOHostPackageService.zip"));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }
    }
}
