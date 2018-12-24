using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSO.Code;
using SSO.Application;
using SSO.Domain.Entity;

namespace SSO.Web.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            WebHelper.RemoveSession("sso_loginuserkey_2018");
            //OperatorProvider.Provider.RemoveCurrent();
            return View();
        }

        /// <summary>
        /// 验证登录功能
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ActionResult CheckLogin(string username, string password)
        {
            string funcCode = Configs.GetValue("LoginFuncCode");
            string systemCode = Configs.GetValue("SoftCode");
            string loginOpt = Configs.GetValue("LoginOptCode");
            string param = string.Format("op=logon&username={0}&password={1}&funccode={2}&systemcode={3}&pageCode={4}&operationCode={5}", username, password, funcCode, systemCode, funcCode, loginOpt);
            string loginUrl = Configs.GetValue("SysMgmtURL") + "/Handle/UserHandler.ashx";
            string responsStr = HttpMethods.HttpPost(loginUrl, param);
            LogonResponseBody body = responsStr.ToObject<LogonResponseBody>();

            //设置用户登录状态
            if (body.result == "success")
            {
                if (!string.IsNullOrEmpty(body.data))
                {
                    UserInfo userinfo = body.data.ToObject<UserInfo>();
                    OperatorModel model = new OperatorModel();
                    model.UserId = userinfo.UserID.ToString();
                    model.UserCode = userinfo.UserName;
                    model.UserName = userinfo.DisplayName;
                    model.UserPwd = userinfo.UserPassword;
                    model.CompanyId = userinfo.PID;
                    model.DepartmentId = userinfo.PID;
                    model.RoleId = userinfo.PID;
                    model.LoginTime = DateTime.Now;
                    model.IsSystem = true;
                    OperatorProvider.Provider.AddCurrent(model);

                    Login_LoginOut_App app = new Login_LoginOut_App();
                    app.LoginIn_SetCert(model.UserCode);
                }
            }
            return Content(responsStr);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult OutLogin()
        {
            //移除当前人的Session信息
            OperatorProvider.Provider.RemoveCurrent();

            //清除用户凭证
            string token = WebHelper.GetCookie("caradigm_sso_token");
            new Cache().RemoveCache(token);

            //清除浏览器的token数据
            WebHelper.RemoveCookie("caradigm_sso_token");

            return View("Index");
        }
    }
}
