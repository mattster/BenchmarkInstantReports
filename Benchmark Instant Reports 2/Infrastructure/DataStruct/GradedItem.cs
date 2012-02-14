using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    public class GradedItem
    {
        public string LetterGrade { get; set; }
        public int NumCorrect { get; set; }
        public int NumTotal { get; set; }
        public decimal NumPoints { get; set; }
        public decimal NumTotalPoints { get; set; }
        public decimal PctCorrect { get; set; }
        public int PassNum { get; set; }
        public int CommendedNum { get; set; }

        public GradedItem()
        {
            LetterGrade = "Z";
            NumCorrect = 0;
            NumTotal = 0;
            NumPoints = 0;
            NumTotalPoints = 0;
            PctCorrect = 0;
            PassNum = 0;
            CommendedNum = 0;
        }
    }
}