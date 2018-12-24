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
    public class CSWCFVersionFiles_BLL
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AjaxResult Add(C_S_WCFVersionFiles entity)
        {
            AjaxResult msg = new AjaxResult();
            List<SqlParameter> parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("WCFVersionInfoId", entity.WCFVersionInfoId));

            string sqlStr = "select * from C_S_WCFVersionInfo where WCFVersionInfoId = @WCFVersionInfoId ";

            DataTable dt = SqlHelper.ExecuteDataTable(sqlStr, parameter.ToArray());

            if (dt.Rows.Count <= 0)
            {
                msg.data = "";
                msg.state = "error";
                msg.message = "新增失败，新增所属版本信息不存在！";
            }
            else
            {
                sqlStr = @"INSERT INTO dbo.C_S_WCFVersionFiles
                                    ( WCFVersionFileId ,WCFVersionInfoId ,FileName ,
                                      FileSize ,FilePath ,CreateUserId ,CreateTime
                                    )
                            VALUES  ( @WCFVersionFileId ,@WCFVersionInfoId,@FILENAME,
                                      @FileSize,@FilePath ,@CreateUserId,@CreateTime)";
                parameter.Clear();
                parameter.Add(new SqlParameter("WCFVersionFileId", entity.WCFVersionFileId));
                parameter.Add(new SqlParameter("WCFVersionInfoId", entity.WCFVersionInfoId));
                parameter.Add(new SqlParameter("FileName", entity.FileName));
                parameter.Add(new SqlParameter("FileSize", entity.FileSize));
                parameter.Add(new SqlParameter("FilePath", entity.FilePath));
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
        /// 获取更新版本文件信息
        /// </summary>
        /// <param name="WCFVersionInfoId"></param>
        /// <returns></returns>
        public DataTable GetWCFVersionFiles(string WCFVersionInfoId)
        {
            string strSql = @"SELECT *,b.VersionCode FROM dbo.C_S_WCFVersionFiles a LEFT JOIN dbo.C_S_WCFVersionInfo b ON b.WCFVersionInfoId = a.WCFVersionInfoId
                                WHERE a.WCFVersionInfoId = @WCFVersionInfoId";

            return SqlHelper.ExecuteDataTable(strSql, new SqlParameter("WCFVersionInfoId", WCFVersionInfoId));
        }
    }


    /// <summary>
    /// WCF版本文件信息
    /// </summary>
    public class C_S_WCFVersionFiles
    {
        /// <summary>
        /// 文件主键
        /// </summary>
        public string WCFVersionFileId { get; set; }
        /// <summary>
        /// 对应版本主键
        /// </summary>
        public string WCFVersionInfoId { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }
        /// <summary>
        /// 文件虚拟路劲
        /// </summary>
        public string FilePath { get; set; }
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
