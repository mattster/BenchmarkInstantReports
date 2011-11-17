using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Windows.Forms;
using Microsoft.Reporting.Common;
using Microsoft.Reporting.WebForms;



namespace Benchmark_Instant_Reports_2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        #region globals

        // constants & globals
        //
        private static DataSet studentListQueryData = new DataSet();        // holds results of the custom query
        private static DataSet studentListQueryData2 = new DataSet();       // try this
        private static DataView studentListDataByTeacher = new DataView();  // custom query filtered by teacher
        private static DataView studentListDataByTeacherPeriod = new DataView();    // custom query filtered by teacher, period
        private static DataSet dsStudentListData = new DataSet();           // the filtered list of students
        private static DataSet dsStudentDataToGrade = new DataSet();               // student data to grade

        public SiteMaster theMasterPage;

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

        protected void ddCampus_SelectedIndexChanged1(object sender, EventArgs e)
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

            ddBenchmark.DataSource = birIF.getTestListForSchool(ddCampus.SelectedValue.ToString());
            ddBenchmark.DataBind();

            ddBenchmark.Enabled = true;
            ddTeacher.Enabled = false;
            btnGenReport.Enabled = false;
            repvwStudentStats2a.Visible = false;

            int bidx = birUtilities.getIndexOfDDItem(birUtilities.savedSelectedTestID(Request), ddBenchmark);
            if (bidx != -1)
            {
                ddBenchmark.SelectedIndex = bidx;
                ddBenchmark_SelectedIndexChanged1(new object(), new EventArgs());
            }

            return;
        }


        protected void ddBenchmark_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //*** User selected a test ***//
            lblNoScanData.Visible = false;
            birUtilities.savedSelectedTestID(Response, ddBenchmark.SelectedItem.ToString());

            dsStudentDataToGrade = birIF.getStudentDataToGrade(ddBenchmark.SelectedItem.ToString(),
                ddCampus.SelectedValue.ToString());

            // get a list of teachers applicable for this query
            string[] listOfTeachers = birUtilities.getUniqueTableColumnStringValues(dsStudentDataToGrade.Tables[0],
                birIF.teacherNameFieldName);
            Array.Sort(listOfTeachers);


            // if there are no students taking this test at this campus (based on the number of teachers applicable), deal with it
            if (listOfTeachers.Count() == 0)
            {
                ddTeacher.Enabled = false;
                //btnGenReport.Enabled = false;
                ddBenchmark.Enabled = true;
                repvwStudentStats2a.Visible = false;
                lblNoScanData.Visible = true;

                return;
            }

            lblNoScanData.Visible = false;


            // activate the Teacher dropdown and populate with the list of teachers
            birUtilities.toggleDDLInitView(ddBenchmark, false);
            ddTeacher.Enabled = true;
            ddTeacher.DataSource = listOfTeachers;
            ddTeacher.DataBind();

            // add option for All Teachers
            //ddTeacher.Items.Insert(0, new ListItem(birIF.allTeachers, birIF.allTeachers));

            birUtilities.toggleDDLInitView(ddTeacher, true);
            btnGenReport.Enabled = false;
            repvwStudentStats2a.Visible = false;

            return;
        }


        protected void ddTeacher_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //*** User selected a teacher ***//
            btnGenReport.Enabled = true;
            repvwStudentStats2a.Visible = false;

            return;
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            DataSet ds1 = new DataSet();
            DataTable ssRresultsDataTable = new DataTable();

            //** User clicked the Generate Report button ***//
            //
            DataTable dtMatchingStudents = new DataTable();
            if (ddTeacher.SelectedItem.ToString() == birIF.allTeachers)
            {
                dtMatchingStudents = dsStudentDataToGrade.Tables[0].Copy();
            }
            else
            {
                string selectFilter = "TEACHER_NAME = \'" + ddTeacher.SelectedItem.ToString().Replace("'", "''") + "\'";
                dtMatchingStudents = birUtilities.getFilteredTable(dsStudentDataToGrade.Tables[0], selectFilter);
            }
            ssRresultsDataTable = StudentStatsIF.generateStudentStatsRepTable(dtMatchingStudents,
                ddBenchmark.SelectedItem.ToString());
            int r = StudentStatsIF.writeStudentStatsResultsToDb(ssRresultsDataTable);

            repvwStudentStats2a.Visible = true;

            // setup the report
            ObjectDataSource ods = new ObjectDataSource();
            ReportDataSource rds = new ReportDataSource();

            // setup parameters for query
            Parameter paramTeacher = new Parameter();
            Parameter paramTestID = new Parameter("parmTestId", DbType.String, ddBenchmark.SelectedItem.ToString());
            Parameter paramCampus = new Parameter("campus", DbType.String, ddCampus.SelectedValue.ToString());
            //if (ddTeacher.SelectedItem.ToString() != birIF.allTeachers)
            //{
            paramTeacher = new Parameter("parmTeacher", DbType.String, ddTeacher.SelectedItem.ToString().Replace("'", "''"));
            ods.FilterExpression = "TEST_ID = \'{0}\' AND CAMPUS = \'{1}\' AND TEACHER = \'{2}\'";
            ods.FilterParameters.Add(paramTestID);
            ods.FilterParameters.Add(paramCampus);
            ods.FilterParameters.Add(paramTeacher);
            //}
            //else
            //{
            //    paramTestID = new Parameter("parmTestId", DbType.String, ddBenchmark.SelectedItem.ToString());
            //    ods.FilterExpression = "TEST_ID = \'{0}\' AND CAMPUS = \'{1}\'";
            //    ods.FilterParameters.Add(paramTestID);
            //    ods.FilterParameters.Add(paramCampus);
            //}

            ods.SelectMethod = "GetDataByUseFilter";
            ods.TypeName = "Benchmark_Instant_Reports_2.DataSetStudentStatsTableAdapters.TEMP_RESULTS_STUDENTSTATSTableAdapter";

            rds = new ReportDataSource("DataSetStudentStatsRep2", ods);
            this.repvwStudentStats2a.LocalReport.DataSources.Clear();
            this.repvwStudentStats2a.LocalReport.DataSources.Add(rds);
            this.repvwStudentStats2a.ShowPrintButton = true;
            this.repvwStudentStats2a.LocalReport.Refresh();


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
            ddBenchmark.Enabled = true;
            ddBenchmark.AutoPostBack = true;
            ddTeacher.Enabled = false;
            ddTeacher.AutoPostBack = true;
            btnGenReport.Enabled = false;
            repvwStudentStats2a.Visible = false;
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


            // load list of benchmarks in Benchmark dropdown
            ddBenchmark.DataSource = birIF.getTestListForSchool(ddCampus.SelectedValue.ToString());
            ddBenchmark.DataBind();

            int bidx = birUtilities.getIndexOfDDItem(birUtilities.savedSelectedTestID(Request), ddBenchmark);
            if (bidx != -1)
            {
                ddBenchmark.SelectedIndex = bidx;
                ddBenchmark_SelectedIndexChanged1(new object(), new EventArgs());
            }

            return;
        }

        //**********************************************************************//
        //** initializes the dropdown menus
        //**
        private void initSelectionBoxes()
        {
            birUtilities.toggleDDLInitView(ddBenchmark, true);
            birUtilities.toggleDDLInitView(ddTeacher, true);
            ddTeacher.Enabled = false;
            return;
        }

    }
}