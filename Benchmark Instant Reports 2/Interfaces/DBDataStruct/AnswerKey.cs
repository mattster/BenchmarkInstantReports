using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    public class AnswerKey
    {
        public string TestID { get; set; }
        public int ItemNum { get; set; }
        public string Answer { get; set; }
        public int Category { get; set; }
        public string TEKS { get; set; }
        public int Weight { get; set; }

        public AnswerKey()
        {
            TestID = "";
            ItemNum = 0;
            Answer = "";
            Category = 0;
            TEKS = "";
            Weight = 1;
        }
    }

}