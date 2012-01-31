using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Account;
using Benchmark_Instant_Reports_2.Helpers;
using Benchmark_Instant_Reports_2.Infrastructure;
using Benchmark_Instant_Reports_2.Interfaces;
using Microsoft.Reporting.WebForms;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.Helpers.Reports;


namespace Benchmark_Instant_Reports_2
{
    public partial class MaterialsReport : ReportPage<ListBox>
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
            if (UIHelper.isDDSeparatorValue(ddCampus.SelectedValue.ToString()))
            {
                RememberHelper.savedSelectedCampus(Response, "");
                return;
            }

            // setup stuff
            RememberHelper.savedSelectedCampus(Response, ddCampus.SelectedItem.ToString());
            lbListTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            lbListTests.DataBind();

            setupTestFilters();
            lbListTests.Enabled = true;
            lbListTests.SelectedIndex = 0;
            repvwMaterialsRep1.Visible = false;
            btnGenReport.Enabled = true;
            repvwMaterialsRep1.Visible = false;

            string[] savedTests = RememberHelper.savedSelectedTestIDs(Request);
            if (savedTests != null)
            {
                lbListTests.ClearSelection();
                UIHelper.selectItemsInLB(lbListTests, savedTests);
                lbListTests_SelectedIndexChanged(new object(), new EventArgs());
            }

            return;
        }


        protected void lbListTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            //*** User selected a set of benchmarks ***//
            RememberHelper.savedSelectedTestIDs(Response, UIHelper.getLBSelectionsAsArray(lbListTests));

            if (lbListTests.GetSelectedIndices().Length > 0)
            {
                btnGenReport.Enabled = true;
                repvwMaterialsRep1.Visible = false;
            }
            else
            {
                btnGenReport.Enabled = false;
                repvwMaterialsRep1.Visible = false;
            }
            return;
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            ScanReportData reportData = ScanRepHelper.generateScanRepTable(ddCampus.SelectedValue.ToString(),
                UIHelper.getLBSelectionsAsArray(lbListTests));

            //setup the report
            repvwMaterialsRep1.Visible = true;
            ObjectDataSource ods = new ObjectDataSource();
            ReportDataSource rds = new ReportDataSource();

            rds = new ReportDataSource(repvwMaterialsRep1.LocalReport.GetDataSourceNames()[0], reportData.GetItems());
            repvwMaterialsRep1.LocalReport.DataSources.Clear();
            repvwMaterialsRep1.LocalReport.DataSources.Add(rds);
            repvwMaterialsRep1.ShowPrintButton = true;

            repvwMaterialsRep1.LocalReport.Refresh();

            return;
        }




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
            btnGenReport.Enabled = false;
            repvwMaterialsRep1.Visible = false;

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


            // load list of tests in Test listbox
            lbListTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            lbListTests.DataBind();

            string[] savedTests = RememberHelper.savedSelectedTestIDs(Request);
            if (savedTests != null)
            {
                lbListTests.ClearSelection();
                UIHelper.selectItemsInLB(lbListTests, savedTests);
                lbListTests_SelectedIndexChanged(new object(), new EventArgs());
            }

            return;
        }

    }
}