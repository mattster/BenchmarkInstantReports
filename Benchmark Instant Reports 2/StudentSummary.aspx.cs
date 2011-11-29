using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Helpers;
using Benchmark_Instant_Reports_2.Infrastructure;
using Microsoft.Reporting.WebForms;

namespace Benchmark_Instant_Reports_2
{
    public partial class StudentSummary : ReportPage<DropDownList>
    {

        #region globals

        public SiteMaster theMasterPage;
        private static TestFilterState _thisTestFilterState = new TestFilterState();
        public override TestFilterState thisTestFilterState
        {
            get { return _thisTestFilterState; }
            set { _thisTestFilterState = value; }
        }

        private static DataSet studentListQueryData = new DataSet();        // holds results of the custom query
        private static DataView studentListDataByTeacher = new DataView();  // custom query filtered by teacher
        private static DataView studentListDataByTeacherPeriod = new DataView();    // custom query filtered by teacher, period
        private static DataSet dsStudentListData = new DataSet();           // the filtered list of students
        private static DataSet dsStudentDataToGrade = new DataSet();        // student data to grade

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {

            // activate only the Campus dropdown, deactivate the
            // others until they are ready to be filled

            if (!IsPostBack)
            {
                initPage();
            }


            // anything else we need to do

            return;

        }


        protected void ddCampus_SelectedIndexChanged(object sender, EventArgs e)
        {
            theMasterPage = Page.Master as SiteMaster;

            //*** User selected a campus ***//

            // return if it is the separator
            if (birUtilities.isDDSeparatorValue(ddCampus.SelectedValue.ToString()))
            {
                birUtilities.savedSelectedCampus(Response, "");
                return;
            }

            // setup stuff
            birUtilities.savedSelectedCampus(Response, ddCampus.SelectedItem.ToString());

            listTests.DataSource = birIF.getTestListForSchool(ddCampus.SelectedValue.ToString());
            listTests.DataBind();

            TestFilter.SetupTestFilterPopup(ddTFCur, ddTFTestType, ddTFTestVersion, ddCampus.SelectedValue.ToString());
            listTests.Enabled = true;
            birUtilities.toggleDDLInitView(listTests, true);
            ddTeacher.Enabled = false;
            btnGenReport.Enabled = false;
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;

            int bidx = birUtilities.getIndexOfDDItem(birUtilities.savedSelectedTestID(Request), listTests);
            if (bidx != -1)
            {
                listTests.SelectedIndex = bidx;
                listTests_SelectedIndexChanged(new object(), new EventArgs());
            }

            return;
        }

        protected void listTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            //*** User selected a test ***//
            lblNoScanData.Visible = false;
            birUtilities.savedSelectedTestID(Response, listTests.SelectedItem.ToString());

            dsStudentDataToGrade = birIF.getStudentDataToGrade(listTests.SelectedItem.ToString(),
                ddCampus.SelectedValue.ToString());

            // get a list of teachers applicable for this query
            string[] listOfTeachers = birUtilities.getUniqueTableColumnStringValues(dsStudentDataToGrade.Tables[0],
                    birIF.teacherNameFieldName);
            Array.Sort(listOfTeachers);


            // if there are no students taking this test at this campus, deal with it
            if (listOfTeachers.Count() == 0)
            {
                ddTeacher.Enabled = false;
                btnGenReport.Enabled = false;
                repvwStudentSummary.Visible = false;
                lblAlignmentNote.Visible = false;

                lblNoScanData.Visible = true;

                return;
            }

            lblNoScanData.Visible = false;

            // activate the Teacher dropdown and populate with the list of teachers
            birUtilities.toggleDDLInitView(listTests, false);
            ddTeacher.Enabled = true;
            ddTeacher.DataSource = listOfTeachers;
            ddTeacher.DataBind();
            birUtilities.toggleDDLInitView(ddTeacher, true);
            btnGenReport.Enabled = false;
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;

            return;
        }

        protected void ddTeacher_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnGenReport.Enabled = true;
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;

            return;
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            //** User clicked the Generate Report button ***//
            //
            string selectFilter = "TEACHER_NAME = \'" + ddTeacher.SelectedItem.ToString().Replace("'", "''") + "\'";
            DataTable dtMatchingStudents = birUtilities.getFilteredTable(dsStudentDataToGrade.Tables[0], selectFilter);



            DataTable ssResultsDataTable = StudentStatsIF.generateStudentStatsRepTable(dtMatchingStudents,
                listTests.SelectedItem.ToString());

            // add in the individual student answer data
            StudentSummaryIF.addStudentAnswerData(ssResultsDataTable, listTests.SelectedItem.ToString(), ddCampus.SelectedValue.ToString());

            int r = StudentStatsIF.writeStudentStatsResultsToDb(ssResultsDataTable);


            repvwStudentSummary.Visible = true;
            lblAlignmentNote.Visible = true;

            // setup the report
            ObjectDataSource ods = new ObjectDataSource();
            ReportDataSource rds = new ReportDataSource();

            // setup parameters for query
            Parameter paramCampus = new Parameter("parmCampus", DbType.String, ddCampus.SelectedValue.ToString());
            Parameter paramTeacher = new Parameter("parmTeacher", DbType.String, ddTeacher.SelectedItem.ToString().Replace("'", "''"));
            Parameter paramTestID = new Parameter("parmTestId", DbType.String, listTests.SelectedItem.ToString());

            ods.SelectMethod = "GetDataByUseFilter";
            ods.FilterExpression = "CAMPUS = \'{0}\' AND TEST_ID = \'{1}\' AND TEACHER = \'{2}\'";
            ods.FilterParameters.Add(paramCampus);
            ods.FilterParameters.Add(paramTestID);
            ods.FilterParameters.Add(paramTeacher);

            ods.TypeName = "Benchmark_Instant_Reports_2.DataSetStudentStatsTableAdapters.TEMP_RESULTS_STUDENTSTATSTableAdapter";

            rds = new ReportDataSource("DataSetStudentStats", ods);
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
            ddTeacher.Enabled = false;
            ddTeacher.AutoPostBack = true;
            btnGenReport.Enabled = false;
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;
            lblNoScanData.Visible = false;

            // load list of campuses in Campus dropdown
            ddCampus.DataSource = birUtilities.getAuthorizedCampusList(Context.User.Identity.Name);
            ddCampus.DataTextField = "SCHOOLNAME";
            ddCampus.DataValueField = "SCHOOL_ABBR";
            ddCampus.DataBind();

            int cidx = birUtilities.getIndexOfDDItem(birUtilities.savedSelectedCampus(Request), ddCampus);
            if (cidx != -1)
                ddCampus.SelectedIndex = cidx;
            else
                ddCampus.SelectedIndex = 0;

            TestFilter.SetupTestFilterPopup(ddTFCur, ddTFTestType, ddTFTestVersion, ddCampus.SelectedValue.ToString());

            // load list of benchmarks in Benchmark dropdown
            listTests.DataSource = birIF.getTestListForSchool(ddCampus.SelectedValue.ToString());
            listTests.DataBind();

            int bidx = birUtilities.getIndexOfDDItem(birUtilities.savedSelectedTestID(Request), listTests);
            if (bidx != -1)
            {
                listTests.SelectedIndex = bidx;
                listTests_SelectedIndexChanged(new object(), new EventArgs());
            }

            return;
        }


        //**********************************************************************//
        //** initializes the dropdown menus
        //**
        //private void initSelectionBoxes()
        //{
        //    birUtilities.toggleDDLInitView(listTests, true);
        //    birUtilities.toggleDDLInitView(ddTeacher, true);
        //    ddTeacher.Enabled = false;
        //    return;
        //}

    }
}