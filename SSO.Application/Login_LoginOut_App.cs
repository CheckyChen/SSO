using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSO.Code;
using SSO.Domain.Entity;

namespace SSO.Application
{

    /// <summary>
    /// 登录登出业务
    /// </summary>
    public class Login_LoginOut_App
    {
        /// <summary>
        /// 登录设置用户凭证
        /// </summary>
        /// <param name="username"></param>
        public void LoginIn_SetCert(string usercode)
        {
            string token = usercode + "_" + Guid.NewGuid().ToString();
            //写令牌token
            WebHelper.WriteCookie("caradigm_sso_token", token);

            //写用户凭证缓存
            double timeout = Configs.GetValue("SSO_TimeOut").ToDouble();
            if (timeout == null)
            {
                timeout = 60;
            }
            Cache cache = new Cache();
            SSO_Cert cert = new SSO_Cert();
            cert.token = token;
            cert.usercode = usercode;
            cert.timeout = DateTime.Now.AddMinutes(timeout);
            cache.WriteCache<SSO_Cert>(cert, token, cert.timeout);
        }


    }
}
