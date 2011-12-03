

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