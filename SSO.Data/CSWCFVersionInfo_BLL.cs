using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SSO.Code;
using System.Data.SqlClient;

namespace SSO.Data
{
    public class CSWCFVersionInfo_BLL
    {

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="page">分页参数</param>
        /// <returns></returns>
        public DataTable FindList(string keywords, Pagination page)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM
                            (SELECT ROW_NUMBER() OVER (ORDER BY VersionCode DESC) AS rowid,
                                    *
                                FROM dbo.C_S_WCFVersionInfo
                            ) tt
                            WHERE 1 = 1 ");

            strSql.Append(" and rowid between " + ((page.page - 1) * page.rows + 1) + " and " + page.page * page.rows + "");
            List<SqlParameter> parameter = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(" and (isnull(VersionCode,'')+isnull(Remark,'')) like @keywords ");
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
        public AjaxResult Add(C_S_WCFVersionInfo entity)
        {
            AjaxResult msg = new AjaxResult();
            List<SqlParameter> parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("VersionCode", entity.VersionCode));

            string sqlStr = "select * from C_S_WCFVersionInfo where VersionCode >= @VersionCode ";

            DataTable dt = SqlHelper.ExecuteDataTable(sqlStr, parameter.ToArray());

            if (dt.Rows.Count > 0)
            {
                msg.data = "";
                msg.state = "error";
                msg.message = "新增失败，新增版本必须大于上个版本！";
            }
            else
            {
                sqlStr = @"INSERT INTO dbo.C_S_WCFVersionInfo
                                    ( WCFVersionInfoId ,VersionCode ,FileSize ,Remark ,
                                      CreateUserId ,CreateTime
                                    )
                            VALUES  (  @WCFVersionInfoId,@VersionCode, 
                                       @FileSize,@Remark,@CreateUserId,@CreateTime)";
                parameter.Clear();
                parameter.Add(new SqlParameter("WCFVersionInfoId", entity.WCFVersionInfoId));
                parameter.Add(new SqlParameter("VersionCode", entity.VersionCode));
                parameter.Add(new SqlParameter("FileSize", entity.FileSize));
                parameter.Add(new SqlParameter("Remark", entity.Remark));
                parameter.Add(new SqlParameter("CreateUserId", entity.CreateUserId));
                parameter.Add(new SqlParameter("CreateTime", entity.CreateTime));

                int result = SqlHelper.ExecuteNonQuery(sqlStr, parameter.ToArray());

                if (result == 1)
                {
                    msg.data = "";
                    msg.state = "success";
                    msg.message = "新增成功！";
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
        /// <param name="WCFVersionInfoId"></param>
        /// <returns></returns>
        public AjaxResult Delete(string WCFVersionInfoId)
        {
            AjaxResult msg = new AjaxResult();

            DataTable dt = SqlHelper.ExecuteDataTable("select * from C_S_WCFVersionInfo where WCFVersionInfoId = @WCFVersionInfoId", new SqlParameter[] { new SqlParameter("WCFVersionInfoId", WCFVersionInfoId) });

            //删除应用的时候，连同应用的版本号一起删除
            string sqlStr = "delete C_S_WCFVersionFiles where WCFVersionInfoId = @WCFVersionInfoId;";
            sqlStr += "delete C_S_WCFVersionInfo where WCFVersionInfoId = @WCFVersionInfoId";
            SqlParameter[] parameter = { new SqlParameter("WCFVersionInfoId", WCFVersionInfoId) };

            int result = SqlHelper.ExecuteNonQuery(sqlStr, parameter);

            if (result > 0)
            {
                msg.data = "";
                msg.state = "success";
                msg.message = "删除成功！";
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
        /// 获取实体
        /// </summary>
        /// <param name="WCFVersionInfoId"></param>
        /// <returns></returns>
        public DataTable FindOne(string WCFVersionInfoId)
        {
            string sqlStr = "select * from C_S_WCFVersionInfo where WCFVersionInfoId = @WCFVersionInfoId";
            SqlParameter[] parameter = { new SqlParameter("WCFVersionInfoId", WCFVersionInfoId) };
            return SqlHelper.ExecuteDataTable(sqlStr, parameter);
        }


        /// <summary>
        /// 获取某应用的最新版本号
        /// </summary>
        /// <param name="WCFVersionInfoId"></param>
        /// <returns></returns>
        public string GetPreviVersion()
        {
            DataTable dt = SqlHelper.ExecuteDataTable("SELECT top 1 * FROM dbo.C_S_WCFVersionInfo order by VersionCode desc");
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
        /// 获取单点登录CS端服务最新版本号
        /// </summary>
        /// <returns></returns>
        public string GetLastVersion()
        {
            string strSql = @"SELECT a.VersionCode,
                                b.FilePath
                            FROM C_S_WCFVersionInfo a
                                LEFT JOIN dbo.C_S_WCFVersionFiles b
                                    ON b.WCFVersionInfoId = a.WCFVersionInfoId
                            WHERE a.VersionCode =
                                    (
                                        SELECT TOP 1
                                            VersionCode
                                        FROM dbo.C_S_WCFVersionInfo
                                        ORDER BY VersionCode DESC
                                    )";
            DataTable dt = SqlHelper.ExecuteDataTable(strSql);
            if (dt.Rows.Count > 0)
            {
                string versionCode = dt.Rows[0]["VersionCode"].ToString();

                string version_path = versionCode;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    version_path += "#" + dt.Rows[i]["FilePath"].ToString();
                }

                return version_path;
            }
            else
            {
                return "";
            }
        }
    }


    /// <summary>
    /// WCF版本信息
    /// </summary>
    public class C_S_WCFVersionInfo
    {
        /// <summary>
        /// 版本信息主键
        /// </summary>
        public string WCFVersionInfoId { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionCode { get; set; }
        /// <summary>
        /// 文件总大小
        /// </summary>
        public string FileSize { get; set; }
        /// <summary>
        /// 备份
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
