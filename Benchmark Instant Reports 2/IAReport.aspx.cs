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
    public partial class IAReport : ReportPage<DropDownList>
    {
        #region globals

        public SiteMaster theMasterPage;
        private static DataToGradeItemCollection studentDataToGrade = new DataToGradeItemCollection();
        private static IAReportData reportData = new IAReportData();
        private static TestFilterState _thisTestFilterState = new TestFilterState();
        public override TestFilterState thisTestFilterState
        {
            get { return _thisTestFilterState; }
            set { _thisTestFilterState = value; }
        }

        private static bool reportDataParmsHaveChanged = true;
        private static string repTypePctCorrectAllTeachers = "% Correct-All Teachers";
        private static string repTypePctCorrectOneTeacher = "% Correct-One Teacher";
        private static string repTypeAnsChoiceOneTeacher = "Ans. Choice-One Teacher";
        private static string repTypeAnsChoiceAllTeachers = "Ans. Choice-All Teachers";
        private static string groupByQ = "Question Num.";
        private static string groupByObj = "Rep. Category";
        private static string groupByTEKS = "TEKS";
        private static string[] reportTypesList = { repTypePctCorrectAllTeachers, repTypePctCorrectOneTeacher, repTypeAnsChoiceAllTeachers, repTypeAnsChoiceOneTeacher };
        private static string[] reportTypesListTeacherOnly = { repTypePctCorrectOneTeacher, repTypeAnsChoiceOneTeacher, repTypeAnsChoiceAllTeachers };
        private static string[] groupByList = { groupByQ, groupByObj, groupByTEKS };
        private static string appPath;
        private static string physicalPath;
        private static string reportPath;

        private static IAReportData resultsData = new IAReportData();

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
            if (UIHelper.IsDDSeparatorValue(ddCampus.SelectedValue.ToString()))
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
            ddTeacher.Visible = false;
            lblSelectTeacher.Visible = false;
            repvwIA.Visible = false;
            reportDataParmsHaveChanged = true;

            ddRepType.DataSource = reportTypesList;
            ddRepType.DataBind();
            ddRepType.SelectedIndex = 0;

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
            ddTeacher.DataSource = listOfTeachers;
            ddTeacher.DataBind();
            UIHelper.ToggleDDInitView(ddTeacher, true);

            // if there are no students taking this test at this campus, deal with it
            if (listOfTeachers.Length == 0)
            {
                ddTeacher.Visible = false;
                lblSelectTeacher.Visible = false;
                repvwIA.Visible = false;
                reportDataParmsHaveChanged = true;
                lblNoScanData.Visible = true;

                return;
            }

            lblNoScanData.Visible = false;
            repvwIA.Visible = false;
            reportDataParmsHaveChanged = true;

            return;
        }


        protected void ddRepType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddRepType.SelectedItem.ToString() == repTypePctCorrectAllTeachers)
            {
                lblSelectTeacher.Visible = false;
                ddTeacher.Visible = false;
                btnGenReport.Enabled = true;
                repvwIA.Visible = false;
                if (!reportDataParmsHaveChanged)
                    setupReportPctCorrectAllTeachers(ddGroupBy.SelectedItem.ToString());
            }
            else if (ddRepType.SelectedItem.ToString() == repTypePctCorrectOneTeacher)
            {
                lblSelectTeacher.Visible = true;
                ddTeacher.Visible = true;
                btnGenReport.Enabled = true;
                repvwIA.Visible = false;
                if (!reportDataParmsHaveChanged && ddTeacher.SelectedIndex >= 0)
                    setupReportPctCorrectOneTeacher(ddGroupBy.SelectedItem.ToString());
            }
            else if (ddRepType.SelectedItem.ToString() == repTypeAnsChoiceOneTeacher)
            {
                lblSelectTeacher.Visible = true;
                ddTeacher.Visible = true;
                btnGenReport.Enabled = true;
                repvwIA.Visible = false;
                if (!reportDataParmsHaveChanged && ddTeacher.SelectedIndex >= 0)
                    setupReportAnsChoiceOneTeacher(ddGroupBy.SelectedItem.ToString());
            }
            else if (ddRepType.SelectedItem.ToString() == repTypeAnsChoiceAllTeachers)
            {
                lblSelectTeacher.Visible = false;
                ddTeacher.Visible = false;
                btnGenReport.Enabled = true;
                repvwIA.Visible = false;
                if (!reportDataParmsHaveChanged)
                    setupReportAnsChoiceAllTeachers(ddGroupBy.SelectedItem.ToString());
            }

            return;
        }


        protected void ddTeacher_SelectedIndexChanged(object sender, EventArgs e)
        {
            UIHelper.ToggleDDInitView(ddTeacher, false);
            btnGenReport.Enabled = true;
            repvwIA.Visible = false;

            if (!reportDataParmsHaveChanged)
                if (ddRepType.SelectedItem.ToString() == repTypePctCorrectOneTeacher)
                    setupReportPctCorrectOneTeacher(ddGroupBy.SelectedItem.ToString());
                else if (ddRepType.SelectedItem.ToString() == repTypeAnsChoiceOneTeacher)
                    setupReportAnsChoiceOneTeacher(ddGroupBy.SelectedItem.ToString());

            return;
        }


        protected void ddGroupBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!reportDataParmsHaveChanged)
                if (ddRepType.SelectedItem.ToString() == repTypePctCorrectAllTeachers)
                    setupReportPctCorrectAllTeachers(ddGroupBy.SelectedItem.ToString());
                else if (ddRepType.SelectedItem.ToString() == repTypePctCorrectOneTeacher)
                    setupReportPctCorrectOneTeacher(ddGroupBy.SelectedItem.ToString());
                else if (ddRepType.SelectedItem.ToString() == repTypeAnsChoiceOneTeacher)
                    setupReportAnsChoiceOneTeacher(ddGroupBy.SelectedItem.ToString());
                else if (ddRepType.SelectedItem.ToString() == repTypeAnsChoiceAllTeachers)
                    setupReportAnsChoiceAllTeachers(ddGroupBy.SelectedItem.ToString());

            return;
        }


        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            // generate results for the given criteria on the page if we need to
            if (reportDataParmsHaveChanged)
            {
                studentDataToGrade = StudentData.GetStudentDataToGrade(DataService, GetSelectedTests(), 
                    GetSelectedSchools());
                resultsData = IARepHelper.GenerateBenchmarkStatsRepTable(DataService, studentDataToGrade, 
                    GetSelectedTests());

                reportDataParmsHaveChanged = false;
            }

            if (ddRepType.SelectedItem.ToString() == repTypePctCorrectAllTeachers)
                setupReportPctCorrectAllTeachers(ddGroupBy.SelectedItem.ToString());
            else if (ddRepType.SelectedItem.ToString() == repTypePctCorrectOneTeacher)
                setupReportPctCorrectOneTeacher(ddGroupBy.SelectedItem.ToString());
            else if (ddRepType.SelectedItem.ToString() == repTypeAnsChoiceOneTeacher)
                setupReportAnsChoiceOneTeacher(ddGroupBy.SelectedItem.ToString());
            else if (ddRepType.SelectedItem.ToString() == repTypeAnsChoiceAllTeachers)
                setupReportAnsChoiceAllTeachers(ddGroupBy.SelectedItem.ToString());

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
            ddTeacher.Visible = false;
            ddTeacher.AutoPostBack = true;
            lblSelectTeacher.Visible = false;
            ddRepType.Enabled = true;
            ddRepType.AutoPostBack = true;
            ddGroupBy.Enabled = true;
            ddGroupBy.AutoPostBack = true;
            btnGenReport.Enabled = true;
            //makeRepsVisible(repsNone, repsNone);
            repvwIA.Visible = false;
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

            // load list of report types in Reports dropdown
            ddRepType.DataSource = reportTypesList;
            ddRepType.DataBind();
            ddRepType.SelectedIndex = 0;

            // load Group By choices for reports
            ddGroupBy.DataSource = groupByList;
            ddGroupBy.DataBind();
            ddGroupBy.SelectedIndex = 0;

            int bidx = UIHelper.GetIndexOfItemInDD(RememberHelper.SavedSelectedTestID(Request), listTests);
            if (bidx != -1)
            {
                listTests.SelectedIndex = bidx;
            }
            listTests_SelectedIndexChanged(new object(), new EventArgs());

            return;
        }


        private void setupReportPctCorrectAllTeachers(string groupBySelection)
        {
            repvwIA.Visible = true;

            if (groupBySelection == groupByQ)
            {
                reportPath = Path.Combine(physicalPath, @"Reports\IA\IARep1a.rdlc");
            }
            else if (groupBySelection == groupByObj)
            {
                reportPath = Path.Combine(physicalPath, @"Reports\IA\IARep1b.rdlc");
            }
            else if (groupBySelection == groupByTEKS)
            {
                reportPath = Path.Combine(physicalPath, @"Reports\IA\IARep1c.rdlc");
            }

            repvwIA.LocalReport.ReportPath = reportPath;
            ReportDataSource rds = new ReportDataSource(repvwIA.LocalReport.GetDataSourceNames()[0], resultsData.GetItems());
            repvwIA.LocalReport.DataSources.Clear();
            repvwIA.LocalReport.DataSources.Add(rds);
            repvwIA.ShowPrintButton = true;
            repvwIA.LocalReport.Refresh();

            return;
        }


        private void setupReportPctCorrectOneTeacher(string groupBySelection)
        {
            repvwIA.Visible = true;

            if (groupBySelection == groupByQ)
            {
                reportPath = Path.Combine(physicalPath, @"Reports\IA\IARep2a.rdlc");
            }
            else if (groupBySelection == groupByObj)
            {
                reportPath = Path.Combine(physicalPath, @"Reports\IA\IARep2b.rdlc");
            }
            else if (groupBySelection == groupByTEKS)
            {
                reportPath = Path.Combine(physicalPath, @"Reports\IA\IARep2c.rdlc");
            }

            repvwIA.LocalReport.ReportPath = reportPath;
            ReportDataSource rds = new ReportDataSource(repvwIA.LocalReport.GetDataSourceNames()[0],
                resultsData.GetItemsWhere(d => d.Teacher == ddTeacher.SelectedItem.ToString().Replace("'", "''")));
            repvwIA.LocalReport.DataSources.Clear();
            repvwIA.LocalReport.DataSources.Add(rds);
            repvwIA.ShowPrintButton = true;
            repvwIA.LocalReport.Refresh();

            return;
        }


        private void setupReportAnsChoiceOneTeacher(string groupBySelection)
        {
            repvwIA.Visible = true;

            if (groupBySelection == groupByQ)
            {
                reportPath = Path.Combine(physicalPath, @"Reports\IA\IARep3a.rdlc");
            }
            else if (groupBySelection == groupByObj)
            {
                reportPath = Path.Combine(physicalPath, @"Reports\IA\IARep3b.rdlc");
            }
            else if (groupBySelection == groupByTEKS)
            {
                reportPath = Path.Combine(physicalPath, @"Reports\IA\IARep3c.rdlc");
            }

            repvwIA.LocalReport.ReportPath = reportPath;
            ReportDataSource rds = new ReportDataSource(repvwIA.LocalReport.GetDataSourceNames()[0],
                resultsData.GetItemsWhere(d => d.Teacher == ddTeacher.SelectedItem.ToString().Replace("'", "''")));
            repvwIA.LocalReport.DataSources.Clear();
            repvwIA.LocalReport.DataSources.Add(rds);
            repvwIA.ShowPrintButton = true;
            repvwIA.LocalReport.Refresh();

            return;
        }


        private void setupReportAnsChoiceAllTeachers(string groupBySelection)
        {
            repvwIA.Visible = true; 

            if (groupBySelection == groupByQ)
            {
                reportPath = Path.Combine(physicalPath, @"Reports\IA\IARep4a.rdlc");
            }
            else if (groupBySelection == groupByObj)
            {
                reportPath = Path.Combine(physicalPath, @"Reports\IA\IARep4b.rdlc");
            }
            else if (groupBySelection == groupByTEKS)
            {
                reportPath = Path.Combine(physicalPath, @"Reports\IA\IARep4c.rdlc");
            }

            repvwIA.LocalReport.ReportPath = reportPath; 
            ReportDataSource rds = new ReportDataSource(repvwIA.LocalReport.GetDataSourceNames()[0], resultsData.GetItems());
            repvwIA.LocalReport.DataSources.Clear();
            repvwIA.LocalReport.DataSources.Add(rds);
            repvwIA.ShowPrintButton = true;
            repvwIA.LocalReport.Refresh();

            return;
        }
    }
}