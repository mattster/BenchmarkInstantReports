
namespace Benchmark_Instant_Reports_2.Metadata
{
    public class TestType : TestMetadataItem
    {
        private static string baseRegExPattern1 = @"[0-9]{4}-[0-9]{2} [ES].* ";
        private static string baseRegExPattern2 = @" \d{1,2}-\d{2}$";
        public override string RegEx
        {
            get { return baseRegExPattern1 + CodeAbbr + baseRegExPattern2; }
        }

        public TestType(string name, string dispabbr, string codeabbr, string elemsec) : base(name, dispabbr, codeabbr, elemsec) { }
    }


    public static partial class AllTestMetadata
    {
        public static TestType[] AllTestTypes = 
        {
            new TestType("CUT", "CUT", "CUT", "S"),
            new TestType("TEKS Check", "TEKS Check", "TC", "B"),
            new TestType("Benchmark", "Benchmark", "BMK", "E"),
            new TestType("Simulation", "Simulation", "SIM", "B"),
            new TestType("Semester Exam", "Sem. Exam", "SEM", "S")
        };
    }

}