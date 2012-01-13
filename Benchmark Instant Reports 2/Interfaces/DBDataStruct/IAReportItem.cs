using Benchmark_Instant_Reports_2.Infrastructure;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    /// <summary>
    /// Item Analysis report data item
    /// </summary>
    public class IAReportItem
    {
        public string Campus { get; set; }                  // key
        public string TestID { get; set; }                  // key
        public string Teacher { get; set; }                 // key
        public string Period { get; set; }                  // key
        public int ItemNum { get; set; }                    // key
        public decimal PctCorrect { get; set; }
        public int NumCorrect { get; set; }
        public int NumTotal { get; set; }
        public int NumA { get; set; }
        public int NumB { get; set; }
        public int NumC { get; set; }
        public int NumD { get; set; }
        public int NumE { get; set; }
        public int NumF { get; set; }
        public int NumG { get; set; }
        public int NumH { get; set; }
        public int NumJ { get; set; }
        public int NumK { get; set; }
        public string Answer { get; set; }
        public int Objective { get; set; }
        public string TEKS { get; set; }

        /// <summary>
        /// default constructor - blank / empty / zero values
        /// </summary>
        public IAReportItem()
        {
            Campus = "";
            TestID = "";
            Teacher = "";
            Period = "";
            ItemNum = 0;
            PctCorrect = 0;
            NumCorrect = 0;
            NumTotal = 0;
            NumA = 0;
            NumB = 0;
            NumC = 0;
            NumD = 0;
            NumE = 0;
            NumF = 0;
            NumG = 0;
            NumH = 0;
            NumJ = 0;
            NumK = 0;
            Answer = "";
            Objective = 0;
            TEKS = "";
        }

        /// <summary>
        /// constructor to create a blank item with just the key elements
        /// </summary>
        /// <param name="campus">Campus ID</param>
        /// <param name="testid">Test ID</param>
        /// <param name="teacher">Teacher Name</param>
        /// <param name="period">Period as a string</param>
        /// <param name="itemnum">test item number</param>
        public IAReportItem(string campus, string testid, string teacher, string period, int itemnum)
        {
            Campus = campus;
            TestID = testid;
            Teacher = teacher;
            Period = period;
            ItemNum = itemnum;
            PctCorrect = 0;
            NumCorrect = 0;
            NumTotal = 0;
            NumA = 0;
            NumB = 0;
            NumC = 0;
            NumD = 0;
            NumE = 0;
            NumF = 0;
            NumG = 0;
            NumH = 0;
            NumJ = 0;
            NumK = 0;
            Answer = "";
            Objective = 0;
            TEKS = "";
        }

    }

    /// <summary>
    /// the multi-part key to be used in a hashtable
    /// </summary>
    public class IAReportItemKey : ClassKey<IAReportItem>
    {
        /// <summary>
        /// create a hash key based on an instance of IAReportItem
        /// </summary>
        /// <param name="ClassReference">instance of an IAReportItem</param>
        public IAReportItemKey(IAReportItem ClassReference) : base(ClassReference) { }

        /// <summary>
        /// return the list of values in the key
        /// </summary>
        /// <returns>an array of objects that contain the values of the actual fields used in the key</returns>
        public override object[] GetKeyValues()
        {
            return new object[] 
            {
                ClassReference.Campus,
                ClassReference.TestID,
                ClassReference.Teacher,
                ClassReference.Period,
                ClassReference.ItemNum
            };
        }
    }
}