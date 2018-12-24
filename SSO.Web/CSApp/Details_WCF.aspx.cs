using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SSO.Data;
using System.Text;

namespace SSO.Web.CSApp
{
    public partial class Details_WCF : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string wcfVersionFilesId = Request["WCFVersionInfoId"];
                DataTable dt = new CSWCFVersionFiles_BLL().GetWCFVersionFiles(wcfVersionFilesId);

                if (dt.Rows.Count > 0)
                {
                    VersionCode.InnerText = dt.Rows[0]["VersionCode"].ToString();
                    CreateTime.InnerText = Convert.ToDateTime(dt.Rows[0]["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    CreateUserId.InnerText = dt.Rows[0]["CreateUserId"].ToString();

                    StringBuilder strHtml = new StringBuilder();

                    strHtml.Append(@"<table><tr><th>文件名</th><th>文件大小</th></tr>");
                    foreach (DataRow dr in dt.Rows)
                    {
                        strHtml.AppendFormat(@"<tr><td>{0}</td><td>{1}</td></tr>", dr["FileName"], dr["FileSize"]);
                    }
                    strHtml.Append("</table>");
                    FileTable.InnerHtml = strHtml.ToString();
                }
            }
        }
    }
}