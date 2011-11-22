using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Metadata;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public static class TestFilter
    {
        //private static string[] getCurricList(string campus)
        //{
        //    List<string> curriclist = new List<string>();
        //    string schtype = birIF.getSchoolType(campus);

        //    curriclist.Add(allIndicator);

        //    if (schtype == "A")                 // both Elem & Sec
        //    {
        //        foreach (Curriculum curric in AllTestMetadata.AllCurriculum)
        //        {
        //            curriclist.Add(curric.DispAbbr);
        //        }
        //    }
        //    else if (schtype == "E")            // Elementary
        //    {
        //        foreach (Curriculum curric in AllTestMetadata.AllCurriculum)
        //        {
        //            if (curric.ElemSec == "E" || curric.ElemSec == "B")
        //                curriclist.Add(curric.DispAbbr);
        //        }
        //    }
        //    else                                // Secondary
        //    {
        //        foreach (Curriculum curric in AllTestMetadata.AllCurriculum)
        //        {
        //            if (curric.ElemSec == "S" || curric.ElemSec == "B")
        //                curriclist.Add(curric.DispAbbr);
        //        }
        //    }

        //    return curriclist.ToArray();
        //}

        //private static string[] getTestTypeList(string campus)
        //{
        //    List<string> testtypelist = new List<string>();
        //    string schtype = birIF.getSchoolType(campus);

        //    testtypelist.Add(allIndicator);

        //    if (schtype == "A")
        //    {
        //        foreach (TestType testtype in AllTestMetadata.AllTestTypes)
        //            testtypelist.Add(testtype.DispAbbr);
        //    }
        //    else if (schtype == "E")
        //    {
        //        foreach (TestType testtype in AllTestMetadata.AllTestTypes)
        //            if (testtype.ElemSec == "E" || testtype.ElemSec == "B")
        //                testtypelist.Add(testtype.DispAbbr);
        //    }
        //    else
        //    {
        //        foreach (TestType testtype in AllTestMetadata.AllTestTypes)
        //            if (testtype.ElemSec == "S" || testtype.ElemSec == "B")
        //                testtypelist.Add(testtype.DispAbbr);
        //    }

        //    return testtypelist.ToArray();
        //}

        public static void setupTestFilterPopup(DropDownList ddTFCur, DropDownList ddTFTT, string campus)
        {
            loadFilterListInDD<Curriculum>(ddTFCur, campus);
            loadFilterListInDD<TestType>(ddTFTT, campus);

            return;
        }


        public static void setFilterButtonImage(Image imgFilterTests, bool filtersapplied)
        {
            if (filtersapplied)
                imgFilterTests.ImageUrl = "~/content/images/f-circ-red-20x20.png";
            else
                imgFilterTests.ImageUrl = "~/content/images/f-circ-20x20.png";

            return;
        }


        public static void filterTests(TestFilterState theTestFilterState, string campus, DropDownList ddl)
        {
            List<string> resultList = new List<string>();
            List<List<string>> filteredTestsLists = new List<List<string>>();

            // find the tests for each filter criteria
            filteredTestsLists.Add(filterTestsByCurric(campus, theTestFilterState.Curric));
            filteredTestsLists.Add(filterTestsByTestType(campus, theTestFilterState.TestType));

            // final test list is a union of all the filtered lists
            resultList = filteredTestsLists[0];
            for (int i = 1; i < filteredTestsLists.Count; i++)
            {
                resultList = resultList.Intersect<string>(filteredTestsLists[i]).ToList<string>();
            }

            // put the tests in the dropdown list
            ddl.DataSource = resultList;
            ddl.DataBind();

            return;
        }





        #region BehindTheCurtain
        // private methods that make it all work

        private static string[] getTestMetadataList<T>(string campus) where T : TestMetadataItem
        {
            List<string> thelist = new List<string>();
            string schtype = birIF.getSchoolType(campus);

            // first add ALL choice
            thelist.Add(References.Constants.allIndicator);

            if (schtype == "A")                 // both Elem & Sec
            {
                foreach (T item in AllTestMetadata.All<T>())
                    thelist.Add(item.DispAbbr);
            }
            else if (schtype == "E")            // Elementary
            {
                foreach (T item in AllTestMetadata.All<T>())
                    if (item.ElemSec == "E" || item.ElemSec == "B")
                        thelist.Add(item.DispAbbr);
            }
            else                                // Secondary
            {
                foreach (T item in AllTestMetadata.All<T>())
                    if (item.ElemSec == "S" || item.ElemSec == "B")
                        thelist.Add(item.DispAbbr);
            }

            return thelist.ToArray();
        }


        private static void loadFilterListInDD<T>(DropDownList ddl, string campus) where T : TestMetadataItem
        {
            ddl.DataSource = getTestMetadataList<T>(campus);
            ddl.DataBind();

            return;
        }

        //private static void loadCurricListInDD(DropDownList ddl, string campus)
        //{
        //    ddl.DataSource = getTestMetadataList<Curriculum>(campus);
        //    ddl.DataBind();

        //    return;
        //}


        private static List<string> filterTestsByCurric(string campus, string curric)
        {
            string[] alltests = birIF.getTestListForSchool(campus);
            List<string> resultList = new List<string>();
            string pattern = getRegExPatternFor<Curriculum>(curric);

            foreach (string curTest in alltests)
            {
                if (Regex.IsMatch(curTest, pattern) || curric == Constants.allIndicator)
                    resultList.Add(curTest);
            }

            return resultList;
        }

        private static List<string> filterTestsByTestType(string campus, string testtype)
        {
            string[] alltests = birIF.getTestListForSchool(campus);
            List<string> resultList = new List<string>();
            string pattern = getRegExPatternFor<TestType>(testtype);

            foreach (string curTest in alltests)
            {
                if (Regex.IsMatch(curTest, pattern) || testtype == Constants.allIndicator)
                    resultList.Add(curTest);
            }

            return resultList;
        }

        //private static List<string> filterTestsBy(string campus, string filteredSelection)
        //{

        //}


        //private static string getRegExPatternForCurric(string curric)
        //{
        //    foreach (Curriculum thisCurric in AllTestMetadata.AllCurriculum)
        //    {
        //        if (thisCurric.DispAbbr == curric)
        //            return thisCurric.RegEx;
        //    }

        //    return ".*";
        //}

        //private static string getRegExPatternForTestType(string testtype)
        //{
        //    foreach (TestType thisTestType in AllTestMetadata.AllTestTypes)
        //    {
        //        if (thisTestType.DispAbbr == testtype)
        //            return thisTestType.RegEx;
        //    }

        //    return ".*";
        //}

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