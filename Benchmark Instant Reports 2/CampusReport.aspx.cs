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


namespace Benchmark_Instant_Reports_2.Classes
{
    public partial class WebForm8 : System.Web.UI.Page
    {
        public SiteMaster theMasterPage;


        protected void Page_Load(object sender, EventArgs e)
        {
            // activate only the Campus dropdown, deactivate the
            // others until they are ready to be filled

            if (!IsPostBack)
            {
                initPage();
                //ddCampus_SelectedIndexChanged1(new object(), new EventArgs());
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
                //birUtilities.toggleDDLInitView(ddCampus, true);
                birUtilities.savedSelectedCampus(Response, "");
                disableSchoolPasswordEntry();
                return;
            }

            // setup stuff
            //birUtilities.toggleDDLInitView(ddCampus, false);
            birUtilities.savedSelectedCampus(Response, ddCampus.SelectedItem.ToString());

            ddBenchmark.DataSource = birIF.getTestListForSchool(ddCampus.SelectedValue.ToString());
            ddBenchmark.DataBind();

            birUtilities.setupTestFilterPopup(ddTFCur, ddCampus.SelectedValue.ToString());

            if (!CampusSecurity.isAuthorized(ddCampus.SelectedValue.ToString(), Request))
            {                                               // not yet authorized - ask for password
                ddBenchmark.Enabled = false;
                btnGenReport.Enabled = false;
                repvwCampusReport1.Visible = false;
                repvwCampusReport2.Visible = false;
                enableSchoolPasswordEntry();
                theMasterPage.updateCampusAuthLabel("none");

                return;
            }


            disableSchoolPasswordEntry();
            ddBenchmark.Enabled = true;
            ddBenchmark.SelectedIndex = 0;
            btnGenReport.Enabled = true;
            repvwCampusReport1.Visible = false;
            repvwCampusReport2.Visible = false;

            theMasterPage.updateCampusAuthLabel(CampusSecurity.isAuthorizedFor(Request));

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
            ddBenchmark.Enabled = true;
            btnGenReport.Enabled = false;
            repvwCampusReport1.Visible = false;
            repvwCampusReport2.Visible = false;

            return;
        }

        protected void popupDDLCur_SelectedIndexChanged2(object sender, EventArgs e)
        {
            // return if it is the separator
            if (birUtilities.isDDSeparatorValue(ddTFCur.SelectedItem.ToString()))
            {
                birUtilities.toggleDDLInitView(ddTFCur, true);
                return;
            }
            birUtilities.filterTestsByCurric(ddCampus.SelectedValue.ToString(), ddBenchmark, ddTFCur.SelectedItem.Value.ToString());
            //ddBenchmark.DataBind();

            return;
        }

        protected void ddBenchmark_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //*** User selected a benchmark ***//
            birUtilities.savedSelectedTestID(Response, ddBenchmark.SelectedItem.ToString());

            btnGenReport.Enabled = true;
            repvwCampusReport1.Visible = false;
            repvwCampusReport2.Visible = false;
            disableSchoolPasswordEntry();

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

            // display authorization info
            theMasterPage.updateCampusAuthLabel(CampusSecurity.isAuthorizedFor(Request));

            // disable all dialog boxes & stuff except campus
            ddCampus.Enabled = true;
            ddCampus.AutoPostBack = true;
            ddBenchmark.Enabled = false;
            ddBenchmark.AutoPostBack = true;
            btnGenReport.Enabled = false;
            disableSchoolPasswordEntry();
            repvwCampusReport1.Visible = false;
            repvwCampusReport2.Visible = false;

            // load list of campuses in Campus dropdown
            ddCampus.DataSource = dbIFOracle.getDataSource(birIF.getCampusListQuery);
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


            int cidx = birUtilities.getIndexOfDDItem(birUtilities.savedSelectedCampus(Request), ddCampus);
            if (cidx != -1)
                ddCampus.SelectedIndex = cidx;
            else
                ddCampus.SelectedIndex = 1;
                //birUtilities.toggleDDLInitView(ddCampus, true);


            if (cidx != -1 && ddCampus.SelectedValue.ToString() != "")
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