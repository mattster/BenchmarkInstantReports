using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    public class ResultsTableItem
    {
        public string Campus { get; set; }
        public string TestID { get; set; }
        public string Teacher { get; set; }
        public string Period { get; set; }
        public int ItemNum { get; set; }
        public decimal PctCorrect { get; set; }
        public int NumCorrect { get; set; }
        public int NumTotal { get; set; }
        public int NumA { get; set; }
        public int NumB { get; set; }
        public int NumC { get; set; }
        public int NumD { get; set; }
        public int NumE { get; set; }
        public int NumF { get; set; }
        public int NumG { get; set; }
        public int NumH { get; set; }
        public int NumJ { get; set; }
        public int NumK { get; set; }
        public string Answer { get; set; }
        public int Objective { get; set; }
        public string TEKS { get; set; }

        public ResultsTableItem()
        {
            Campus = "";
            TestID = "";
            Teacher = "";
            Period = "";
            ItemNum = 0;
            PctCorrect = 0;
            NumCorrect = 0;
            NumTotal = 0;
            NumA = 0;
            NumB = 0;
            NumC = 0;
            NumD = 0;
            NumE = 0;
            NumF = 0;
            NumG = 0;
            NumH = 0;
            NumJ = 0;
            NumK = 0;
            Answer = "";
            Objective = 0;
            TEKS = "";
        }

    }
}