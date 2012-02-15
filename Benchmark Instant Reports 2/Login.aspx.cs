using System;
using System.Web.Security;
using Benchmark_Instant_Reports_2.Account;
using Benchmark_Instant_Reports_2.Infrastructure;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2
{
    public partial class Login : DataEnabledPage
    {
        public SiteMaster theMasterPage;

        protected void Page_Load(object sender, EventArgs e)
        {
            //theMasterPage.hideLogoutButton();

            if (!IsPostBack)
            {
                ddLoginCampus.DataSource = DataService.SchoolRepo.FindAll();
                ddLoginCampus.DataTextField = "Name";
                ddLoginCampus.DataValueField = "Abbr";
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

        protected void tbPasswordCampus_TextChanged(object sender, EventArgs e)
        {
            tbPasswordAdmin.Text = "";

            return;
        }

        protected void tbPasswordAdmin_TextChanged(object sender, EventArgs e)
        {
            tbPasswordCampus.Text = "";

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
                username = Constants.UsernameAllCampuses;
            }
            else
                return;


            if (Authorize.AuthorizeUser(DataService, ddLoginCampus.SelectedValue.ToString(), enteredpassword))
                FormsAuthentication.RedirectFromLoginPage(username, true);

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