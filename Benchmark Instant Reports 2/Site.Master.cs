using System;
using System.Web.Security;
using System.Web.UI.WebControls;


namespace Benchmark_Instant_Reports_2
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MenuWithCampusRep.Visible = true;
            MenuSpecial.Visible = false;

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
            FormsAuthentication.SignOut();
            Response.Redirect("Login.aspx");

        }








    }
}
