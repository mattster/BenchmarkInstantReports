using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

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

        public static DataSet ProcessAnswerKeyWithMultiAnswers(DataSet dsAnswerKey, string testid)
        {
            TestTemplate template = TestTemplateHandler.getTestTemplate(testid);
            DataTable dtUpdatedKey = dsAnswerKey.Tables[0].Clone();

            // combine the multi-part answers
            for (int i = 0; i < template.MULTPartBFirst; i++)
            {
                DataRow newrow = dtUpdatedKey.NewRow();
                newrow.ItemArray = dsAnswerKey.Tables[0].Rows[i].ItemArray;

                if ((i >= template.MULTPartAFirst - 1) && (i <= template.MULTPartALast - 1))
                {
                    newrow["ANSWER"] = newrow["ANSWER"].ToString() + 
                        dsAnswerKey.Tables[0].Rows[template.MULTPartBFirst + (i - template.MULTPartAFirst + 1) - 1]["ANSWER"].ToString(); 
                }

                dtUpdatedKey.Rows.Add(newrow);
            }

            DataSet dsReturn = new DataSet();
            dsReturn.Tables.Add(dtUpdatedKey);

            return dsReturn;

        }
    }
}