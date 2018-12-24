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
    public class PersonalAppInfo_BLL
    {

        /// <summary>
        /// 根据应用编码获取用户本地路径
        /// </summary>
        /// <param name="appCode">应用编码</param>
        /// <param name="hostNama">机器名称</param>
        /// <returns></returns>
        public string GetLocalDir(string appCode, string hostNama)
        {
            string strSql = "select * from PersonalAppInfo where appcode = @appcode and userid = @hostNama";
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("appcode", appCode));
            param.Add(new SqlParameter("hostNama", hostNama));

            DataTable dt = SqlHelper.ExecuteDataTable(strSql, param.ToArray());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["LocalDir"].ToString();
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 记录用户的
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AjaxResult SetPersonalAppInfo(PersonalAppInfo entity)
        {

            AjaxResult msg = new AjaxResult();
            string strSql = "select * from PersonalAppInfo where UserId=@UserId and AppCode=@AppCode ";
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("UserId", entity.UserId));
            param.Add(new SqlParameter("AppCode", entity.AppCode));

            DataTable dt = SqlHelper.ExecuteDataTable(strSql, param.ToArray());

            param.Clear();
            if (dt.Rows.Count == 1)
            {
                strSql = "UPDATE [dbo].[PersonalAppInfo] SET LocalDir =@LocalDir  WHERE  UserId = @UserId and AppCode = @AppCode";
            }
            else if (dt.Rows.Count > 1)
            {
                strSql = "delete from PersonalAppInfo where UserId = @UserId and AppCode = @AppCode;";
                strSql += "insert into PersonalAppInfo values(newid(),@AppCode,@UserId,@LocalDir)";
            }
            else
            {
                strSql = "insert into PersonalAppInfo values(newid(),@AppCode,@UserId,@LocalDir)";
            }


            param.Add(new SqlParameter("AppCode", entity.AppCode));
            param.Add(new SqlParameter("UserId", entity.UserId));
            param.Add(new SqlParameter("LocalDir", entity.LocalDir));

            int result = SqlHelper.ExecuteNonQuery(strSql, param.ToArray());
            if (result > 0)
            {
                msg.message = "记录成功";
                msg.state = "success";
                msg.data = "";
            }
            else
            {
                msg.message = "记录成功";
                msg.state = "error";
                msg.data = "";
            }
            return msg;
        }

    }

    public class PersonalAppInfo
    {
        public string PersonalAppInfoId { get; set; }
        public string AppCode { get; set; }
        public string UserId { get; set; }
        public string LocalDir { get; set; }
    }
}
