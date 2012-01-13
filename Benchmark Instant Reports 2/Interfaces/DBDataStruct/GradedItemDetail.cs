using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    public class GradedItemDetail
    {
        public int ItemNum { get; set; }
        public bool Correct { get; set; }
        public string CorrectAnswer { get; set; }
        public string StudentAnswer { get; set; }
        public int Category { get; set; }
        public string TEKS { get; set; }

        public GradedItemDetail()
        {
            ItemNum = 0;
            Correct = false;
            CorrectAnswer = "";
            StudentAnswer = "";
            Category = 0;
            TEKS = "";
        }
    }
}