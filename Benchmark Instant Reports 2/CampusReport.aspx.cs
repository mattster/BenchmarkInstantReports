using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Helpers;
using Microsoft.Reporting.WebForms;
using Benchmark_Instant_Reports_2.Infrastructure;
using Benchmark_Instant_Reports_2.Interfaces;

namespace Benchmark_Instant_Reports_2
{
    public partial class CampusReport : ReportPage<DropDownList>
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
            if (birUtilities.isDDSeparatorValue(ddCampus.SelectedValue.ToString()) || ddCampus.SelectedValue.ToString() == "")
            {
                birUtilities.savedSelectedCampus(Response, "");
                return;
            }

            // setup stuff
            birUtilities.savedSelectedCampus(Response, ddCampus.SelectedItem.ToString());

            listTests.DataSource = birIF.getTestListForSchool(ddCampus.SelectedValue.ToString());
            listTests.DataBind();

            setupTestFilters();
            listTests.Enabled = true;
            listTests.SelectedIndex = 0;
            repvwCampusReport1.Visible = false;
            repvwCampusReport2.Visible = false;
            

            int bidx = birUtilities.getIndexOfDDItem(birUtilities.savedSelectedTestID(Request), listTests);
            if (bidx != -1)
            {
                listTests.SelectedIndex = bidx;
                listTests_SelectedIndexChanged(new object(), new EventArgs());
            }

            return;
        }

        protected void listTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            //*** User selected a benchmark ***//
            birUtilities.savedSelectedTestID(Response, listTests.SelectedItem.ToString());

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
                ds1 = birIF.getStudentDataToGrade(listTests.SelectedItem.ToString());

                dtResultsDataTable = StudentStatsIF.generateStudentStatsRepTable(ds1.Tables[0],
                    listTests.SelectedItem.ToString());
                int r = StudentStatsIF.writeStudentStatsResultsToDb(dtResultsDataTable);

                this.repvwCampusReport1.Visible = false;
                this.repvwCampusReport2.Visible = true;

                // setup the report
                ObjectDataSource ods = new ObjectDataSource();
                ReportDataSource rds = new ReportDataSource();

                // setup parameters for query
                Parameter paramTestID = new Parameter("parmTestId", DbType.String, listTests.SelectedItem.ToString());

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
                ds1 = birIF.getStudentDataToGrade(listTests.SelectedItem.ToString(), ddCampus.SelectedValue.ToString());

                dtResultsDataTable = StudentStatsIF.generateStudentStatsRepTable(ds1.Tables[0],
                    listTests.SelectedItem.ToString());
                int r = StudentStatsIF.writeStudentStatsResultsToDb(dtResultsDataTable);

                this.repvwCampusReport1.Visible = true;
                this.repvwCampusReport2.Visible = false;

                // setup the report
                ObjectDataSource ods = new ObjectDataSource();
                ReportDataSource rds = new ReportDataSource();

                // setup parameters for query
                Parameter paramCampus = new Parameter("campus", DbType.String, ddCampus.SelectedValue.ToString());
                Parameter paramTestID = new Parameter("parmTestId", DbType.String, listTests.SelectedItem.ToString());

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
            //if (!Page.ClientScript.IsStartupScriptRegistered("hideLoading"))
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", "hideLoading();", true);
            //}


            theMasterPage = Page.Master as SiteMaster;

            // disable all dialog boxes & stuff except campus
            ddCampus.Enabled = true;
            ddCampus.AutoPostBack = true;
            listTests.Enabled = true;
            listTests.AutoPostBack = true;
            btnGenReport.Enabled = true;
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

            int cidx = birUtilities.getIndexOfDDItem(birUtilities.savedSelectedCampus(Request), ddCampus);
            if (cidx != -1)
                ddCampus.SelectedIndex = cidx;
            else
                ddCampus.SelectedIndex = 0;

            setupTestFilters();

            listTests.DataSource = birIF.getTestListForSchool(ddCampus.SelectedValue.ToString());
            listTests.DataBind();

            int bidx = birUtilities.getIndexOfDDItem(birUtilities.savedSelectedTestID(Request), listTests);
            if (bidx != -1)
            {
                listTests.SelectedIndex = bidx;
            }
            listTests_SelectedIndexChanged(new object(), new EventArgs());

            return;
        }
    }
}