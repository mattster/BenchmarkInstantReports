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
        public int TFFirst { get; set; }
        public int TFLast { get; set; }
        public int MULTPartAFirst { get; set; }
        public int MULTPartALast { get; set; }
        public int MULTPartBFirst { get; set; }
        public int MULTPartBLast { get; set; }
        public TestTemplatetype TemplateType { get; set; }
        
        public enum TestTemplatetype
        {
            StandardMC,
            EOCGrids,
            Grade68Grids,
            Grade45Grids,
            Grade3Grids,
            Special48AE4MULT18AE,
            Special25MULT75AE,
            Special70AE10TF,
            Special40AE10TF30AE,
            Special21AE18TF31AE,
            Special30AE15TF20AE,
            Other
        }

        // standard template
        public TestTemplate(string name, TestTemplatetype templatetype)
        {
            Name = name;
            TemplateType = templatetype;

            TFFirst = 0;
            TFLast = 0;
            MULTPartAFirst = 0;
            MULTPartALast = 0;
            MULTPartBFirst = 0;
            MULTPartBLast = 0;
            NumGrids = 0;
            GridIndexFirst = 0;
            GridIndexLast = 0;
        }

        // tempaltes with grids
        public TestTemplate(string name, int numgrids, int gridindexfirst, int gridindexlast, TestTemplatetype templatetype)
        {
            Name = name;
            NumGrids = numgrids;
            GridIndexFirst = gridindexfirst;
            GridIndexLast = gridindexlast;
            TemplateType = templatetype;

            TFFirst = 0;
            TFLast = 0;
            MULTPartAFirst = 0;
            MULTPartALast = 0;
            MULTPartBFirst = 0;
            MULTPartBLast = 0;
        }

        // templates with a section of true / false items
        public TestTemplate(string name, int tffirst, int tflast, TestTemplatetype templatetype)
        {
            Name = name;
            TFFirst = tffirst;
            TFLast = tflast;
            TemplateType = templatetype;

            NumGrids = 0;
            GridIndexFirst = 0;
            GridIndexLast = 0;
            MULTPartAFirst = 0;
            MULTPartALast = 0;
            MULTPartBFirst = 0;
            MULTPartBLast = 0;
        }

        // tempaltes with a section of multiple responses
        public TestTemplate(string name, int multpartafirst, int multpartalast, int multpartbfirst, int multpartblast, TestTemplatetype templatetype)
        {
            Name = name;
            MULTPartAFirst = multpartafirst;
            MULTPartALast = multpartalast;
            MULTPartBFirst = multpartbfirst;
            MULTPartBLast = multpartblast;
            TemplateType = templatetype;

            NumGrids = 0;
            GridIndexFirst = 0;
            GridIndexLast = 0;
            TFFirst = 0;
            TFLast = 0;
        }
    }
}