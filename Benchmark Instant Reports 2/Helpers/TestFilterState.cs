using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public class TestFilterState
    {
        public string Curric { get; set; }
        public bool CurricFiltered
        {
            get { return (Curric == Constants.allIndicator) ? false : true; }
        }

        public string Subject { get; set; }
        public bool SubjectFiltered
        {
            get { return (Subject == Constants.allIndicator) ? false : true; }
        }

        public string TestType { get; set; }
        public bool TestTypeFiltered
        {
            get { return (TestType == Constants.allIndicator) ? false : true; }
        }

        public string TestVersion { get; set; }
        public bool TestVersionFiltered
        {
            get { return (TestVersion == Constants.allIndicator) ? false : true; }
        }

        public bool AreAnyFiltersApplied
        {
            get { return CurricFiltered || SubjectFiltered || TestTypeFiltered || TestVersionFiltered; }
        }

        // default constructor
        public TestFilterState()
        {
            Curric = Constants.allIndicator;
            Subject = Constants.allIndicator;
            TestType = Constants.allIndicator;
            TestVersion = Constants.allIndicator;
        }

        // constructor with specific values
        public TestFilterState(string curric, string subject, string testtype, string testversion)
        {
            Curric = curric;
            Subject = subject;
            TestType = testtype;
            TestVersion = testversion;
        }

        // reset all filters
        public void Reset()
        {
            Curric = Constants.allIndicator;
            Subject = Constants.allIndicator;
            TestType = Constants.allIndicator;
            TestVersion = Constants.allIndicator;
        }

    }
}