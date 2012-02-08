using System.Collections.Generic;

namespace Benchmark_Instant_Reports_2.Grading
{
    public static partial class TestDefinitionData
    {
        /// <summary>
        /// a list of TestTemplate objects representing all the defined templates
        /// that have True/False items
        /// </summary>
        public static List<TestTemplate> TFTemplates = new List<TestTemplate>
        {
            new TestTemplate("70-AE-10-TF", 71, 80, TestTemplate.TestTemplatetype.Special70AE10TF),
            new TestTemplate("40-AE-10-TF-30-AE", 41, 50, TestTemplate.TestTemplatetype.Special40AE10TF30AE),
            new TestTemplate("21-AE-18-TF-31-AE", 22, 40, TestTemplate.TestTemplatetype.Special21AE18TF31AE),
            new TestTemplate("30-AE-15-TF-20-AE", 31, 45, TestTemplate.TestTemplatetype.Special30AE15TF20AE)            
        };
    }
}