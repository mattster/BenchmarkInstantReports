using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Classes
{
    public class Curriculum
    {
        public string Name { get; set; }
        public string DispAbbr { get; set; }
        public string CodeAbbr { get; set; }
        public string ElemSec { get; set; }
        public string RegEx { get; private set; }

        private static string baseRegExPattern1 = @"[0-9]{4}-[0-9]{2} [ES]";
        private static string baseRegExPattern2 = @" .*";

        public Curriculum(string name, string dispabbr, string codeabbr, string elemsec)
        {
            Name = name;
            DispAbbr = dispabbr;
            CodeAbbr = codeabbr;
            RegEx = baseRegExPattern1 + codeabbr + baseRegExPattern2;
            ElemSec = elemsec;
        }
    }

    public static partial class AllTestMetadata
    {
        public static Curriculum[] AllCurriculum = 
        {
            new Curriculum("Science", "Science", "C", "B"),
            new Curriculum("Math", "Math", "M", "B"),
            new Curriculum("Social Studies", "Social Studies", "S", "B"),
            new Curriculum("Reading", "Reading", "R", "E"),
            new Curriculum("Writing", "Writing", "W", "E"),
            new Curriculum("English and Language Arts", "ELA", "ELA", "S"),
            new Curriculum("Languages Other Than English", "LOTE", "LOTE", "S"),
            new Curriculum("Technology", "Technology", "TECH", "S"),
            new Curriculum("Music", "Music", "MU", "S"),
            new Curriculum("Band and Orchestra", "Band / Orch.", "BO", "B"),
        };

    }
}