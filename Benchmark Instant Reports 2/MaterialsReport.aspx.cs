using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.Common;
using Microsoft.Reporting.WebForms;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Interfaces;


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
                birUtilities.savedSelectedCampus(Response, "");

                return;
            }
            
            // setup stuff
            birUtilities.savedSelectedCampus(Response, ddCampus.SelectedItem.ToString());

            string schoolTypeList;

            //if (ddCampus.SelectedValue == "ALL Elementary")
            //    schoolTypeList = "\'E\'";
            //else if (ddCampus.SelectedValue == "ALL Secondary")
            //    schoolTypeList = "\'S\'";
            //else
                schoolTypeList = birUtilities.getSchoolTypeList(ddCampus.SelectedValue);
    
            string benchmarkListQuery = Queries.GetTestListBySchoolTypes.Replace("@schoolTypeList", schoolTypeList);
            lbBenchmark.DataSource = dbIFOracle.getDataSource(benchmarkListQuery);
            lbBenchmark.DataTextField = "TEST_ID";
            lbBenchmark.DataValueField = "TEST_ID";
            lbBenchmark.DataBind();

            lbBenchmark.Enabled = true;
            lbBenchmark.SelectedIndex = 0;
            btnGenReport.Enabled = true;
            repvwMaterialsRep1.Visible = false;

            string[] savedTests = birUtilities.savedSelectedTestIDs(Request);
            if (savedTests != null)
            {
                lbBenchmark.ClearSelection();
                birUtilities.selectItemsInLB(lbBenchmark, savedTests);
                lbBenchmark_SelectedIndexChanged1(new object(), new EventArgs());
            }

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
            DataTable queryRepResultsTable = ScanReportIF.generateQueryRepTable(ddCampus.SelectedValue.ToString(),
                birUtilities.getLBSelectionsAsArray(lbBenchmark));

                //setup the report
                repvwMaterialsRep1.Visible = true;
                ObjectDataSource ods = new ObjectDataSource();
                ReportDataSource rds = new ReportDataSource();

                rds = new ReportDataSource(repvwMaterialsRep1.LocalReport.GetDataSourceNames()[0], queryRepResultsTable);
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

            // disable all dialog boxes & stuff except campus
            ddCampus.Enabled = true;
            ddCampus.AutoPostBack = true;
            lbBenchmark.Enabled = true;
            lbBenchmark.AutoPostBack = true;
            btnGenReport.Enabled = false;
            repvwMaterialsRep1.Visible = false;

            // load list of campuses in Campus dropdown
            ddCampus.DataSource = birUtilities.getAuthorizedCampusList(Context.User.Identity.Name);
            ddCampus.DataTextField = "SCHOOLNAME";
            ddCampus.DataValueField = "SCHOOL_ABBR";
            ddCampus.DataBind();

            int cidx = birUtilities.getIndexOfDDItem(birUtilities.savedSelectedCampus(Request), ddCampus);
            if (cidx != -1)
                ddCampus.SelectedIndex = cidx;
            else
                ddCampus.SelectedIndex = 0;


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
            string benchmarkListQuery = Queries.GetTestListBySchoolTypes.Replace("@schoolTypeList", schoolTypeList);
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


    }
}