using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Helpers;
using Benchmark_Instant_Reports_2.Infrastructure;
using Microsoft.Reporting.WebForms;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Interfaces;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.Helpers.Reports;
using Benchmark_Instant_Reports_2.Account;

namespace Benchmark_Instant_Reports_2
{
    public partial class BenchmarkStats : ReportPage<DropDownList>
    {
        #region globals

        public SiteMaster theMasterPage;
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
        private static string repsNone = "NONE";
        private static string[] reportTypesList = { repTypePctCorrectAllTeachers, repTypePctCorrectOneTeacher, repTypeAnsChoiceAllTeachers, repTypeAnsChoiceOneTeacher };
        private static string[] reportTypesListTeacherOnly = { repTypePctCorrectOneTeacher, repTypeAnsChoiceOneTeacher, repTypeAnsChoiceAllTeachers };
        private static string[] groupByList = { groupByQ, groupByObj, groupByTEKS };

        private static IAReportData resultsData = new IAReportData();

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

            // setup stuff if authorized as an administrator

            return;


        }


        protected void ddCampus_SelectedIndexChanged(object sender, EventArgs e)
        {
            theMasterPage = Page.Master as SiteMaster;

            //*** User selected a campus ***//

            // return if it is the separator
            if (UIHelper.isDDSeparatorValue(ddCampus.SelectedValue.ToString()))
            {
                RememberHelper.savedSelectedCampus(Response, "");
                return;
            }

            // setup stuff
            RememberHelper.savedSelectedCampus(Response, ddCampus.SelectedItem.ToString());

            listTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            listTests.DataBind();

            setupTestFilters();
            listTests.Enabled = true;
            ddTeacher.Visible = false;
            lblSelectTeacher.Visible = false;
            makeRepsVisible(repsNone, repsNone);
            reportDataParmsHaveChanged = true;

                ddRepType.DataSource = reportTypesList;
                ddRepType.DataBind();
                ddRepType.SelectedIndex = 0;

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
            //*** User selected a test ***//
            lblNoScanData.Visible = false;
            RememberHelper.savedSelectedTestID(Response, listTests.SelectedItem.ToString());

            DataSet ds1 = birIF.getTeachersForTestCampus(listTests.SelectedItem.ToString(), ddCampus.SelectedValue.ToString());
            string[] listOfTeachers = birUtilities.getUniqueTableColumnStringValues(ds1.Tables[0], Constants.TeacherNameFieldName);
            ddTeacher.DataSource = listOfTeachers;
            ddTeacher.DataBind();
            UIHelper.toggleDDLInitView(ddTeacher, true);


            // if there are no students taking this test at this campus, deal with it
            if (listOfTeachers.Count() == 0)
            {
                ddTeacher.Visible = false;
                lblSelectTeacher.Visible = false;
                makeRepsVisible(repsNone, repsNone);
                reportDataParmsHaveChanged = true;
                lblNoScanData.Visible = true;

                return;
            }

            lblNoScanData.Visible = false;
            makeRepsVisible(repsNone, repsNone);
            reportDataParmsHaveChanged = true;

            return;
        }


        protected void ddRepType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //** User changed the report type **//

            if (ddRepType.SelectedItem.ToString() == repTypePctCorrectAllTeachers)
            {
                lblSelectTeacher.Visible = false;
                ddTeacher.Visible = false;
                btnGenReport.Enabled = true;
                makeRepsVisible(repsNone, repsNone);
                if (!reportDataParmsHaveChanged)
                    setupReportPctCorrectAllTeachers(ddGroupBy.SelectedItem.ToString());
            }
            else if (ddRepType.SelectedItem.ToString() == repTypePctCorrectOneTeacher)
            {
                lblSelectTeacher.Visible = true;
                ddTeacher.Visible = true;
                btnGenReport.Enabled = true; // was false
                makeRepsVisible(repsNone, repsNone);
                if (!reportDataParmsHaveChanged && ddTeacher.SelectedIndex >= 0)
                    setupReportPctCorrectOneTeacher(ddGroupBy.SelectedItem.ToString());
            }
            else if (ddRepType.SelectedItem.ToString() == repTypeAnsChoiceOneTeacher)
            {
                lblSelectTeacher.Visible = true;
                ddTeacher.Visible = true;
                btnGenReport.Enabled = true; // was false
                makeRepsVisible(repsNone, repsNone);
                if (!reportDataParmsHaveChanged && ddTeacher.SelectedIndex >= 0)
                    setupReportAnsChoiceOneTeacher(ddGroupBy.SelectedItem.ToString());
            }
            else if (ddRepType.SelectedItem.ToString() == repTypeAnsChoiceAllTeachers)
            {
                lblSelectTeacher.Visible = false;
                ddTeacher.Visible = false;
                btnGenReport.Enabled = true;
                makeRepsVisible(repsNone, repsNone);
                if (!reportDataParmsHaveChanged)
                    setupReportAnsChoiceAllTeachers(ddGroupBy.SelectedItem.ToString());
            }

            return;
        }


        protected void ddTeacher_SelectedIndexChanged(object sender, EventArgs e)
        {
            //** User selected a teacher

            UIHelper.toggleDDLInitView(ddTeacher, false);
            btnGenReport.Enabled = true;
            makeRepsVisible(repsNone, repsNone);

            if (!reportDataParmsHaveChanged)
                if (ddRepType.SelectedItem.ToString() == repTypePctCorrectOneTeacher)
                    setupReportPctCorrectOneTeacher(ddGroupBy.SelectedItem.ToString());
                else if (ddRepType.SelectedItem.ToString() == repTypeAnsChoiceOneTeacher)
                    setupReportAnsChoiceOneTeacher(ddGroupBy.SelectedItem.ToString());

            return;
        }


        protected void ddGroupBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //** User selected a Group-By method
            //** don't need to check anything here because an active
            //** report will automatically change with AutoPostBack set
            //** to true

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
            //** User clicked the Generate Report button ***//
            List<StudentListItem> studentData = new List<StudentListItem>();
            
            // generate results for the given criteria on the page if we need to
            if (reportDataParmsHaveChanged)
            {
                // do a new query by school
                if (ddTeacher.SelectedIndex != 0)
                {
                    studentData = StudentData.GetStudentDataToGrade(listTests.SelectedItem.ToString(),
                        ddCampus.SelectedValue.ToString(), ddTeacher.SelectedValue.ToString());
                }
                else
                {
                    studentData = StudentData.GetStudentDataToGrade(listTests.SelectedItem.ToString(),
                        ddCampus.SelectedValue.ToString());
                }
                resultsData = IARepHelper.GenerateBenchmarkStatsRepTable(studentData,
                    listTests.SelectedItem.ToString(), ddCampus.SelectedValue.ToString());

                reportDataParmsHaveChanged = false;
            }

            if (ddRepType.SelectedItem.ToString() == repTypePctCorrectAllTeachers)
            {
                setupReportPctCorrectAllTeachers(ddGroupBy.SelectedItem.ToString());
                return;
            }
            else if (ddRepType.SelectedItem.ToString() == repTypePctCorrectOneTeacher)
            {
                setupReportPctCorrectOneTeacher(ddGroupBy.SelectedItem.ToString());

                return;
            }
            else if (ddRepType.SelectedItem.ToString() == repTypeAnsChoiceOneTeacher)
            {
                setupReportAnsChoiceOneTeacher(ddGroupBy.SelectedItem.ToString());

                return;
            }
            else if (ddRepType.SelectedItem.ToString() == repTypeAnsChoiceAllTeachers)
            {
                setupReportAnsChoiceAllTeachers(ddGroupBy.SelectedItem.ToString());

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
            theMasterPage = Page.Master as SiteMaster;

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
            makeRepsVisible(repsNone, repsNone);
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

            // load list of report types in Reports dropdown
            //ddRepType.DataSource = reportTypesListTeacherOnly;
            ddRepType.DataSource = reportTypesList;
            ddRepType.DataBind();
            ddRepType.SelectedIndex = 0;

            // load Group By choices for reports
            ddGroupBy.DataSource = groupByList;
            ddGroupBy.DataBind();
            ddGroupBy.SelectedIndex = 0;

            int bidx = UIHelper.getIndexOfDDItem(RememberHelper.savedSelectedTestID(Request), listTests);
            if (bidx != -1)
            {
                listTests.SelectedIndex = bidx;
            }
            listTests_SelectedIndexChanged(new object(), new EventArgs());

            return;
        }


        //**********************************************************************//
        //** setup the Report by Teacher report
        //**
        private void setupReportPctCorrectAllTeachers(string groupBySelection)
        {
            makeRepsVisible(repTypePctCorrectAllTeachers, groupBySelection);
            
            if (groupBySelection == groupByQ)
            {
                ReportDataSource rds = new ReportDataSource(repvwBenchmarkStats1a.LocalReport.GetDataSourceNames()[0], resultsData.GetItems());
                repvwBenchmarkStats1a.LocalReport.DataSources.Clear();
                repvwBenchmarkStats1a.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats1a.ShowPrintButton = true;
                repvwBenchmarkStats1a.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByObj)
            {
                ReportDataSource rds = new ReportDataSource(repvwBenchmarkStats1b.LocalReport.GetDataSourceNames()[0], resultsData.GetItems());
                repvwBenchmarkStats1b.LocalReport.DataSources.Clear();
                repvwBenchmarkStats1b.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats1b.ShowPrintButton = true;
                repvwBenchmarkStats1b.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByTEKS)
            {
                ReportDataSource rds = new ReportDataSource(repvwBenchmarkStats1c.LocalReport.GetDataSourceNames()[0], resultsData.GetItems());
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
        private void setupReportPctCorrectOneTeacher(string groupBySelection)
        {
            makeRepsVisible(repTypePctCorrectOneTeacher, groupBySelection);

            if (groupBySelection == groupByQ)
            {
                ReportDataSource rds = new ReportDataSource(repvwBenchmarkStats2a.LocalReport.GetDataSourceNames()[0], 
                    resultsData.GetItemsWhere(d => d.Teacher == ddTeacher.SelectedItem.ToString().Replace("'", "''")));
                repvwBenchmarkStats2a.LocalReport.DataSources.Clear();
                repvwBenchmarkStats2a.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats2a.ShowPrintButton = true;
                repvwBenchmarkStats2a.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByObj)
            {
                ReportDataSource rds = new ReportDataSource(repvwBenchmarkStats2b.LocalReport.GetDataSourceNames()[0],
                    resultsData.GetItemsWhere(d => d.Teacher == ddTeacher.SelectedItem.ToString().Replace("'", "''")));
                repvwBenchmarkStats2b.LocalReport.DataSources.Clear();
                repvwBenchmarkStats2b.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats2b.ShowPrintButton = true;
                repvwBenchmarkStats2b.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByTEKS)
            {
                ReportDataSource rds = new ReportDataSource(repvwBenchmarkStats2c.LocalReport.GetDataSourceNames()[0],
                    resultsData.GetItemsWhere(d => d.Teacher == ddTeacher.SelectedItem.ToString().Replace("'", "''")));
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
        private void setupReportAnsChoiceOneTeacher(string groupBySelection)
        {
            makeRepsVisible(repTypeAnsChoiceOneTeacher, groupBySelection);

            if (groupBySelection == groupByQ)
            {
                ReportDataSource rds = new ReportDataSource(repvwBenchmarkStats3a.LocalReport.GetDataSourceNames()[0], 
                    resultsData.GetItemsWhere(d => d.Teacher == ddTeacher.SelectedItem.ToString().Replace("'", "''")));
                repvwBenchmarkStats3a.LocalReport.DataSources.Clear();
                repvwBenchmarkStats3a.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats3a.ShowPrintButton = true;
                repvwBenchmarkStats3a.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByObj)
            {
                ReportDataSource rds = new ReportDataSource(repvwBenchmarkStats3b.LocalReport.GetDataSourceNames()[0],
                    resultsData.GetItemsWhere(d => d.Teacher == ddTeacher.SelectedItem.ToString().Replace("'", "''")));
                repvwBenchmarkStats3b.LocalReport.DataSources.Clear();
                repvwBenchmarkStats3b.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats3b.ShowPrintButton = true;
                repvwBenchmarkStats3b.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByTEKS)
            {
                ReportDataSource rds = new ReportDataSource(repvwBenchmarkStats3c.LocalReport.GetDataSourceNames()[0], 
                    resultsData.GetItemsWhere(d => d.Teacher == ddTeacher.SelectedItem.ToString().Replace("'", "''")));
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
        private void setupReportAnsChoiceAllTeachers(string groupBySelection)
        {
            makeRepsVisible(repTypeAnsChoiceAllTeachers, groupBySelection);

            if (groupBySelection == groupByQ)
            {
                ReportDataSource rds = new ReportDataSource(repvwBenchmarkStats4a.LocalReport.GetDataSourceNames()[0], resultsData.GetItems());
                repvwBenchmarkStats4a.LocalReport.DataSources.Clear();
                repvwBenchmarkStats4a.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats4a.ShowPrintButton = true;
                repvwBenchmarkStats4a.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByObj)
            {
                ReportDataSource rds = new ReportDataSource(repvwBenchmarkStats4b.LocalReport.GetDataSourceNames()[0], resultsData.GetItems());
                repvwBenchmarkStats4b.LocalReport.DataSources.Clear();
                repvwBenchmarkStats4b.LocalReport.DataSources.Add(rds);
                repvwBenchmarkStats4b.ShowPrintButton = true;
                repvwBenchmarkStats4b.LocalReport.Refresh();
            }
            else if (groupBySelection == groupByTEKS)
            {
                ReportDataSource rds = new ReportDataSource(repvwBenchmarkStats4c.LocalReport.GetDataSourceNames()[0], resultsData.GetItems());
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

            else if (theReport == repTypePctCorrectAllTeachers)
            {
                if (groupBy == groupByQ)
                    repvwBenchmarkStats1a.Visible = true;
                else if (groupBy == groupByObj)
                    repvwBenchmarkStats1b.Visible = true;
                else if (groupBy == groupByTEKS)
                    repvwBenchmarkStats1c.Visible = true;
            }

            else if (theReport == repTypePctCorrectOneTeacher)
            {
                if (groupBy == groupByQ)
                    repvwBenchmarkStats2a.Visible = true;
                else if (groupBy == groupByObj)
                    repvwBenchmarkStats2b.Visible = true;
                else if (groupBy == groupByTEKS)
                    repvwBenchmarkStats2c.Visible = true;
            }

            else if (theReport == repTypeAnsChoiceOneTeacher)
            {
                if (groupBy == groupByQ)
                    repvwBenchmarkStats3a.Visible = true;
                else if (groupBy == groupByObj)
                    repvwBenchmarkStats3b.Visible = true;
                else if (groupBy == groupByTEKS)
                    repvwBenchmarkStats3c.Visible = true;
            }

            else if (theReport == repTypeAnsChoiceAllTeachers)
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