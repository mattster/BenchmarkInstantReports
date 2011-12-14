using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Grading
{
    public class MultiAnswerTemplateHandler
    {
        public static bool IsMultiAnswerTemplate(string testid)
        {
            TestTemplate.TestTemplatetype templatetype = TestTemplateHandler.getTestTemplateType(testid);

            // is test template in the defined MultiAnswer templates?
            foreach (TestTemplate curTestTemplate in TestDefinitionData.MultiAnswerTemplates)
                if (curTestTemplate.TemplateType == templatetype)
                    return true;

            return false;
        }

        public static void ProcessAnswerStringWithMultiAnswers(string[] ansStringArray, string testid, string campus)
        {
            TestTemplate template = TestTemplateHandler.getTestTemplate(testid);

            // combine the answer in Part B with the answer in Part A
            for (int i = 0; i < (template.MULTPartALast - template.MULTPartAFirst); i++)
            {
                ansStringArray[template.MULTPartAFirst + i -1] = ansStringArray[template.MULTPartAFirst + i -1] + 
                    ansStringArray[template.MULTPartBFirst + i - 1];
            }
        }

    }
}