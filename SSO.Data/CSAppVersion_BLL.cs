using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSO.Code;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace SSO.Data
{

    /// <summary>
    /// C_S_AppVersion逻辑处理类
    /// </summary>
    public class CSAppVersion_BLL
    {

        Log log = LogFactory.GetLogger(typeof(CSAppVersion_BLL));
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public DataTable FindList(string keywords, Pagination page)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT ROW_NUMBER() OVER (ORDER BY AppVersionId ASC) AS rowid,
                             tt.* FROM( SELECT * FROM C_S_AppVersion ) tt where 1=1 ");

            strSql.Append(" and rowid between " + ((page.page - 1) * page.rows + 1) + " and " + page.page * page.rows + "");
            List<SqlParameter> parameter = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(" and (isnull(AppFileName,'')+isnull(VersionCode,'')) like @keywords ");
                parameter.Add(new SqlParameter("keywords", '%' + keywords + '%'));
            }

            if (!string.IsNullOrEmpty(page.sidx))
            {
                strSql.Append(page.sidx);
            }
            return SqlHelper.ExecuteDataTable(strSql.ToString(), parameter.ToArray());
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AjaxResult Add(C_S_AppVersion entity)
        {

            DataTable dt = SqlHelper.ExecuteDataTable("select * from C_S_Application where ApplicationId = @ApplicationId", new SqlParameter[] { new SqlParameter("ApplicationId", entity.ApplicationId) });
            entity.Create();
            AjaxResult msg = new AjaxResult();
            string sqlStr = @"INSERT INTO dbo.C_S_AppVersion(AppVersionId,ApplicationId,VersionCode,AppFileName,
                                     AppDirectory,AppSize,Remark,CreateBy,CreateTime)
                                VALUES  ( @AppVersionId , @ApplicationId , 
                                          @VersionCode ,  @AppFileName , 
                                          @AppDirectory ,  @AppSize , 
                                          @Remark ,  @CreateBy ,  @CreateTime)";
            SqlParameter[] parameter = {
               new SqlParameter("AppVersionId",entity.AppVersionId), 
               new SqlParameter("ApplicationId",entity.ApplicationId),   
               new SqlParameter("VersionCode",entity.VersionCode),  
               new SqlParameter("AppFileName",entity.AppFileName),  
               new SqlParameter("AppDirectory",entity.AppDirectory),                 
               new SqlParameter("AppSize",entity.AppSize),                 
               new SqlParameter("Remark",entity.Remark),  
               new SqlParameter("CreateBy",entity.CreateBy),  
              new SqlParameter("CreateTime",entity.CreateTime)};
            int result = SqlHelper.ExecuteNonQuery(sqlStr, parameter);
            if (result == 1)
            {
                msg.data = "";
                msg.state = "success";
                msg.message = "新增成功！";

                #region ===========记录操作日志=============
                C_S_Log log = new C_S_Log()
                {
                    OperateType = 0,//上传
                    OperateUserId = entity.CreateBy,
                    ApplicationId = entity.ApplicationId,
                    Content = "上传应用",
                    ApplicationCode = "",
                    IPAddress = Net.Ip,
                    MachineName = Net.Host
                };

                if (dt.Rows.Count > 0)
                {
                    log.ApplicationCode = dt.Rows[0]["ApplicationCode"].ToString();
                }

                CSLog_BLL.Add(log);
                #endregion =================================

            }
            else
            {
                msg.data = "";
                msg.state = "error";
                msg.message = "新增失败！";
            }
            return msg;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="appVersionId"></param>
        /// <returns></returns>
        public AjaxResult Delete(string appVersionId)
        {
            AjaxResult msg = new AjaxResult();
            try
            {
                string sqlStr1 = "select * from C_S_AppVersion where AppVersionId = @AppVersionId";
                SqlParameter[] parameter = { new SqlParameter("AppVersionId", appVersionId) };
                DataTable dt = SqlHelper.ExecuteDataTable(sqlStr1, parameter);

                if (dt.Rows.Count > 0)
                {
                    string applicationId = dt.Rows[0]["ApplicationId"].ToString();


                    DataTable dt_app = SqlHelper.ExecuteDataTable("select * from C_S_Application where ApplicationId = @ApplicationId", new SqlParameter[] { new SqlParameter("ApplicationId", applicationId) });

                    //先删除版本文件数据再删除应用版本数据
                    string sqlStr = @"delete C_S_VersionFile where AppVersionId = @AppVersionId
                                      ;delete C_S_AppVersion where AppVersionId = @AppVersionId";

                    string path = dt.Rows[0]["AppDirectory"].ToString();

                    SqlParameter[] parameter1 = { new SqlParameter("AppVersionId", appVersionId) };

                    int result = SqlHelper.ExecuteNonQuery(sqlStr, parameter1);

                    if (result > 0)
                    {
                        msg.data = "";
                        msg.state = "success";
                        msg.message = "删除成功！";

                        #region ===========记录操作日志=================
                        C_S_Log log = new C_S_Log()
                        {
                            OperateType = 3,//移除
                            OperateUserId = OperatorProvider.Provider.GetCurrent().UserName,
                            ApplicationId = applicationId,
                            Content = "删除版本",
                            ApplicationCode = "",
                            IPAddress = Net.Ip,
                            MachineName = Net.Host
                        };

                        if (dt.Rows.Count > 0)
                        {
                            log.ApplicationCode = dt_app.Rows[0]["ApplicationCode"].ToString();
                        }
                        CSLog_BLL.Add(log);
                        #endregion======================================

                    }
                    else
                    {
                        msg.data = "";
                        msg.state = "error";
                        msg.message = "删除失败！";
                    }
                }
                else
                {
                    msg.data = "";
                    msg.state = "success";
                    msg.message = "已删除";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                msg.data = "";
                msg.state = "error";
                msg.message = "删除失败，程序发生异常！";
            }

            return msg;
        }



        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="appVersionId"></param>
        /// <returns></returns>
        public AjaxResult Update(C_S_AppVersion entity, string appVersionId)
        {
            AjaxResult msg = new AjaxResult();
            entity.Update(appVersionId);

            string sqlStr = @"UPDATE dbo.C_S_AppVersion
                                SET ApplicationId = @ApplicationId,
                                    VersionCode = @VersionCode,
                                    AppFileName = @AppFileName,
                                    AppDirectory = @AppDirectory,
                                    AppSize = @AppSize,
                                    Remark = @Remark,
                                    UpdateBy = @UpdateBy,
                                    UpdateTime = @UpdateTime
                                WHERE AppVersionId = @AppVersionId";
            SqlParameter[] parameter = {
               new SqlParameter("AppVersionId",entity.AppVersionId), 
               new SqlParameter("ApplicationId",entity.ApplicationId),   
               new SqlParameter("VersionCode",entity.VersionCode),  
                new SqlParameter("AppFileName",entity.AppFileName), 
               new SqlParameter("AppDirectory",entity.AppDirectory),                 
               new SqlParameter("AppSize",entity.AppSize),                 
               new SqlParameter("Remark",entity.Remark),  
               new SqlParameter("UpdateBy",entity.UpdateBy),  
              new SqlParameter("UpdateTime",entity.UpdateTime)};
            int result = SqlHelper.ExecuteNonQuery(sqlStr, parameter);
            if (result == 1)
            {
                msg.data = "";
                msg.state = "success";
                msg.message = "修改成功！";
            }
            else
            {
                msg.data = "";
                msg.state = "error";
                msg.message = "修改失败！";
            }
            return msg;
        }


        /// <summary>
        /// 获取版本历史记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetCSAppVersion(string applicationId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM C_S_AppVersion where ApplicationId = @ApplicationId order by createtime desc ");
            SqlParameter[] parameter = { new SqlParameter("ApplicationId", applicationId) };
            return SqlHelper.ExecuteDataTable(strSql.ToString(), parameter.ToArray());
        }


        /// <summary>
        /// 获取更新包的地址
        /// </summary>
        /// <param name="appCode"></param>
        /// <param name="versionCode"></param>
        /// <returns></returns>
        public string GetUriByAppCodeVersion(string appCode, string versionCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT b.*,a.ApplicationName,a.ApplicationCode
                            FROM dbo.C_S_Application a
                                LEFT JOIN dbo.C_S_AppVersion b
                                    ON b.ApplicationId = a.ApplicationId
                            WHERE a.ApplicationCode = @ApplicationCode
                                  AND b.VersionCode = @VersionCode;");
            SqlParameter[] parameter = { new SqlParameter("ApplicationCode", appCode),
                                         new SqlParameter("VersionCode", versionCode)};

            DataTable dt = SqlHelper.ExecuteDataTable(strSql.ToString(), parameter.ToArray());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["AppDirectory"].ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 通过应用编码获取最高版本
        /// </summary>
        /// <returns></returns>
        public string GetLastVersionByAppCode(string appCode)
        {
            string strSql = @"SELECT MAX(b.VersionCode) FROM dbo.C_S_Application a  
                              LEFT JOIN dbo.C_S_AppVersion b ON b.ApplicationId = a.ApplicationId 
                              WHERE a.ApplicationCode=@ApplicationCode";
            SqlParameter[] param = { new SqlParameter("ApplicationCode", appCode) };

            DataTable dt = SqlHelper.ExecuteDataTable(strSql, param);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }
    }


    /// <summary>
    /// CS端实体类
    /// </summary>
    public class C_S_AppVersion
    {
        public string AppVersionId { set; get; }
        public string ApplicationId { set; get; }
        public string VersionCode { set; get; }
        public string AppFileName { set; get; }
        public string AppDirectory { set; get; }
        public string AppSize { set; get; }
        public string Remark { set; get; }
        public string CreateBy { set; get; }
        public DateTime? CreateTime { set; get; }
        public string UpdateBy { set; get; }
        public DateTime? UpdateTime { set; get; }

        public void Create()
        {
            //this.AppVersionId = Guid.NewGuid().ToString();
            this.CreateBy = OperatorProvider.Provider.GetCurrent().UserName;
            this.CreateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public void Update(string AppVersionId)
        {
            this.AppVersionId = AppVersionId;
            this.UpdateBy = OperatorProvider.Provider.GetCurrent().UserName;
            this.UpdateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }


}
