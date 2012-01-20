using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Exceptions;

namespace Benchmark_Instant_Reports_2.Grading
{
    public class AnswerKey
    {
        public static List<AnswerKeyItem> getTestAnswerKey(string testID, string campus, int ansKeyIncAmt, string teacher, string period)
        {
            List<AnswerKeyItem> distAnsKey = getDistrictAnswerKey(testID, campus);
            List<AnswerKeyItem> finalData = getCampusAnswerKey(testID, campus, ansKeyIncAmt, teacher, period);

            finalData.InsertRange(0, distAnsKey.ToArray<AnswerKeyItem>());
            finalData.Sort();

            // convert answer key items if necessary
            if (TFTemplateHandler.IsTFTemplate(testID))
                finalData = TFTemplateHandler.ProcessAnswerKeyWithTFQ(finalData, testID);

            if (MultiAnswerTemplateHandler.IsMultiAnswerTemplate(testID))
                finalData = MultiAnswerTemplateHandler.ProcessAnswerKeyWithMultiAnswersQ(finalData, testID);

            return finalData;
        }

        

        //**********************************************************************//
        //** return a DataSet with the District (common) portion of the 
        //** answer key for the specified test
        //**
        public static List<AnswerKeyItem> getDistrictAnswerKey(string testID, string campus = "")
        {
            string qs = Queries.GetDistrictTestAnswerKey.Replace("@testId", testID);
            List<AnswerKeyItem> thisAnswerKey = DBIOWorkaround.ReturnAnswerKey(qs);

            // check for any items that need to be dropped from the test
            int[] distDropList = ExceptionHandler.getItemDropList(testID, campus);
            if (distDropList != null)
            {
                // remove the dropped items from the results
                int curItemNum = new int();
                for (int rowIdx = 0; rowIdx < thisAnswerKey.Count; rowIdx++)
                {
                    // if this row's item number is in the drop list, then delete it
                    curItemNum = thisAnswerKey[rowIdx].ItemNum;
                    for (int j = 0; j < distDropList.Length; j++)
                    {
                        if (curItemNum == distDropList[j])
                        {
                            thisAnswerKey.Remove(thisAnswerKey.Find(delegate(AnswerKeyItem aki) { return aki.ItemNum == curItemNum; }));
                            break;
                        }
                    }
                }
            }

            return thisAnswerKey;
        }


        public static List<AnswerKeyItem> getCampusAnswerKey(string testID, string campus, int ansKeyIncAmt, string teacher, string period)
        {
            ansKeyIncAmt = (ansKeyIncAmt == 0) ? Constants.MaxNumTestQuestions : ansKeyIncAmt;

            int minItemNum = 1 + ansKeyIncAmt;
            int maxItemNum = ansKeyIncAmt + Constants.MaxNumTestQuestions;
            int readItemNum = new int();

            string qs = Queries.GetCampusTestAnswerKey.Replace("@testId", testID);
            qs = qs.Replace("@schoolAbbr", campus);
            qs = qs.Replace("@itemNumStart", minItemNum.ToString());
            qs = qs.Replace("@itemNumEnd", maxItemNum.ToString());
            List<AnswerKeyItem> thisAnswerKey = DBIOWorkaround.ReturnAnswerKey(qs);

            // adjust the item numbers if necessary
            if (ansKeyIncAmt > 0)
            {
                for (int i = 0; i < thisAnswerKey.Count; i++)
                {
                    readItemNum = thisAnswerKey[i].ItemNum;
                    thisAnswerKey[i].ItemNum = readItemNum - ansKeyIncAmt;
                }
            }

            // check for any items that need to be dropped from the test
            int[] campusdroplist = ExceptionHandler.getItemDropList(testID, campus, teacher, period);
            if (campusdroplist != null)
            {
                // remove the dropped items from the results
                int curitemnum = new int();
                for (int rowidx = 0; rowidx < thisAnswerKey.Count; rowidx++)
                {
                    // if this row's item number is in the drop list, then delete it
                    curitemnum = thisAnswerKey[rowidx].ItemNum;
                    for (int j = 0; j < campusdroplist.Length; j++)
                    {
                        if (curitemnum == campusdroplist[j])
                        {
                            thisAnswerKey.Remove(thisAnswerKey.Find(delegate(AnswerKeyItem aki) { return aki.ItemNum == curitemnum; }));
                            break;
                        }
                    }
                }
            }

            return thisAnswerKey;
        }

    }
}