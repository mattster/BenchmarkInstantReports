using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Data;
using System.Configuration;
using System.Web.Security;


namespace Benchmark_Instant_Reports_2
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MenuWithCampusRep.Visible = true;
            MenuDefault.Visible = false;

            CampusAuthLabel.Text = Context.User.Identity.Name;
            if (!Context.User.Identity.IsAuthenticated)
                pnlAuthInfo.Visible = false;


        }

        protected void NavigationMenu_MenuItemClick(object sender, MenuEventArgs e)
        {

        }


        public void updateCampusAuthLabel(string campus)
        {
            CampusAuthLabel.Text = campus;

            if (campus != "none")
                LogoutButton.Visible = true;
            else
                LogoutButton.Visible = false;
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            //CampusSecurity.deAuthorize(Response);
            //updateCampusAuthLabel("none");

            FormsAuthentication.SignOut();
            Response.Redirect("Login.aspx");

        }








    }
}
