using System;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Helpers;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Infrastructure
{
    public abstract class ReportPage<T> : System.Web.UI.Page where T : ListControl
    {
        // will be injected by StructureMap setter injection
        public IRepoService DataService { get; set; }


        public virtual TestFilterState thisTestFilterState { get; set; }

        public DropDownList ddTFCur = new DropDownList();
        public DropDownList ddTFSubj;// = new DropDownList();
        public DropDownList ddTFTestType;// = new DropDownList();
        public DropDownList ddTFTestVersion;// = new DropDownList();
        public DropDownList ddCampus;
        public DropDownList listTests;
        public ListBox lbListTests;
        public Image imgFilterTests = new Image();
        public Label lblTestsFiltered = new Label();


        protected void ddTFCur_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dd = sender as DropDownList;
            thisTestFilterState.Curric = dd.SelectedItem.Value.ToString();
            runTestFilter();
            updateTestFilterDisplay(thisTestFilterState.AreAnyFiltersApplied);

            TestFilter.SetSubjectFilters(ddTFSubj, thisTestFilterState, ddCampus.SelectedValue.ToString());

            return;
        }

        protected void ddTFSubj_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dd = sender as DropDownList;
            thisTestFilterState.Subject = dd.SelectedItem.Value.ToString();
            runTestFilter();
            updateTestFilterDisplay(thisTestFilterState.AreAnyFiltersApplied);

            return;
        }

        protected void ddTFTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dd = sender as DropDownList;
            thisTestFilterState.TestType = dd.SelectedItem.Value.ToString();
            runTestFilter();
            updateTestFilterDisplay(thisTestFilterState.AreAnyFiltersApplied);

            return;
        }

        protected void ddTFTestVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dd = sender as DropDownList;
            thisTestFilterState.TestVersion = dd.SelectedItem.Value.ToString();
            runTestFilter();
            updateTestFilterDisplay(thisTestFilterState.AreAnyFiltersApplied);

            return;
        }

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



        private void updateTestFilterDisplay(bool filtersapplied)
        {
            // set filter button image
            TestFilter.SetFilterButtonImage(imgFilterTests, filtersapplied);

            // display tests filtered label
            lblTestsFiltered.Visible = filtersapplied;

            return;
        }

        private void runTestFilter()
        {
            if (typeof(T) == typeof(DropDownList))
                TestFilter.FilterTests<T>(thisTestFilterState, ddCampus.SelectedValue.ToString(), listTests as T,
                    DataService);
            else
                TestFilter.FilterTests<T>(thisTestFilterState, ddCampus.SelectedValue.ToString(), lbListTests as T,
                    DataService);

            return;
        }


    }
}