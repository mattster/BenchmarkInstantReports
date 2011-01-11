using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Benchmark_Instant_Reports_2
{
    public class birExceptions
    {
        //**** the exception data ****//
#region exceptiondata
        // ** test versions for specific tests, teachers, periods **
        private static string exTest1 = "2010-12 SC Biology SEM 22-24";
        private static string exCampus11 = "LHFC";
        private static string exTeacher111 = "LANG, LUCIANA";
        private static string[] exPeriod111 = { "03", "04", "06", "07" };
        private static string exTeacher112 = "ALTIMORE, MICHELLE";
        private static string[] exPeriod112 = { "02", "03" };
        private static string exTeacher113 = "FLETCHER, COLLEEN";
        private static string[] exPeriod113 = { "02", "03" };

        private static string exCampus12 = "PHS";
        private static string exTeacher121 = "ANDREWS, SUSAN";
        private static string[] exPeriod121 = { "01", "02", "03", "04", "06", "07" };
        private static string exTeacher122 = "BODNER, KATHERINE";
        private static string[] exPeriod122 = { "01", "05", "06" };
        private static string exTeacher123 = "SCHOENROCK, DEEDRA";
        private static string[] exPeriod123 = { "07" };
        private static string exTeacher124 = "WALDERMAN, KRISTIANA";
        private static string[] exPeriod124 = { "04" };

        private static string exCampus13 = "BHS";
        private static string exTeacher131 = "MIKESELL, WILLIAM";
        private static string[] exPeriod131 = { "01", "05", "07" };       
        private static string exTeacher132 = "GILL, STEVEN";
        private static string[] exPeriod132 = { "02", "03", "06" };
        private static string exTeacher133 = "DUTTON, TANJA";
        private static string[] exPeriod133 = { "04", "06" };
        private static string exTeacher134 = "GIRON, JESUSITO";
        private static string[] exPeriod134 = { "06", "07" };
        private static string exTeacher135 = "GRINSFELDER, LISA";
        private static string[] exPeriod135 = { "02", "06", "07" };
        private static string exTeacher136 = "GALVAN, CARRIE";
        private static string[] exPeriod136 = { "02", "04", "05", "07" };
        private static string exTeacher137 = "JACKSON, LETREANNA";
        private static string[] exPeriod137 = { "05" };


        private static string exTest2 = "2010-12 SC Chemistry SEM 22-26";
        private static string exCampus21 = "BHS";
        private static string exTeacher211 = "GILLIAM, LEONA";
        private static string[] exPeriod211 = { "01", "02", "04", "07" };

        private static string exTest3 = "2010-12 SC Biology-M SEM 22-36";
        private static string exCampus31 = "BHS";
        private static string exTeacher311 = "MIKESELL, WILLIAM";
        private static string[] exPeriod311 = { "01", "05", "07" };
        private static string exTeacher312 = "GILL, STEVEN";
        private static string[] exPeriod312 = { "02", "03", "06" };
        private static string exTeacher313 = "DUTTON, TANJA";
        private static string[] exPeriod313 = { "04", "06" };
        private static string exTeacher314 = "GIRON, JESUSITO";
        private static string[] exPeriod314 = { "06", "07" };
        private static string exTeacher315 = "GRINSFELDER, LISA";
        private static string[] exPeriod315 = { "02", "06", "07" };
        private static string exTeacher316 = "GALVAN, CARRIE";
        private static string[] exPeriod316 = { "02", "04", "05", "07" };
        private static string exTeacher317 = "JACKSON, LETREANNA";
        private static string[] exPeriod317 = { "05" };




        // ** District item drop lists for tests and campuses **
        private static string ddTest1 = "2010-12 SC IPC SEM 22-25";
        private static string ddCampus11 = "LHHS";
        private static int[] ddItems11 = { 5, 10, 12, 17, 36, 40 };
        private static string ddCampus12 = "LHFC";
        private static int[] ddItems12 = { 5, 10, 12, 17, 36, 40 };
        
        private static string ddTest2 = "2010-12 SC IPC-M SEM 22-37";
        private static string ddCampus21 = "LHHS";
        private static int[] ddItems21 = { 5, 11, 15, 29 };
        private static string ddCampus22 = "LHFC";
        private static int[] ddItems22 = { 5, 11, 15, 29 };

        // ** Campus item drop lists for tests and campuses **
        private static string cdTest1 = "2010-12 SC IPC-M SEM 22-37";
        private static string cdCampus11 = "LHHS";
        private static int[] cdItems11 = { 32, 33, 34, 35, 36, 37, 38, 39, 40 };
        private static string cdCampus12 = "LHFC";
        private static int[] cdItems12 = { 32, 33, 34, 35, 36, 37, 38, 39, 40 };
        
        private static string cdTest2 = "2010-12 SC Chemistry-M SEM 22-38";
        private static string cdCampus21 = "LHHS";
        private static int[] cdItems21 = { 29, 30, 31, 32, 33, 34, 35 };

#endregion


        //**********************************************************************//
        //** returns a number to be added to the item_num in the campus-
        //** specific answer key to get a specific version; 0 if no special
        //** version defined
        //**
        public static int campusAnswerKeyVersionIncrement(string testID, string campus, string teacher, string period)
        {
            int incAmt = birIF.maxNumTestQuestions;

            if (testID == exTest1)
            {
                if (campus == exCampus11)
                {
                    if (teacher == exTeacher111)
                    {
                        for (int i = 0; i < exPeriod111.Length; i++)
                            if (period == exPeriod111[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher112)
                    {
                        for (int i = 0; i < exPeriod112.Length; i++)
                            if (period == exPeriod112[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher113)
                    {
                        for (int i = 0; i < exPeriod113.Length; i++)
                            if (period == exPeriod113[i])
                                return incAmt;
                    }
                }
                else if (campus == exCampus12)
                {
                    if (teacher == exTeacher121)
                    {
                        for (int i = 0; i < exPeriod121.Length; i++)
                            if (period == exPeriod121[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher122)
                    {
                        for (int i = 0; i < exPeriod122.Length; i++)
                            if (period == exPeriod122[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher123)
                    {
                        for (int i = 0; i < exPeriod123.Length; i++)
                            if (period == exPeriod123[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher124)
                    {
                        for (int i = 0; i < exPeriod124.Length; i++)
                            if (period == exPeriod124[i])
                                return incAmt;
                    }
                }
                else if (campus == exCampus13)
                {
                    if (teacher == exTeacher131)
                    {
                        for (int i = 0; i < exPeriod131.Length; i++)
                            if (period == exPeriod131[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher132)
                    {
                        for (int i = 0; i < exPeriod132.Length; i++)
                            if (period == exPeriod132[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher133)
                    {
                        for (int i = 0; i < exPeriod133.Length; i++)
                            if (period == exPeriod133[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher134)
                    {
                        for (int i = 0; i < exPeriod134.Length; i++)
                            if (period == exPeriod134[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher135)
                    {
                        for (int i = 0; i < exPeriod135.Length; i++)
                            if (period == exPeriod135[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher136)
                    {
                        for (int i = 0; i < exPeriod136.Length; i++)
                            if (period == exPeriod136[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher137)
                    {
                        for (int i = 0; i < exPeriod137.Length; i++)
                            if (period == exPeriod137[i])
                                return incAmt;
                    }
                }
            }
            else if (testID == exTest2)
            {
                if (campus == exCampus21)
                {
                    if (teacher == exTeacher211)
                    {
                        for (int i = 0; i < exPeriod211.Length; i++)
                            if (period == exPeriod211[i])
                                return incAmt;
                    }
                }
            }
            else if (testID == exTest3)
            {
                if (campus == exCampus31)
                {
                    if (teacher == exTeacher311)
                    {
                        for (int i = 0; i < exPeriod311.Length; i++)
                            if (period == exPeriod311[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher312)
                    {
                        for (int i = 0; i < exPeriod312.Length; i++)
                            if (period == exPeriod312[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher313)
                    {
                        for (int i = 0; i < exPeriod313.Length; i++)
                            if (period == exPeriod313[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher314)
                    {
                        for (int i = 0; i < exPeriod314.Length; i++)
                            if (period == exPeriod314[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher315)
                    {
                        for (int i = 0; i < exPeriod315.Length; i++)
                            if (period == exPeriod315[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher316)
                    {
                        for (int i = 0; i < exPeriod316.Length; i++)
                            if (period == exPeriod316[i])
                                return incAmt;
                    }
                    else if (teacher == exTeacher317)
                    {
                        for (int i = 0; i < exPeriod317.Length; i++)
                            if (period == exPeriod317[i])
                                return incAmt;
                    }
                }
            }
            
            return 0;
        }

        //**********************************************************************//
        //** return an array of integers representing the item numbers on
        //** a district answer key to drop, i.e. not count, for the 
        //** specified test and campus
        //**
        public static int[] getDistrictItemDropList(string testID, string campus)
        {
            if (testID == ddTest1)
            {
                if (campus == ddCampus11)
                {
                    return ddItems11;
                }
                else if (campus == ddCampus12)
                {
                    return ddItems12;
                }
            }
            else if (testID == ddTest2)
            {
                if (campus == ddCampus21)
                {
                    return ddItems21;
                }
                else if (campus == ddCampus22)
                {
                    return ddItems22;
                }
            }

            return null;
        }


        //**********************************************************************//
        //** return an array of integers representing the item numbers on
        //** a campus answer key to drop, i.e. not count, for the 
        //** specified test and campus
        //**
        public static int[] getCampusItemDropList(string testID, string campus)
        {
            if (testID == cdTest1)
            {
                if (campus == cdCampus11)
                {
                    return cdItems11;
                }
                else if (campus == cdCampus12)
                {
                    return cdItems12;
                }
            }
            else if (testID == cdTest2)
            {
                if (campus == cdCampus21)
                {
                    return cdItems21;
                }
            }

            return null;
        }

    }
}