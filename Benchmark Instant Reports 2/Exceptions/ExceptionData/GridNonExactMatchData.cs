using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Exceptions
{
    public static partial class ExceptionData
    {
        public static List<GridNonExactMatch> gridNonExactMatches = new List<GridNonExactMatch>
        {
            // October 2011 Math TEKS Checks
            new GridNonExactMatch("2011-10 SM Algebra 1 TC 4-31", new int[] { 19, 20 }),
            new GridNonExactMatch("PreAP Geometry TC Oct 2011", new int[] { 19, 20}),
            new GridNonExactMatch("2011-10 SM PreAP Algebra 1 TC 4-30", 20),
            new GridNonExactMatch("2011-10 SM Grade 7 Math TC 4-27", new int[] { 29, 30 }),
            new GridNonExactMatch("2011-10 SM Grade 8 Math TC 4-29", 30),
            new GridNonExactMatch("2011-10 SM Grade 7 Math PreAP TC 4-28", 30),

            // December Elementary Math Tests
            new GridNonExactMatch("2011-12 EM 3rd Grade Math TC 10-64", new int[] { 23, 24 }),
            new GridNonExactMatch("2011-12 EM 4th Grade Math TC 10-65", new int[] { 35, 36 }),
            new GridNonExactMatch("2011-12 EM 5th Grade Math TC 10-66", new int[] { 37, 38 }),
            new GridNonExactMatch("2011-12 EM 6th Grade Math TC 10-67", new int[] { 39, 40 }),

            // December Elementary Science
            new GridNonExactMatch("2011-12 EC 6th Grade Science TC 11-70", 25),

            // Math Semester Exams December 2011
            new GridNonExactMatch("2011-12 SM Algebra 2 SEM 14-27", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2011-12 SM Algebra 2-M SEM 14-34", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2011-12 SM Algebra I SEM 14-21", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2011-12 SM Algebra I-M SEM 14-30", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2011-12 SM Geometry SEM 14-24", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2011-12 SM Geometry-M SEM 14-32", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2011-12 SM Grade 7 Math PreAP SEM 14-18", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2011-12 SM Grade 7 Math SEM 14-17", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2011-12 SM Grade 7 Math-M SEM 14-28", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2011-12 SM Grade 8 Math SEM 14-19", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2011-12 SM Grade 8 Math-M SEM 14-29", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2011-12 SM Grade 9 Geometry SEM 14-23", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2011-12 SM Math Models SEM 14-26", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2011-12 SM Math Models-M SEM 14-33", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2011-12 SM PreAP Algebra I SEM 14-20", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2011-12 SM PreAP Geometry SEM 14-25", new int[] { 61, 62, 63 }),
            
            // Math CUTs Spring 2012
            new GridNonExactMatch("2012-02 SM Geometry PreAP CUT 70-11", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2012-02 SM Grade 9 Geometry CUT 79-11", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2012-02 SM Grade 7 Math PreAP CUT 77-11", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2012-02 SM Grade 8 Math CUT 78-14", new int[] { 61, 62, 63 }),
            new GridNonExactMatch("2012-02 SM Grade 8 Math-M CUT 78-15", new int[] { 61, 62, 63 }),


            // Elementary Math and Science SIMs Spring 2012
            new GridNonExactMatch("2012-01 EC Grade 5 Science TC 19-66", 25),
            new GridNonExactMatch("2012-01 EM Grade 5 Math SIM 21-67", new int[] { 47, 48, 49, 50}),
            new GridNonExactMatch("2012-01 EM Grade 5 Math-M SIM 21-68", 40),

        };

    }
}