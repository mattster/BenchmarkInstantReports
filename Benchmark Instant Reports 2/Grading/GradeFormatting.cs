using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Infrastructure;

namespace Benchmark_Instant_Reports_2.Grading
{
    public class GradeFormatting
    {
        private static string _dispDataStart = @"<table>";
        private static string _dispDataEnd = @"</table>";

        private static string _dispDataRowStart = @"<tr><td><table>";
        private static string _dispDataRowEnd = @"</table></td></tr>";

        private static string _dispDataRowItemNumsStart = @"<tr>";
        private static string _dispDataRowItemNumsEnd = @"</td>";

        private static string _dispDataRowItemNumStart = @"<td>";
        private static string _dispDataRowItemNumEnd = @"</td>";

        private static string _dispDataRowRespsStart = @"<tr>";
        private static string _dispDataRowRespsEnd = @"</td>";

        private static string _dispDataRowRespStart = @"<td>";
        private static string _dispDataRowRespEnd = @"</td>";


        /// <summary>
        /// creates a string containing html formatting for the Student Summary item data
        /// </summary>
        /// <param name="gradedAnsString">the student's graded answers for the report</param>
        /// <param name="ansKey">the answer key for this test</param>
        /// <returns>a string containing html markup for the Student Summary item data</returns>
        public static string CreateStudentSummaryDisplayField(List<ItemInfo<string>> gradedAnswers, AnswerKeyItemData ansKey)
        {
            //int[] itemNums = ansKey.GetItems().OrderBy(ak => ak.ItemNum).Select(ak => ak.ItemNum).ToArray();

            string finalString = _dispDataStart + _dispDataRowStart;

            // go through each item in the answer key
            int charsOnCurRow = 0;
            int newlength = 0;
            string itemNumString = _dispDataRowItemNumsStart;
            string responseString = _dispDataRowRespsStart;

            foreach (var curAns in ansKey.GetItems().OrderBy(ak => ak.ItemNum))
            {
                string curResponse = gradedAnswers.Where(ga => ga.ItemNum == curAns.ItemNum).First().Info;
                if (curResponse.Length < curAns.ItemNum.ToString().Length)
                    newlength = curAns.ItemNum.ToString().Length;
                else newlength = curResponse.Length;

                if (charsOnCurRow + newlength > Constants.NumColumnsInFormattedLine)
                {
                    // need a new row - end the current row and add it to the finalString
                    itemNumString += _dispDataRowItemNumsEnd;
                    responseString += _dispDataRowRespsEnd;
                    finalString += itemNumString + responseString + _dispDataRowEnd;

                    // begin a new row
                    itemNumString = _dispDataRowItemNumsStart;
                    responseString = _dispDataRowRespsStart;
                    charsOnCurRow = 0;
                }

                itemNumString += _dispDataRowItemNumStart +
                                 curAns.ItemNum.ToString() +
                                 _dispDataRowItemNumEnd;
                responseString += _dispDataRowRespStart +
                                  curResponse +
                                  _dispDataRowRespEnd;
                charsOnCurRow += newlength;
            }

            // end the current row and add it to the finalString
            itemNumString += _dispDataRowItemNumsEnd;
            responseString += _dispDataRowRespsEnd;
            finalString += itemNumString + responseString + _dispDataRowEnd;

            finalString += _dispDataEnd;

            return finalString;
        }


        //
    }
}