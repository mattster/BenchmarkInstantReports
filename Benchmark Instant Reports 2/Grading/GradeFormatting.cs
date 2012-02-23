using System;
using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure;
using Benchmark_Instant_Reports_2.Infrastructure.DataStruct;
using Benchmark_Instant_Reports_2.References;

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

        private static string linestarthtmlthing = @"<span style=""color:white"">.</span>";

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



        /// <summary>
        /// creates a formatted answer string containing item numbers and student responses
        /// for use in the Student Summary report
        /// </summary>
        /// <param name="gradedAnsString">a comma-separated string of just student responses:
        /// CorrectAnswerIndicator if student's response was correct, the actual response otherwise</param>
        /// <param name="ansKey">the answer key for this test</param>
        /// <returns>a formatted string with newlines for displaying in the Student Summary report</returns>
        public static string createStudentGradedAnsString(List<ItemInfo<string>> gradedAnswers, AnswerKeyItemData ansKey)
        {
            string[] resultsString = new string[Constants.MaxFormattedAnsGroups];
            string[] titleString = new string[Constants.MaxFormattedAnsGroups];
            string formattedString = "";
            int rowIndex = 0;

            int[] itemNums = ansKey.GetItems().Select(ak => ak.ItemNum).ToArray();
            Array.Sort(itemNums);

            for (int itemNumIdx = 0; itemNumIdx < itemNums.Length; itemNumIdx++)
            {
                // write this one
                int curItemNum = itemNums[itemNumIdx];
                string curItemNumStr = curItemNum.ToString();
                string curAnswer = gradedAnswers.Where(ga => ga.ItemNum == curItemNum).First().Info;

                // do this because the reportviewer doesn't render an initial first &nbsp;
                if (resultsString[rowIndex] == null)
                {
                    titleString[rowIndex] = linestarthtmlthing;
                    resultsString[rowIndex] = linestarthtmlthing;
                }

                // add leading spaces to right-justify if needed
                int cellLength = Math.Max(2, Math.Max(curItemNumStr.Length, curAnswer.Length));
                titleString[rowIndex] += 
                    RepeatStr("&nbsp;", cellLength - curItemNumStr.Length);
                resultsString[rowIndex] +=
                    RepeatStr("&nbsp;", cellLength - curAnswer.Length);


                // write the item num & answer
                titleString[rowIndex] += curItemNumStr;
                resultsString[rowIndex] += curAnswer;

                // add separator space
                titleString[rowIndex] += "&nbsp;";
                resultsString[rowIndex] += "&nbsp;";

                // check if the next one will fit on this row
                if ((itemNumIdx + 1) < itemNums.Length)
                {
                    // check if item num will fit
                    string nextItemNumStr = itemNums[itemNumIdx + 1].ToString();
                    string nextAnswer = gradedAnswers.Where(ga => ga.ItemNum == itemNums[itemNumIdx + 1]).First().Info;
                    if (GetDisplayLength(titleString[rowIndex]) + nextItemNumStr.Length >= Constants.NumColumnsInFormattedLine ||
                        GetDisplayLength(resultsString[rowIndex]) + nextAnswer.Length >= Constants.NumColumnsInFormattedLine)
                    {
                        // next item will not fit; go to next row
                        rowIndex++;
                    }
                }
            }

            // put the rows together, insert newlines
            for (int j = 0; j <= rowIndex; j++)
                formattedString += titleString[j] + "<br />" + resultsString[j] + "<br />";

            return formattedString;
        }




        private static int GetDisplayLength(string str)
        {
            //str = str.Replace("&nbsp;", " ");
            return str.Replace("&nbsp;", " ").Replace(linestarthtmlthing, "").Length;
        }


        private static string RepeatStr(string s, int n)
        {
            if (n == 0) return "";
            return String.Concat(Enumerable.Repeat(s, n));
        }
    }
}