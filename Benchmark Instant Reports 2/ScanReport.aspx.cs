﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Account;
using Benchmark_Instant_Reports_2.Helpers;
using Benchmark_Instant_Reports_2.Helpers.Reports;
using Benchmark_Instant_Reports_2.Infrastructure;
using Benchmark_Instant_Reports_2.Interfaces;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.References;
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

            //lbListTests.DataSource = birIF.getTestListForSchool(ddCampus.SelectedValue.ToString());
            lbListTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            lbListTests.DataBind();

            setupTestFilters();
            lbListTests.Enabled = true;
            lbListTests.SelectedIndex = 0;
            repvwScanReport1.Visible = false;
            repvwScanReport2.Visible = false;

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

            //if (lbListTests.GetSelectedIndices().Length > 0)
            if (lbListTests.SelectedIndex > -1)
            {
                RememberHelper.savedSelectedTestIDs(Response, UIHelper.getLBSelectionsAsArray(lbListTests));

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
                UIHelper.getLBSelectionsAsArray(lbListTests));

            if (ddCampus.SelectedValue == Constants.DispAllElementary || 
                ddCampus.SelectedValue == Constants.DispAllSecondary)
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
            ddCampus.DataSource = Authorize.getAuthorizedCampusList(Context.User.Identity.Name, DataService);
            ddCampus.DataTextField = "Name";
            ddCampus.DataValueField = "Abbr";
            ddCampus.DataBind();

            // add option for "ALL" if authorized as admin
            if (CampusSecurity.isAuthorizedAsAdmin(Context.User.Identity.Name))
            {
                ddCampus.Items.Insert(0, new ListItem(Constants.DispAllSecondary, Constants.DispAllSecondary));
                ddCampus.Items.Insert(0, new ListItem(Constants.DispAllElementary, Constants.DispAllElementary));
                ddCampus.SelectedIndex = 0;
            }

            int cidx = UIHelper.getIndexOfDDItem(RememberHelper.savedSelectedCampus(Request), ddCampus);
            if (cidx != -1)
                ddCampus.SelectedIndex = cidx;
            else
                ddCampus.SelectedIndex = 0;

            setupTestFilters();

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