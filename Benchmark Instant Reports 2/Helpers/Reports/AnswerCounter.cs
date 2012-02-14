using Benchmark_Instant_Reports_2.Infrastructure.DataStruct;

namespace Benchmark_Instant_Reports_2.Helpers.Reports
{
    /// <summary>
    /// used to tabulate data for the Item Analysis report
    /// </summary>
    public class AnswerCounter
    {
        public int a, b, c, d, e, f, g, h, j, k;


        /// <summary>
        /// default Constructor
        /// </summary>
        public AnswerCounter()
        {
            a = 0;
            b = 0;
            c = 0;
            d = 0;
            e = 0;
            f = 0;
            g = 0;
            h = 0;
            j = 0;
            k = 0;
        }


        /// <summary>
        /// update the counters based on an existing report data item
        /// </summary>
        /// <param name="item">IAReportItem object</param>
        public void UpdateFromReportItem(IAReportItem item)
        {
            a = item.NumA;
            b = item.NumB;
            c = item.NumC;
            d = item.NumD;
            e = item.NumE;
            f = item.NumF;
            g = item.NumG;
            h = item.NumH;
            j = item.NumJ;
            k = item.NumK;

            return;
        }


        /// <summary>
        /// update a report data item with the current counters
        /// </summary>
        /// <param name="item">IAReportItem to update</param>
        public void UpdateToReportItem(IAReportItem item)
        {
            item.NumA = a;
            item.NumB = b;
            item.NumC = c;
            item.NumD = d;
            item.NumE = e;
            item.NumF = f;
            item.NumG = g;
            item.NumH = h;
            item.NumJ = j;
            item.NumK = k;
            
            return;
        }


        /// <summary>
        /// increment the appropriate counter
        /// </summary>
        /// <param name="x">answer representing the counter to increment</param>
        public void Increment(string x)
        {
            if (x.ToUpper() == "A")
                a++;
            else if (x.ToUpper() == "B")
                b++;
            else if (x.ToUpper() == "C")
                c++;
            else if (x.ToUpper() == "D")
                d++;
            else if (x.ToUpper() == "E")
                e++;
            else if (x.ToUpper() == "F")
                f++;
            else if (x.ToUpper() == "G")
                g++;
            else if (x.ToUpper() == "H")
                h++;
            else if (x.ToUpper() == "J")
                j++;
            else if (x.ToUpper() == "K")
                k++;

            return;
        }


        /// <summary>
        /// reset counters to 0
        /// </summary>
        public void Reset()
        {
            a = 0;
            b = 0;
            c = 0;
            d = 0;
            e = 0;
            f = 0;
            g = 0;
            h = 0;
            j = 0;
            k = 0;

            return;
        }

    }
}