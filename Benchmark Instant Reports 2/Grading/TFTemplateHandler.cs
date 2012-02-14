using Benchmark_Instant_Reports_2.Infrastructure.DataStruct;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Grading
{
    public static class TFTemplateHandler
    {
        /// <summary>
        /// determines whether a test has true/false items or not
        /// </summary>
        /// <param name="test">Test object of the test to check</param>
        /// <returns>true if true/false items are included in this test, false otherwise</returns>
        public static bool IsTFTemplate(Test test)
        {
            TestTemplate template = TestTemplateHandler.getTestTemplate(test);

            foreach (TestTemplate curTestTemplate in TestDefinitionData.TFTemplates)
                if (curTestTemplate.TemplateType == template.TemplateType)
                    return true;

            return false;
        }


        /// <summary>
        /// convert an answer key to work with true/false items
        /// </summary>
        /// <param name="theAnswerKey">the Answer Key as defined the database</param>
        /// <param name="test">Test object of this answer key</param>
        /// <returns>a new AnswerKeyItemData collection with the converted answer key</returns>
        public static AnswerKeyItemData ProcessAnswerKeyWithTFQ(AnswerKeyItemData theAnswerKey, Test test)
        {
            TestTemplate template = TestTemplateHandler.getTestTemplate(test);
            AnswerKeyItemData finalData = new AnswerKeyItemData();

            // change the "A" to "T" and "B" to "F" for the true/false items
            for (int i = 0; i < theAnswerKey.Count; i++)
            {
                AnswerKeyItem newItem = new AnswerKeyItem();
                //newItem = theAnswerKey.GetItemWhere(delegate(AnswerKeyItem aki) { return aki.ItemNum == i + 1; });
                newItem = theAnswerKey.GetItemWhere(aki => aki.ItemNum == i + 1);

                if ((i >= template.TFFirst - 1) && (i < template.TFLast - 1))
                {
                    if (newItem.Answer == "A")
                        newItem.Answer = "T";
                    else newItem.Answer = "F";
                }

                finalData.Add(newItem);
            }

            return finalData;
        }
    }
}