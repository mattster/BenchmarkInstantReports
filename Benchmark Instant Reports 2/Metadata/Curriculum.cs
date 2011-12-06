using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Metadata
{
    public class Curriculum : TestMetadataItem
    {
        private static string baseRegExPattern1 = @"[0-9]{4}-[0-9]{2} [ES]";
        private static string baseRegExPattern2 = @" .* \d{1,2}-\d{2}$";
        public override string RegEx
        {
            get { return baseRegExPattern1 + CodeAbbr + baseRegExPattern2; }
        }

        public Curriculum(string name, string dispabbr, string codeabbr, string elemsec) : base(name, dispabbr, codeabbr, elemsec) { }
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
            //new Curriculum("Technology", "Technology", "TECH", "S"),
            //new Curriculum("Music", "Music", "MU", "S"),
            //new Curriculum("Band and Orchestra", "Band / Orch.", "BO", "B"),
        };

    }
}