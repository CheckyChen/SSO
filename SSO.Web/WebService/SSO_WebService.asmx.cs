using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SSO.Code;
using SSO.Domain.Entity;
using SSO.Data;

namespace SSO.Web.WebService
{
    /// <summary>
    /// SSO_WebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://www.caradigmcn.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class SSO_WebService : System.Web.Services.WebService
    {
        Log log = LogFactory.GetLogger(typeof(SSO_WebService));
        /// <summary>
        /// 根据token获取用户凭证
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetCertByToken(string token)
        {
            AjaxResult msg = new AjaxResult();
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    msg.state = "400";
                    msg.message = "请求参数有误";
                    msg.data = "";
                }
                else
                {
                    Cache cache = new Cache();
                    SSO_Cert cert = cache.GetCache<SSO_Cert>(token);
                    log.Debug("获取用户凭证：" + cert.ToJson());
                    if (cert != null)
                    {
                        msg.state = "200";
                        msg.data = cert.usercode;
                        msg.message = "获取成功";
                    }
                    else
                    {
                        //请求成功，但没有返回任何数据
                        msg.state = "205";
                        msg.data = "";
                        msg.message = "获取失败";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //内部服务器发生错误
                msg.state = "500";
                msg.data = "";
                msg.message = "程序异常。";
            }

            return Serialize.GetXmlDoc(msg).InnerXml;
        }


        /// <summary>
        /// 根据IP地址获取token
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string GetTokenByIP(string ipAddress)
        {
            AjaxResult msg = new AjaxResult();
            try
            {
                if (Validate.IsValidIP(ipAddress))
                {
                    msg.state = "400";
                    msg.message = "请求IP地址有误";
                    msg.data = "";
                }
                else
                {
                    Cache cache = new Cache();
                    string token = cache.GetCache<string>(ipAddress);
                    if (token != null)
                    {
                        msg.state = "200";
                        msg.data = token;
                        msg.message = "获取token成功";
                    }
                    else
                    {
                        //请求成功，但没有返回任何数据
                        msg.state = "205";
                        msg.data = "";
                        msg.message = "获取token失败";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //内部服务器发生错误
                msg.state = "500";
                msg.data = "";
                msg.message = "程序异常。";
            }

            return Serialize.GetXmlDoc(msg).InnerXml;
        }


        /// <summary>
        /// 验证用户凭证
        /// </summary>
        /// <param name="cert">用户凭证</param>
        /// <param name="funcCode">登录代码</param>
        /// <param name="systemCode">系统代码</param>
        /// <returns></returns>
        [WebMethod]
        public string CheckLoginByCert(string cert, string funcCode, string systemCode)
        {
            AjaxResult msg = new AjaxResult();
            try
            {
                string sysMgtUrl = Configs.GetValue("SysMgmtURL");
                string handler_data = string.Empty;

                if (sysMgtUrl.Last() != '/')
                {
                    handler_data += "/";
                }

                handler_data += "Handle/UserHandler.ashx";

                //var post_obj = new
                //{
                //    op = "logonSSO",
                //    username = cert,
                //    funccode = funcCode,
                //    systemcode = systemCode
                //};
                // string postData = post_obj.ToJson();
                //string respones = HttpMethods.HttpPost(sysMgtUrl + handler_data, postData);

                string postData = "?op=logonSSO&username=" + cert + "&funccode=" + funcCode + "&systemcode=" + systemCode;
                string respones = HttpMethods.HttpGet(sysMgtUrl + handler_data + postData);

                if (!string.IsNullOrEmpty(respones))
                {
                    LogonResponseBody body = respones.ToObject<LogonResponseBody>();

                    if (body.result == "success")
                    {
                        log.Debug("获取成功：" + respones);
                        msg.data = respones;
                        msg.message = "验证成功";
                        msg.state = "200";
                    }
                    else
                    {
                        log.Debug("验证失败：" + respones);
                        msg.data = respones;
                        msg.message = "验证失败";
                        msg.state = "205";
                    }
                }
                else
                {
                    log.Debug("验证失败respones为空：" + respones);
                    msg.data = respones;
                    msg.message = "验证失败";
                    msg.state = "400";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                msg.data = "";
                msg.message = "验证失败，服务器内部错误。";
                msg.state = "500";
            }

            return Serialize.GetXmlDoc(msg).InnerXml;
        }


        /// <summary>
        /// 验证用户凭证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="funcCode">登录代码</param>
        /// <param name="systemCode">系统代码</param>
        /// <returns></returns>
        [WebMethod]
        public string CheckLoginByPwd(string username, string password, string funcCode, string systemCode)
        {
            AjaxResult msg = new AjaxResult();
            try
            {
                string sysMgtUrl = Configs.GetValue("SysMgmtURL");
                string handler_data = string.Empty;

                if (sysMgtUrl.Last() != '/')
                {
                    handler_data += "/";
                }

                handler_data += "Handle/UserHandler.ashx";

                //var post_obj = new
                //{
                //    op = "logonSSO",
                //    username = username,
                //    funccode = funcCode,
                //    systemcode = systemCode
                //};
                // string postData = post_obj.ToJson();
                //string respones = HttpMethods.HttpPost(sysMgtUrl + handler_data, postData);

                string postData = "?op=logon&username=" + username + "&password=" + password + "&funccode=" + funcCode + "&systemcode=" + systemCode;
                string respones = HttpMethods.HttpGet(sysMgtUrl + handler_data + postData);

                if (!string.IsNullOrEmpty(respones))
                {
                    LogonResponseBody body = respones.ToObject<LogonResponseBody>();

                    if (body.result == "success")
                    {
                        msg.data = respones;
                        msg.message = "登录成功";
                        msg.state = "200";
                    }
                    else
                    {
                        msg.data = respones;
                        msg.message = "登录失败";
                        msg.state = "205";
                    }
                }
                else
                {
                    msg.data = respones;
                    msg.message = "请求失败";
                    msg.state = "400";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                msg.data = "";
                msg.message = "服务器内部错误。";
                msg.state = "500";
            }

            return Serialize.GetXmlDoc(msg).InnerXml;
        }



        /// <summary>
        /// 根据应用编码获取最新版本号
        /// </summary>
        /// <param name="applicationCode">应用编码</param>
        /// <returns></returns>
        [WebMethod]
        public string GetLastVersionCodeByApplicationCode(string applicationCode)
        {
            return new CSApplication_BLL().GetLastVersionByApplicationCode(applicationCode);
        }


        /// <summary>
        /// 获取应用文件网络地址
        /// </summary>
        /// <param name="appCode"></param>
        /// <param name="versionCode"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetUriByAppCodeVersion(string appCode, string versionCode)
        {
            return new CSAppVersion_BLL().GetUriByAppCodeVersion(appCode, versionCode);
        }

        /// <summary>
        /// 获取文件虚拟路径
        /// </summary>
        /// <param name="appCode">应用编码</param>
        /// <param name="versionCode">版本号</param>
        /// <returns></returns>
        [WebMethod]
        public List<string> GetUriByAppCodeVersion_new(string appCode, string versionCode)
        {
            return new CSVersionFile_BLL().GetFileVirtual(appCode, versionCode);
        }

        /// <summary>
        /// 设置用户本地安装路径
        /// </summary>
        /// <param name="hostName">用户主键</param>
        /// <param name="appCode">应用编码</param>
        /// <param name="localDir">本地路径</param>
        /// <returns></returns>
        [WebMethod]
        public string InsertPersonalAppInfo(string hostName, string appCode, string localDir)
        {
            PersonalAppInfo entity = new PersonalAppInfo();

            if (string.IsNullOrEmpty(hostName))
            {
                return "主机名称不能为空！";
            }
            if (string.IsNullOrEmpty(appCode))
            {
                return "应用编码不能为空！";
            }
            if (string.IsNullOrEmpty(localDir))
            {
                return "本地路径不能为空！";
            }

            entity.UserId = hostName;
            entity.AppCode = appCode;
            entity.LocalDir = localDir;
            AjaxResult msg = new PersonalAppInfo_BLL().SetPersonalAppInfo(entity);
            return msg.message;
        }


        /// <summary>
        /// 获取用户访问的地址和最新版本
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="appCode"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetDirByHostName(string hostName, string appCode)
        {
            return new PersonalAppInfo_BLL().GetLocalDir(appCode, hostName);
        }


        /// <summary>
        /// 获取最新版本
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetAppLastVersionCode(string appCode)
        {
            return new CSAppVersion_BLL().GetLastVersionByAppCode(appCode);
        }

        /// <summary>
        /// 获取版本单点登录CS端服务最新版本号
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string GetWCFLastVersionCode()
        {
            return new CSWCFVersionInfo_BLL().GetLastVersion();
        }

        /// <summary>
        /// 客户端更新日志
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appCode"></param>
        /// <param name="ip_HostName_state"></param>
        /// <returns></returns>
        [WebMethod]
        public string UpdateLog(string userId, string appCode, string ip_HostName_state)
        {
            return CSLog_BLL.WriteUpdateLog(appCode, userId, ip_HostName_state).ToJson();
        }

    }
}
