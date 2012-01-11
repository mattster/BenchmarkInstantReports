using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    public class AnswerKeyItem : IComparable<AnswerKeyItem>
    {
        public string TestID { get; set; }
        public int ItemNum { get; set; }
        public string Answer { get; set; }
        public int Category { get; set; }
        public string TEKS { get; set; }
        public int Weight { get; set; }

        public AnswerKeyItem()
        {
            TestID = "";
            ItemNum = 0;
            Answer = "";
            Category = 0;
            TEKS = "";
            Weight = 1;
        }

        // implement IComparable.CompareTo in order to Sort a list
        public int CompareTo(AnswerKeyItem other)
        {
            if (other == null) return 1;

            return ItemNum.CompareTo(other.ItemNum);
        }
    }

}