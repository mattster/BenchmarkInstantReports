
namespace Benchmark_Instant_Reports_2.Metadata
{
    public class TestVersion : TestMetadataItem
    {
        private static string baseRegExPattern1 = "^";
        private static string baseRegExPattern2 = @" \d{1,2}-\d{2}";
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
            new TestVersion("Regular", "Regular", @"((?!\bPreAP\b)(?!-M).)*", "B"),
            new TestVersion("Modified", "Modified", ".*-M.*", "B"),
            new TestVersion("PreAP", "PreAP", ".*PreAP.*", "S"),
            new TestVersion("AP", "AP", @"((?!\bPre).)*AP.*", "S")
        };
    }
}