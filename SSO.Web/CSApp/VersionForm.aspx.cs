using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SSO.Web.CSApp
{
    public partial class VersionForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["AppVersionId"] == null)
                {
                    AppVersionId.Value = Guid.NewGuid().ToString();
                }
            }
        }
    }
}