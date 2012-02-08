using Benchmark_Instant_Reports_2.Exceptions;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Grading
{
    public static class GridHandler
    {
        /// <summary>
        /// determines whether a test has griddable items or not
        /// </summary>
        /// <param name="test">Test object of the test to check</param>
        /// <returns>true if griddable items are included in this test, false otherwise</returns>
        public static bool isGriddable(Test test)
        {
            TestTemplate template = TestTemplateHandler.getTestTemplate(test);

            foreach (TestTemplate curTestTemplate in TestDefinitionData.GridTemplates)
            {
                if (curTestTemplate.TemplateType == template.TemplateType)
                    return true;
            }

            return false;
        }


        /// <summary>
        /// converts a raw answer string based on the special setup used for griddable items
        /// </summary>
        /// <param name="ansStringArray">the raw answer string array</param>
        /// <param name="test">Test object of the test for this answer string</param>
        /// <param name="schoolAbbr">School abbreviation of the school for this answer string; 
        /// defaults to ""</param>
        public static void ProcessAnswerStringWithGrids(string[] ansStringArray, Test test, string schoolAbbr="")
        {
            TestTemplate template = TestTemplateHandler.getTestTemplate(test);

            // do specific conversions / combinations for each grid type
            gridCombinations(ansStringArray, template);

            // remove leading zeroes from all grids
            for (int i = template.GridIndexFirst; i <= template.GridIndexLast && i < ansStringArray.Length; i++)
            {
                while (ansStringArray[i] != "" && ansStringArray[i][0] == '0')
                    ansStringArray[i] = ansStringArray[i].Substring(1, ansStringArray[i].Length - 1);
            }

            // if grid is identified as a non-exact match, remove trailing stuff
            for (int i = template.GridIndexFirst; i <= template.GridIndexLast && i < ansStringArray.Length; i++)
            {
                if (ExceptionHandler.isGriddableNonExactMatch(test.TestID, i + 1, schoolAbbr))
                {
                    while (ansStringArray[i].Contains(".") && ansStringArray[i].EndsWith("0"))
                        ansStringArray[i] = ansStringArray[i].Substring(0, ansStringArray[i].Length - 1);
                }
            }

            // if grid is identified as a non-exact match, remove trailing decimals
            for (int i = template.GridIndexFirst; i <= template.GridIndexLast && i < ansStringArray.Length; i++)
            {
                if (ExceptionHandler.isGriddableNonExactMatch(test.TestID, i + 1, schoolAbbr))
                {
                    while (ansStringArray[i].EndsWith("."))
                        ansStringArray[i] = ansStringArray[i].Substring(0, ansStringArray[i].Length - 1);
                }
            }

            return;
        }





        /// <summary>
        /// combine data for griddable items from the raw answer string based on the type of grid
        /// </summary>
        /// <param name="ansStringArray">the array of raw student responses</param>
        /// <param name="template">specific TestTemplate of the test</param>
        private static void gridCombinations(string[] ansStringArray, TestTemplate template)
        {
            if (template.TemplateType == TestTemplate.TestTemplatetype.EOCGrids)
            {
                // for each grid, add the sign only if it is negative
                for (int i = 0; i < template.NumGrids; i++)
                {
                    if (ansStringArray.Length > template.GridIndexLast + i + 1)
                        if (ansStringArray[template.GridIndexLast + i + 1] == "-")
                            ansStringArray[template.GridIndexFirst + i] = 
                                ansStringArray[template.GridIndexLast + i + 1] + ansStringArray[template.GridIndexFirst + i];
                }
            }
            else if (template.TemplateType == TestTemplate.TestTemplatetype.Grade68Grids)
            {
                // for each grid, combine the question pairs with a decimal in beteween
                for (int i = 0; i < template.NumGrids; i++)
                {
                    if (ansStringArray.Length > template.GridIndexLast + i + 1)
                        if (ansStringArray[template.GridIndexLast + i + 1] != "" &&
                            ansStringArray[template.GridIndexLast + i + 1] != " ")
                        {
                            ansStringArray[template.GridIndexFirst + i] = ansStringArray[template.GridIndexFirst + i] + "." +
                                ansStringArray[template.GridIndexLast + i + 1];
                        }
                }
            }

            return;
        }


    }
}