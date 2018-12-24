using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using SSO.Code;
using SSO.Domain.Entity;

namespace SSO.Web.Handle
{
    /// <summary>
    /// SSO_Handler 的摘要说明
    /// </summary>
    public class SSO_Handler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");

            string token = context.Request["token"];

            if (!string.IsNullOrEmpty(token))
            {
                string opt = context.Request["opt"];
                switch (opt)
                {
                    case "LoginOutPush":

                        WebHelper.RemoveSession("sso_loginuserkey_2018");
                        new Cache().RemoveCache(token);
                        context.Response.Redirect("../Login/Index");

                        break;
                    default:
                        break;
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