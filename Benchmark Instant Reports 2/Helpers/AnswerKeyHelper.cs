using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Exceptions;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public class AnswerKeyHelper
    {
        /// <summary>
        /// get a collection of AnwerKeyItem's for the specified test and school; includes
        /// the campus-specific answers and the district answers
        /// </summary>
        /// <param name="dataservice">IRepoService access to data</param>
        /// <param name="test">Test object of the test to use</param>
        /// <param name="schoolAbbr">School abbreviation of the school to use</param>
        /// <param name="teacher">teacher name in case of a specific key for a teacher/period</param>
        /// <param name="period">period in case of a specific key for a teacher/period</param>
        /// <returns>AnswerKeyItemData collection of answer key item data</returns>
        public static AnswerKeyItemData GetTestAnswerKeyData(IRepoService dataservice, Test test, string schoolAbbr,
            string teacher, string period)
        {
            AnswerKeyItemData finalData = new AnswerKeyItemData();
            int ansKeyIncAmt = ExceptionHandler.campusAnswerKeyVersionIncrement(test.TestID, schoolAbbr, 
                teacher, period);
            AnswerKeyItemData districtAnsKey = GetDistrictAnswerKeyItems(dataservice, test, schoolAbbr);
            AnswerKeyItemData campusAnsKey = GetCampusAnswerKeyItems(dataservice, test, schoolAbbr, ansKeyIncAmt, teacher, period);

            finalData.Add(districtAnsKey.GetItems().ToList());
            finalData.Add(campusAnsKey.GetItems().ToList());
            finalData.Sort(d => d.ItemNum);

            // convert answer key items if necessary
            if (TFTemplateHandler.IsTFTemplate(test.TestID))
                finalData = TFTemplateHandler.ProcessAnswerKeyWithTFQ(finalData, test.TestID);

            if (MultiAnswerTemplateHandler.IsMultiAnswerTemplate(test.TestID))
                finalData = MultiAnswerTemplateHandler.ProcessAnswerKeyWithMultiAnswersQ(finalData, test.TestID);

            return finalData;
        }



        /// <summary>
        /// get a collection of AnswerKeyItem's for the specified test and school; includes
        /// just the district / common portion of the answer key
        /// </summary>
        /// <param name="dataservice">IRepoService access to data</param>
        /// <param name="test">Test object of the test to use</param>
        /// <param name="schoolAbbr">School abbreviation of the school to use</param>
        /// <returns>AnswerKeyItemData collection of answer key item data</returns>
        public static AnswerKeyItemData GetDistrictAnswerKeyItems(IRepoService dataservice, Test test, 
            string schoolAbbr = "")
        {
            AnswerKeyItemData finalData = new AnswerKeyItemData();

            var answerKey = dataservice.AnswerKeyRepo.FindKeyForTest(test.TestID);
            List<AnswerKey> answerKeyList = answerKey.ToList();

            // check for any items that need to be dropped from the test
            int[] distDropList = ExceptionHandler.getItemDropList(test.TestID, schoolAbbr);
            if (distDropList != null)
            {
                // remove the dropped items from the results
                int curItemNum = new int();
                for (int rowIdx = 0; rowIdx < answerKeyList.Count; rowIdx++)
                {
                    // if this row's item number is in the drop list, then delete it
                    curItemNum = answerKeyList[rowIdx].ItemNum;
                    for (int j = 0; j < distDropList.Length; j++)
                    {
                        if (curItemNum == distDropList[j])
                        {
                            answerKeyList.Remove(answerKeyList.Find(delegate(AnswerKey ak) { return ak.ItemNum == curItemNum; }));
                            break;
                        }
                    }
                }
            }

            finalData = new AnswerKeyItemData(answerKeyList);
            return finalData;
        }


        /// <summary>
        /// get a collection of AnswerKeyItem's for the specified test and school; includes
        /// just the campus-specific portion of the answer key
        /// </summary>
        /// <param name="dataservice">IRepoService access to data</param>
        /// <param name="test">Test object of the test to use</param>
        /// <param name="schoolAbbr">School abbreviation of the school to use</param>
        /// <param name="ansKeyIncAmt">multiplier to find the specific campus answer key</param>
        /// <param name="teacher">teacher name in case of a specific key for a teacher/period</param>
        /// <param name="period">period in case of a specific key for a teacher/period</param>
        /// <returns>AnswerKeyItemData collection of answer key item data</returns>
        public static AnswerKeyItemData GetCampusAnswerKeyItems(IRepoService dataservice, Test test, string schoolAbbr,
            int ansKeyIncAmt, string teacher, string period)
        {
            AnswerKeyItemData finalData = new AnswerKeyItemData();
            var answerKey = dataservice.AnswerKeyCampusRepo.FindKeyForTest(test.TestID, schoolAbbr);
            List<AnswerKeyCampus> answerKeyList = answerKey.ToList();

            ansKeyIncAmt = (ansKeyIncAmt == 0) ? Constants.MaxNumTestQuestions : ansKeyIncAmt;
            int minItemNum = 1 + ansKeyIncAmt;
            int maxItemNum = ansKeyIncAmt + Constants.MaxNumTestQuestions;
            int readItemNum = new int();

            List<AnswerKeyCampus> finalAnswerKeyList = answerKeyList.Where(ak =>
                ak.ItemNum >= minItemNum && ak.ItemNum <= maxItemNum).ToList();

            // adjust the item numbers if necessary
            if (ansKeyIncAmt > 0)
            {
                for (int i = 0; i < finalAnswerKeyList.Count; i++)
                {
                    readItemNum = finalAnswerKeyList[i].ItemNum;
                    finalAnswerKeyList[i].ItemNum = readItemNum - ansKeyIncAmt;
                }
            }

            // check for any items that need to be dropped from the test
            int[] campusdroplist = ExceptionHandler.getItemDropList(test.TestID, schoolAbbr, teacher, period);
            if (campusdroplist != null)
            {
                // remove the dropped items from the results
                int curitemnum = new int();
                for (int rowidx = 0; rowidx < finalAnswerKeyList.Count; rowidx++)
                {
                    // if this row's item number is in the drop list, then delete it
                    curitemnum = finalAnswerKeyList[rowidx].ItemNum;
                    for (int j = 0; j < campusdroplist.Length; j++)
                    {
                        if (curitemnum == campusdroplist[j])
                        {
                            finalAnswerKeyList.Remove(finalAnswerKeyList.Find(delegate(AnswerKeyCampus akc)
                                { return akc.ItemNum == curitemnum; }));
                            break;
                        }
                    }
                }
            }

            finalData = new AnswerKeyItemData(finalAnswerKeyList);
            return finalData;
        }

    }
}