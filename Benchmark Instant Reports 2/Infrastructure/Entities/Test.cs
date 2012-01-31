using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Infrastructure.Entities
{
    public class Test
    {
        public string TestID { get; set; }
        public int TestYear { get; set; }
        public int TestMonth { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Language { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public int Grade { get; set; }
        public int NumItems { get; set; }
        public int NumPoints { get; set; }
        public int PassNum { get; set; }
        public int CommendedNum { get; set; }
        public int CompScore { get; set; }
        public int Projection { get; set; }
        public string TestTemplate { get; set; }
        public string CustomQueryOrig { get; set; }
        public int TestIDNum { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public Constants.SchoolType SchoolType { get; set; }
        public string CustomQuery { get; set; }
        public Constants.SchoolType SecSchoolType { get; set; }
    }
}