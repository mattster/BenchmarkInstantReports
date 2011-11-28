using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Helpers;
using Benchmark_Instant_Reports_2.Infrastructure;
using Microsoft.Reporting.WebForms;


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

            TestFilter.SetupTestFilterPopup(ddTFCur, ddTFTestType, ddTFTestVersion, ddCampus.SelectedValue.ToString());
            lbListTests.Enabled = true;
            lbListTests.SelectedIndex = 0;
            btnGenReport.Enabled = true;
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

            if (lbListTests.GetSelectedIndices().Length > 0)
            {
                birUtilities.savedSelectedTestIDs(Response, birUtilities.getLBSelectionsAsArray(lbListTests));

                btnGenReport.Enabled = true;
                repvwScanReport1.Visible = false;
                repvwScanReport2.Visible = false;
            }
            else
            {
                btnGenReport.Enabled = false;
                repvwScanReport1.Visible = false;
                repvwScanReport2.Visible = false;
            }
            return;
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            //** User clicked the Generate Report button ***//
            //

            DataTable scanRepResultsTable = ScanReportIF.generateScanRepTable(ddCampus.SelectedValue.ToString(),
                birUtilities.getLBSelectionsAsArray(lbListTests));
            // write results to database
            int r = ScanReportIF.writeScanReportResultsToDb(scanRepResultsTable);

            if (ddCampus.SelectedValue == "ALL Elementary" || ddCampus.SelectedValue == "ALL Secondary")
            {
                // setup the report
                repvwScanReport2.Visible = true;
                repvwScanReport1.Visible = false;
                ObjectDataSource ods = new ObjectDataSource();
                ReportDataSource rds = new ReportDataSource();

                // setup parameters for query
                Parameter paramCampus;
                if (ddCampus.SelectedValue == "ALL Elementary")
                    paramCampus = new Parameter("parmCampus", DbType.String,
                        birUtilities.convertStringArrayForQuery(birIF.getElemAbbrList()));
                else
                    paramCampus = new Parameter("parmCampus", DbType.String,
                        birUtilities.convertStringArrayForQuery(birIF.getSecAbbrList()));

                Parameter paramTestIDList = new Parameter("parmTestIdList", DbType.String,
                    birUtilities.convertStringArrayForQuery(birUtilities.getLBSelectionsAsArray(lbListTests)));

                ods.SelectMethod = "GetData";
                ods.FilterExpression = "CAMPUS IN ({0}) AND TEST_ID IN ({1})";
                ods.FilterParameters.Add(paramCampus);
                ods.FilterParameters.Add(paramTestIDList);

                ods.TypeName = "Benchmark_Instant_Reports_2.DataSetScanReportTableAdapters.TEMP_RESULTS_SCANREPORTTableAdapter";

                rds = new ReportDataSource("DataSetScanReport", ods);
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
                ObjectDataSource ods = new ObjectDataSource();
                ReportDataSource rds = new ReportDataSource();

                // setup parameters for query
                Parameter paramCampus = new Parameter("parmCampus", DbType.String, ddCampus.SelectedValue.ToString());
                Parameter paramTestIDList = new Parameter("parmTestIdList", DbType.String,
                    birUtilities.convertStringArrayForQuery(birUtilities.getLBSelectionsAsArray(lbListTests)));

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
            btnGenReport.Enabled = false;
            repvwScanReport1.Visible = false;
            repvwScanReport2.Visible = false;

            // load list of campuses in Campus dropdown
            ddCampus.DataSource = birUtilities.getAuthorizedCampusList(Context.User.Identity.Name);
            ddCampus.DataTextField = "SCHOOLNAME";
            ddCampus.DataValueField = "SCHOOL_ABBR";
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

            TestFilter.SetupTestFilterPopup(ddTFCur, ddTFTestType, ddTFTestVersion, ddCampus.SelectedValue.ToString());

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
            else
            {

            }

            return;
        }







    }
}