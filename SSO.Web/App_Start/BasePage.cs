using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSO.Code;

namespace SSO.Web
{
    public class BasePage : System.Web.UI.Page
    {

        protected override void OnPreInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (OperatorProvider.Provider.GetCurrent() == null)
                {
                    HttpContext.Current.Response.Redirect("../Login/Index");
                }
            }
        }


    }
}