using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Helpers;
using System.Web.UI.WebControls;

namespace Benchmark_Instant_Reports_2.Infrastructure
{
    public abstract class ReportPage : System.Web.UI.Page
    {
        public virtual TestFilterState thisTestFilterState { get; set; }

        public DropDownList ddTFCur = new DropDownList();
        public DropDownList ddTFTestType = new DropDownList();
        public DropDownList ddCampus = new DropDownList();
        public DropDownList ddBenchmark = new DropDownList();
        public Image imgFilterTests = new Image();
        public Label lblTestsFiltered = new Label();


        protected void ddTFCur_SelectedIndexChanged(object sender, EventArgs e)
        {
            thisTestFilterState.Curric = ddTFCur.SelectedItem.Value.ToString();
            TestFilter.FilterTests(thisTestFilterState, ddCampus.SelectedValue.ToString(), ddBenchmark);
            updateTestFilterDisplay(thisTestFilterState.AreAnyFiltersApplied);

            return;
        }

        protected void ddTFTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            thisTestFilterState.TestType = ddTFTestType.SelectedItem.Value.ToString();
            TestFilter.FilterTests(thisTestFilterState, ddCampus.SelectedValue.ToString(), ddBenchmark);
            updateTestFilterDisplay(thisTestFilterState.AreAnyFiltersApplied);

            return;
        }

        protected void ddTFTestVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void btnTFReset_Click(object sender, EventArgs e)
        {
            thisTestFilterState.Reset();
            ddTFCur.SelectedIndex = 0;
            ddTFTestType.SelectedIndex = 0;

            TestFilter.FilterTests(thisTestFilterState, ddCampus.SelectedValue.ToString(), ddBenchmark);
            updateTestFilterDisplay(thisTestFilterState.AreAnyFiltersApplied);

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

    }
}