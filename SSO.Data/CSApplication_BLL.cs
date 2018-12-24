using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SSO.Code;

namespace SSO.Data
{
    public class CSApplication_BLL
    {

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public DataTable FindList(string keywords, Pagination page)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM
                            (SELECT ROW_NUMBER() OVER (ORDER BY ApplicationId ASC) AS rowid,
                                    *,
                                    (
                                        SELECT COUNT(AppVersionId)
                                        FROM dbo.C_S_AppVersion
                                        WHERE ApplicationId = C_S_Application.ApplicationId
                                    ) versionCount
                                FROM C_S_Application
                            ) tt
                            WHERE 1 = 1 ");

            strSql.Append(" and rowid between " + ((page.page - 1) * page.rows + 1) + " and " + page.page * page.rows + "");
            List<SqlParameter> parameter = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(" and (isnull(ApplicationName,'')+isnull(ApplicationCode,'')) like @keywords ");
                parameter.Add(new SqlParameter("keywords", '%' + keywords + '%'));
            }

            if (!string.IsNullOrEmpty(page.sidx))
            {
                strSql.Append(" order by " + page.sidx);
            }
            return SqlHelper.ExecuteDataTable(strSql.ToString(), parameter.ToArray());
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AjaxResult Add(C_S_Application entity)
        {
            AjaxResult msg = new AjaxResult();
            List<SqlParameter> parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("ApplicationName", entity.ApplicationName));
            parameter.Add(new SqlParameter("ApplicationCode", entity.ApplicationCode));
            parameter.Add(new SqlParameter("Company", entity.Company));

            string sqlStr = "select * from C_S_Application where (ApplicationName = @ApplicationName or ApplicationCode=@ApplicationCode) and Company = @Company";

            DataTable dt = SqlHelper.ExecuteDataTable(sqlStr, parameter.ToArray());

            if (dt.Rows.Count > 0)
            {
                msg.data = "";
                msg.state = "error";
                msg.message = "新增失败，该应用已经存在！";
            }
            else
            {
                sqlStr = @"INSERT INTO dbo.C_S_Application (ApplicationId,ApplicationName,ApplicationCode,
                                 Company,SortCode,Remark,CreateBy,CreateTime,StartFileName)
                                VALUES  ( @ApplicationId ,@ApplicationName ,
                                          @ApplicationCode ,  @Company, @SortCode ,                                      
                                          @Remark ,  @CreateBy , @CreateTime,@StartFileName) ";
                parameter.Clear();
                parameter.Add(new SqlParameter("ApplicationId", entity.ApplicationId));
                parameter.Add(new SqlParameter("ApplicationName", entity.ApplicationName));
                parameter.Add(new SqlParameter("ApplicationCode", entity.ApplicationCode));
                parameter.Add(new SqlParameter("SortCode", entity.SortCode));
                parameter.Add(new SqlParameter("Company", entity.Company));
                parameter.Add(new SqlParameter("Remark", entity.ApplicationName));
                parameter.Add(new SqlParameter("CreateBy", entity.CreateBy));
                parameter.Add(new SqlParameter("CreateTime", entity.CreateTime));
                parameter.Add(new SqlParameter("StartFileName", entity.StartFileName));

                int result = SqlHelper.ExecuteNonQuery(sqlStr, parameter.ToArray());

                if (result == 1)
                {
                    msg.data = "";
                    msg.state = "success";
                    msg.message = "新增成功！";


                    #region =====================写操作日志======================
                    C_S_Log log = new C_S_Log()
                                {
                                    OperateType = 4,//新增
                                    OperateUserId = entity.CreateBy,
                                    ApplicationId = entity.ApplicationId,
                                    ApplicationCode = entity.ApplicationCode,
                                    Content = "新增应用",
                                    IPAddress = Net.Ip,
                                    MachineName = Net.Host
                                };

                    CSLog_BLL.Add(log);
                    #endregion=================================================
                }
                else
                {
                    msg.data = "";
                    msg.state = "error";
                    msg.message = "新增失败！";
                }
            }
            return msg;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ApplicationId"></param>
        /// <returns></returns>
        public AjaxResult Delete(string ApplicationId)
        {
            AjaxResult msg = new AjaxResult();

            DataTable dt = SqlHelper.ExecuteDataTable("select * from C_S_Application where ApplicationId = @ApplicationId", new SqlParameter[] { new SqlParameter("ApplicationId", ApplicationId) });

            //删除应用的时候，连同应用的版本号一起删除
            string sqlStr = "delete C_S_AppVersion where ApplicationId = @ApplicationId;";
            sqlStr += "delete C_S_Application where ApplicationId = @ApplicationId";
            SqlParameter[] parameter = { new SqlParameter("ApplicationId", ApplicationId) };

            int result = SqlHelper.ExecuteNonQuery(sqlStr, parameter);
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
                            ApplicationId = ApplicationId,
                            Content = "删除应用",
                            ApplicationCode = "",
                            IPAddress = Net.Ip,
                            MachineName = Net.Host
                        };

                if (dt.Rows.Count > 0)
                {
                    log.ApplicationCode = dt.Rows[0]["ApplicationCode"].ToString();
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
            return msg;
        }



        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ApplicationId"></param>
        /// <returns></returns>
        public AjaxResult Update(C_S_Application entity, string ApplicationId)
        {
            AjaxResult msg = new AjaxResult();

            string sqlStr = @"UPDATE dbo.C_S_Application
                                SET ApplicationName = @ApplicationName,
                                    ApplicationCode = @ApplicationCode,
                                    SortCode = @SortCode,
                                    Company = @Company,
                                    Remark = @Remark,
                                    UpdateBy = @UpdateBy,
                                    UpdateTime = @UpdateTime,
                                    StartFileName = @StartFileName
                                WHERE ApplicationId = @ApplicationId";
            SqlParameter[] parameter = {
               new SqlParameter("ApplicationId",entity.ApplicationId), 
               new SqlParameter("ApplicationName",entity.ApplicationName),   
               new SqlParameter("ApplicationCode",entity.ApplicationCode),  
               new SqlParameter("SortCode",entity.SortCode),  
               new SqlParameter("Company",entity.Company),  
               new SqlParameter("Remark",entity.Remark),             
               new SqlParameter("UpdateBy",entity.UpdateBy),  
               new SqlParameter("UpdateTime",entity.UpdateTime),
               new SqlParameter("StartFileName",entity.StartFileName),};
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
        /// 获取实体
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public DataTable FindOne(string applicationId)
        {
            string sqlStr = "select * from C_S_Application where ApplicationId = @ApplicationId";
            SqlParameter[] parameter = { new SqlParameter("ApplicationId", applicationId) };
            return SqlHelper.ExecuteDataTable(sqlStr, parameter);
        }


        /// <summary>
        /// 获取某应用的最新版本号
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public string GetPreviVersion(string applicationId)
        {
            DataTable dt = SqlHelper.ExecuteDataTable("SELECT top 1 * FROM dbo.C_S_AppVersion WHERE ApplicationId = '" + applicationId + "' order by createtime desc");
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["VersionCode"].ToString();
            }
            else
            {
                return "无";
            }
        }


        /// <summary>
        /// 获取应用最新版本号
        /// </summary>
        /// <param name="applicationCode">应用编码</param>
        /// <returns></returns>
        public string GetLastVersionByApplicationCode(string applicationCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT b.*,a.StartFileName
                            FROM dbo.C_S_Application a
                                LEFT JOIN dbo.C_S_AppVersion b
                                    ON b.ApplicationId = a.ApplicationId
                            WHERE a.ApplicationCode = @ApplicationCode order by b.createtime desc");
            SqlParameter[] para = { new SqlParameter("ApplicationCode", applicationCode) };
            DataTable dt = SqlHelper.ExecuteDataTable(strSql.ToString(), para);

            if (dt.Rows.Count > 0)
            {
                string versionCode = dt.Rows[0]["VersionCode"].ToString();
                return versionCode + "##" + dt.Rows[0]["StartFileName"].ToString();
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
    public class C_S_Application
    {
        public string ApplicationId { set; get; }
        public string ApplicationName { set; get; }
        public string ApplicationCode { set; get; }
        public string Company { set; get; }
        public string SortCode { set; get; }
        public string Remark { set; get; }
        public string StartFileName { set; get; }
        public string CreateBy { set; get; }
        public DateTime? CreateTime { set; get; }
        public string UpdateBy { set; get; }
        public DateTime? UpdateTime { set; get; }

        public void Create()
        {
            this.ApplicationId = Guid.NewGuid().ToString();
            this.CreateBy = OperatorProvider.Provider.GetCurrent().UserName;
            this.CreateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public void Update(string ApplicationId)
        {
            this.ApplicationId = ApplicationId;
            this.UpdateBy = OperatorProvider.Provider.GetCurrent().UserName;
            this.UpdateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
