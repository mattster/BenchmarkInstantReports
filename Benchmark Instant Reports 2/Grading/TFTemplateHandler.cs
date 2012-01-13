using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
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


        public static DataSet ProcessAnswerKeyWithTF(DataSet dsAnswerKey, string testid)
        {
            TestTemplate template = TestTemplateHandler.getTestTemplate(testid);
            DataTable dtUpdatedKey = dsAnswerKey.Tables[0].Clone();

            // change the "A" to "T" and "B" to "F" for the true/false items
            for (int i = 0; i < dsAnswerKey.Tables[0].Rows.Count; i++)
            {
                DataRow newrow = dtUpdatedKey.NewRow();
                newrow.ItemArray = dsAnswerKey.Tables[0].Rows[i].ItemArray;

                if ((i >= template.TFFirst - 1) && (i < template.TFLast - 1))
                {
                    if (newrow["ANSWER"].ToString() == "A")
                        newrow["ANSWER"] = "T";
                    else newrow["ANSWER"] = "F";
                }

                dtUpdatedKey.Rows.Add(newrow);
            }

            DataSet dsReturn = new DataSet();
            dsReturn.Tables.Add(dtUpdatedKey);

            return dsReturn;
        }

        public static List<AnswerKeyItem> ProcessAnswerKeyWithTFQ(List<AnswerKeyItem> theAnswerKey, string testid)
        {
            TestTemplate template = TestTemplateHandler.getTestTemplate(testid);
            //DataTable dtUpdatedKey = dsAnswerKey.Tables[0].Clone();
            List<AnswerKeyItem> finalData = new List<AnswerKeyItem>();

            // change the "A" to "T" and "B" to "F" for the true/false items
            //for (int i = 0; i < dsAnswerKey.Tables[0].Rows.Count; i++)
            for (int i = 0; i < theAnswerKey.Count; i++)
            {
                //DataRow newrow = dtUpdatedKey.NewRow();
                AnswerKeyItem newItem = new AnswerKeyItem();
                //newrow.ItemArray = dsAnswerKey.Tables[0].Rows[i].ItemArray;
                newItem = theAnswerKey.Find(delegate(AnswerKeyItem aki) { return aki.ItemNum == i + 1; });

                if ((i >= template.TFFirst - 1) && (i < template.TFLast - 1))
                {
                    //if (newrow["ANSWER"].ToString() == "A")
                    if (newItem.Answer == "A")
                        //newrow["ANSWER"] = "T";
                        newItem.Answer = "T";
                    //else newrow["ANSWER"] = "F";
                    else newItem.Answer = "F";
                }

                //dtUpdatedKey.Rows.Add(newrow);
                finalData.Add(newItem);
            }

            //DataSet dsReturn = new DataSet();
            //dsReturn.Tables.Add(dtUpdatedKey);

            //return dsReturn;
            return finalData;
        }
    }
}