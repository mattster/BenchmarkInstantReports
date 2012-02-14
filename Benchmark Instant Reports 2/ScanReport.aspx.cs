using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Account;
using Benchmark_Instant_Reports_2.Helpers;
using Benchmark_Instant_Reports_2.Helpers.Reports;
using Benchmark_Instant_Reports_2.Infrastructure;
using Benchmark_Instant_Reports_2.Infrastructure.DataStruct;
using Benchmark_Instant_Reports_2.References;
using Microsoft.Reporting.WebForms;

namespace Benchmark_Instant_Reports_2
{
    public partial class ScanReport : ReportPage<ListBox>
    {
        public SiteMaster theMasterPage;
        private static ScanReportData reportData = new ScanReportData();
        private static TestFilterState _thisTestFilterState = new TestFilterState();
        public override TestFilterState thisTestFilterState
        {
            get { return _thisTestFilterState; }
            set { _thisTestFilterState = value; }
        }

        private static string repTypeScanData = "Show Scanned Data";
        private static string repTypeMissingData = "Show Students Not Yet Scanned";
        private static string[] repTypesList = new string[] { repTypeScanData, repTypeMissingData };


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

            if (ddCampus.SelectedValue == Constants.DispAllElementary ||
                ddCampus.SelectedValue == Constants.DispAllSecondary)
            {
                rbJustScanData.Visible = false;
                rbScanAndMissing.Visible = false;
            }
            else
            {
                rbJustScanData.Visible = true;
                rbScanAndMissing.Visible = true;
            }

            RememberHelper.SaveSelectedCampus(Response, ddCampus.SelectedItem.ToString());

            lbListTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            lbListTests.DataBind();

            setupTestFilters();
            lbListTests.Enabled = true;
            lbListTests.SelectedIndex = 0;
            repvwScanReport1.Visible = false;
            repvwScanReport2.Visible = false;

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

            repvwScanReport1.Visible = false;
            repvwScanReport2.Visible = false;

            return;
        }


        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            var selectedSchools = GetSelectedSchools();
            var selectedTests = GetSelectedTests();

            reportData = ScanRepHelper.GenerateScanReportData(DataService, selectedSchools, selectedTests);
            PreslugData reportDataMissingStudents = ScanRepHelper.GenerateMissingStudentData(DataService,
                selectedSchools, selectedTests);

            if (selectedSchools.Count > 1)
            {
                // setup the report for multiple schools
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
                //setup the report for a single school
                repvwScanReport1.Visible = true;
                repvwScanReport2.Visible = false;
                ReportDataSource rds = new ReportDataSource(repvwScanReport1.LocalReport.GetDataSourceNames()[0],
                    reportData.GetItems());
                repvwScanReport1.LocalReport.DataSources.Clear();
                repvwScanReport1.LocalReport.DataSources.Add(rds);

                if (rbScanAndMissing.Checked)
                {
                    ReportDataSource rds2 = new ReportDataSource(repvwScanReport1.LocalReport.GetDataSourceNames()[1],
                        reportDataMissingStudents.GetItems());
                    repvwScanReport1.LocalReport.DataSources.Add(rds2);
                }
                else
                {
                    PreslugData blank = new PreslugData();
                    ReportDataSource rds2 = new ReportDataSource(repvwScanReport1.LocalReport.GetDataSourceNames()[1],
                        blank.GetItems());
                    repvwScanReport1.LocalReport.DataSources.Add(rds2);
                }

                repvwScanReport1.ShowPrintButton = true;
                repvwScanReport1.LocalReport.Refresh();
            }

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
            repvwScanReport1.Visible = false;
            repvwScanReport2.Visible = false;

            // load list of campuses in Campus dropdown
            ddCampus.DataSource = Authorize.GetAuthorizedSchools(Context.User.Identity.Name, DataService);
            ddCampus.DataTextField = "Name";
            ddCampus.DataValueField = "Abbr";
            ddCampus.DataBind();

            // add option for "ALL" if authorized as admin
            if (Authorize.IsAuthorizedForAllCampuses(Context.User.Identity.Name))
            {
                ddCampus.Items.Insert(0, new ListItem(Constants.DispAllSecondary, Constants.DispAllSecondary));
                ddCampus.Items.Insert(0, new ListItem(Constants.DispAllElementary, Constants.DispAllElementary));
                ddCampus.SelectedIndex = 0;
            }

            int cidx = UIHelper.GetIndexOfItemInDD(RememberHelper.SavedSelectedCampus(Request), ddCampus);
            if (cidx != -1)
                ddCampus.SelectedIndex = cidx;
            else
                ddCampus.SelectedIndex = 0;
            if (ddCampus.SelectedValue == Constants.DispAllElementary ||
                ddCampus.SelectedValue == Constants.DispAllSecondary)
            {
                rbJustScanData.Visible = false;
                rbScanAndMissing.Visible = false;
            }
            else
            {
                rbJustScanData.Visible = true;
                rbScanAndMissing.Visible = true;
            }

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