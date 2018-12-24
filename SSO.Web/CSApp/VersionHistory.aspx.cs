using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSO.Data;

namespace SSO.Web.CSApp
{
    public partial class VersionHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string applicationId = Request["ApplicationId"];
                string appName = Request["AppName"];
                string company = Request["Company"];

                AppName.InnerText = appName;
                p_Company.InnerText = company;
                DataTable dt = new CSAppVersion_BLL().GetCSAppVersion(applicationId);

                StringBuilder strHtml = new StringBuilder();
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strHtml.AppendFormat(@"<li class='work'>
                    <input class='radio' id='work{0}' name='works' type='radio' {1} />
                    <div class='relative'>
                        <label for='work{0}' style='font-size:16px;'>&nbsp;{2}</label>
                        <span class='date'> {6}</span>
                        <span class='circle'></span>
                    </div>
                    <div class='content'>
                        <p>
                            &nbsp;&nbsp;版本号：{3}</br>
                            &nbsp;&nbsp;大&nbsp;&nbsp;&nbsp;小：{4}</br>
                            &nbsp;&nbsp;上传人：{5}</br></br><a class='btn btn-default' onclick='deleteVersion(this)' data-id='{7}' style='color:blue;cursor:pointer;'>移除该版本</a>  
                        </p>
                    </div>
                </li>",
                   i, i == 0 ? "checked" : "", dt.Rows[i]["VersionCode"], dt.Rows[i]["VersionCode"], dt.Rows[i]["AppSize"], dt.Rows[i]["CreateBy"], Convert.ToDateTime(dt.Rows[i]["CreateTime"]).ToString("yyyy-MM-dd HH:mm"), dt.Rows[i]["AppVersionId"]);
                    }
                }
                else
                {
                    strHtml.Append("<p style='color:red;'>暂时没有上传该应用历史版本。</p>");
                }

                timeline.InnerHtml = strHtml.ToString();
            }
        }
    }
}