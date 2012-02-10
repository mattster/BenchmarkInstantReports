using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    public class GradedTestData
    {
        public string LetterGrade { get; set; }
        public int NumCorrect { get; set; }
        public int NumTotal { get; set; }
        public double NumPoints { get; set; }
        public double NumTotalPoints { get; set; }
        public double PctCorrect { get; set; }
        public int PassNum { get; set; }
        public int CommendedNum { get; set; }
        public List<ItemInfo<string>> Responses { get; set; }
        public List<ItemInfo<bool>> ResponsesCorrect { get; set; }
        public string GradedAnswers { get; set; }
        public string GradedAnswersFormatted { get; set; }


        public GradedTestData()
        {
            LetterGrade = "Z";
            NumCorrect = 0;
            NumTotal = 0;
            NumPoints = 0;
            NumTotalPoints = 0;
            PctCorrect = 0;
            PassNum = 0;
            CommendedNum = 0;
            Responses = new List<ItemInfo<string>>();
            ResponsesCorrect = new List<ItemInfo<bool>>();
            GradedAnswers = "";
            GradedAnswersFormatted = "";
        }
    }
}