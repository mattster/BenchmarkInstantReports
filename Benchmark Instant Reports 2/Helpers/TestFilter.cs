﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.Metadata;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Interfaces;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public static class TestFilter
    {
        public static void SetupTestFilterPopup(DropDownList ddTFCur, DropDownList ddTFSubj, DropDownList ddTFTT,
            DropDownList ddTFTV, Constants.SchoolType schType)
        {
            loadFilterListInDD<Curriculum>(ddTFCur, schType);
            loadFilterListInDD<Subject>(ddTFSubj, schType);
            loadFilterListInDD<TestType>(ddTFTT, schType);
            loadFilterListInDD<TestVersion>(ddTFTV, schType);

            return;
        }

        public static void SetFilterButtonImage(Image imgFilterTests, bool filtersapplied)
        {
            if (filtersapplied)
                imgFilterTests.ImageUrl = "~/content/images/f-circ-red-20x20.png";
            else
                imgFilterTests.ImageUrl = "~/content/images/f-circ-20x20.png";

            return;
        }

        public static void SetSubjectFilters(DropDownList ddTFSubj, TestFilterState theTestFilterState, string campus)
        {
            List<string> applicableSubjects = new List<string>();
            applicableSubjects.Add(References.Constants.AllIndicator);

            foreach (Subject s in AllTestMetadata.AllSubjects)
                if (s.CurricApplicable == theTestFilterState.Curric)
                    applicableSubjects.Add(s.DispAbbr);

            ddTFSubj.DataSource = applicableSubjects;
            ddTFSubj.DataBind();

            return;
        }


        public static void FilterTests<T>(TestFilterState theTestFilterState, string campus, T testbox,
            IRepoService dataservice) where T : System.Web.UI.WebControls.ListControl
        {
            List<string> resultList = new List<string>();
            List<List<string>> filteredTestsLists = new List<List<string>>();

            // find the tests for each filter criteria
            filteredTestsLists.Add(filterTestsBy<Curriculum>(campus, theTestFilterState.Curric, dataservice));
            filteredTestsLists.Add(filterTestsBy<Subject>(campus, theTestFilterState.Subject, dataservice));
            filteredTestsLists.Add(filterTestsBy<TestType>(campus, theTestFilterState.TestType, dataservice));
            filteredTestsLists.Add(filterTestsBy<TestVersion>(campus, theTestFilterState.TestVersion, dataservice));

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
        // private methods that make it all work

        private static string[] getTestMetadataList<T>(Constants.SchoolType schType) where T : TestMetadataItem
        {
            List<string> thelist = new List<string>();

            // first add ALL choice
            thelist.Add(References.Constants.AllIndicator);

            if (schType == Constants.SchoolType.All)                 // both Elem & Sec
            {
                foreach (T item in AllTestMetadata.All<T>())
                    thelist.Add(item.DispAbbr);
            }
            else if (schType == Constants.SchoolType.Elementary)            // Elementary
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


        private static void loadFilterListInDD<T>(DropDownList ddl, Constants.SchoolType schType) where T : TestMetadataItem
        {
            ddl.DataSource = getTestMetadataList<T>(schType);
            ddl.DataBind();

            return;
        }


        private static List<string> filterTestsBy<T>(string abbr, string filteredSelection,
            IRepoService dataservice) where T : TestMetadataItem
        {
            IList<string> alltests = dataservice.GetTestIDsForSchool(abbr);
            List<string> resultList = new List<string>();
            string pattern = getRegExPatternFor<T>(filteredSelection);

            foreach (string curTest in alltests)
            {
                if (Regex.IsMatch(curTest, pattern) || filteredSelection == Constants.AllIndicator)
                    resultList.Add(curTest);
            }

            return resultList;
        }


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