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

            for (int i = 0; i < itemNums.Length; i++)
            {
                // write this one
                if (itemNums[i].ToString().Length <= 2)
                {
                    titleString[rowIndex] += string.Format("{0,2} ", itemNums[i]);
                    resultsString[rowIndex] += string.Format("{0,2} ",
                        (gradedAnswers.Where(ga => ga.ItemNum == itemNums[i]).First().Info) != "" 
                        ? gradedAnswers.Where(ga => ga.ItemNum == itemNums[i]).First().Info 
                        : "_");
                }
                else
                {
                    titleString[rowIndex] += string.Format("{0,3} ", itemNums[i]);
                    resultsString[rowIndex] += string.Format("{0,3} ",
                        (gradedAnswers.Where(ga => ga.ItemNum == itemNums[i]).First().Info) != ""
                        ? gradedAnswers.Where(ga => ga.ItemNum == itemNums[i]).First().Info
                        : "_");
                }


                // check if the next one will fit on this row
                if ((i + 1) < itemNums.Length)
                {
                    if ((Constants.NumColumnsInFormattedLine -
                        (resultsString[rowIndex].Length + itemNums[i + 1].ToString().Length)) < 2)
                    {
                        // next item will not fit; delete extra space at end and go to next row
                        titleString[rowIndex] = titleString[rowIndex].Substring(0, titleString[rowIndex].Length - 1);
                        resultsString[rowIndex] = resultsString[rowIndex].Substring(0, resultsString[rowIndex].Length - 1);
                        rowIndex++;
                    }
                }
            }

            // put the rows together, insert newlines
            for (int j = 0; j <= rowIndex; j++)
                formattedString += String.Format("{0}\n{1}\n", titleString[j], resultsString[j]);

            // remove the last newline - we don't need it
            formattedString = formattedString.Substring(0, formattedString.Length - 1);

            return formattedString;
        }
    }
}