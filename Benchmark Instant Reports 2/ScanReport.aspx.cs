using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.Common;
using Microsoft.Reporting.WebForms;


namespace Benchmark_Instant_Reports_2.Classes
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        
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
                lbBenchmark.Enabled = false;
                btnGenReport.Enabled = false;
                repvwScanReport1.Visible = false;
                enableSchoolPasswordEntry();

                return;
            }


            lblIncorrectPassword.Visible = false;
            disableSchoolPasswordEntry();
            lbBenchmark.Enabled = true;
            lbBenchmark.SelectedIndex = 0;
            btnGenReport.Enabled = true;
            repvwScanReport1.Visible = false;

            return;
        }

        protected void btnEnterPassword_Click(object sender, EventArgs e)
        {
            // *** check password ***//

            if (birUtilities.sessionAuthList.checkEnteredPassword(txtbxSchoolPassword.Text.ToString(), ddCampus.SelectedValue.ToString()))
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
            lbBenchmark.Enabled = true;
            btnGenReport.Enabled = false;
            repvwScanReport1.Visible = false;

            return;
        }

        protected void lbBenchmark_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //*** User selected a set of benchmarks ***//

            if (lbBenchmark.GetSelectedIndices().Length > 0)
            {
                btnGenReport.Enabled = true;
                repvwScanReport1.Visible = false;
                disableSchoolPasswordEntry();
            }
            else
            {
                btnGenReport.Enabled = false;
                repvwScanReport1.Visible = false;
                disableSchoolPasswordEntry();
            }
            return;
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            //** User clicked the Generate Report button ***//
            //

            DataTable scanRepResultsTable = ScanReportIF.generateScanRepTable(ddCampus.SelectedValue.ToString(),
                birUtilities.getLBSelectionsAsArray(lbBenchmark));
            // write results to database
            int r = ScanReportIF.writeScanReportResultsToDb(scanRepResultsTable);

            //setup the report
            repvwScanReport1.Visible = true;
            ObjectDataSource ods = new ObjectDataSource();
            ReportDataSource rds = new ReportDataSource();

            // setup parameters for query
            Parameter paramCampus = new Parameter("parmCampus", DbType.String, ddCampus.SelectedValue.ToString());
            Parameter paramTestIDList = new Parameter("parmTestIdList", DbType.String,
                birUtilities.convertStringArrayForQuery(birUtilities.getLBSelectionsAsArray(lbBenchmark)));

            ods.SelectMethod = "GetData";
            ods.FilterExpression = "CAMPUS = \'{0}\' AND  TEST_ID IN ({1})";
            ods.FilterParameters.Add(paramCampus);
            ods.FilterParameters.Add(paramTestIDList);

            ods.TypeName = "Benchmark_Instant_Reports_2.DataSetScanReportTableAdapters.TEMP_RESULTS_SCANREPORTTableAdapter";

            


            rds = new ReportDataSource("DataSetScanReport", ods);
            repvwScanReport1.LocalReport.DataSources.Clear();
            repvwScanReport1.LocalReport.DataSources.Add(rds);
            repvwScanReport1.ShowPrintButton = true;
            repvwScanReport1.LocalReport.Refresh();

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
            // disable all dialog boxes & stuff except campus
            ddCampus.Enabled = true;
            ddCampus.AutoPostBack = true;
            lbBenchmark.Enabled = false;
            lbBenchmark.AutoPostBack = true;
            btnGenReport.Enabled = false;
            disableSchoolPasswordEntry();
            lblIncorrectPassword.Visible = false;
            repvwScanReport1.Visible = false;

            // load list of campuses in Campus dropdown
            ddCampus.DataSource = dbIFOracle.getDataSource(birIF.getCampusListQuery);
            ddCampus.DataTextField = "SCHOOLNAME";
            ddCampus.DataValueField = "SCHOOL_ABBR";
            ddCampus.DataBind();
            birUtilities.toggleDDLInitView(ddCampus, true);


            // load list of benchmarks in Benchmark listbox
            lbBenchmark.DataSource = dbIFOracle.getDataSource(birIF.getBenchmarkListQuery);
            lbBenchmark.DataTextField = "TEST_ID";
            lbBenchmark.DataValueField = "TEST_ID";
            lbBenchmark.DataBind();

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