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
        public static List<AnswerKeyItem> getTestAnswerKeyQ(string testID, string campus, int ansKeyIncAmt, string teacher, string period)
        {
            //DataSet dsBothAnsKeysUnsorted = getDistrictAnswerKey(testID, campus);
            List<AnswerKeyItem> distAnsKey = getDistrictAnswerKeyQ(testID, campus);
            //DataSet dsCampusAnsKey = getCampusAnswerKey(testID, campus, ansKeyIncAmt, teacher, period);
            List<AnswerKeyItem> finalData = getCampusAnswerKeyQ(testID, campus, ansKeyIncAmt, teacher, period);
            //DataSet dsBothAnsKeys = new DataSet();
            //List<AnswerKeyItem> finalData = new List<AnswerKeyItem>();

            //dsBothAnsKeysUnsorted.Tables[0].Merge(dsCampusAnsKey.Tables[0], true, MissingSchemaAction.Ignore);
            finalData.InsertRange(0, distAnsKey.ToArray<AnswerKeyItem>());
            //DataTable dtSorted = sortTableByColumn(dsBothAnsKeysUnsorted.Tables[0], "ITEM_NUM");
            finalData.Sort();

            //dsBothAnsKeys.Tables.Add(dtSorted);

            // convert answer key items if necessary
            if (TFTemplateHandler.IsTFTemplate(testID))
                //dsBothAnsKeys = TFTemplateHandler.ProcessAnswerKeyWithTF(dsBothAnsKeys, testID);
                finalData = TFTemplateHandler.ProcessAnswerKeyWithTFQ(finalData, testID);

            if (MultiAnswerTemplateHandler.IsMultiAnswerTemplate(testID))
                //dsBothAnsKeys = MultiAnswerTemplateHandler.ProcessAnswerKeyWithMultiAnswersQ(dsBothAnsKeys, testID);
                finalData = MultiAnswerTemplateHandler.ProcessAnswerKeyWithMultiAnswersQ(finalData, testID);

            //dsBothAnsKeys.Tables[0].AcceptChanges();
            //return dsBothAnsKeys;
            return finalData;
        }


        //**********************************************************************//
        //** return a DataSet with the District (common) portion of the 
        //** answer key for the specified test
        //**
        public static List<AnswerKeyItem> getDistrictAnswerKeyQ(string testID, string campus)
        {
            string qs = Queries.GetDistrictTestAnswerKey.Replace("@testId", testID);
            //DataSet dsDistrictAnsKey = dbIFOracle.getDataRows(qs);
            List<AnswerKeyItem> thisAnswerKey = DBIOWorkaround.ReturnAnswerKey(qs);

            // check for any items that need to be dropped from the test
            int[] distDropList = ExceptionHandler.getItemDropList(testID, campus);
            if (distDropList != null)
            {
                // remove the dropped items from the results
                int curItemNum = new int();
                //for (int rowIdx = 0; rowIdx < dsDistrictAnsKey.Tables[0].Rows.Count; rowIdx++)
                for (int rowIdx = 0; rowIdx < thisAnswerKey.Count; rowIdx++)
                {
                    // if this row's item number is in the drop list, then delete it
                    //curItemNum = (int)(decimal)dsDistrictAnsKey.Tables[0].Rows[rowIdx]["ITEM_NUM"];
                    curItemNum = thisAnswerKey[rowIdx].ItemNum;
                    for (int j = 0; j < distDropList.Length; j++)
                    {
                        if (curItemNum == distDropList[j])
                        {
                            //dsDistrictAnsKey.Tables[0].Rows[rowIdx].Delete();
                            thisAnswerKey.Remove(thisAnswerKey.Find(delegate(AnswerKeyItem aki) { return aki.ItemNum == curItemNum; }));
                            break;
                        }
                    }
                }
            }


            //dsDistrictAnsKey.Tables[0].AcceptChanges();
            //return dsDistrictAnsKey;
            return thisAnswerKey;
        }


        public static List<AnswerKeyItem> getCampusAnswerKeyQ(string testID, string campus, int ansKeyIncAmt, string teacher, string period)
        {
            ansKeyIncAmt = (ansKeyIncAmt == 0) ? Constants.MaxNumTestQuestions : ansKeyIncAmt;

            int minItemNum = 1 + ansKeyIncAmt;
            int maxItemNum = ansKeyIncAmt + Constants.MaxNumTestQuestions;
            int readItemNum = new int();
            //string lblItemNum = "ITEM_NUM";

            string qs = Queries.GetCampusTestAnswerKey.Replace("@testId", testID);
            qs = qs.Replace("@schoolAbbr", campus);
            qs = qs.Replace("@itemNumStart", minItemNum.ToString());
            qs = qs.Replace("@itemNumEnd", maxItemNum.ToString());
            //DataSet dsCampusAnsKey = dbIFOracle.getDataRows(qs);
            List<AnswerKeyItem> thisAnswerKey = DBIOWorkaround.ReturnAnswerKey(qs);

            // adjust the item numbers if necessary


            if (ansKeyIncAmt > 0)
            {
                //DataSet dsCampusAnsKeyModified = new DataSet();
                //for (int i = 0; i < dsCampusAnsKey.Tables[0].Rows.Count; i++)
                for (int i = 0; i < thisAnswerKey.Count; i++)
                {
                    //readItemNum = (int)(decimal)dsCampusAnsKey.Tables[0].Rows[i][lblItemNum];
                    readItemNum = thisAnswerKey[i].ItemNum;
                    //dsCampusAnsKey.Tables[0].Rows[i][lblItemNum] = readItemNum - ansKeyIncAmt;
                    thisAnswerKey[i].ItemNum = readItemNum - ansKeyIncAmt;
                }
            }

            // check for any items that need to be dropped from the test
            int[] campusdroplist = ExceptionHandler.getItemDropList(testID, campus, teacher, period);
            if (campusdroplist != null)
            {
                // remove the dropped items from the results
                int curitemnum = new int();
                //for (int rowidx = 0; rowidx < dsCampusAnsKey.Tables[0].Rows.Count; rowidx++)
                for (int rowidx = 0; rowidx < thisAnswerKey.Count; rowidx++)
                {
                    // if this row's item number is in the drop list, then delete it
                    //curitemnum = (int)(decimal)dsCampusAnsKey.Tables[0].Rows[rowidx][lblItemNum];
                    curitemnum = thisAnswerKey[rowidx].ItemNum;
                    for (int j = 0; j < campusdroplist.Length; j++)
                    {
                        if (curitemnum == campusdroplist[j])
                        {
                            //dsCampusAnsKey.Tables[0].Rows[rowidx].Delete();
                            thisAnswerKey.Remove(thisAnswerKey.Find(delegate(AnswerKeyItem aki) { return aki.ItemNum == curitemnum; }));
                            break;
                        }
                    }
                }
            }

            //dsCampusAnsKey.Tables[0].AcceptChanges();
            //return dsCampusAnsKey;
            return thisAnswerKey;
        }

    }
}