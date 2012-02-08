using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Metadata;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public static class TestFilter
    {
        /// <summary>
        /// initialize the TestFilter popup control
        /// </summary>
        /// <param name="ddTFCur">Curriculum filter DropDown list control</param>
        /// <param name="ddTFSubj">Subject filter DropDown list control</param>
        /// <param name="ddTFTT">Test type filter DropDown list control</param>
        /// <param name="ddTFTV">Test version filter DropDown list control</param>
        /// <param name="schType">SchoolType type of school</param>
        public static void SetupTestFilterPopup(DropDownList ddTFCur, DropDownList ddTFSubj, DropDownList ddTFTT,
            DropDownList ddTFTV, Constants.SchoolType schType)
        {
            loadFilterListInDD<Curriculum>(ddTFCur, schType);
            loadFilterListInDD<Subject>(ddTFSubj, schType);
            loadFilterListInDD<TestType>(ddTFTT, schType);
            loadFilterListInDD<TestVersion>(ddTFTV, schType);

            return;
        }


        /// <summary>
        /// set the filter button image appropriately
        /// </summary>
        /// <param name="imgFilterTests">the Image control on the page to set</param>
        /// <param name="filtersapplied">true if filters are applied, false otherwise</param>
        public static void SetFilterButtonImage(Image imgFilterTests, bool filtersapplied)
        {
            if (filtersapplied)
                imgFilterTests.ImageUrl = "~/content/images/f-circ-red-20x20.png";
            else
                imgFilterTests.ImageUrl = "~/content/images/f-circ-20x20.png";

            return;
        }


        /// <summary>
        /// fill the Subject filter DropDown control with valid subjects for filtering
        /// </summary>
        /// <param name="ddTFSubj">Subject filter DropDown control</param>
        /// <param name="theTestFilterState">TestFilterState object representing current filter status</param>
        public static void SetSubjectFilters(DropDownList ddTFSubj, TestFilterState theTestFilterState)
        {
            List<string> applicableSubjects = new List<string>();
            applicableSubjects.Add(Constants.AllIndicator);

            foreach (Subject subj in AllTestMetadata.AllSubjects)
                if (subj.CurricApplicable == theTestFilterState.Curric)
                    applicableSubjects.Add(subj.DispAbbr);

            ddTFSubj.DataSource = applicableSubjects;
            ddTFSubj.DataBind();

            return;
        }


        /// <summary>
        /// perform a filter on the list of tests based on the filter criteria
        /// </summary>
        /// <typeparam name="T">Type of the control holding the list of tests (ListBox or DropDown)</typeparam>
        /// <param name="dataservice">IRepoService access to data</param>
        /// <param name="theTestFilterState">TestFilterState object representing current filter status</param>
        /// <param name="schoolAbbr">School abbreviation of the school to use when filtering</param>
        /// <param name="testbox">the control of type T holding the list of tests</param>
        public static void FilterTests<T>(IRepoService dataservice, TestFilterState theTestFilterState, 
            string schoolAbbr, T testbox)
            where T : System.Web.UI.WebControls.ListControl
        {
            List<string> resultList = new List<string>();
            List<List<string>> filteredTestsLists = new List<List<string>>();

            // find the tests for each filter criteria
            filteredTestsLists.Add(filterTestsBy<Curriculum>(dataservice, schoolAbbr, theTestFilterState.Curric));
            filteredTestsLists.Add(filterTestsBy<Subject>(dataservice, schoolAbbr, theTestFilterState.Subject));
            filteredTestsLists.Add(filterTestsBy<TestType>(dataservice, schoolAbbr, theTestFilterState.TestType));
            filteredTestsLists.Add(filterTestsBy<TestVersion>(dataservice, schoolAbbr, theTestFilterState.TestVersion));

            // final test list is a union of all the filtered lists
            resultList = filteredTestsLists[0];
            for (int i = 1; i < filteredTestsLists.Count; i++)
            {
                resultList = resultList.Intersect<string>(filteredTestsLists[i]).ToList<string>();
            }

            // put the tests in the dropdown list
            testbox.DataSource = resultList;
            testbox.DataBind();

            return;
        }





        #region BehindTheCurtain
        /// <summary>
        /// returns a list of items to place in a filter control
        /// </summary>
        /// <typeparam name="T">TestMetadataItem type of metadata items to retrieve</typeparam>
        /// <param name="schType">SchoolType type of the School to use for the filter</param>
        /// <returns>string array of items to put in the filter control</returns>
        private static string[] getTestMetadataList<T>(Constants.SchoolType schType) where T : TestMetadataItem
        {
            List<string> thelist = new List<string>();

            // first add ALL choice
            thelist.Add(Constants.AllIndicator);

            if (schType == Constants.SchoolType.All)                    // both Elem & Sec
            {
                foreach (T item in AllTestMetadata.All<T>())
                    thelist.Add(item.DispAbbr);
            }
            else if (schType == Constants.SchoolType.Elementary)        // Elementary
            {
                foreach (T item in AllTestMetadata.All<T>())
                    if (item.ElemSec == "E" || item.ElemSec == "B")
                        thelist.Add(item.DispAbbr);
            }
            else                                                        // Secondary
            {
                foreach (T item in AllTestMetadata.All<T>())
                    if (item.ElemSec == "S" || item.ElemSec == "B")
                        thelist.Add(item.DispAbbr);
            }

            return thelist.ToArray();
        }


        /// <summary>
        /// load a filter DropDown control with the applicable metadata items
        /// </summary>
        /// <typeparam name="T">TestMetadataItem type of metadata items to retrieve</typeparam>
        /// <param name="ddl">DropDownList filter control to load</param>
        /// <param name="schType">SchoolType type of the School to use for the filter</param>
        private static void loadFilterListInDD<T>(DropDownList ddl, Constants.SchoolType schType) 
            where T : TestMetadataItem
        {
            ddl.DataSource = getTestMetadataList<T>(schType);
            ddl.DataBind();

            return;
        }


        /// <summary>
        /// return a list of TestIDs that result from applying a filter criteria
        /// </summary>
        /// <typeparam name="T">TestMetadataItem type of metadata items used</typeparam>
        /// <param name="dataservice">IRepoService access to data</param>
        /// <param name="schoolAbbr">School abbreviation of the school to use for the filter</param>
        /// <param name="filteredSelection">the selected item to filter by</param>
        /// <returns>a string array of TestIDs</returns>
        private static List<string> filterTestsBy<T>(IRepoService dataservice, string schoolAbbr, 
            string filteredSelection) where T : TestMetadataItem
        {
            IList<string> alltests = dataservice.GetTestIDsForSchool(schoolAbbr);
            List<string> resultList = new List<string>();
            string pattern = getRegExPatternFor<T>(filteredSelection);

            foreach (string curTest in alltests)
            {
                if (Regex.IsMatch(curTest, pattern) || filteredSelection == Constants.AllIndicator)
                    resultList.Add(curTest);
            }

            return resultList;
        }


        /// <summary>
        /// returns the RegEx pattern for a specific filter selection
        /// </summary>
        /// <typeparam name="T">TestMetadataItem type of metadata items used</typeparam>
        /// <param name="identifier">the value of the selected filter item</param>
        /// <returns>a string containing the regular expression pattern</returns>
        private static string getRegExPatternFor<T>(string identifier) where T : TestMetadataItem
        {
            foreach (T item in AllTestMetadata.All<T>())
            {
                if (item.DispAbbr == identifier)
                    return item.RegEx;
            }

            return ".*";
        }

        #endregion
    }
}