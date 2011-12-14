using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.References;
using System.Data;

namespace Benchmark_Instant_Reports_2.Grading
{
    public static class TestTemplateHandler
    {
        public static TestTemplate.TestTemplatetype getTestTemplateType(string testid)
        {
            string templateName = getTestTemplateName(testid);

            // check for griddable types
            foreach (TestTemplate curTestTemplate in TestDefinitionData.GridTemplates)
                if (curTestTemplate.Name == templateName)
                    return curTestTemplate.TemplateType;

            // check for True-False types
            foreach (TestTemplate curTestTemplate in TestDefinitionData.TFTemplates)
                if (curTestTemplate.Name == templateName)
                    return curTestTemplate.TemplateType;

            // check for MultiAnswer types
            foreach (TestTemplate curTestTemplate in TestDefinitionData.MultiAnswerTemplates)
                if (curTestTemplate.Name == templateName)
                    return curTestTemplate.TemplateType;

            // check for other custom types

            // default type is standard Multiple Choice
            return TestTemplate.TestTemplatetype.StandardMC;
        }


        public static TestTemplate getTestTemplate(string testid)
        {
            string templateName = getTestTemplateName(testid);
            
            // check for griddable types
            foreach (TestTemplate curTestTemplate in TestDefinitionData.GridTemplates)
                if (curTestTemplate.Name == templateName)
                    return curTestTemplate;

            // check for True-False types
            foreach (TestTemplate curTestTemplate in TestDefinitionData.TFTemplates)
                if (curTestTemplate.Name == templateName)
                    return curTestTemplate;

            // check for MultiAnswer types
            foreach (TestTemplate curTestTemplate in TestDefinitionData.MultiAnswerTemplates)
                if (curTestTemplate.Name == templateName)
                    return curTestTemplate;

            // check for other custom types

            // default type is standard Multiple Choice
            return new TestTemplate(templateName, TestTemplate.TestTemplatetype.StandardMC);
        }



        private static string getTestTemplateName(string testid)
        {
            string qs = Queries.queryGetTestTemplate.Replace("@testId", testid);
            DataSet ds = dbIFOracle.getDataRows(qs);

            return ds.Tables[0].Rows[0][0].ToString();
        }

    }
}