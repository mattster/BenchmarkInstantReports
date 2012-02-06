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
    public partial class StudentStats : ReportPage<DropDownList>
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

            //*** User selected a campus ***//

            // return if it is the separator
            if (UIHelper.isDDSeparatorValue(ddCampus.SelectedValue.ToString()) ||
                ddCampus.SelectedValue.ToString() == "")
            {
                RememberHelper.savedSelectedCampus(Response, "");
                return;
            }

            RememberHelper.savedSelectedCampus(Response, ddCampus.SelectedItem.ToString());

            listTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            listTests.DataBind();

            setupTestFilters();
            listTests.Enabled = true;
            UIHelper.toggleDDLInitView(listTests, true);
            btnGenReport.Enabled = false;
            repvwStudentStats2a.Visible = false;
            repvwStudentStats2b.Visible = false;

            int bidx = UIHelper.getIndexOfDDItem(RememberHelper.savedSelectedTestID(Request), listTests);
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
            RememberHelper.savedSelectedTestID(Response, listTests.SelectedItem.ToString());

            studentDataToGrade = StudentData.GetStudentDataToGrade(DataService, GetSelectedTests(), GetSelectedSchools());
            
            // get a list of teachers applicable for this query
            string[] listOfTeachers = studentDataToGrade.GetItems().Select(d => d.TeacherName).Distinct().ToArray();
            Array.Sort(listOfTeachers);

            // if there are no students taking this test at this campus (based on the number of teachers applicable), deal with it
            if (listOfTeachers.Length == 0)
            {
                repvwStudentStats2a.Visible = false;
                repvwStudentStats2b.Visible = false;
                lblNoScanData.Visible = true;
                return;
            }

            lblNoScanData.Visible = false;

            // activate the Teacher dropdown and populate with the list of teachers
            UIHelper.toggleDDLInitView(listTests, false);
            ddTeacher.DataSource = listOfTeachers;
            ddTeacher.DataBind();

            // add option for All Teachers
            //ddTeacher.Items.Insert(0, new ListItem(birIF.allTeachers, birIF.allTeachers));

            UIHelper.toggleDDLInitView(ddTeacher, true);
            repvwStudentStats2a.Visible = false;
            repvwStudentStats2b.Visible = false;

            return;
        }


        protected void ddTeacher_SelectedIndexChanged(object sender, EventArgs e)
        {
            repvwStudentStats2a.Visible = false;
            repvwStudentStats2b.Visible = false;
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


            if (TestHelper.UsesWeightedAnswers(DataService, GetSelectedTests().First()))
            {
                //test with weighted items
                ReportDataSource rds = new ReportDataSource(repvwStudentStats2b.LocalReport.GetDataSourceNames()[0],
                    reportData.GetItemsWhere(i => i.Teacher == ddTeacher.SelectedItem.ToString()));
                repvwStudentStats2b.Visible = true;
                repvwStudentStats2a.Visible = false;

                this.repvwStudentStats2b.LocalReport.DataSources.Clear();
                this.repvwStudentStats2b.LocalReport.DataSources.Add(rds);
                this.repvwStudentStats2b.ShowPrintButton = true;
                this.repvwStudentStats2b.LocalReport.Refresh();
            }
            else
            {
                ReportDataSource rds = new ReportDataSource(repvwStudentStats2a.LocalReport.GetDataSourceNames()[0],
                    reportData.GetItemsWhere(i => i.Teacher == ddTeacher.SelectedItem.ToString()));
                repvwStudentStats2a.Visible = true;
                repvwStudentStats2b.Visible = false;

                this.repvwStudentStats2a.LocalReport.DataSources.Clear();
                this.repvwStudentStats2a.LocalReport.DataSources.Add(rds);
                this.repvwStudentStats2a.ShowPrintButton = true;
                this.repvwStudentStats2a.LocalReport.Refresh();
            }

            return;
        }





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
            repvwStudentStats2a.Visible = false;
            repvwStudentStats2b.Visible = false;
            lblNoScanData.Visible = false;

            // load list of campuses in Campus dropdown
            ddCampus.DataSource = Authorize.getAuthorizedCampusList(Context.User.Identity.Name, DataService);
            ddCampus.DataTextField = "Name";
            ddCampus.DataValueField = "Abbr";
            ddCampus.DataBind();

            int cidx = UIHelper.getIndexOfDDItem(RememberHelper.savedSelectedCampus(Request), ddCampus);
            if (cidx != -1)
                ddCampus.SelectedIndex = cidx;
            else
                ddCampus.SelectedIndex = 0;

            setupTestFilters();

            // load list of benchmarks in Benchmark dropdown
            listTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            listTests.DataBind();

            int bidx = UIHelper.getIndexOfDDItem(RememberHelper.savedSelectedTestID(Request), listTests);
            if (bidx != -1)
            {
                listTests.SelectedIndex = bidx;
            }
            listTests_SelectedIndexChanged(new object(), new EventArgs());

            return;
        }

    }
}