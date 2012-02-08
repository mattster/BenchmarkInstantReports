using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Account;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.Helpers;
using Benchmark_Instant_Reports_2.Helpers.Reports;
using Benchmark_Instant_Reports_2.Infrastructure;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
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

            // return if it is the separator
            if (UIHelper.IsDDSeparatorValue(ddCampus.SelectedValue.ToString()) || 
                ddCampus.SelectedValue.ToString() == "")
            {
                RememberHelper.SaveSelectedCampus(Response, "");
                return;
            }

            RememberHelper.SaveSelectedCampus(Response, ddCampus.SelectedItem.ToString());

            listTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            listTests.DataBind();

            setupTestFilters();
            listTests.Enabled = true;
            listTests.SelectedIndex = 0;
            repvwCampusReport1.Visible = false;
            repvwCampusReport2.Visible = false;


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

            repvwCampusReport1.Visible = false;
            repvwCampusReport2.Visible = false;
            
            return;
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            var schools = GetSelectedSchools();
            var tests = GetSelectedTests();
            studentDataToGrade = StudentData.GetStudentDataToGrade(DataService, tests, schools);
            reportData = StGradesRepHelper.GenerateStudentGradesReportData(DataService, studentDataToGrade, tests);

            if (schools.Count > 1)
            {   
                // Show All Campuses
                this.repvwCampusReport1.Visible = false;
                this.repvwCampusReport2.Visible = true;

                ReportDataSource rds = new ReportDataSource(repvwCampusReport2.LocalReport.GetDataSourceNames()[0],
                    reportData.GetItems());
                this.repvwCampusReport2.LocalReport.DataSources.Clear();
                this.repvwCampusReport2.LocalReport.DataSources.Add(rds);
                this.repvwCampusReport2.ShowPrintButton = true;
                this.repvwCampusReport2.LocalReport.Refresh();
                
            }
            else
            {   
                // Show One Campus
                this.repvwCampusReport1.Visible = true;
                this.repvwCampusReport2.Visible = false;

                ReportDataSource rds = new ReportDataSource(repvwCampusReport1.LocalReport.GetDataSourceNames()[0],
                    reportData.GetItems());
                this.repvwCampusReport1.LocalReport.DataSources.Clear();
                this.repvwCampusReport1.LocalReport.DataSources.Add(rds);
                this.repvwCampusReport1.ShowPrintButton = true;
                this.repvwCampusReport1.LocalReport.Refresh();
            }

            return;
        }



        //************************************************************************************************
        //** some stuff
        //************************************************************************************************


        //**********************************************************************//
        //** initialize the page
        //**
        private void initPage()
        {
            theMasterPage = Page.Master as SiteMaster;

            // disable all dialog boxes & stuff except campus
            ddCampus.Enabled = true;
            ddCampus.AutoPostBack = true;
            listTests.Enabled = true;
            listTests.AutoPostBack = true;
            btnGenReport.Enabled = true;
            repvwCampusReport1.Visible = false;
            repvwCampusReport2.Visible = false;
            

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