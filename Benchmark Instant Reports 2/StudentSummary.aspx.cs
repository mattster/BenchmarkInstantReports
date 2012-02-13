using System;
using System.Linq;
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
    public partial class StudentSummary : ReportPage<DropDownList>
    {

        #region globals

        public SiteMaster theMasterPage;
        private static DataToGradeItemCollection studentDataToGrade = new DataToGradeItemCollection();
        private static StGradeReportData reportData = new StGradeReportData();
        private static TestFilterState _thisTestFilterState = new TestFilterState();
        public override TestFilterState thisTestFilterState
        {
            get { return _thisTestFilterState; }
            set { _thisTestFilterState = value; }
        }

        #endregion


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
                ddCampus.SelectedIndex = 0;
                return;
            }

            // setup stuff
            RememberHelper.SaveSelectedCampus(Response, ddCampus.SelectedItem.ToString());

            listTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            listTests.DataBind();

            setupTestFilters();
            listTests.Enabled = true;
            UIHelper.ToggleDDInitView(listTests, true);
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;

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
            lblNoScanData.Visible = false;
            RememberHelper.SaveSelectedTestID(Response, listTests.SelectedItem.ToString());

            studentDataToGrade = StudentData.GetStudentDataToGrade(DataService, GetSelectedTests(), GetSelectedSchools());

            // get a list of teachers applicable for this query
            string[] listOfTeachers = studentDataToGrade.GetItems().Select(d => d.TeacherName).Distinct().ToArray();
            Array.Sort(listOfTeachers);
            
            // if there are no students taking this test at this campus, deal with it
            if (listOfTeachers.Length == 0)
            {
                repvwStudentSummary.Visible = false;
                lblAlignmentNote.Visible = false;
                lblNoScanData.Visible = true;
                return;
            }

            lblNoScanData.Visible = false;

            // activate the Teacher dropdown and populate with the list of teachers
            UIHelper.ToggleDDInitView(listTests, false);
            ddTeacher.DataSource = listOfTeachers;
            ddTeacher.DataBind();
            UIHelper.ToggleDDInitView(ddTeacher, true);
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;

            return;
        }


        protected void ddTeacher_SelectedIndexChanged(object sender, EventArgs e)
        {
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;
            btnGenReport.Enabled = true;

            return;
        }


        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            var schools = GetSelectedSchools();
            var tests = GetSelectedTests();

            if (studentDataToGrade.Count == 0)
                studentDataToGrade = StudentData.GetStudentDataToGrade(DataService, tests, schools);

            reportData = StGradesRepHelper.GenerateStudentGradesReportData(DataService, studentDataToGrade, tests);

            repvwStudentSummary.Visible = true;
            lblAlignmentNote.Visible = true;

            ReportDataSource rds = new ReportDataSource(repvwStudentSummary.LocalReport.GetDataSourceNames()[0],
                reportData.GetItemsWhere(i => i.Teacher == ddTeacher.SelectedItem.ToString(), i => i.StudentName));
            repvwStudentSummary.LocalReport.DataSources.Clear();
            repvwStudentSummary.LocalReport.DataSources.Add(rds);
            repvwStudentSummary.ShowPrintButton = true;
            repvwStudentSummary.LocalReport.Refresh();

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
            ddTeacher.Enabled = true;
            ddTeacher.AutoPostBack = true;
            btnGenReport.Enabled = false;
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;
            lblNoScanData.Visible = false;

            // load list of campuses in Campus dropdown
            ddCampus.DataSource = Authorize.GetAuthorizedSchools(Context.User.Identity.Name, DataService);
            ddCampus.DataTextField = "Name";
            ddCampus.DataValueField = "Abbr";
            ddCampus.DataBind();

            int cidx = UIHelper.GetIndexOfItemInDD(RememberHelper.SavedSelectedCampus(Request), ddCampus);
            if (cidx != -1)
                ddCampus.SelectedIndex = cidx;
            else
                ddCampus.SelectedIndex = 0;

            setupTestFilters();

            // load list of benchmarks in Benchmark dropdown
            listTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            listTests.DataBind();

            int bidx = UIHelper.GetIndexOfItemInDD(RememberHelper.SavedSelectedTestID(Request), listTests);
            if (bidx != -1)
            {
                listTests.SelectedIndex = bidx;
            }
            listTests_SelectedIndexChanged(new object(), new EventArgs());

            return;
        }

    }
}