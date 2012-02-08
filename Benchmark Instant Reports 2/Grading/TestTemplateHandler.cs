using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Grading
{
    public static class TestTemplateHandler
    {
        /// <summary>
        /// returns a TestTemplate object based on the template used in a test
        /// </summary>
        /// <param name="test">Test object to check</param>
        /// <returns>TestTemplate object instance of the template used on the test</returns>
        public static TestTemplate getTestTemplate(Test test)
        {
            // check for griddable types
            foreach (TestTemplate curTestTemplate in TestDefinitionData.GridTemplates)
                if (curTestTemplate.Name == test.TestTemplate)
                    return curTestTemplate;

            // check for True-False types
            foreach (TestTemplate curTestTemplate in TestDefinitionData.TFTemplates)
                if (curTestTemplate.Name == test.TestTemplate)
                    return curTestTemplate;

            // check for MultiAnswer types
            foreach (TestTemplate curTestTemplate in TestDefinitionData.MultiAnswerTemplates)
                if (curTestTemplate.Name == test.TestTemplate)
                    return curTestTemplate;

            // check for other custom types


            // default type is standard Multiple Choice
            return new TestTemplate(test.TestTemplate, TestTemplate.TestTemplatetype.StandardMC);
        }

    }
}