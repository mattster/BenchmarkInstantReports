using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Helpers;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Infrastructure
{
    /// <summary>
    /// base class used by all report web pages; defines common
    /// methods and members each report needs; inherits from
    /// a base data repository enabled class that contains
    /// the repository service injected by StructureMap
    /// </summary>
    /// <typeparam name="T">Type of control that holds the list of Tests - ListBox or DropDown</typeparam>
    public abstract class ReportPage<T> : DataEnabledPage where T : ListControl
    {
        public virtual TestFilterState thisTestFilterState { get; set; }

        public DropDownList ddTFCur = new DropDownList();
        public DropDownList ddTFSubj;
        public DropDownList ddTFTestType;
        public DropDownList ddTFTestVersion;
        public DropDownList ddCampus;
        public DropDownList listTests;
        public ListBox lbListTests;
        public Image imgFilterTests = new Image();
        public Label lblTestsFiltered = new Label();


        /// <summary>
        /// TestFilter Curriculum DropDown selection has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddTFCur_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dd = sender as DropDownList;
            thisTestFilterState.Curric = dd.SelectedItem.Value.ToString();
            runTestFilter();
            updateTestFilterDisplay(thisTestFilterState.AreAnyFiltersApplied);

            TestFilter.SetSubjectFilters(ddTFSubj, thisTestFilterState);

            return;
        }


        /// <summary>
        /// TestFilter Subject DropDown selection has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddTFSubj_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dd = sender as DropDownList;
            thisTestFilterState.Subject = dd.SelectedItem.Value.ToString();
            runTestFilter();
            updateTestFilterDisplay(thisTestFilterState.AreAnyFiltersApplied);

            return;
        }


        /// <summary>
        /// TestFilter TestType DropDown selection has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddTFTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dd = sender as DropDownList;
            thisTestFilterState.TestType = dd.SelectedItem.Value.ToString();
            runTestFilter();
            updateTestFilterDisplay(thisTestFilterState.AreAnyFiltersApplied);

            return;
        }


        /// <summary>
        /// TestFilter TestVersion DropDown selection has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddTFTestVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dd = sender as DropDownList;
            thisTestFilterState.TestVersion = dd.SelectedItem.Value.ToString();
            runTestFilter();
            updateTestFilterDisplay(thisTestFilterState.AreAnyFiltersApplied);

            return;
        }


        /// <summary>
        /// TestFilter Reset button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTFReset_Click(object sender, EventArgs e)
        {
            thisTestFilterState.Reset();
            ddTFCur.SelectedIndex = 0;
            ddTFSubj.SelectedIndex = 0;
            ddTFTestType.SelectedIndex = 0;
            ddTFTestVersion.SelectedIndex = 0;

            runTestFilter();
            updateTestFilterDisplay(thisTestFilterState.AreAnyFiltersApplied);

            return;
        }


        /// <summary>
        /// initialize the TestFilter items
        /// </summary>
        protected void setupTestFilters()
        {
            Constants.SchoolType schType = new Constants.SchoolType();
            if (ddCampus.SelectedValue.ToString() == Constants.DispAllElementary)
                schType = Constants.SchoolType.Elementary;
            else if (ddCampus.SelectedValue.ToString() == Constants.DispAllSecondary)
                schType = Constants.SchoolType.AllSecondary;
            else
            {
                var school = DataService.SchoolRepo.FindBySchoolAbbr(ddCampus.SelectedValue.ToString());
                schType = DataService.SchoolRepo.GetSchoolType(school);
            }
            TestFilter.SetupTestFilterPopup(ddTFCur, ddTFSubj, ddTFTestType, ddTFTestVersion, schType);

            btnTFReset_Click(new object(), new EventArgs());

            return;
        }


        /// <summary>
        /// returns a list of Test objects based on the current Test ID(s)
        /// that are selected on the page
        /// </summary>
        /// <returns>List-Test- list of Test objects representing what is selected</returns>
        protected List<Test> GetSelectedTests()
        {
            List<Test> finalData = new List<Test>();
            
            if (typeof(T) == typeof(DropDownList))
            {
                Test test = DataService.TestRepo.FindByTestID(listTests.SelectedItem.ToString());
                finalData.Add(test);
            }
            else
            {
                foreach (string testid in UIHelper.GetLBSelectionsAsArray(lbListTests))
                {
                    Test test = DataService.TestRepo.FindByTestID(testid);
                    finalData.Add(test);
                }
            }

            return finalData;
        }


        /// <summary>
        /// returns a list of School objects based on the current Campus
        /// selection, including All Elementary or All Secondary
        /// </summary>
        /// <returns>List-School- list of School objects representing the campus(es) selected</returns>
        protected List<School> GetSelectedSchools()
        {
            List<School> finalData = new List<School>();
            if (ddCampus.SelectedValue.ToString() == Constants.DispAllElementary)
            {
                var schools = DataService.SchoolRepo.FindELCampuses();
                foreach (var school in schools)
                    finalData.Add(school);
            }
            else if (ddCampus.SelectedValue.ToString() == Constants.DispAllSecondary)
            {
                var schools = DataService.SchoolRepo.FindSECCampuses();
                foreach (var school in schools)
                    finalData.Add(school);
            }
            else
            {
                var school = DataService.SchoolRepo.FindBySchoolAbbr(ddCampus.SelectedValue.ToString());
                finalData.Add(school);
            }

            return finalData;
        }




        /// <summary>
        /// update the test filter display items based on filter state
        /// </summary>
        /// <param name="filtersapplied">true if any filters are applied, false if none</param>
        private void updateTestFilterDisplay(bool filtersapplied)
        {
            // set filter button image
            TestFilter.SetFilterButtonImage(imgFilterTests, filtersapplied);

            // display tests filtered label
            lblTestsFiltered.Visible = filtersapplied;

            return;
        }


        /// <summary>
        /// apply the filter criteria to the list of tests
        /// </summary>
        private void runTestFilter()
        {
            if (typeof(T) == typeof(DropDownList))
                TestFilter.FilterTests<T>(DataService, thisTestFilterState, ddCampus.SelectedValue.ToString(), 
                    listTests as T);
            else
                TestFilter.FilterTests<T>(DataService, thisTestFilterState, ddCampus.SelectedValue.ToString(), 
                    lbListTests as T);

            return;
        }


    }
}