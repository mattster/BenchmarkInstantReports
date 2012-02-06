using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;

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

        public static void ProcessAnswerStringWithMultiAnswers(string[] ansStringArray, string testid)
        {
            TestTemplate template = TestTemplateHandler.getTestTemplate(testid);

            // combine the answer in Part B with the answer in Part A
            for (int i = 0; i < (template.MULTPartALast - template.MULTPartAFirst); i++)
            {
                ansStringArray[template.MULTPartAFirst + i -1] = ansStringArray[template.MULTPartAFirst + i -1] + 
                    ansStringArray[template.MULTPartBFirst + i - 1];
            }
        }


        public static AnswerKeyItemData ProcessAnswerKeyWithMultiAnswersQ(AnswerKeyItemData theAnswerKey, string testid)
        {
            TestTemplate template = TestTemplateHandler.getTestTemplate(testid);
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