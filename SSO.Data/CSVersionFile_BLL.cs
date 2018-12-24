using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSO.Code;
using System.Data.SqlClient;
using System.Data;

namespace SSO.Data
{
    public class CSVersionFile_BLL
    {
        Log log = LogFactory.GetLogger(typeof(CSVersionFile_BLL));

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AjaxResult Add(C_S_VersionFile entity)
        {
            AjaxResult msg = new AjaxResult();

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO dbo.C_S_VersionFile       
                            VALUES  (  @VersionFileId ,@AppVersionId,@FileName, 
                                       @FileVersionCode ,@VirtualPath ,
                                       @FileSize,@CreateUserId,GETDATE())");

            List<SqlParameter> param = new List<SqlParameter>() { 
                new SqlParameter("VersionFileId",entity.VersionFileId),
                new SqlParameter("AppVersionId",entity.AppVersionId),
                new SqlParameter("FileName",entity.FileName),
                new SqlParameter("FileVersionCode",entity.FileVersionCode),
                new SqlParameter("VirtualPath",entity.VirtualPath),
                new SqlParameter("FileSize",entity.FileSize),
                new SqlParameter("CreateUserId",entity.CreateUserId)
            };

            try
            {
                int result = SqlHelper.ExecuteNonQuery(strSql.ToString(), param.ToArray());
                if (result == 1)
                {
                    msg.data = "";
                    msg.state = "success";
                    msg.message = "保存成功！";
                }
                else
                {
                    msg.data = "";
                    msg.state = "fail";
                    msg.message = "保存失败！";
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                msg.data = "";
                msg.state = "error";
                msg.message = "程序发生异常！";
            }

            return msg;
        }


        /// <summary>
        /// 根据版本主键获取版本文件虚拟路径
        /// </summary>
        /// <param name="AppVersionId">版本主键</param>
        /// <returns></returns>
        public string GetFileByAppVersionId(string AppVersionId)
        {
            string strSql = "SELECT * FROM dbo.C_S_VersionFile WHERE AppVersionId = @AppVersionId";
            SqlParameter[] param = new SqlParameter[] { new SqlParameter("AppVersionId", AppVersionId) };

            DataTable dt = SqlHelper.ExecuteDataTable(strSql, param);

            List<string> pathLi = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                pathLi.Add(dt.Rows[i]["VirtualPath"].ToString());
            }

            return pathLi.ToJson();
        }

        /// <summary>
        /// 获取版本文件的虚拟路径
        /// </summary>
        /// <param name="appCode">应用编码</param>
        /// <param name="version">版本号</param>
        /// <returns></returns>
        public List<string> GetFileVirtual(string appCode, string version)
        {
            string strSql = @"SELECT a.* FROM dbo.C_S_VersionFile a
                                LEFT JOIN dbo.C_S_AppVersion b
                                    ON b.AppVersionId = a.AppVersionId
                                LEFT JOIN dbo.C_S_Application c
                                    ON b.ApplicationId = c.ApplicationId
                            WHERE c.ApplicationCode = @ApplicationCode AND a.FileVersionCode = @FileVersionCode";

            SqlParameter[] param = new SqlParameter[] { 
             new SqlParameter("ApplicationCode",appCode),
             new SqlParameter("FileVersionCode",version)
            };

            DataTable dt = SqlHelper.ExecuteDataTable(strSql, param);

            List<string> pathLi = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                pathLi.Add(dt.Rows[i]["VirtualPath"].ToString());
            }

            return pathLi;
        }
    }


    /// <summary>
    /// 应用版本对应的文件
    /// </summary>
    public class C_S_VersionFile
    {
        /// <summary>
        /// 文件主键
        /// </summary>
        public string VersionFileId { get; set; }
        /// <summary>
        /// 版本主键    
        /// </summary>
        public string AppVersionId { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string FileVersionCode { get; set; }
        /// <summary>
        /// 虚拟路径
        /// </summary>
        public string VirtualPath { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }
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
