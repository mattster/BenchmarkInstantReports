﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Exceptions
{
    public static partial class ExceptionData
    {
        public static List<DropList> DropLists = new List<DropList>
                {
                    // 2010-2011 Tests
                    new DropList("2010-12 SC IPC-M SEM 22-37", new string[] { "LHHS", "LHFC"}, new int[] { 32, 33, 34, 35, 36, 37, 38, 39, 40 }),
                    new DropList("2010-12 SC Chemistry-M SEM 22-38", "LHHS", new int[] { 29, 30, 31, 32, 33, 34, 35 }),
                    new DropList("2010-12 SC IPC SEM 22-25", new string[] { "LHHS", "LHFC" }, new int[] { 5, 10, 12, 17, 36, 40 }),
                    new DropList("2010-12 SC IPC-M SEM 22-37", new string[] { "LHHS", "LHFC" }, new int[] { 5, 11, 15, 29 }),
                    new DropList("2011-04 SS AP Human Geo Practice TC 43-19", 62),
                    
                    // 2011-2012 Tests
                    new DropList("2011-08 SC Biology Unit 1 CUT 91-11", 13),
                    new DropList("2011-08 SC Biology Unit 1 PreAP CUT 91-12", 13),
                    new DropList("2011-08 SC Biology Unit 1 CUT 91-11", 11),
                    new DropList("2011-08 SC Biology Unit 1 CUT 91-11", "LHHS", "NICHOLS, CANDICE", "04", 
                        new int[] { 32, 33, 34, 35, 36, 37, 38, 39, 40 }),
                    new DropList("2011-08 SC Biology Unit 1 CUT 91-11", new string[] { "BHS", "LHHS", "LHFC", "RHS", "PHS" }, 
                        new int[] { 14, 15, 16, 17, 18, 19, 20 }),
                    new DropList("2011-09 SS US History Unit 2 CUT 81-21", "RHS", "ESPARZA, DAVID", "05", 
                        new int[] { 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 }),
                    new DropList("2011-09 SC Physics Unit 1 CUT 91-14", 
                        new int[] { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 
                                    41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60 }),
                    new DropList("2011-09 SC Physics Unit 1 PreAP CUT 91-15", 
                        new int[] { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
                                    41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60 }),
                    new DropList("2011-09 SC Physics Unit 1 T2 CUT 91-21", "BHS", "PARKER, WILLIAM", new string[] { "01", "02", "03" }, 20),
                    new DropList("2011-09 SC Grade 8 Sci Unit 2 T1-T2-M CUT 98-12", 11),
                    new DropList("2011-10 SELA Grade 7 Reading SIM 2-02", new int[] { 31, 32, 33 }),
                    new DropList("2011-10 SELA Grade 8 Reading SIM 2-03", 46),
                    new DropList("2011-10 SELA Grade 8 Revise_Edit SIM 2-05", 8),
                    new DropList("2011-10 SM Algebra 2-M TC 4-44", 8),
                    new DropList("2011-10 SS US History Unit 4 CUT 81-41", new string[] { "PHS", "LHHS" }, 
                        new int[] { 18, 19, 20 }),
                    new DropList("2011-10 SS US History Unit 4 CUT 81-41", "RHS", "ESPARZA, DAVID", "05", 
                        new int[] { 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40 }),
                    new DropList("2011-10 SC Grade 7 Sci Unit 2 T2-T3 PreAP CUT 97-33", "FMJH", "CONOVER, AMY", new string[] { "02", "03", "06" }, 
                        new int[] { 21, 22, 23 }),
                    new DropList("2011-11 SELA English I Reading EOC SIM 7-51", 25),
                    new DropList("2011-11 SELA English II SIM 7-53", new int[] { 29, 30, 31 }),
                    new DropList("2011-11 SC Physics Unit 2 T3-T4 CUT 91-41", "LHHS", "VALDEZ, MATTHEW", new string[] { "01", "03", "04", "06", "07" }, 
                        new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 })

                };

    }
}