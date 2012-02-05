

namespace Benchmark_Instant_Reports_2.Exceptions
{
    public static class ExceptionHandler
    {
        private static string noAltAnswerValue = "XXXXXXXXXXXXXXXXXXNOALTANSWERXXXXXXXXXXXXXXXXXX";


        public static bool isGriddableNonExactMatch(string testID, string campus, int itemNum)
        {
            foreach (GridNonExactMatch curNonExactMatch in ExceptionData.gridNonExactMatches)
            {
                if (curNonExactMatch.Equals(testID, campus, itemNum))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// returns an answer key multiplier to find the appropriate answer key for
        /// the specified Test, Campus, Teacher and Period
        /// </summary>
        /// <param name="testID">TestID to look up</param>
        /// <param name="campus">School abbreviation of the campus to look up</param>
        /// <param name="teacher">Teacher name to look up</param>
        /// <param name="period">Period to look up</param>
        /// <returns>an integer multiplier for the answer key numbers</returns>
        public static int campusAnswerKeyVersionIncrement(string testID, string campus, string teacher, string period)
        {
            foreach (AnswerKeyIncrement curAdditionalAnswerKey in ExceptionData.additionalAnswerKeys)
            {
                if (curAdditionalAnswerKey.Equals(testID, campus, teacher, period))
                    return curAdditionalAnswerKey.IncrementAmt;
            }

            return 0;

        }


        public static int[] getItemDropList(string testID, string campus)
        {
            foreach (DropList curDropItem in ExceptionData.DropLists)
            {
                if (curDropItem.Equals(testID, campus))
                    return curDropItem.Items;
            }

            return null;

        }


        public static int[] getItemDropList(string testID, string campus, string teacher, string period)
        {
            foreach (DropList curDropItem in ExceptionData.DropLists)
            {
                if (curDropItem.Equals(testID, campus, teacher, period))
                    return curDropItem.Items;
            }

            return null;
        }


        public static string getAlternateAnswer(string testID, int curItemNum, string campus = "")
        {
            foreach (AlternateAnswer curAltAnswer in ExceptionData.alternateAnswers)
            {
                if (curAltAnswer.Equals(testID, campus, curItemNum))
                    return curAltAnswer.AltAnswer;
            }

            return noAltAnswerValue;
        }

    }
}