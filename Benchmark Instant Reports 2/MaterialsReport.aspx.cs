using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Account;
using Benchmark_Instant_Reports_2.Helpers;
using Benchmark_Instant_Reports_2.Helpers.Reports;
using Benchmark_Instant_Reports_2.Infrastructure;
using Benchmark_Instant_Reports_2.Infrastructure.DataStruct;
using Microsoft.Reporting.WebForms;


namespace Benchmark_Instant_Reports_2
{
    public partial class MaterialsReport : ReportPage<ListBox>
    {
        public SiteMaster theMasterPage;
        private static ScanReportData reportData = new ScanReportData();
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

            return;
        }

        protected void ddCampus_SelectedIndexChanged(object sender, EventArgs e)
        {
            theMasterPage = Page.Master as SiteMaster;

            // return if it is the separator
            if (UIHelper.IsDDSeparatorValue(ddCampus.SelectedValue.ToString()))
            {
                RememberHelper.SaveSelectedCampus(Response, "");
                ddCampus.SelectedIndex = 0;
                return;
            }

            RememberHelper.SaveSelectedCampus(Response, ddCampus.SelectedItem.ToString());

            lbListTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            lbListTests.DataBind();

            setupTestFilters();
            lbListTests.Enabled = true;
            lbListTests.SelectedIndex = 0;
            repvwMaterialsRep1.Visible = false;

            string[] savedTests = RememberHelper.SavedSelectedTestIDs(Request);
            if (savedTests != null)
            {
                lbListTests.ClearSelection();
                UIHelper.SelectItemsInLB(lbListTests, savedTests);
                lbListTests_SelectedIndexChanged(new object(), new EventArgs());
            }

            return;
        }


        protected void lbListTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbListTests.SelectedIndex > -1)
                RememberHelper.SavedSelectedTestIDs(Response, UIHelper.GetLBSelectionsAsArray(lbListTests));

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
            var selectedSchools = GetSelectedSchools();
            reportData = ScanRepHelper.GenerateScanReportData(DataService, selectedSchools, GetSelectedTests());

            repvwMaterialsRep1.Visible = true;
            ReportDataSource rds = new ReportDataSource(repvwMaterialsRep1.LocalReport.GetDataSourceNames()[0], reportData.GetItems());
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
            btnGenReport.Enabled = true;
            repvwMaterialsRep1.Visible = false;

            // load list of campuses in Campus dropdown
            ddCampus.DataSource = Authorize.GetAuthorizedSchools(Context.User.Identity.Name, DataService);
            ddCampus.DataTextField = "Name";
            ddCampus.DataValueField = "Abbr";
            ddCampus.DataBind();

            int cidx = UIHelper.GetIndexOfItemInDD(RememberHelper.SavedSelectedCampus(Request), ddCampus);
            if (cidx != -1)
                ddCampus.SelectedIndex = cidx;
            else
                ddCampus.SelectedIndex = 0;

            setupTestFilters();

            // load list of tests in Test listbox
            lbListTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            lbListTests.DataBind();

            string[] savedTests = RememberHelper.SavedSelectedTestIDs(Request);
            if (savedTests != null)
            {
                lbListTests.ClearSelection();
                UIHelper.SelectItemsInLB(lbListTests, savedTests);
                lbListTests_SelectedIndexChanged(new object(), new EventArgs());
            }

            return;
        }

    }
}