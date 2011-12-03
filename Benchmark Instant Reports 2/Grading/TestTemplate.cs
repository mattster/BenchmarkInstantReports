using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Grading
{
    public class TestTemplate
    {
        public string Name { get; set; }
        //public int MaxNumQuestions { get; set; }
        public int NumGrids { get; set; }
        public int GridIndexFirst { get; set; }
        public int GridIndexLast { get; set; }
        public TestTemplatetype TemplateType { get; set; }
        
        public enum TestTemplatetype
        {
            StandardMC,
            EOCGrids,
            Grade68Grids,
            Grade45Grids,
            Grade3Grids,
            Other
        }

        public TestTemplate(string name, int numgrids, int gridindexfirst, int gridindexlast, TestTemplatetype templatetype)
        {
            Name = name;
            NumGrids = numgrids;
            GridIndexFirst = gridindexfirst;
            GridIndexLast = gridindexlast;
            TemplateType = templatetype;
        }
    }
}