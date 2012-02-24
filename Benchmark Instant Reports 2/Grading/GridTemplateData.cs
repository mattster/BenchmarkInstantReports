using System.Collections.Generic;

namespace Benchmark_Instant_Reports_2.Grading
{
    public static partial class TestDefinitionData
    {
        /// <summary>
        /// a list of TestTemplate objects representing all the defined templates
        /// that have grids
        /// </summary>
        public static List<TestTemplate> GridTemplates = new List<TestTemplate>
        {
            new TestTemplate("18-AE-FK-2GRID-EOC", 2, 18, 19, TestTemplate.TestTemplatetype.EOCGrids),
            new TestTemplate("19-AE-FK-1GRID-EOC", 1, 19, 19, TestTemplate.TestTemplatetype.EOCGrids),
            new TestTemplate("22-AE-FK-2GRID-G3", 2, 22, 23, TestTemplate.TestTemplatetype.Grade3Grids),
            new TestTemplate("24-AE-FK-1GRID-G45", 1, 24, 24, TestTemplate.TestTemplatetype.Grade45Grids),
            new TestTemplate("24-AE-FK-1GRID-G68", 1, 24, 24, TestTemplate.TestTemplatetype.Grade68Grids),
            new TestTemplate("28-AE-FK-2GRID-G68", 2, 28, 29, TestTemplate.TestTemplatetype.Grade68Grids),
            new TestTemplate("29-AE-FK-1GRID-G68", 1, 29, 29, TestTemplate.TestTemplatetype.Grade68Grids),
            new TestTemplate("34-AE-FK-2GRID-G45", 2, 34, 35, TestTemplate.TestTemplatetype.Grade45Grids),
            new TestTemplate("36-AE-FK-1GRID-G3", 1, 36, 36, TestTemplate.TestTemplatetype.Grade3Grids),
            new TestTemplate("36-AE-FK-2GRID-G45", 2, 36, 37, TestTemplate.TestTemplatetype.Grade45Grids),
            new TestTemplate("37-AE-FK-1GRID-G45", 1, 37, 37, TestTemplate.TestTemplatetype.Grade3Grids),
            new TestTemplate("38-AE-FK-2GRID-G68", 2, 38, 39, TestTemplate.TestTemplatetype.Grade68Grids),
            new TestTemplate("39-AE-FK-1GRID-G45", 1, 39, 39, TestTemplate.TestTemplatetype.Grade45Grids),
            new TestTemplate("41-AE-FK-1GRID-G68", 1, 41, 41, TestTemplate.TestTemplatetype.Grade68Grids),
            new TestTemplate("43-AE-FK-3GRID-G3", 3, 43, 45, TestTemplate.TestTemplatetype.Grade3Grids),
            new TestTemplate("45-AE-FK-3GRID-G45", 3, 45, 47, TestTemplate.TestTemplatetype.Grade45Grids),
            new TestTemplate("46-AE-FK-4GRID-G45", 4, 46, 49, TestTemplate.TestTemplatetype.Grade45Grids),
            new TestTemplate("60-AE-FK-3GRID-EOC", 3, 60, 62, TestTemplate.TestTemplatetype.EOCGrids),
            new TestTemplate("60-AE-FK-3GRID-G68", 3, 60, 62, TestTemplate.TestTemplatetype.Grade68Grids),
            new TestTemplate("60-AE-3GRID-EOC", 3, 60, 62, TestTemplate.TestTemplatetype.EOCGrids),
            new TestTemplate("60-AE-3GRID-G68", 3, 60, 62, TestTemplate.TestTemplatetype.Grade68Grids),
            new TestTemplate("4GRID-G68", 4, 0, 3, TestTemplate.TestTemplatetype.Grade68Grids),
        };
    }
}