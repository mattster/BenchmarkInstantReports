using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Grading
{
    public static partial class TestDefinitionData
    {
        public static List<TestTemplate> MultiAnswerTemplates = new List<TestTemplate>
        {
            new TestTemplate("25-MULT-75-AE", 1, 25, 101, 125, TestTemplate.TestTemplatetype.Special25MULT75AE),
            new TestTemplate("48-AE-4-MULT-18-AE", 49, 52, 71, 74, TestTemplate.TestTemplatetype.Special48AE4MULT18AE),
        };
    }
}