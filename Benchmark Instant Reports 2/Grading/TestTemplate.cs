
namespace Benchmark_Instant_Reports_2.Grading
{
    /// <summary>
    /// data structure containing information about a Test Template
    /// </summary>
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

        
        /// <summary>
        /// Constructor for a standard test template
        /// </summary>
        /// <param name="name">name of the template</param>
        /// <param name="templatetype">TestTemplatetype type of the template</param>
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


        /// <summary>
        /// Contructor for a test template with grids
        /// </summary>
        /// <param name="name">name of the template</param>
        /// <param name="numgrids">number of griddable items on the template</param>
        /// <param name="gridindexfirst">index (0-based) of the first griddable answer</param>
        /// <param name="gridindexlast">index (0-based) of the last griddable answer</param>
        /// <param name="templatetype">TestTemplatetype of the template</param>
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


        /// <summary>
        /// Constructor for a test template with True/False type items
        /// </summary>
        /// <param name="name">name of the template</param>
        /// <param name="tffirst">index (0-based) of the first T/F item</param>
        /// <param name="tflast">index (0-based) of the last T/F item</param>
        /// <param name="templatetype">TestTemplatetype of the template</param>
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


        /// <summary>
        /// Constructor for a test template with multiple-letter items
        /// </summary>
        /// <param name="name">name of the template</param>
        /// <param name="multpartafirst">index (0-based) of the first item of the first part of a multiple-letter item</param>
        /// <param name="multpartalast">index (0-based) of the last item of the first part of a multiple-letter item</param>
        /// <param name="multpartbfirst">index (0-based) of the first item of the second part of a multiple-letter item</param>
        /// <param name="multpartblast">index (0-based) of the last item of the second part of a multiple-letter item</param>
        /// <param name="templatetype">TestTemplatetype of the template</param>
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