using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;

namespace Benchmark_Instant_Reports_2.Grading
{
    public static class TFTemplateHandler
    {
        public static bool IsTFTemplate(string testid)
        {
            TestTemplate.TestTemplatetype templatetype = TestTemplateHandler.getTestTemplateType(testid);

            // is test template in the defined TF templates?
            foreach (TestTemplate curTestTemplate in TestDefinitionData.TFTemplates)
                if (curTestTemplate.TemplateType == templatetype)
                    return true;

            return false;
        }



        public static AnswerKeyItemData ProcessAnswerKeyWithTFQ(AnswerKeyItemData theAnswerKey, string testid)
        {
            TestTemplate template = TestTemplateHandler.getTestTemplate(testid);
            AnswerKeyItemData finalData = new AnswerKeyItemData();

            // change the "A" to "T" and "B" to "F" for the true/false items
            for (int i = 0; i < theAnswerKey.Count; i++)
            {
                AnswerKeyItem newItem = new AnswerKeyItem();
                newItem = theAnswerKey.GetItemWhere(delegate(AnswerKeyItem aki) { return aki.ItemNum == i + 1; });

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