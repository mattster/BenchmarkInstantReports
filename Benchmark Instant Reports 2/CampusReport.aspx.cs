using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.Common;
using Microsoft.Reporting.WebForms;
using AjaxControlToolkit;
using Benchmark_Instant_Reports_2.Helpers;


namespace Benchmark_Instant_Reports_2.Classes
{
    public partial class WebForm8 : System.Web.UI.Page
    {
        public SiteMaster theMasterPage;
        private static TestFilterState thisTestFilterState = new TestFilterState();
        

        protected void Page_Load(object sender, EventArgs e)
        {
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
            if (birUtilities.isDDSeparatorValue(ddCampus.SelectedValue.ToString()) || ddCampus.SelectedValue.ToString() == "")
            {
                birUtilities.savedSelectedCampus(Response, "");
                return;
            }

            // setup stuff
            birUtilities.savedSelectedCampus(Response, ddCampus.SelectedItem.ToString());

            ddBenchmark.DataSource = birIF.getTestListForSchool(ddCampus.SelectedValue.ToString());
            ddBenchmark.DataBind();

            TestFilter.SetupTestFilterPopup(ddTFCur, ddTFTestType, ddCampus.SelectedValue.ToString());
            ddBenchmark.Enabled = true;
            ddBenchmark.SelectedIndex = 0;
            btnGenReport.Enabled = true;
            repvwCampusReport1.Visible = false;
            repvwCampusReport2.Visible = false;

            return;
        }


        protected void popupDDLCur_SelectedIndexChanged2(object sender, EventArgs e)
        {
            thisTestFilterState.Curric = ddTFCur.SelectedItem.Value.ToString();
            TestFilter.FilterTests(thisTestFilterState, ddCampus.SelectedValue.ToString(), ddBenchmark);
            updateTestFilterDisplay(thisTestFilterState.AreAnyFiltersApplied);

            return;
        }

        protected void popupDDLTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            thisTestFilterState.TestType = ddTFTestType.SelectedItem.Value.ToString();
            TestFilter.FilterTests(thisTestFilterState, ddCampus.SelectedValue.ToString(), ddBenchmark);
            updateTestFilterDisplay(thisTestFilterState.AreAnyFiltersApplied);
            
            return;
        }

        protected void btnTFReset_Click(object sender, EventArgs e)
        {
            thisTestFilterState.Reset();
            ddTFCur.SelectedIndex = 0;
            ddTFTestType.SelectedIndex = 0;
            
            TestFilter.FilterTests(thisTestFilterState, ddCampus.SelectedValue.ToString(), ddBenchmark);
            updateTestFilterDisplay(thisTestFilterState.AreAnyFiltersApplied);
            
            return;
        }

        protected void ddBenchmark_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //*** User selected a benchmark ***//
            birUtilities.savedSelectedTestID(Response, ddBenchmark.SelectedItem.ToString());

            btnGenReport.Enabled = true;
            repvwCampusReport1.Visible = false;
            repvwCampusReport2.Visible = false;

            return;
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            //** User clicked the Generate Report button ***//
            //

            DataSet ds1 = new DataSet();
            DataTable dtResultsDataTable = new DataTable();

            if (ddCampus.SelectedValue.ToString() == "ALL Elementary" || ddCampus.SelectedValue.ToString() == "ALL Secondary")
            {   // Show All Campuses
                ds1 = birIF.getStudentDataToGrade(ddBenchmark.SelectedItem.ToString());

                dtResultsDataTable = StudentStatsIF.generateStudentStatsRepTable(ds1.Tables[0],
                    ddBenchmark.SelectedItem.ToString());
                int r = StudentStatsIF.writeStudentStatsResultsToDb(dtResultsDataTable);

                this.repvwCampusReport1.Visible = false;
                this.repvwCampusReport2.Visible = true;

                // setup the report
                ObjectDataSource ods = new ObjectDataSource();
                ReportDataSource rds = new ReportDataSource();

                // setup parameters for query
                Parameter paramTestID = new Parameter("parmTestId", DbType.String, ddBenchmark.SelectedItem.ToString());

                ods.SelectMethod = "GetDataByUseFilter";

                ods.FilterExpression = "TEST_ID = \'{0}\'";
                ods.FilterParameters.Add(paramTestID);

                ods.TypeName = "Benchmark_Instant_Reports_2.DataSetStudentStatsTableAdapters.TEMP_RESULTS_STUDENTSTATSTableAdapter";

                rds = new ReportDataSource("DataSetStudentStatsC", ods);
                this.repvwCampusReport2.LocalReport.DataSources.Clear();
                this.repvwCampusReport2.LocalReport.DataSources.Add(rds);
                this.repvwCampusReport2.ShowPrintButton = true;
                this.repvwCampusReport2.LocalReport.Refresh();
            }
            else
            {   // Show One Campus
                ds1 = birIF.getStudentDataToGrade(ddBenchmark.SelectedItem.ToString(), ddCampus.SelectedValue.ToString());

                dtResultsDataTable = StudentStatsIF.generateStudentStatsRepTable(ds1.Tables[0],
                    ddBenchmark.SelectedItem.ToString());
                int r = StudentStatsIF.writeStudentStatsResultsToDb(dtResultsDataTable);

                this.repvwCampusReport1.Visible = true;
                this.repvwCampusReport2.Visible = false;

                // setup the report
                ObjectDataSource ods = new ObjectDataSource();
                ReportDataSource rds = new ReportDataSource();

                // setup parameters for query
                Parameter paramCampus = new Parameter("campus", DbType.String, ddCampus.SelectedValue.ToString());
                Parameter paramTestID = new Parameter("parmTestId", DbType.String, ddBenchmark.SelectedItem.ToString());

                ods.SelectMethod = "GetDataByUseFilter";

                ods.FilterExpression = "TEST_ID = \'{0}\' AND CAMPUS = \'{1}\'";
                ods.FilterParameters.Add(paramTestID);
                ods.FilterParameters.Add(paramCampus);

                ods.TypeName = "Benchmark_Instant_Reports_2.DataSetStudentStatsTableAdapters.TEMP_RESULTS_STUDENTSTATSTableAdapter";

                rds = new ReportDataSource("DataSetStudentStatsC", ods);
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
            ddBenchmark.Enabled = true;
            ddBenchmark.AutoPostBack = true;
            btnGenReport.Enabled = false;
            repvwCampusReport1.Visible = false;
            repvwCampusReport2.Visible = false;

            // load list of campuses in Campus dropdown
            ddCampus.DataSource = birUtilities.getAuthorizedCampusList(Context.User.Identity.Name);
            ddCampus.DataTextField = "SCHOOLNAME";
            ddCampus.DataValueField = "SCHOOL_ABBR";
            ddCampus.DataBind();

            // add option for "ALL" if authorized as admin
            //if (CampusSecurity.isAuthorizedAsAdmin(Request))
            //{
            //    ddCampus.Items.Insert(0, new ListItem("ALL Secondary", "ALL Secondary"));
            //    ddCampus.Items.Insert(0, new ListItem("ALL Elementary", "ALL Elementary"));
            //    ddCampus.SelectedIndex = 0;
            //}

            //// setup test filters
            //birUtilities.setupTestFilterPopup(ddTFCur);
            TestFilter.SetupTestFilterPopup(ddTFCur, ddTFTestType, ddCampus.SelectedValue.ToString());


            int cidx = birUtilities.getIndexOfDDItem(birUtilities.savedSelectedCampus(Request), ddCampus);
            if (cidx != -1)
                ddCampus.SelectedIndex = cidx;
            else
                ddCampus.SelectedIndex = 0;


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



        private void updateTestFilterDisplay(bool filtersapplied)
        {
            // set filter button image
            TestFilter.SetFilterButtonImage(imgFilterTests, filtersapplied);

            // display tests filtered label
            lblTestsFiltered.Visible = filtersapplied;

            return;
        }




    }
}