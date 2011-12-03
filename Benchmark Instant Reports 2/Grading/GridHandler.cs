using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.References;
using System.Data;
using Benchmark_Instant_Reports_2.Exceptions;

namespace Benchmark_Instant_Reports_2.Grading
{
    public static class GridHandler
    {

        public static bool isGriddable(string testid)
        {
            TestTemplate.TestTemplatetype templatetype = TestTemplateHandler.getTestTemplateType(testid);

            // is test template in the defined griddable templates?
            foreach (TestTemplate curTestTemplate in TestDefinitionData.GridTemplates)
            {
                if (curTestTemplate.TemplateType == templatetype)
                    return true;
            }

            return false;
        }


        public static void ProcessAnswerStringWithGrids(string[] ansStringArray, string testid, string campus)
        {
            TestTemplate template = TestTemplateHandler.getTestTemplate(testid);

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
                if (ExceptionHandler.isGriddableNonExactMatch(testid, campus, i + 1))
                {
                    while (ansStringArray[i].Contains(".") && ansStringArray[i].EndsWith("0"))
                        ansStringArray[i] = ansStringArray[i].Substring(0, ansStringArray[i].Length - 1);
                }
            }

            // if grid is identified as a non-exact match, remove trailing decimals
            for (int i = template.GridIndexFirst; i <= template.GridIndexLast && i < ansStringArray.Length; i++)
            {
                if (ExceptionHandler.isGriddableNonExactMatch(testid, campus, i + 1))
                {
                    while (ansStringArray[i].EndsWith("."))
                        ansStringArray[i] = ansStringArray[i].Substring(0, ansStringArray[i].Length - 1);
                }
            }

            return;
        }



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