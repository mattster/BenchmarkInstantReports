using System;
using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure;
using Benchmark_Instant_Reports_2.Infrastructure.DataStruct;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Grading
{
    /// <summary>
    /// methods to perform proper formatting for displayed information, primarily the 
    /// info displayed in the Student Summary report
    /// </summary>
    public class GradeFormatting
    {
        private static string linestarthtmlthing = @"<span style=""color:white"">.</span>";


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






        /// <summary>
        /// Get the actual displayed length of a string that contains
        /// the html code "&nbsp;" for spaces
        /// </summary>
        /// <param name="str">the string to use</param>
        /// <returns>the legnth that will be displayed, counting &nbsp; as 1 character</returns>
        private static int GetDisplayLength(string str)
        {
            //str = str.Replace("&nbsp;", " ");
            return str.Replace("&nbsp;", " ").Replace(linestarthtmlthing, "").Length;
        }


        /// <summary>
        /// simple utility to create a string of repeating characters or 
        /// groups of characters
        /// </summary>
        /// <param name="s">the string to repeat</param>
        /// <param name="n">the number of times to repeat</param>
        /// <returns>a string consisting of n repetitions of the string s</returns>
        private static string RepeatStr(string s, int n)
        {
            if (n == 0) return "";
            return String.Concat(Enumerable.Repeat(s, n));
        }
    }
}