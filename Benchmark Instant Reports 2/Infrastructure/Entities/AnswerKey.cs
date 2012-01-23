using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Infrastructure.Entities
{
    public class AnswerKey
    {
        public string TestID { get; set; }
        public int ItemNum { get; set; }
        public string Answer { get; set; }
        public int Objective { get; set; }
        public string TEKS { get; set; }
        public double Weight { get; set; }
    }
}