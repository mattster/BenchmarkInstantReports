using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Interfaces.EF
{
    public class BenchmarkStat
    {
        public virtual string Campus { get; set; }                  // key
        public virtual string TestID { get; set; }                  // key
        public virtual string Teacher { get; set; }                 // key
        public virtual string Period { get; set; }                  // key
        public virtual int ItemNum { get; set; }                    // key
        public virtual decimal PctCorrect { get; set; }
        public virtual int NumCorrect { get; set; }
        public virtual int NumTotal { get; set; }
        public virtual int NumA { get; set; }
        public virtual int NumB { get; set; }
        public virtual int NumC { get; set; }
        public virtual int NumD { get; set; }
        public virtual int NumE { get; set; }
        public virtual int NumF { get; set; }
        public virtual int NumG { get; set; }
        public virtual int NumH { get; set; }
        public virtual int NumJ { get; set; }
        public virtual int NumK { get; set; }
        public virtual string Answer { get; set; }
        public virtual int Objective { get; set; }
        public virtual string TEKS { get; set; }
    }
}