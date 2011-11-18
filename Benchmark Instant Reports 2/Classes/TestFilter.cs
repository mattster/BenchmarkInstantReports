using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Classes
{
    public class TestFilterState
    {
        public bool Curric { get; set; }
        public bool Subject { get; set; }
        public bool TestType { get; set; }
        public bool AreAnyFiltersApplied { 
            get { return Curric || Subject || TestType; }  
        }

        public TestFilterState()
        {
            Curric = false;
            Subject = false;
            TestType = false;
        }

        public TestFilterState(bool curric, bool subject, bool testtype)
        {
            Curric = curric;
            Subject = subject;
            TestType = testtype;
        }

    }
    
    public class TestFilter
    {

    
    
    }
}