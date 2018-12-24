using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSO.Code;
using System.Data;
using System.Data.SqlClient;

namespace SSO.Data
{
    public class CSLog_BLL
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
                            (SELECT ROW_NUMBER() OVER (ORDER BY OperateTime DESC) AS rowid,
                                    *
                                FROM dbo.C_S_Log
                            ) tt
                            WHERE 1 = 1 ");

            strSql.Append(" and rowid between " + ((page.page - 1) * page.rows + 1) + " and " + page.page * page.rows + "");
            List<SqlParameter> parameter = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(" and (isnull(ApplicationCode,'')+isnull(OperateUserId,'')+isnull(MachineName,'')) like @keywords ");
                parameter.Add(new SqlParameter("keywords", '%' + keywords + '%'));
            }

            if (!string.IsNullOrEmpty(page.sidx))
            {
                strSql.Append(" order by " + page.sidx);
            }
            return SqlHelper.ExecuteDataTable(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="entity">日志实体</param>
        /// <returns></returns>
        public static AjaxResult Add(C_S_Log entity)
        {
            AjaxResult msg = new AjaxResult();

            string strSql = string.Format(@"INSERT INTO dbo.C_S_Log
                                            VALUES  ( NEWID() ,@OperateType ,@OperateUserId ,@ApplicationId , 
                                            @ApplicationCode ,@Content ,GETDATE() ,@IPAddress, @MachineName  )");

            SqlParameter[] parameter = new SqlParameter[]{ 
                new SqlParameter("OperateType",entity.OperateType),
                new SqlParameter("OperateUserId",entity.OperateUserId),
                new SqlParameter("ApplicationId",entity.ApplicationId),
                new SqlParameter("ApplicationCode",entity.ApplicationCode),
                new SqlParameter("Content",entity.Content),                
                new SqlParameter("IPAddress",entity.IPAddress),
                new SqlParameter("MachineName",entity.MachineName)
            };

            int tmp = SqlHelper.ExecuteNonQuery(strSql, parameter);
            if (tmp >= 1)
            {
                msg.data = "成功";
                msg.state = 1;
                msg.message = "写入日志成功。";
            }
            else
            {
                msg.data = "失败";
                msg.state = -1;
                msg.message = "写入日志失败。";
            }
            return msg;
        }


        /// <summary>
        /// 客户端更新日志
        /// </summary>
        /// <param name="appCode"></param>
        /// <param name="userId"></param>
        /// <param name="ip_HostName_state"></param>
        /// <returns></returns>
        public static AjaxResult WriteUpdateLog(string appCode, string userId, string ip_HostName_state)
        {
            AjaxResult msg = new AjaxResult();

            //获取用户数据
            string sqlStr = "SELECT * FROM dbo.[User] WHERE UserID=@UserID";
            DataTable dt = SqlHelper.ExecuteDataTable(sqlStr, new SqlParameter[] { new SqlParameter("UserID", userId) });

            string userName = string.Empty;
            if (dt.Rows.Count > 0)
            {
                userName = dt.Rows[0]["DisplayName"].ToString();
            }

            //获取应用数据
            dt = SqlHelper.ExecuteDataTable("SELECT * FROM dbo.C_S_Application WHERE ApplicationCode=@ApplicationCode", new SqlParameter[] { new SqlParameter("ApplicationCode", appCode) });

            string applicationId = string.Empty;

            if (dt.Rows.Count > 0)
            {
                applicationId = dt.Rows[0]["ApplicationId"].ToString();
            }

            //拆分获得 ip（ip地址）  hostName（客户端）  state （更新状态）
            string[] ip_hostName_state = ip_HostName_state.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            //默认更新成功
            int operateType = 1;
            string content = "客户端更新成功。";
            //如果更新失败
            if (ip_hostName_state.Last() == "fail")
            {
                operateType = 2;
                content = "客户端更新失败。";
            }
            C_S_Log entity = new C_S_Log()
            {
                OperateType = operateType,
                OperateUserId = userName,
                ApplicationId = applicationId,
                ApplicationCode = appCode,
                Content = content,
                IPAddress = ip_hostName_state[0],
                MachineName = ip_hostName_state[1]
            };

            msg = Add(entity);
            return msg;
        }
    }


    /// <summary>
    /// </summary>
    public class C_S_Log
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string LogId { get; set; }
        /// <summary>
        /// 0:上传;1:更新成功;2:更新失败:3:移除;4:新增
        /// </summary>
        public int OperateType { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperateUserId { get; set; }
        /// <summary>
        /// 应用主键
        /// </summary>
        public string ApplicationId { get; set; }
        /// <summary>
        /// 应用编码
        /// </summary>
        public string ApplicationCode { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OperateTime { get; set; }
        /// <summary>
        /// 操作人IP
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// 操作机器名
        /// </summary>
        public string MachineName { get; set; }
    }
}
