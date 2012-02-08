using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Grading
{
    public class MultiAnswerTemplateHandler
    {
        /// <summary>
        /// determines whether a test has multiple-letter response items
        /// </summary>
        /// <param name="test">Test object of the test to check</param>
        /// <returns>true if multiple-letter responses are included in this test, false otherwise</returns>
        public static bool IsMultiAnswerTemplate(Test test)
        {
            TestTemplate template = TestTemplateHandler.getTestTemplate(test);

            foreach (TestTemplate curTestTemplate in TestDefinitionData.MultiAnswerTemplates)
                if (curTestTemplate.TemplateType == template.TemplateType)
                    return true;

            return false;
        }


        /// <summary>
        /// converts a raw answer string bsed on the special setup used for multiple-letter items
        /// </summary>
        /// <param name="ansStringArray">the raw answer string array</param>
        /// <param name="testid">TestID of the test for this answer string</param>
        public static void ProcessAnswerStringWithMultiAnswers(string[] ansStringArray, Test test)
        {
            TestTemplate template = TestTemplateHandler.getTestTemplate(test);

            // combine the answer in Part B with the answer in Part A
            for (int i = 0; i < (template.MULTPartALast - template.MULTPartAFirst); i++)
            {
                ansStringArray[template.MULTPartAFirst + i -1] = ansStringArray[template.MULTPartAFirst + i -1] + 
                    ansStringArray[template.MULTPartBFirst + i - 1];
            }
        }


        /// <summary>
        /// combine data for multiple-letter responses for the Answer Key
        /// </summary>
        /// <param name="theAnswerKey">the Answer Key as defined in the database</param>
        /// <param name="test">Test object of this answer key</param>
        /// <returns>a new AnswerKeyItemData collection with the converted answer key</returns>
        public static AnswerKeyItemData ProcessAnswerKeyWithMultiAnswers(AnswerKeyItemData theAnswerKey, Test test)
        {
            TestTemplate template = TestTemplateHandler.getTestTemplate(test);
            AnswerKeyItemData finalData = new AnswerKeyItemData();

            // combine the multi-part answers
            for (int i = 0; i < template.MULTPartBFirst; i++)
            {
                AnswerKeyItem newItem = new AnswerKeyItem();
                newItem = theAnswerKey.GetItemWhere(delegate(AnswerKeyItem aki) { return aki.ItemNum == i + 1; });

                if ((i >= template.MULTPartAFirst - 1) && (i <= template.MULTPartALast - 1))
                {
                    newItem.Answer = newItem.Answer + theAnswerKey.GetItemWhere(delegate(AnswerKeyItem aki)
                    {
                        return aki.ItemNum == ((template.MULTPartBFirst + (i - template.MULTPartAFirst + 1) - 1) + 1);
                    }
                    ).Answer;
                }

                finalData.Add(newItem);
            }

            return finalData;
        }

    }
}