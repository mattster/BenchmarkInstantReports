using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Helpers;
using Benchmark_Instant_Reports_2.Infrastructure;
using Microsoft.Reporting.WebForms;
using Benchmark_Instant_Reports_2.Interfaces;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.Helpers.Reports;
using Benchmark_Instant_Reports_2.Account;

namespace Benchmark_Instant_Reports_2
{
    public partial class ScanReport : ReportPage<ListBox>
    {
        public SiteMaster theMasterPage;
        private static TestFilterState _thisTestFilterState = new TestFilterState();
        public override TestFilterState thisTestFilterState
        {
            get { return _thisTestFilterState; }
            set { _thisTestFilterState = value; }
        }


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

            lbListTests.DataSource = birIF.getTestListForSchool(ddCampus.SelectedValue.ToString());
            lbListTests.DataBind();

            setupTestFilters();
            lbListTests.Enabled = true;
            lbListTests.SelectedIndex = 0;
            repvwScanReport1.Visible = false;
            repvwScanReport2.Visible = false;

            string[] savedTests = birUtilities.savedSelectedTestIDs(Request);
            if (savedTests != null)
            {
                lbListTests.ClearSelection();
                birUtilities.selectItemsInLB(lbListTests, savedTests);
                lbListTests_SelectedIndexChanged(new object(), new EventArgs());
            }

            return;
        }

        protected void lbListTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            //*** User selected a set of benchmarks ***//

            //if (lbListTests.GetSelectedIndices().Length > 0)
            if (lbListTests.SelectedIndex > -1)
            {
                birUtilities.savedSelectedTestIDs(Response, birUtilities.getLBSelectionsAsArray(lbListTests));

                repvwScanReport1.Visible = false;
                repvwScanReport2.Visible = false;
            }
            else
            {
                repvwScanReport1.Visible = false;
                repvwScanReport2.Visible = false;
            }
            return;
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            //** User clicked the Generate Report button ***//
            //

            ScanReportData reportData = ScanRepHelper.generateScanRepTable(ddCampus.SelectedValue.ToString(),
                birUtilities.getLBSelectionsAsArray(lbListTests));

            if (ddCampus.SelectedValue == "ALL Elementary" || ddCampus.SelectedValue == "ALL Secondary")
            {
                // setup the report
                repvwScanReport2.Visible = true;
                repvwScanReport1.Visible = false;
                ReportDataSource rds = new ReportDataSource(repvwScanReport2.LocalReport.GetDataSourceNames()[0], reportData.GetItems());
                repvwScanReport2.LocalReport.DataSources.Clear();
                repvwScanReport2.LocalReport.DataSources.Add(rds);
                repvwScanReport2.ShowPrintButton = true;
                repvwScanReport2.LocalReport.Refresh();
            }

            else
            {
                //setup the report
                repvwScanReport1.Visible = true;
                repvwScanReport2.Visible = false;
                ReportDataSource rds = new ReportDataSource(repvwScanReport1.LocalReport.GetDataSourceNames()[0], reportData.GetItems());
                repvwScanReport1.LocalReport.DataSources.Clear();
                repvwScanReport1.LocalReport.DataSources.Add(rds);
                repvwScanReport1.ShowPrintButton = true;
                repvwScanReport1.LocalReport.Refresh();
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
            lbListTests.Enabled = true;
            lbListTests.AutoPostBack = true;
            btnGenReport.Enabled = true;
            repvwScanReport1.Visible = false;
            repvwScanReport2.Visible = false;

            // load list of campuses in Campus dropdown
            //ddCampus.DataSource = birUtilities.getAuthorizedCampusList(Context.User.Identity.Name);
            //ddCampus.DataTextField = "SCHOOLNAME";
            //ddCampus.DataValueField = "SCHOOL_ABBR";
            ddCampus.DataSource = Authorize.getAuthorizedCampusList(Context.User.Identity.Name, DataService);
            ddCampus.DataTextField = "Name";
            ddCampus.DataValueField = "Abbr";
            ddCampus.DataBind();

            // add option for "ALL" if authorized as admin
            if (CampusSecurity.isAuthorizedAsAdmin(Context.User.Identity.Name))
            {
                ddCampus.Items.Insert(0, new ListItem("ALL Secondary", "ALL Secondary"));
                ddCampus.Items.Insert(0, new ListItem("ALL Elementary", "ALL Elementary"));
                ddCampus.SelectedIndex = 0;
            }

            int cidx = birUtilities.getIndexOfDDItem(birUtilities.savedSelectedCampus(Request), ddCampus);
            if (cidx != -1)
                ddCampus.SelectedIndex = cidx;
            else
                ddCampus.SelectedIndex = 0;

            setupTestFilters();

            // load list of benchmarks in Benchmark listbox
            lbListTests.DataSource = birIF.getTestListForSchool(ddCampus.SelectedValue.ToString());
            lbListTests.DataBind();

            string[] savedTests = birUtilities.savedSelectedTestIDs(Request);
            if (savedTests != null)
            {
                lbListTests.ClearSelection();
                birUtilities.selectItemsInLB(lbListTests, savedTests);
                lbListTests_SelectedIndexChanged(new object(), new EventArgs());
            }

            return;
        }







    }
}