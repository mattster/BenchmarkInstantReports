﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Helpers;
using Microsoft.Reporting.WebForms;
using Benchmark_Instant_Reports_2.Infrastructure;
using Benchmark_Instant_Reports_2.Interfaces;
using System.Collections.Generic;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.Helpers.Reports;
using Benchmark_Instant_Reports_2.Account;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2
{
    public partial class CampusReport : ReportPage<DropDownList>
    {
        public SiteMaster theMasterPage;
        private static StGradeReportData resultsData = new StGradeReportData();
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
            if (UIHelper.isDDSeparatorValue(ddCampus.SelectedValue.ToString()) || ddCampus.SelectedValue.ToString() == "")
            {
                RememberHelper.savedSelectedCampus(Response, "");
                return;
            }

            // setup stuff
            RememberHelper.savedSelectedCampus(Response, ddCampus.SelectedItem.ToString());

            listTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            listTests.DataBind();

            setupTestFilters();
            listTests.Enabled = true;
            listTests.SelectedIndex = 0;
            repvwCampusReport1.Visible = false;
            repvwCampusReport2.Visible = false;


            int bidx = UIHelper.getIndexOfDDItem(RememberHelper.savedSelectedTestID(Request), listTests);
            if (bidx != -1)
            {
                listTests.SelectedIndex = bidx;
                listTests_SelectedIndexChanged(new object(), new EventArgs());
            }

            return;
        }

        protected void listTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            RememberHelper.savedSelectedTestID(Response, listTests.SelectedItem.ToString());

            repvwCampusReport1.Visible = false;
            repvwCampusReport2.Visible = false;
            
            return;
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            List<StudentListItem> studentData = new List<StudentListItem>();

            if (ddCampus.SelectedValue.ToString() == Constants.DispAllElementary || 
                ddCampus.SelectedValue.ToString() == Constants.DispAllSecondary)
            {   
                // Show All Campuses
                studentData = StudentData.GetStudentDataToGrade(DataService, listTests.SelectedItem.ToString(),
                    ddCampus.SelectedValue.ToString());

                resultsData = StGradesRepHelper.GenerateStudentStatsReportData(studentData, listTests.SelectedItem.ToString());

                this.repvwCampusReport1.Visible = false;
                this.repvwCampusReport2.Visible = true;

                ReportDataSource rds = new ReportDataSource(repvwCampusReport2.LocalReport.GetDataSourceNames()[0],
                    resultsData.GetItems());
                this.repvwCampusReport2.LocalReport.DataSources.Clear();
                this.repvwCampusReport2.LocalReport.DataSources.Add(rds);
                this.repvwCampusReport2.ShowPrintButton = true;
                this.repvwCampusReport2.LocalReport.Refresh();
                
            }
            else
            {   // Show One Campus
                studentData = StudentData.GetStudentDataToGrade(listTests.SelectedItem.ToString(),
                    ddCampus.SelectedValue.ToString());

                resultsData = StGradesRepHelper.GenerateStudentStatsReportData(studentData, listTests.SelectedItem.ToString());
                this.repvwCampusReport1.Visible = true;
                this.repvwCampusReport2.Visible = false;

                ReportDataSource rds = new ReportDataSource(repvwCampusReport1.LocalReport.GetDataSourceNames()[0],
                    resultsData.GetItems());
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

            // disable all dialog boxes & stuff except campus
            ddCampus.Enabled = true;
            ddCampus.AutoPostBack = true;
            listTests.Enabled = true;
            listTests.AutoPostBack = true;
            btnGenReport.Enabled = true;
            repvwCampusReport1.Visible = false;
            repvwCampusReport2.Visible = false;
            

            // load list of campuses in Campus dropdown
            ddCampus.DataSource = Authorize.getAuthorizedCampusList(Context.User.Identity.Name, DataService);
            ddCampus.DataTextField = "Name";
            ddCampus.DataValueField = "Abbr";
            ddCampus.DataBind();

            // add option for "ALL" if authorized as admin
            //if (CampusSecurity.isAuthorizedAsAdmin(Request))
            //{
            //    ddCampus.Items.Insert(0, new ListItem("ALL Secondary", "ALL Secondary"));
            //    ddCampus.Items.Insert(0, new ListItem("ALL Elementary", "ALL Elementary"));
            //    ddCampus.SelectedIndex = 0;
            //}

            int cidx = UIHelper.getIndexOfDDItem(RememberHelper.savedSelectedCampus(Request), ddCampus);
            if (cidx != -1)
                ddCampus.SelectedIndex = cidx;
            else
                ddCampus.SelectedIndex = 0;

            setupTestFilters();

            listTests.DataSource = DataService.GetTestIDsForSchool(ddCampus.SelectedValue.ToString());
            listTests.DataBind();

            int bidx = UIHelper.getIndexOfDDItem(RememberHelper.savedSelectedTestID(Request), listTests);
            if (bidx != -1)
            {
                listTests.SelectedIndex = bidx;
                listTests_SelectedIndexChanged(new object(), new EventArgs());
            }
            ddCampus_SelectedIndexChanged(new object(), new EventArgs());
            return;
        }
    }
}