using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Helpers.Reports
{
    public class AnswerCounter
    {
                    public int a, b, c, d, e, f, g, h, j, k;


            // default constructor
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