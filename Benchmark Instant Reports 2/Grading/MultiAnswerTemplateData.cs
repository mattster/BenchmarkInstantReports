using System.Collections.Generic;

namespace Benchmark_Instant_Reports_2.Grading
{
    public static partial class TestDefinitionData
    {
        /// <summary>
        /// a list of TestTemplate objects representing all the defined templates
        /// that have items with multiple letter responses
        /// </summary>
        public static List<TestTemplate> MultiAnswerTemplates = new List<TestTemplate>
        {
            new TestTemplate("25-MULT-75-AE", 1, 25, 101, 125, TestTemplate.TestTemplatetype.Special25MULT75AE),
            new TestTemplate("48-AE-4-MULT-18-AE", 49, 52, 71, 74, TestTemplate.TestTemplatetype.Special48AE4MULT18AE),
        };
    }
}