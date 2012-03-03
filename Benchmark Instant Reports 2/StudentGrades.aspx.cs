using System;
using System.IO;
using System.Linq;
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
    public partial class StudentGrades : ReportPage<DropDownList>
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
        private static string appPath;
        private static string physicalPath;
        private static string reportPath;

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
            //repvwStudentGrades.Visible = false;
            //lblNoScanData.Visible = false;

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
            UIHelper.ToggleDDInitView(listTests, true);

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
            
            // get a list of teachers
            string[] listOfTeachers = studentDataToGrade.GetItems().Select(d => d.TeacherName).Distinct().ToArray();
            Array.Sort(listOfTeachers);

            // if there are no students taking this test at this campus (based on the number of teachers applicable), deal with it
            if (listOfTeachers.Length == 0)
            {
                repvwStudentGrades.Visible = false;
                lblNoScanData.Visible = true;
                return;
            }

            lblNoScanData.Visible = false;

            // activate the Teacher dropdown and populate with the list of teachers
            UIHelper.ToggleDDInitView(listTests, false);
            ddTeacher.DataSource = listOfTeachers;
            ddTeacher.DataBind();

            UIHelper.ToggleDDInitView(ddTeacher, true);
            repvwStudentGrades.Visible = false;

            return;
        }


        protected void ddTeacher_SelectedIndexChanged(object sender, EventArgs e)
        {
            repvwStudentGrades.Visible = false;
            btnGenReport.Enabled = true;

            return;
        }


        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            var schools = GetSelectedSchools();
            var tests = GetSelectedTests();

            if (studentDataToGrade.Count == 0)
                studentDataToGrade = StudentData.GetStudentDataToGrade(DataService, tests, schools);

            reportData = StGradesRepHelper.GenerateStudentGradesReportData(DataService,
                studentDataToGrade, tests);

            repvwStudentGrades.Visible = true;

            if (TestHelper.UsesWeightedAnswers(DataService, GetSelectedTests().First()))
            {
                // test with weighted items
                reportPath = Path.Combine(physicalPath, @"Reports\Grades\StudentGradesRep2.rdlc");
                repvwStudentGrades.LocalReport.ReportPath = reportPath;
                ReportDataSource rds = new ReportDataSource(repvwStudentGrades.LocalReport.GetDataSourceNames()[0],
                    reportData.GetItemsWhere(i => i.Teacher == ddTeacher.SelectedItem.ToString(), i => i.StudentID));

                repvwStudentGrades.LocalReport.DataSources.Clear();
                repvwStudentGrades.LocalReport.DataSources.Add(rds);
                repvwStudentGrades.ShowPrintButton = true;
                repvwStudentGrades.LocalReport.Refresh();
            }
            else
            {
                // test with non-weighted items
                reportPath = Path.Combine(physicalPath, @"Reports\Grades\StudentGradesRep1.rdlc");
                repvwStudentGrades.LocalReport.ReportPath = reportPath;
                ReportDataSource rds = new ReportDataSource(repvwStudentGrades.LocalReport.GetDataSourceNames()[0],
                    reportData.GetItemsWhere(i => i.Teacher == ddTeacher.SelectedItem.ToString(), i => i.StudentName));

                repvwStudentGrades.LocalReport.DataSources.Clear();
                repvwStudentGrades.LocalReport.DataSources.Add(rds);
                repvwStudentGrades.ShowPrintButton = true;
                repvwStudentGrades.LocalReport.Refresh();
            }


            return;
        }





        //**********************************************************************//
        //** initialize the page
        //**
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
            ddTeacher.Enabled = true;
            ddTeacher.AutoPostBack = true;
            btnGenReport.Enabled = false;
            repvwStudentGrades.Visible = false;
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