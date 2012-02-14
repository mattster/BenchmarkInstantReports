
namespace Benchmark_Instant_Reports_2.Infrastructure.DataStruct
{
    /// <summary>
    /// Scan Report data item
    /// </summary>
    public class ScanReportItem
    {
        public string Campus { get; set; }      // key
        public string TestID { get; set; }      // key
        public string Teacher { get; set; }     // key
        public string Period { get; set; }      // key
        public int NumScanned { get; set; }
        public int NumQueried { get; set; }

        /// <summary>
        /// default constructor - blank / empty / zero items
        /// </summary>
        public ScanReportItem()
        {
            Campus = "";
            TestID = "";
            Teacher = "";
            Period = "";
            NumScanned = 0;
            NumQueried = 0;
        }

        /// <summary>
        /// default constructor to create a blank item with just the key elements
        /// </summary>
        /// <param name="campus">Campus</param>
        /// <param name="testid">Test ID</param>
        /// <param name="teacher">Teacher</param>
        /// <param name="period">Period</param>
        public ScanReportItem(string campus, string testid, string teacher, string period)
        {
            Campus = campus;
            TestID = testid;
            Teacher = teacher;
            Period = period;
        }
    }



    /// <summary>
    /// the multi-part key to be used in a hashtable
    /// </summary>
    public class ScanReportItemKey : ClassKey<ScanReportItem>
    {
        /// <summary>
        /// create a hash key based on an instance of ScanReportItem
        /// </summary>
        /// <param name="ClassReference">instance of a ScanReportItem</param>
        public ScanReportItemKey(ScanReportItem ClassReference) : base(ClassReference) { }

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
                ClassReference.Period
            };
        }
    }

}