using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSO.Data;

namespace SSO.Web.CSApp
{
    public partial class Form_WCF : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (HttpContext.Current.Request["WCFVersionInfoId"] == null)
                {
                    WCFVersionInfoId.Value = Guid.NewGuid().ToString();
                }
                //获取上个版本号
                string previewVersion = new CSWCFVersionInfo_BLL().GetPreviVersion();
                previVersion.InnerText = previewVersion;
            }
        }
    }
}