using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Metadata
{
    public class TestVersion : TestMetadataItem
    {
        private static string baseRegExPattern1 = @"[0-9]{4}-[0-9]{2} [ES]";
        private static string baseRegExPattern2 = @" .*";
        public override string RegEx
        {
            get { return baseRegExPattern1 + CodeAbbr + baseRegExPattern2; }
        }

        public TestVersion(string name, string dispabbr, string codeabbr, string elemsec) : base(name, dispabbr, codeabbr, elemsec) { }
    }


    public static partial class AllTestMetadata
    {
        public static TestVersion[] AllTestVersions = 
        {
            new TestVersion("Regular", "Regular", "", "B"),
            new TestVersion("Modified", "Modified", "-M", "B"),
            new TestVersion("PreAP", "PreAP", "PreAP", "S"),
            new TestVersion("AP", "AP", "(Pre){0}AP", "S")
        };
    }
}