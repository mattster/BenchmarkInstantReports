using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Metadata
{
    public static partial class AllTestMetadata
    {
        public static IEnumerable<T> All<T>()  where T : TestMetadataItem
        {
            List<T> retlist = new List<T>(); 
            if (typeof(T) == typeof(Curriculum))
            {
                foreach (Curriculum c in AllCurriculum)
                    yield return c as T;
            }
            else if (typeof(T) == typeof(TestType))
            {
                foreach (TestType tt in AllTestTypes)
                    yield return tt as T;
            }
            else
                yield return null;
        }
    }
}