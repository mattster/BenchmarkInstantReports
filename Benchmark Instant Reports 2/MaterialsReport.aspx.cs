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
   


    public partial class WebForm5 : System.Web.UI.Page
    {
        public SiteMaster theMasterPage;

        
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

            string schoolTypeList;

            //if (ddCampus.SelectedValue == "ALL Elementary")
            //    schoolTypeList = "\'E\'";
            //else if (ddCampus.SelectedValue == "ALL Secondary")
            //    schoolTypeList = "\'S\'";
            //else
                schoolTypeList = birUtilities.getSchoolTypeList(ddCampus.SelectedValue);
    
            string benchmarkListQuery = birIF.getBenchmarkListQuery.Replace("@schoolTypeList", schoolTypeList);
            lbBenchmark.DataSource = dbIFOracle.getDataSource(benchmarkListQuery);
            lbBenchmark.DataTextField = "TEST_ID";
            lbBenchmark.DataValueField = "TEST_ID";
            lbBenchmark.DataBind();

            if (!CampusSecurity.isAuthorized(ddCampus.SelectedValue.ToString(), Request))
            {                                               // not yet authorized - ask for password
                lbBenchmark.Enabled = false;
                btnGenReport.Enabled = false;
                repvwMaterialsRep1.Visible = false;
                enableSchoolPasswordEntry();
                theMasterPage.updateCampusAuthLabel("none");

                return;
            }


            disableSchoolPasswordEntry();
            lbBenchmark.Enabled = true;
            lbBenchmark.SelectedIndex = 0;
            btnGenReport.Enabled = true;
            repvwMaterialsRep1.Visible = false;

            theMasterPage.updateCampusAuthLabel(CampusSecurity.isAuthorizedFor(Request));

            string[] savedTests = birUtilities.savedSelectedTestIDs(Request);
            if (savedTests != null)
            {
                lbBenchmark.ClearSelection();
                birUtilities.selectItemsInLB(lbBenchmark, savedTests);
                lbBenchmark_SelectedIndexChanged1(new object(), new EventArgs());
            }

            return;
        }

        protected void btnEnterPassword_Click(object sender, EventArgs e)
        {
            theMasterPage = Page.Master as SiteMaster;

            // *** check password ***//

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
            lbBenchmark.Enabled = true;
            btnGenReport.Enabled = false;
            repvwMaterialsRep1.Visible = false;

            return;
        }

        protected void lbBenchmark_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //*** User selected a set of benchmarks ***//
            birUtilities.savedSelectedTestIDs(Response, birUtilities.getLBSelectionsAsArray(lbBenchmark));

            if (lbBenchmark.GetSelectedIndices().Length > 0)
            {
                btnGenReport.Enabled = true;
                repvwMaterialsRep1.Visible = false;
                disableSchoolPasswordEntry();
            }
            else
            {
                btnGenReport.Enabled = false;
                repvwMaterialsRep1.Visible = false;
                disableSchoolPasswordEntry();
            }
            return;
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            //** User clicked the Generate Report button ***//
            //

            DataTable queryRepResultsTable = ScanReportIF.generateQueryRepTable(ddCampus.SelectedValue.ToString(),
                birUtilities.getLBSelectionsAsArray(lbBenchmark));
            // write results to database
            int r = ScanReportIF.writeScanReportResultsToDb(queryRepResultsTable);


                //setup the report
                repvwMaterialsRep1.Visible = true;
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
                repvwMaterialsRep1.LocalReport.DataSources.Clear();
                repvwMaterialsRep1.LocalReport.DataSources.Add(rds);
                repvwMaterialsRep1.ShowPrintButton = true;

                repvwMaterialsRep1.LocalReport.Refresh();

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
            lbBenchmark.Enabled = false;
            lbBenchmark.AutoPostBack = true;
            btnGenReport.Enabled = false;
            disableSchoolPasswordEntry();
            repvwMaterialsRep1.Visible = false;

            // load list of campuses in Campus dropdown
            ddCampus.DataSource = dbIFOracle.getDataSource(birIF.getCampusListQuery);
            ddCampus.DataTextField = "SCHOOLNAME";
            ddCampus.DataValueField = "SCHOOL_ABBR";
            ddCampus.DataBind();

            // add option for "ALL" if authorized as admin
            //if (CampusSecurity.isAuthorizedAsAdmin(Request))
            //{
            //    //ddCampus.Items.Insert(0, new ListItem("ALL Secondary", "ALL Secondary"));
            //    //ddCampus.Items.Insert(0, new ListItem("ALL Elementary", "ALL Elementary"));
            //    //ddCampus.SelectedIndex = 0;
            //}

            int cidx = birUtilities.getIndexOfDDItem(birUtilities.savedSelectedCampus(Request), ddCampus);
            if (cidx != -1)
                ddCampus.SelectedIndex = cidx;
            else
                birUtilities.toggleDDLInitView(ddCampus, true);


            // load list of benchmarks in Benchmark listbox
            string schoolTypeList = "";
            if (cidx != -1)
            {
                schoolTypeList = birUtilities.getSchoolTypeList(ddCampus.SelectedValue);
            }
            else
            {
                schoolTypeList = "\'E\',\'S\'";
            }
            string benchmarkListQuery = birIF.getBenchmarkListQuery.Replace("@schoolTypeList", schoolTypeList);
            lbBenchmark.DataSource = dbIFOracle.getDataSource(benchmarkListQuery);
            lbBenchmark.DataTextField = "TEST_ID";
            lbBenchmark.DataValueField = "TEST_ID";
            lbBenchmark.DataBind();

            string[] savedTests = birUtilities.savedSelectedTestIDs(Request);
            if (savedTests != null)
            {
                lbBenchmark.ClearSelection();
                birUtilities.selectItemsInLB(lbBenchmark, savedTests);
                lbBenchmark_SelectedIndexChanged1(new object(), new EventArgs());
            }
            else
            {

            }

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