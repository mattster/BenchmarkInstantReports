using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Metadata
{
    public class TestMetadataItem
    {
        public string Name { get; set; }
        public string DispAbbr { get; set; }
        public string CodeAbbr { get; set; }
        public string ElemSec { get; set; }
        public virtual string RegEx { get; set; }

        public TestMetadataItem(string name, string dispabbr, string codeabbr, string elemsec)
        {
            Name = name;
            DispAbbr = dispabbr;
            CodeAbbr = codeabbr;
            ElemSec = elemsec;
        }
    }
}