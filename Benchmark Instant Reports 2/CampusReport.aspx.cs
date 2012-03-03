using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Account;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.Helpers;
using Benchmark_Instant_Reports_2.Helpers.Reports;
using Benchmark_Instant_Reports_2.Infrastructure;
using Benchmark_Instant_Reports_2.Infrastructure.DataStruct;
using Microsoft.Reporting.WebForms;


namespace Benchmark_Instant_Reports_2
{
    public partial class CampusReport : ReportPage<DropDownList>
    {
        public SiteMaster theMasterPage;
        private static DataToGradeItemCollection studentDataToGrade = new DataToGradeItemCollection();
        private static StGradeReportData reportData = new StGradeReportData();
        private static TestFilterState _thisTestFilterState = new TestFilterState();
        public override TestFilterState thisTestFilterState
        {
            get { return _thisTestFilterState; }
            set { _thisTestFilterState = value; }
        }
        private static string appPath;
        private static string physicalPath;
        private static string reportPath;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initPage();
            }

            return;
        }

        protected void ddCampus_SelectedIndexChanged(object sender, EventArgs e)
        {
            theMasterPage = Page.Master as SiteMaster;
            repvwCampusRep.Visible = false;

            // return if it is the separator
            if (UIHelper.IsDDSeparatorValue(ddCampus.SelectedValue.ToString()) || 
                ddCampus.SelectedValue.ToString() == "")
            {
                RememberHelper.SaveSelectedCampus(Response, "");
                ddCampus.SelectedIndex = 0;
                return;
            }

            RememberHelper.SaveSelectedCampus(Response, ddCampus.SelectedItem.ToString());

            listTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            listTests.DataBind();

            setupTestFilters();
            listTests.Enabled = true;
            listTests.SelectedIndex = 0;


            int bidx = UIHelper.GetIndexOfItemInDD(RememberHelper.SavedSelectedTestID(Request), listTests);
            if (bidx != -1)
            {
                listTests.SelectedIndex = bidx;
                listTests_SelectedIndexChanged(new object(), new EventArgs());
            }

            return;
        }

        protected void listTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            RememberHelper.SaveSelectedTestID(Response, listTests.SelectedItem.ToString());

            repvwCampusRep.Visible = false;            
            
            return;
        }


        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            var schools = GetSelectedSchools();
            var tests = GetSelectedTests();
            studentDataToGrade = StudentData.GetStudentDataToGrade(DataService, tests, schools);
            reportData = StGradesRepHelper.GenerateStudentGradesReportData(DataService, studentDataToGrade, tests);
            repvwCampusRep.Visible = true;

            if (schools.Count > 1)
            {   
                // Show All Campuses
                reportPath = Path.Combine(physicalPath, @"Reports\Campus\CampusRep2.rdlc");
            }
            else
            {   
                // Show One Campus
                reportPath = Path.Combine(physicalPath, @"Reports\Campus\CampusRep1.rdlc");
            }

            repvwCampusRep.LocalReport.ReportPath = reportPath;
            ReportDataSource rds = new ReportDataSource(repvwCampusRep.LocalReport.GetDataSourceNames()[0],
                reportData.GetItems());
            this.repvwCampusRep.LocalReport.DataSources.Clear();
            this.repvwCampusRep.LocalReport.DataSources.Add(rds);
            this.repvwCampusRep.ShowPrintButton = true;
            this.repvwCampusRep.LocalReport.Refresh();

            return;
        }




        private void initPage()
        {
            theMasterPage = Page.Master as SiteMaster;
            appPath = HttpContext.Current.Request.ApplicationPath;
            physicalPath = HttpContext.Current.Request.MapPath(appPath);

            // disable all dialog boxes & stuff except campus
            ddCampus.Enabled = true;
            ddCampus.AutoPostBack = true;
            listTests.Enabled = true;
            listTests.AutoPostBack = true;
            btnGenReport.Enabled = true;
            repvwCampusRep.Visible = false;
            

            // load list of campuses in Campus dropdown
            ddCampus.DataSource = Authorize.GetAuthorizedSchools(Context.User.Identity.Name, DataService);
            ddCampus.DataTextField = "Name";
            ddCampus.DataValueField = "Abbr";
            ddCampus.DataBind();

            // add option for "ALL" if authorized as admin
            //if (CampusSecurity.isAuthorizedAsAdmin(Request))
            //{
            //    ddCampus.Items.Insert(0, new ListItem("ALL Secondary", "ALL Secondary"));
            //    ddCampus.Items.Insert(0, new ListItem("ALL Elementary", "ALL Elementary"));
            //    ddCampus.SelectedIndex = 0;
            //}

            int cidx = UIHelper.GetIndexOfItemInDD(RememberHelper.SavedSelectedCampus(Request), ddCampus);
            if (cidx != -1)
                ddCampus.SelectedIndex = cidx;
            else
                ddCampus.SelectedIndex = 0;

            setupTestFilters();

            listTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            listTests.DataBind();

            int bidx = UIHelper.GetIndexOfItemInDD(RememberHelper.SavedSelectedTestID(Request), listTests);
            if (bidx != -1)
            {
                listTests.SelectedIndex = bidx;
                listTests_SelectedIndexChanged(new object(), new EventArgs());
            }
            ddCampus_SelectedIndexChanged(new object(), new EventArgs());
            return;
        }
    }
}