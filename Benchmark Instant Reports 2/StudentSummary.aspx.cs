﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.Common;
using Microsoft.Reporting.WebForms;

namespace Benchmark_Instant_Reports_2
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        

        #region globals

        // constants & globals
        //
        private static DataSet studentListQueryData = new DataSet();        // holds results of the custom query
        private static DataView studentListDataByTeacher = new DataView();  // custom query filtered by teacher
        private static DataView studentListDataByTeacherPeriod = new DataView();    // custom query filtered by teacher, period
        private static DataSet dsStudentListData = new DataSet();           // the filtered list of students

        

        #endregion


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
            //*** User selected a campus ***//

            // return if it is the separator
            if (birUtilities.isDDSeparatorValue(ddCampus.SelectedValue.ToString()))
            {
                ddCampus.SelectedIndex = 0;
                return;
            }
            
            // setup stuff
            birUtilities.toggleDDLInitView(ddCampus, false);

            if (!birUtilities.sessionAuthList.isAuthorized(ddCampus.SelectedValue.ToString()))
            {                                               // not yet authorized - ask for password
                ddBenchmark.Enabled = false;
                ddTeacher.Enabled = false;
                lbPeriod.Enabled = false;
                btnGenReport.Enabled = false;
                repvwStudentSummary.Visible = false;
                lblAlignmentNote.Visible = false; 
                enableSchoolPasswordEntry();

                return;
            }


            lblIncorrectPassword.Visible = false;
            disableSchoolPasswordEntry();
            ddBenchmark.Enabled = true;
            birUtilities.toggleDDLInitView(ddBenchmark, true);
            ddTeacher.Enabled = false;
            lbPeriod.Enabled = false;
            btnGenReport.Enabled = false;
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;

            return;
        }

        protected void btnEnterPassword_Click(object sender, EventArgs e)
        {
            //*** check password ***//

            if (birUtilities.sessionAuthList.checkEnteredPassword(txtbxSchoolPassword.Text.ToString(), ddCampus.SelectedValue.ToString()))
            {                                                   // authentication succeeded
                lblIncorrectPassword.Visible = false;
                disableSchoolPasswordEntry();
            }
            else                                                // authentication failed        
            {
                lblIncorrectPassword.Visible = true;
                enableSchoolPasswordEntry();
                return;
            }


            disableSchoolPasswordEntry();
            ddBenchmark.Enabled = true;
            birUtilities.toggleDDLInitView(ddBenchmark, true);
            ddTeacher.Enabled = false;
            lbPeriod.Enabled = false;
            btnGenReport.Enabled = false;
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;

            return;
        }

        protected void ddBenchmark_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //*** User selected a test ***//
            // load a list of teachers applicable to that campus & test
            // and activate the Teacher dropdown

            // load the main query - list of students
            studentListQueryData = birIF.getStudentScanListData(ddBenchmark.SelectedItem.ToString(),
                ddCampus.SelectedValue.ToString());

            // get a list of teachers applicable for this query
            string[] listOfTeachers = birUtilities.getUniqueTableColumnStringValues(studentListQueryData.Tables[0],
                    birIF.teacherNameFieldName);
            Array.Sort(listOfTeachers);


            // if there are no students taking this test at this campus, deal with it
            if (listOfTeachers.Count() == 0)
            {
                ddTeacher.Enabled = false;
                lbPeriod.Enabled = false;
                btnGenReport.Enabled = false;
                disableSchoolPasswordEntry();
                ddBenchmark.Enabled = false;
                repvwStudentSummary.Visible = false;
                lblAlignmentNote.Visible = false;
                return;
            }

            // activate the Teacher dropdown and populate with the list of teachers
            birUtilities.toggleDDLInitView(ddBenchmark, false);
            ddTeacher.Enabled = true;
            ddTeacher.DataSource = listOfTeachers;
            ddTeacher.DataBind();
            birUtilities.toggleDDLInitView(ddTeacher, true);
            lbPeriod.Enabled = false;
            btnGenReport.Enabled = false;
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;
            disableSchoolPasswordEntry();

            return;
        }

        protected void ddTeacher_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //*** User selected a teacher ***//
            // load a list of periods applicable to that campus, test & teacher
            // and activate the period dropdown


            // get a list of the periods applicable for this campus, test & teacher

            string selectedTeacherFilter = birIF.teacherNameFieldName + " = \'" + ddTeacher.SelectedItem + "\'";
            if (studentListQueryData.Tables.Count == 0)
            {
                studentListQueryData = birIF.getStudentScanListData(ddBenchmark.SelectedItem.ToString(),
                    ddCampus.SelectedValue.ToString());
            }

            studentListDataByTeacher = new DataView(studentListQueryData.Tables[0], selectedTeacherFilter,
                "PERIOD ASC", DataViewRowState.CurrentRows);
            string[] listOfPeriods = birUtilities.getUniqueTableColumnStringValues(studentListDataByTeacher.ToTable(), "PERIOD");
            Array.Sort(listOfPeriods);

            // activate the Period dropdown and populate with the list of periods
            birUtilities.toggleDDLInitView(ddTeacher, false);
            lbPeriod.Enabled = true;
            lbPeriod.DataSource = listOfPeriods;
            lbPeriod.DataBind();
            btnGenReport.Enabled = false;
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;
            disableSchoolPasswordEntry();

            return;
        }

        protected void lbPeriod_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //*** User selected a period ***//
            // now we're ready to generate a report, so activate the button

            if (lbPeriod.GetSelectedIndices().Length > 0)
            {
                btnGenReport.Enabled = true;
                repvwStudentSummary.Visible = false;
                lblAlignmentNote.Visible = false;
                disableSchoolPasswordEntry();
            }
            else
            {
                btnGenReport.Enabled = false;
                repvwStudentSummary.Visible = false;
                lblAlignmentNote.Visible = false;
                disableSchoolPasswordEntry();

            }
            return;
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            //** User clicked the Generate Report button ***//
            //

            // generate results for the given criteria on the page
            DataSet ds1 = birIF.getStudentScanListData(ddBenchmark.SelectedItem.ToString(),
                ddCampus.SelectedValue.ToString());            // do a new query by school

            string teacherq = birIF.teacherNameFieldName + " = \'" + ddTeacher.SelectedItem + "\'";
            string periodq = "PERIOD in (" + 
                birUtilities.convertStringArrayForQuery(birUtilities.getLBSelectionsAsArray(lbPeriod)) + ")";
            DataView dv1 = new DataView(ds1.Tables[0], teacherq, "STUDENT_NAME ASC", DataViewRowState.CurrentRows);
            int c1 = dv1.Count;     // wipe
            DataView dv2 = new DataView(dv1.ToTable(), periodq, "STUDENT_NAME ASC", DataViewRowState.CurrentRows);
            int c2 = dv2.Count;     // wipe

            DataTable ssResultsDataTable = StudentStatsIF.generateStudentStatsRepTable(dv2.ToTable(),
                ddBenchmark.SelectedItem.ToString(), ddCampus.SelectedValue.ToString());
            
            // add in the individual student answer data
            StudentSummaryIF.addStudentAnswerData(ssResultsDataTable, ddBenchmark.SelectedItem.ToString(), ddCampus.SelectedValue.ToString());

            int r = StudentStatsIF.writeStudentStatsResultsToDb(ssResultsDataTable);


            repvwStudentSummary.Visible = true;
            lblAlignmentNote.Visible = true;

            // setup the report
            ObjectDataSource ods = new ObjectDataSource();
            ReportDataSource rds = new ReportDataSource();

            // setup parameters for query
            Parameter paramCampus = new Parameter("parmCampus", DbType.String, ddCampus.SelectedValue.ToString());
            Parameter paramTeacher = new Parameter("parmTeacher", DbType.String, ddTeacher.SelectedItem.ToString());
            Parameter paramPeriod = new Parameter("parmPeriod", DbType.String, 
                birUtilities.convertStringArrayForQuery(birUtilities.getLBSelectionsAsArray(lbPeriod)));
            Parameter paramTestID = new Parameter("parmTestId", DbType.String, ddBenchmark.SelectedItem.ToString());

            ods.SelectMethod = "GetDataByUseFilter";
            ods.FilterExpression = "CAMPUS = \'{0}\' AND TEST_ID = \'{1}\' AND TEACHER = \'{2}\' AND PERIOD IN ({3})";
            ods.FilterParameters.Add(paramCampus);
            ods.FilterParameters.Add(paramTestID);
            ods.FilterParameters.Add(paramTeacher);
            ods.FilterParameters.Add(paramPeriod);

            ods.TypeName = "Benchmark_Instant_Reports_2.DataSetStudentStatsTableAdapters.TEMP_RESULTS_STUDENTSTATSTableAdapter";

            rds = new ReportDataSource("DataSetStudentStats", ods);
            repvwStudentSummary.LocalReport.DataSources.Clear();
            repvwStudentSummary.LocalReport.DataSources.Add(rds);
            repvwStudentSummary.ShowPrintButton = true;
            repvwStudentSummary.LocalReport.Refresh();

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
            // disable all dialog boxes & stuff except campus
            ddCampus.Enabled = true;
            ddCampus.AutoPostBack = true;
            ddBenchmark.Enabled = false;
            ddBenchmark.AutoPostBack = true;
            ddTeacher.Enabled = false;
            ddTeacher.AutoPostBack = true;
            lbPeriod.Enabled = false;
            lbPeriod.AutoPostBack = true;
            lbPeriod.SelectionMode = ListSelectionMode.Multiple;
            btnGenReport.Enabled = false;
            disableSchoolPasswordEntry();
            lblIncorrectPassword.Visible = false;
            repvwStudentSummary.Visible = false;
            lblAlignmentNote.Visible = false;
            
            // load list of campuses in Campus dropdown
            ddCampus.DataSource = dbIFOracle.getDataSource(birIF.getCampusListQuery);
            ddCampus.DataTextField = "SCHOOLNAME";
            ddCampus.DataValueField = "SCHOOL_ABBR";
            ddCampus.DataBind();
            birUtilities.toggleDDLInitView(ddCampus, true);


            // load list of benchmarks in Benchmark dropdown
            ddBenchmark.DataSource = dbIFOracle.getDataSource(birIF.getBenchmarkListQuery);
            ddBenchmark.DataTextField = "TEST_ID";
            ddBenchmark.DataValueField = "TEST_ID";
            ddBenchmark.DataBind();

            return;
        }


        //**********************************************************************//
        //** initializes the dropdown menus
        //**
        private void initSelectionBoxes()
        {
            birUtilities.toggleDDLInitView(ddCampus, true);
            birUtilities.toggleDDLInitView(ddBenchmark, true);
            birUtilities.toggleDDLInitView(ddTeacher, true);
            ddTeacher.Enabled = false;
            //toggleDDLInitView(ddPeriod, true);
            lbPeriod.Enabled = false;
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