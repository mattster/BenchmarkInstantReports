using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Helpers
{
    /// <summary>
    /// object used to track the current filter state and selections
    /// </summary>
    public class TestFilterState
    {
        public string Curric { get; set; }
        public bool CurricFiltered
        {
            get { return (Curric == Constants.AllIndicator) ? false : true; }
        }

        public string Subject { get; set; }
        public bool SubjectFiltered
        {
            get { return (Subject == Constants.AllIndicator) ? false : true; }
        }

        public string TestType { get; set; }
        public bool TestTypeFiltered
        {
            get { return (TestType == Constants.AllIndicator) ? false : true; }
        }

        public string TestVersion { get; set; }
        public bool TestVersionFiltered
        {
            get { return (TestVersion == Constants.AllIndicator) ? false : true; }
        }

        public bool AreAnyFiltersApplied
        {
            get { return CurricFiltered || SubjectFiltered || TestTypeFiltered || TestVersionFiltered; }
        }

        
        /// <summary>
        /// Default constructor - no filters applied
        /// </summary>
        public TestFilterState()
        {
            Curric = Constants.AllIndicator;
            Subject = Constants.AllIndicator;
            TestType = Constants.AllIndicator;
            TestVersion = Constants.AllIndicator;
        }


        /// <summary>
        /// Constructor with specific filter values applied
        /// </summary>
        /// <param name="curric">selected filter for Curriculum</param>
        /// <param name="subject">selected filter for Subject</param>
        /// <param name="testtype">selected filter for Test Type</param>
        /// <param name="testversion">selected filter for Test Version</param>
        public TestFilterState(string curric, string subject, string testtype, string testversion)
        {
            Curric = curric;
            Subject = subject;
            TestType = testtype;
            TestVersion = testversion;
        }

        
        /// <summary>
        /// reset all filters to an unfiltered state
        /// </summary>
        public void Reset()
        {
            Curric = Constants.AllIndicator;
            Subject = Constants.AllIndicator;
            TestType = Constants.AllIndicator;
            TestVersion = Constants.AllIndicator;
        }

    }
}