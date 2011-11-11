using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.Common;
using Microsoft.Reporting.WebForms;

namespace Benchmark_Instant_Reports_2
{
    public partial class WebForm4 : System.Web.UI.Page
    {


        #region globals

        // constants & globals
        //
        private static DataSet studentListQueryData = new DataSet();        // holds results of the custom query
        private static DataView studentListDataByTeacher = new DataView();  // custom query filtered by teacher
        private static DataView studentListDataByTeacherPeriod = new DataView();    // custom query filtered by teacher, period
        private static DataSet dsStudentListData = new DataSet();           // the filtered list of students
        private static DataSet dsStudentDataToGrade = new DataSet();        // student data to grade

        public SiteMaster theMasterPage;


        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {

            // activate only the Campus dropdown, deactivate the
            // others until they are ready to be filled

            if (!IsPostBack)
            {
                initPage();
                ddCampus_SelectedIndexChanged1(new object(), new EventArgs());
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
                birUtilities.toggleDDLInitView(ddCampus, true);
                birUtilities.savedSelectedCampus(Response, "");
                disableSchoolPasswordEntry();
                return;
            }

            // setup stuff
            birUtilities.toggleDDLInitView(ddCampus, false);
            birUtilities.savedSelectedCampus(Response, ddCampus.SelectedItem.ToString());

            ddBenchmark.DataSource = birIF.getTestListForSchool(ddCampus.SelectedValue.ToString());
            ddBenchmark.DataBind();

            if (!CampusSecurity.isAuthorized(ddCampus.SelectedValue.ToString(), Request))
            {                                               // not yet authorized - ask for password
                ddBenchmark.Enabled = false;
                ddTeacher.Enabled = false;
                btnGenReport.Enabled = false;
                repvwStudentSummary.Visible = false;
                lblAlignmentNote.Visible = false;
                enableSchoolPasswordEntry();
                theMasterPage.updateCampusAuthLabel("none");

                return;
            }


            disableSchoolPasswordEntry();
            ddBenchmark.Enabled = true;
            birUtilities.toggleDDLInitView(ddBenchmark, true);
            ddTeacher.Enabled = false;
            btnGenReport.Enabled = false;
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;

            theMasterPage.updateCampusAuthLabel(CampusSecurity.isAuthorizedFor(Request));

            int bidx = birUtilities.getIndexOfDDItem(birUtilities.savedSelectedTestID(Request), ddBenchmark);
            if (bidx != -1)
            {
                ddBenchmark.SelectedIndex = bidx;
                ddBenchmark_SelectedIndexChanged1(new object(), new EventArgs());
            }

            return;
        }

        protected void btnEnterPassword_Click(object sender, EventArgs e)
        {
            theMasterPage = Page.Master as SiteMaster;

            //*** check password ***//

            if (CampusSecurity.checkEnteredPassword(txtbxSchoolPassword.Text.ToString(), ddCampus.SelectedValue.ToString(), Response))
            {                                                   // authentication succeeded
                disableSchoolPasswordEntry();
                theMasterPage.updateCampusAuthLabel(Response.Cookies[CampusSecurity.authcookiename].Value);
            }
            else                                                // authentication failed        
            {
                this.mpupIncorrectPassword.Show();
                enableSchoolPasswordEntry();
                theMasterPage.updateCampusAuthLabel("none");
                return;
            }


            disableSchoolPasswordEntry();
            ddBenchmark.Enabled = true;
            birUtilities.toggleDDLInitView(ddBenchmark, true);
            ddTeacher.Enabled = false;
            btnGenReport.Enabled = false;
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;

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


            // if there are no students taking this test at this campus, deal with it
            if (listOfTeachers.Count() == 0)
            {
                ddTeacher.Enabled = false;
                btnGenReport.Enabled = false;
                disableSchoolPasswordEntry();
                //ddBenchmark.Enabled = false;
                repvwStudentSummary.Visible = false;
                lblAlignmentNote.Visible = false;

                lblNoScanData.Visible = true;

                return;
            }

            lblNoScanData.Visible = false;

            // activate the Teacher dropdown and populate with the list of teachers
            birUtilities.toggleDDLInitView(ddBenchmark, false);
            ddTeacher.Enabled = true;
            ddTeacher.DataSource = listOfTeachers;
            ddTeacher.DataBind();
            birUtilities.toggleDDLInitView(ddTeacher, true);
            btnGenReport.Enabled = false;
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;
            disableSchoolPasswordEntry();

            return;
        }

        protected void ddTeacher_SelectedIndexChanged1(object sender, EventArgs e)
        {
            btnGenReport.Enabled = true;
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;
            disableSchoolPasswordEntry();

            return;
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            //** User clicked the Generate Report button ***//
            //
            string selectFilter = "TEACHER_NAME = \'" + ddTeacher.SelectedItem.ToString().Replace("'", "''") + "\'";
            DataTable dtMatchingStudents = birUtilities.getFilteredTable(dsStudentDataToGrade.Tables[0], selectFilter);



            DataTable ssResultsDataTable = StudentStatsIF.generateStudentStatsRepTable(dtMatchingStudents,
                ddBenchmark.SelectedItem.ToString());

            // add in the individual student answer data
            StudentSummaryIF.addStudentAnswerData(ssResultsDataTable, ddBenchmark.SelectedItem.ToString(), ddCampus.SelectedValue.ToString());

            int r = StudentStatsIF.writeStudentStatsResultsToDb(ssResultsDataTable);


            repvwStudentSummary.Visible = true;
            lblAlignmentNote.Visible = true;

            // setup the report
            ObjectDataSource ods = new ObjectDataSource();
            ReportDataSource rds = new ReportDataSource();

            // setup parameters for query
            Parameter paramCampus = new Parameter("parmCampus", DbType.String, ddCampus.SelectedValue.ToString());
            Parameter paramTeacher = new Parameter("parmTeacher", DbType.String, ddTeacher.SelectedItem.ToString().Replace("'", "''"));
            Parameter paramTestID = new Parameter("parmTestId", DbType.String, ddBenchmark.SelectedItem.ToString());

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

            // display authorization info
            theMasterPage.updateCampusAuthLabel(CampusSecurity.isAuthorizedFor(Request));

            // disable all dialog boxes & stuff except campus
            ddCampus.Enabled = true;
            ddCampus.AutoPostBack = true;
            ddBenchmark.Enabled = false;
            ddBenchmark.AutoPostBack = true;
            ddTeacher.Enabled = false;
            ddTeacher.AutoPostBack = true;
            btnGenReport.Enabled = false;
            disableSchoolPasswordEntry();
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;
            lblNoScanData.Visible = false;

            // load list of campuses in Campus dropdown
            ddCampus.DataSource = dbIFOracle.getDataSource(birIF.getCampusListQuery);
            ddCampus.DataTextField = "SCHOOLNAME";
            ddCampus.DataValueField = "SCHOOL_ABBR";
            ddCampus.DataBind();

            int cidx = birUtilities.getIndexOfDDItem(birUtilities.savedSelectedCampus(Request), ddCampus);
            if (cidx != -1)
                ddCampus.SelectedIndex = cidx;
            else
                birUtilities.toggleDDLInitView(ddCampus, true);


            // load list of benchmarks in Benchmark dropdown
            if (cidx != -1)
                ddBenchmark.DataSource = birIF.getTestListForSchool(ddCampus.SelectedValue.ToString());
            else
                ddBenchmark.DataSource = birIF.getTestListForSchool("ALL");
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
            birUtilities.toggleDDLInitView(ddCampus, true);
            birUtilities.toggleDDLInitView(ddBenchmark, true);
            birUtilities.toggleDDLInitView(ddTeacher, true);
            ddTeacher.Enabled = false;
            return;
        }


        //**********************************************************************//
        //** keep school password stuff turned off
        //**
        private void disableSchoolPasswordEntry()
        {
            lblEnterSchoolPassword.Visible = false;
            txtbxSchoolPassword.Visible = false;
            txtbxSchoolPassword.Text = "";
            btnEnterPassword.Visible = false;

            return;
        }


        //**********************************************************************//
        //** turn on school password entry
        //**
        private void enableSchoolPasswordEntry()
        {
            lblEnterSchoolPassword.Visible = true;
            txtbxSchoolPassword.Visible = true;
            txtbxSchoolPassword.Text = "";
            btnEnterPassword.Visible = true;

            return;
        }



    }
}