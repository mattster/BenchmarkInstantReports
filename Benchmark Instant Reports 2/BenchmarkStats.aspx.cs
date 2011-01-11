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
    public partial class WebForm3 : System.Web.UI.Page
    {
        #region PublicStuff
        private static DataSet studentListQueryData = new DataSet();        // holds results of the custom query
        private static DataView studentListDataByTeacher = new DataView();  // custom query filtered by teacher
        private static DataView studentListDataByTeacherPeriod = new DataView();    // custom query filtered by teacher, period
        private static DataSet dsStudentListData = new DataSet();           // the filtered list of students
        private static bool reportDataParmsHaveChanged = true;
        private static string repTypeResultsByTeacher = "Results By Teacher";
        private static string repTypeResultsByPeriod = "Results By Period";
        private static string repTypeResultsByAnsTeacher = "Results by Answer Choice (Teacher)";
        private static string repTypeResultsByAnsCampus = "Results by Answer Choice (Campus)";
        private static string groupByQ = "Question Num.";
        private static string groupByObj = "Objective";
        private static string groupByTEKS = "TEKS";
        private static string repsNone = "NONE";
        private static string[] reportTypesList = 
            { repTypeResultsByTeacher, repTypeResultsByPeriod, repTypeResultsByAnsTeacher, repTypeResultsByAnsCampus };
        private static string[] groupByList = { groupByQ, groupByObj, groupByTEKS };

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
            //*** User selected a campus ***//
            
            // return if it is the separator
            if (birUtilities.isDDSeparatorValue(ddCampus.SelectedValue.ToString()))
            {
                birUtilities.toggleDDLInitView(ddCampus, true);
                disableSchoolPasswordEntry();
                return;
            }
            
            // setup stuff
            birUtilities.toggleDDLInitView(ddCampus, false);

            if (!birUtilities.sessionAuthList.isAuthorized(ddCampus.SelectedValue.ToString()))
            {                                               // not yet authorized - ask for password
                ddBenchmark.Enabled = false;
                ddRepType.Enabled = false;
                ddTeacher.Visible = false;
                lblSelectTeacher.Visible = false;
                btnGenReport.Enabled = false;
                makeRepsVisible(repsNone, repsNone);
                enableSchoolPasswordEntry();
                return;
            }


            lblIncorrectPassword.Visible = false;
            disableSchoolPasswordEntry();
            ddBenchmark.Enabled = true;
            birUtilities.toggleDDLInitView(ddBenchmark, true);
            ddRepType.Enabled = false;
            ddTeacher.Visible = false;
            lblSelectTeacher.Visible = false;
            btnGenReport.Enabled = false;
            makeRepsVisible(repsNone, repsNone);
            reportDataParmsHaveChanged = true;

            return;
        }


        protected void btnEnterPassword_Click(object sender, EventArgs e)
        {
            // *** check password ***//

            if (birUtilities.sessionAuthList.checkEnteredPassword(txtbxSchoolPassword.Text.ToString(), 
                ddCampus.SelectedValue.ToString()))
            {                                                   // authentication succeeded
                lblIncorrectPassword.Visible = false;
                disableSchoolPasswordEntry();
            }
            else                                                // authentication failed        
            {
                lblIncorrectPassword.Visible = true;
                enableSchoolPasswordEntry();
                return;
            }

            disableSchoolPasswordEntry();
            ddBenchmark.Enabled = true;
            birUtilities.toggleDDLInitView(ddBenchmark, true);
            ddRepType.Enabled = false;
            ddTeacher.Visible = false;
            lblSelectTeacher.Visible = false;
            btnGenReport.Enabled = false;
            makeRepsVisible(repsNone, repsNone);

            return;
        }


        protected void ddBenchmark_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //*** User selected a test ***//

            // load the main query - list of students
            studentListQueryData = birIF.executeStudentListQuery(ddBenchmark.SelectedItem.ToString(),
                ddCampus.SelectedValue.ToString());

            // get a list of teachers applicable for this query
            string[] listOfTeachers = birUtilities.getUniqueTableColumnStringValues(studentListQueryData.Tables[0],
                    birIF.teacherNameFieldName);
            ddTeacher.DataSource = listOfTeachers;
            ddTeacher.DataBind();
            birUtilities.toggleDDLInitView(ddTeacher, true);


            // if there are no students taking this test at this campus, deal with it
            if (listOfTeachers.Count() == 0)
            {
                btnGenReport.Enabled = false;
                disableSchoolPasswordEntry();
                ddBenchmark.Enabled = false;
                ddRepType.Enabled = false;
                ddTeacher.Visible = false;
                lblSelectTeacher.Visible = false;
                makeRepsVisible(repsNone, repsNone);
                reportDataParmsHaveChanged = true;
                return;
            }

            ddRepType.Enabled = true;
            ddGroupBy.Enabled = true;
            btnGenReport.Enabled = true;
            makeRepsVisible(repsNone, repsNone);
            disableSchoolPasswordEntry();
            reportDataParmsHaveChanged = true;

            return;
        }


        protected void ddRepType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //** User changed the report type **//

            if (ddRepType.SelectedItem.ToString() == repTypeResultsByTeacher)
            {
                lblSelectTeacher.Visible = false;
                ddTeacher.Visible = false;
                lblGroupBy.Enabled = true;
                btnGenReport.Enabled = true;
                makeRepsVisible(repsNone, repsNone);
                disableSchoolPasswordEntry();
                if (!reportDataParmsHaveChanged)
                    setupReportByTeacher(ddGroupBy.SelectedItem.ToString());
            }
            else if (ddRepType.SelectedItem.ToString() == repTypeResultsByPeriod)
            {
                lblSelectTeacher.Visible = true;
                ddTeacher.Visible = true;
                btnGenReport.Enabled = false;
                makeRepsVisible(repsNone, repsNone);
                disableSchoolPasswordEntry();
                if (!reportDataParmsHaveChanged && ddTeacher.SelectedIndex >= 0)
                    setupReportByPeriod(ddGroupBy.SelectedItem.ToString());
            }
            else if (ddRepType.SelectedItem.ToString() == repTypeResultsByAnsTeacher)
            {
                lblSelectTeacher.Visible = true;
                ddTeacher.Visible = true;
                btnGenReport.Enabled = false;
                makeRepsVisible(repsNone, repsNone);
                disableSchoolPasswordEntry();
                if (!reportDataParmsHaveChanged && ddTeacher.SelectedIndex >= 0)
                    setupReportByAnsTeacher(ddGroupBy.SelectedItem.ToString());
            }
            else if (ddRepType.SelectedItem.ToString() == repTypeResultsByAnsCampus)
            {
                lblSelectTeacher.Visible = false;
                ddTeacher.Visible = false;
                btnGenReport.Enabled = true;
                makeRepsVisible(repsNone, repsNone);
                disableSchoolPasswordEntry();
                if (!reportDataParmsHaveChanged)
                    setupReportByAnsCampus(ddGroupBy.SelectedItem.ToString());
            }

            return;
        }


        protected void ddTeacher_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //** User selected a teacher

            birUtilities.toggleDDLInitView(ddTeacher, false);
            btnGenReport.Enabled = true;
            makeRepsVisible(repsNone, repsNone);
            disableSchoolPasswordEntry();

            if (!reportDataParmsHaveChanged)
                if (ddRepType.SelectedItem.ToString() == repTypeResultsByPeriod)
                    setupReportByPeriod(ddGroupBy.SelectedItem.ToString());
                else if (ddRepType.SelectedItem.ToString() == repTypeResultsByAnsTeacher)
                    setupReportByAnsTeacher(ddGroupBy.SelectedItem.ToString());

            return;
        }


        protected void ddGroupBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //** User selected a Group-By method
            //** don't need to check anything here because an active
            //** report will automatically change with AutoPostBack set
            //** to true

            if (!reportDataParmsHaveChanged)
                if (ddRepType.SelectedItem.ToString() == repTypeResultsByTeacher)
                    setupReportByTeacher(ddGroupBy.SelectedItem.ToString());
                else if (ddRepType.SelectedItem.ToString() == repTypeResultsByPeriod)
                    setupReportByPeriod(ddGroupBy.SelectedItem.ToString());
                else if (ddRepType.SelectedItem.ToString() == repTypeResultsByAnsTeacher)
                    setupReportByAnsTeacher(ddGroupBy.SelectedItem.ToString());
                else if (ddRepType.SelectedItem.ToString() == repTypeResultsByAnsCampus)
                    setupReportByAnsCampus(ddGroupBy.SelectedItem.ToString());
          
            return;
        }


        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            //** User clicked the Generate Report button ***//
            int r = new int();

            // generate results for the given criteria on the page if we need to
            if (reportDataParmsHaveChanged)
            {
                // do a new query by school
                DataSet ds1 = birIF.getStudentScanListData(ddBenchmark.SelectedItem.ToString(),
                    ddCampus.SelectedValue.ToString());

                DataTable bsResultsDataTable = BenchmarkStatsIF.generateBenchmarkStatsRepTable(ds1.Tables[0],
                    ddBenchmark.SelectedItem.ToString(), ddCampus.SelectedValue.ToString());

                r = BenchmarkStatsIF.writeBenchmarkStatsResultsToDb(bsResultsDataTable);
                reportDataParmsHaveChanged = false;
            }

            if (ddRepType.SelectedItem.ToString() == repTypeResultsByTeacher)
            {
                // **** report Results by Teacher ****
                setupReportByTeacher(ddGroupBy.SelectedItem.ToString());
                return;
            }
            else if (ddRepType.SelectedItem.ToString() == repTypeResultsByPeriod)
            {
                // **** report Results by Period ****
                setupReportByPeriod(ddGroupBy.SelectedItem.ToString());

                return;
            }
            else if (ddRepType.SelectedItem.ToString() == repTypeResultsByAnsTeacher)
            {
                // **** report Results by Answer (Teacher) ****
                setupReportByAnsTeacher(ddGroupBy.SelectedItem.ToString());

                return;
            }
            else if (ddRepType.SelectedItem.ToString() == repTypeResultsByAnsCampus)
            {
                // **** report Results by Answer (Campus) ****
                setupReportByAnsCampus(ddGroupBy.SelectedItem.ToString());

                return;
            }






        }







        //************************************************************************************************
        //** some stuff
        //************************************************************************************************


        //**********************************************************************//
        //** initialize the page
        //**
        private void initPage()
        {
            // disable all dialog boxes & stuff except campus
            ddCampus.Enabled = true;
            ddCampus.AutoPostBack = true;
            ddBenchmark.Enabled = false;
            ddBenchmark.AutoPostBack = true;
            ddTeacher.Visible = false;
            ddTeacher.AutoPostBack = true;
            lblSelectTeacher.Visible = false;
            ddRepType.Enabled = false;
            ddRepType.AutoPostBack = true;
            ddGroupBy.Enabled = false;
            ddGroupBy.AutoPostBack = true;
            btnGenReport.Enabled = false;
            disableSchoolPasswordEntry();
            lblIncorrectPassword.Visible = false;
            makeRepsVisible(repsNone, repsNone);

            // load list of campuses in Campus dropdown
            ddCampus.DataSource = dbIFOracle.getDataSource(birIF.getCampusListQuery);
            ddCampus.DataTextField = "SCHOOLNAME";
            ddCampus.DataValueField = "SCHOOL_ABBR";
            ddCampus.DataBind();
            birUtilities.toggleDDLInitView(ddCampus, true);


            // load list of benchmarks in Benchmark dropdown
            ddBenchmark.DataSource = dbIFOracle.getDataSource(birIF.getBenchmarkListQuery);
            ddBenchmark.DataTextField = "TEST_ID";
            ddBenchmark.DataValueField = "TEST_ID";
            ddBenchmark.DataBind();

            // load list of report types in Reports dropdown
            ddRepType.DataSource = reportTypesList;
            ddRepType.DataBind();
            ddRepType.SelectedIndex = 0;

            // load Group By choices for reports
            ddGroupBy.DataSource = groupByList;
            ddGroupBy.DataBind();
            ddGroupBy.SelectedIndex = 0;

            return;
        }

        
        //**********************************************************************//
        //** initializes the dropdown menus
        //**
        private void initSelectionBoxes()
        {
            birUtilities.toggleDDLInitView(ddCampus, true);
            birUtilities.toggleDDLInitView(ddBenchmark, true);
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
        

        //**********************************************************************//
        //** setup the Report by Teacher report
        //**
        private void setupReportByTeacher(string groupBySelection)
        {
            makeRepsVisible(repTypeResultsByTeacher, groupBySelection);

            // setup the report
            ObjectDataSource ods = new ObjectDataSource();
            ReportDataSource rds = new ReportDataSource();

            // setup parameters for query
            Parameter paramCampus = new Parameter("parmCampus", DbType.String, ddCampus.SelectedValue.ToString());
            Parameter paramTestID = new Parameter("parmTestId", DbType.String, ddBenchmark.SelectedItem.ToString());

            ods.SelectMethod = "GetDataByUseFilter";
            ods.FilterExpression = "CAMPUS = \'{0}\' AND  TEST_ID = \'{1}\'";
            ods.FilterParameters.Add(paramCampus);
            ods.FilterParameters.Add(paramTestID);

            ods.TypeName = "Benchmark_Instant_Reports_2.DataSetBenchmarkStatsTableAdapters.TEMP_RESULTS_BENCHMARKSTATSTableAdapter";


            rds = new ReportDataSource("DataSetBenchmarkStats", ods);

            if (groupBySelection == groupByQ)
            {
                repvwBenchmarkStats1a.LocalReport.DataSources.Clear();
                repvwBenchmarkStats1a.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats1a.ShowPrintButton = true;
                repvwBenchmarkStats1a.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByObj)
            {
                repvwBenchmarkStats1b.LocalReport.DataSources.Clear();
                repvwBenchmarkStats1b.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats1b.ShowPrintButton = true;
                repvwBenchmarkStats1b.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByTEKS)
            {
                repvwBenchmarkStats1c.LocalReport.DataSources.Clear();
                repvwBenchmarkStats1c.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats1c.ShowPrintButton = true;
                repvwBenchmarkStats1c.LocalReport.Refresh();
            }

            return;
        }


        //**********************************************************************//
        //** setup the Report by Period report
        //**
        private void setupReportByPeriod(string groupBySelection)
        {
            makeRepsVisible(repTypeResultsByPeriod, groupBySelection);

            // setup the report
            ObjectDataSource ods = new ObjectDataSource();
            ReportDataSource rds = new ReportDataSource();

            // setup parameters for query
            Parameter paramCampus = new Parameter("parmCampus", DbType.String, ddCampus.SelectedValue.ToString());
            Parameter paramTestID = new Parameter("parmTestId", DbType.String, ddBenchmark.SelectedItem.ToString());
            Parameter paramTeacher = new Parameter("parmTeacher", DbType.String, ddTeacher.SelectedItem.ToString());

            ods.SelectMethod = "GetDataByUseFilter";
            ods.FilterExpression = "CAMPUS = \'{0}\' AND  TEST_ID = \'{1}\' AND TEACHER = \'{2}\'";
            ods.FilterParameters.Add(paramCampus);
            ods.FilterParameters.Add(paramTestID);
            ods.FilterParameters.Add(paramTeacher);

            ods.TypeName = "Benchmark_Instant_Reports_2.DataSetBenchmarkStatsTableAdapters.TEMP_RESULTS_BENCHMARKSTATSTableAdapter";

            rds = new ReportDataSource("DataSetBenchmarkStats", ods);

            if (groupBySelection == groupByQ)
            {
                repvwBenchmarkStats2a.LocalReport.DataSources.Clear();
                repvwBenchmarkStats2a.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats2a.ShowPrintButton = true;
                repvwBenchmarkStats2a.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByObj)
            {
                repvwBenchmarkStats2b.LocalReport.DataSources.Clear();
                repvwBenchmarkStats2b.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats2b.ShowPrintButton = true;
                repvwBenchmarkStats2b.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByTEKS)
            {
                repvwBenchmarkStats2c.LocalReport.DataSources.Clear();
                repvwBenchmarkStats2c.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats2c.ShowPrintButton = true;
                repvwBenchmarkStats2c.LocalReport.Refresh();
            }

            return;
        }


        //**********************************************************************//
        //** setup the Report by Answer Choice by Teacher report
        //**
        private void setupReportByAnsTeacher(string groupBySelection)
        {
            makeRepsVisible(repTypeResultsByAnsTeacher, groupBySelection);

            // setup the report
            ObjectDataSource ods = new ObjectDataSource();
            ReportDataSource rds = new ReportDataSource();

            // setup parameters for query
            Parameter paramCampus = new Parameter("parmCampus", DbType.String, ddCampus.SelectedValue.ToString());
            Parameter paramTestID = new Parameter("parmTestId", DbType.String, ddBenchmark.SelectedItem.ToString());
            Parameter paramTeacher = new Parameter("parmTeacher", DbType.String, ddTeacher.SelectedItem.ToString());

            ods.SelectMethod = "GetDataByUseFilter";
            ods.FilterExpression = "CAMPUS = \'{0}\' AND  TEST_ID = \'{1}\' AND TEACHER = \'{2}\'";
            ods.FilterParameters.Add(paramCampus);
            ods.FilterParameters.Add(paramTestID);
            ods.FilterParameters.Add(paramTeacher);

            ods.TypeName = "Benchmark_Instant_Reports_2.DataSetBenchmarkStatsTableAdapters.TEMP_RESULTS_BENCHMARKSTATSTableAdapter";

            rds = new ReportDataSource("DataSetBenchmarkStats", ods);

            if (groupBySelection == groupByQ)
            {
                repvwBenchmarkStats3a.LocalReport.DataSources.Clear();
                repvwBenchmarkStats3a.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats3a.ShowPrintButton = true;
                repvwBenchmarkStats3a.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByObj)
            {
                repvwBenchmarkStats3b.LocalReport.DataSources.Clear();
                repvwBenchmarkStats3b.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats3b.ShowPrintButton = true;
                repvwBenchmarkStats3b.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByTEKS)
            {
                repvwBenchmarkStats3c.LocalReport.DataSources.Clear();
                repvwBenchmarkStats3c.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats3c.ShowPrintButton = true;
                repvwBenchmarkStats3c.LocalReport.Refresh();
            }

            return;
        }


        //**********************************************************************//
        //** setup the Report by Answer Choice by Campus report
        //**
        private void setupReportByAnsCampus(string groupBySelection)
        {
            makeRepsVisible(repTypeResultsByAnsCampus, groupBySelection);

            // setup the report
            ObjectDataSource ods = new ObjectDataSource();
            ReportDataSource rds = new ReportDataSource();

            // setup parameters for query
            Parameter paramCampus = new Parameter("parmCampus", DbType.String, ddCampus.SelectedValue.ToString());
            Parameter paramTestID = new Parameter("parmTestId", DbType.String, ddBenchmark.SelectedItem.ToString());

            ods.SelectMethod = "GetDataByUseFilter";
            ods.FilterExpression = "CAMPUS = \'{0}\' AND  TEST_ID = \'{1}\'";
            ods.FilterParameters.Add(paramCampus);
            ods.FilterParameters.Add(paramTestID);

            ods.TypeName = "Benchmark_Instant_Reports_2.DataSetBenchmarkStatsTableAdapters.TEMP_RESULTS_BENCHMARKSTATSTableAdapter";

            rds = new ReportDataSource("DataSetBenchmarkStats", ods);

            if (groupBySelection == groupByQ)
            {
                repvwBenchmarkStats4a.LocalReport.DataSources.Clear();
                repvwBenchmarkStats4a.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats4a.ShowPrintButton = true;
                repvwBenchmarkStats4a.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByObj)
            {
                repvwBenchmarkStats4b.LocalReport.DataSources.Clear();
                repvwBenchmarkStats4b.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats4b.ShowPrintButton = true;
                repvwBenchmarkStats4b.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByTEKS)
            {
                repvwBenchmarkStats4c.LocalReport.DataSources.Clear();
                repvwBenchmarkStats4c.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats4c.ShowPrintButton = true;
                repvwBenchmarkStats4c.LocalReport.Refresh();
            }
            
            return;
        }


        //**********************************************************************//
        //** make specified reports visible
        //**
        private void makeRepsVisible(string theReport, string groupBy)
        {
            repvwBenchmarkStats1a.Visible = false;
            repvwBenchmarkStats1b.Visible = false;
            repvwBenchmarkStats1c.Visible = false;
            repvwBenchmarkStats2a.Visible = false;
            repvwBenchmarkStats2b.Visible = false;
            repvwBenchmarkStats2c.Visible = false;
            repvwBenchmarkStats3a.Visible = false;
            repvwBenchmarkStats3b.Visible = false;
            repvwBenchmarkStats3c.Visible = false; 
            repvwBenchmarkStats4a.Visible = false;
            repvwBenchmarkStats4b.Visible = false;
            repvwBenchmarkStats4c.Visible = false;

            if (theReport == repsNone || groupBy == repsNone)
                return;

            else if (theReport == repTypeResultsByTeacher)
            {
                if (groupBy == groupByQ)
                    repvwBenchmarkStats1a.Visible = true;
                else if (groupBy == groupByObj)
                    repvwBenchmarkStats1b.Visible = true;
                else if (groupBy == groupByTEKS)
                    repvwBenchmarkStats1c.Visible = true;
            }

            else if (theReport == repTypeResultsByPeriod)
            {
                if (groupBy == groupByQ)
                    repvwBenchmarkStats2a.Visible = true;
                else if (groupBy == groupByObj)
                    repvwBenchmarkStats2b.Visible = true;
                else if (groupBy == groupByTEKS)
                    repvwBenchmarkStats2c.Visible = true;
            }

            else if (theReport == repTypeResultsByAnsTeacher)
            {
                if (groupBy == groupByQ)
                    repvwBenchmarkStats3a.Visible = true;
                else if (groupBy == groupByObj)
                    repvwBenchmarkStats3b.Visible = true;
                else if (groupBy == groupByTEKS)
                    repvwBenchmarkStats3c.Visible = true;
            }

            else if (theReport == repTypeResultsByAnsCampus)
            {
                if (groupBy == groupByQ)
                    repvwBenchmarkStats4a.Visible = true;
                else if (groupBy == groupByObj)
                    repvwBenchmarkStats4b.Visible = true;
                else if (groupBy == groupByTEKS)
                    repvwBenchmarkStats4c.Visible = true;
            }

            return;
        }
    }
}