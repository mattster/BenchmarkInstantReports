using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    /// <summary>
    /// represents a data entity from the Scans data table (aci.benchmark);
    /// contains the scan data produced by Lexmark
    /// </summary>
    public class ScanItem
    {
        public string DateScannedStr { get; set; }      
        public int ScanSequence { get; set; }           // key
        public string Imagepath { get; set; }           // key
        public string Name { get; set; }
        public string StudentID { get; set; }
        public string TestID { get; set; }
        public string Language { get; set; }
        public string Exempt { get; set; }
        public string PreSlugged { get; set; }
        public string Answers { get; set; }

        /// <summary>
        /// constructor to create a blank item
        /// </summary>
        public ScanItem()
        {
            DateScannedStr = "";
            ScanSequence = 0;
            Imagepath = "";
            Name = "";
            StudentID = "";
            TestID = "";
            Language = "E";
            Exempt = "N";
            PreSlugged = "Y";
            Answers = "";
        }
    }


    /// <summary>
    /// the multi-part key to be used in a hashtable
    /// </summary>
    public class ScanItemKey : ClassKey<ScanItem>
    {
        /// <summary>
        /// create a hash key based on an instance of ScanItem
        /// </summary>
        /// <param name="ClassReference">instance of an IAReportItem</param>
        public ScanItemKey(ScanItem ClassReference) : base(ClassReference) { }

        /// <summary>
        /// return the list of values in the key
        /// </summary>
        /// <returns>an array of objects that contain the values of the actual fields used in the key</returns>
        public override object[] GetKeyValues()
        {
            return new object[] 
            {
                ClassReference.Imagepath,
                ClassReference.ScanSequence
            };
        }
    }

}