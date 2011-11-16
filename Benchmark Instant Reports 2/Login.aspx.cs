using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Benchmark_Instant_Reports_2
{
    public partial class Login : System.Web.UI.Page
    {
        public SiteMaster theMasterPage;

        protected void Page_Load(object sender, EventArgs e)
        {
            //theMasterPage.hideLogoutButton();

            if (!IsPostBack)
            {
                ddLoginCampus.DataSource = dbIFOracle.getDataSource(birIF.getCampusListQuery);
                ddLoginCampus.DataTextField = "SCHOOLNAME";
                ddLoginCampus.DataValueField = "SCHOOL_ABBR";
                ddLoginCampus.DataBind();
            }
            return;
        }

        protected void ddLoginCampus_SelectedIndexChanged(object sender, EventArgs e)
        {
            // clear admin password box
            tbPasswordAdmin.Text = "";

            return;
        }


        protected void Login_Click(object sender, EventArgs e)
        {
            string enteredpassword;
            string username;

            if (tbPasswordCampus.Text.Length > 0)
            {
                enteredpassword = tbPasswordCampus.Text.ToString();
                username = ddLoginCampus.SelectedValue.ToString();
            }
            else if (tbPasswordAdmin.Text.Length > 0)
            {
                enteredpassword = tbPasswordAdmin.Text.ToString();
                username = birIF.usernameAllCampuses;
            }
            else
                return;


            if (CampusSecurity.checkEnteredPassword(enteredpassword, ddLoginCampus.SelectedValue.ToString(), Response))
            {                                                   
                // authentication succeeded
                FormsAuthentication.RedirectFromLoginPage(username, true);
            }
            else                                                
            {
                // authentication failed        
                this.mpupIncorrectPassword.Show();
                tbPasswordAdmin.Text = "";
                tbPasswordCampus.Text = "";

                return;
            }

            return;
        }
    }
}