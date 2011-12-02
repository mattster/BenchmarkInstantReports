using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Benchmark_Instant_Reports_2.Exceptions;

namespace Benchmark_Instant_Reports_2
{
    public class birExceptions
    {


        #region exceptiondata


        #region test_versions_for_specific_tests_teachers_periods
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

        private static string exTest4 = "2011-11 SC Grade 8 Sci Unit 3 T2 CUT 98-41";
        private static string exCampus41 = "PHJH";
        private static string exTeacher411 = "ANDERSON, ANGINELL";
        private static string[] exPeriod411 = { "01", "05" };




        #endregion

        #region district_item_drop_lists_for_tests_campuses

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

        private static string ddTest3 = "2011-04 SS AP Human Geo Practice TC 43-19";
        private static string ddCampus31 = "ALL";
        private static int[] ddItems31 = { 62 };

        private static string ddTest4 = "2011-08 SC Biology Unit 1 CUT 91-11";
        private static string ddCampus41 = "ALL";
        //private static int[] ddItems41 = { 13, 16, 17, 18, 19, 20 };
        private static int[] ddItems41 = { 13 };

        private static string ddTest5 = "2011-08 SC Biology Unit 1 PreAP CUT 91-12";
        private static string ddCampus51 = "ALL";
        //private static int[] ddItems51 = { 13, 16, 17, 18, 19, 20 };
        private static int[] ddItems51 = { 13 };

        private static string ddTest6 = "2011-08 SC Biology Unit 1 CUT 91-11";
        private static string ddCampus61 = "ALL";
        //private static int[] ddItems61 = { 11, 14, 15, 16 };
        private static int[] ddItems61 = { 11 };

        private static string ddTest7 = "2011-09 SC Physics Unit 1 CUT 91-14";
        private static string ddCampus71 = "ALL";
        private static int[] ddItems71 = { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
                                             41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60 };

        private static string ddTest8 = "2011-09 SC Physics Unit 1 PreAP CUT 91-15";
        private static string ddCampus81 = "ALL";
        private static int[] ddItems81 = { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
                                             41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60 };

        private static string ddTest9 = "2011-09 SC Grade 8 Sci Unit 2 T1-T2-M CUT 98-12";
        private static string ddCampus91 = "ALL";
        private static int[] ddItems91 = { 11 };

        private static string ddTest10 = "2011-10 SELA Grade 7 Reading SIM 2-02";
        private static string ddCampus101 = "ALL";
        private static int[] ddItems101 = { 31, 32, 33 };

        private static string ddTest11 = "2011-10 SELA Grade 8 Reading SIM 2-03";
        private static string ddCampus111 = "ALL";
        private static int[] ddItems111 = { 46 };

        private static string ddTest12 = "2011-10 SELA Grade 8 Revise_Edit SIM 2-05";
        private static string ddCampus121 = "ALL";
        private static int[] ddItems121 = { 8 };

        private static string ddTest13 = "2011-11 SELA English II SIM 7-53";
        private static string ddCampus131 = "ALL";
        private static int[] ddItems131 = { 29, 30, 31 };

        private static string ddTest14 = "2011-10 SM Algebra 2-M TC 4-44";
        private static string ddCampus141 = "ALL";
        private static int[] ddItems141 = { 8 };

        private static string ddTest15 = "2011-10 SS US History Unit 4 CUT 81-41";
        private static string ddCampus151 = "PHS";
        private static string ddCampus152 = "LHHS";
        private static int[] ddItems151 = { 18, 19, 20 };

        private static string ddTest16 = "2011-11 SELA English I Reading EOC SIM 7-51";
        private static string ddCampus161 = "ALL";
        private static int[] ddItems161 = { 25 };

        #endregion

        #region campus_item_drop_lists_for_tests_campuses

        private static string cdTest1 = "2010-12 SC IPC-M SEM 22-37";
        private static string cdCampus11 = "LHHS";
        private static int[] cdItems11 = { 32, 33, 34, 35, 36, 37, 38, 39, 40 };
        private static string cdCampus12 = "LHFC";
        private static int[] cdItems12 = { 32, 33, 34, 35, 36, 37, 38, 39, 40 };

        private static string cdTest2 = "2010-12 SC Chemistry-M SEM 22-38";
        private static string cdCampus21 = "LHHS";
        private static int[] cdItems21 = { 29, 30, 31, 32, 33, 34, 35 };

        private static string cdTest3 = "2011-08 SC Biology Unit 1 CUT 91-11";
        private static string cdCampus31 = "LHHS";
        private static string cdTeacher31 = "NICHOLS, CANDICE";
        private static string cdPeriod31 = "04";
        private static int[] cdItems31 = { 21, 22, 23, 24, 25 };

        private static string cdTest4 = "2011-08 SC Biology Unit 1 CUT 91-11";
        private static string cdCampus41 = "BHS";
        private static int[] cdItems41 = { 14, 15, 16, 17, 18, 19, 20 };
        private static string cdCampus42 = "LHHS";
        private static int[] cdItems42 = { 14, 15, 16, 17, 18, 19, 20 };
        private static string cdCampus43 = "LHFC";
        private static int[] cdItems43 = { 14, 15, 16, 17, 18, 19, 20 };
        private static string cdCampus44 = "RHS";
        private static int[] cdItems44 = { 14, 15, 16, 17, 18, 19, 20 };
        private static string cdCampus45 = "PHS";
        private static int[] cdItems45 = { 14, 15, 16, 17, 18, 19, 20 };

        private static string cdTest5 = "2011-09 SS US History Unit 2 CUT 81-21";
        private static string cdCampus51 = "RHS";
        private static string cdTeacher51 = "ESPARZA, DAVID";
        private static string cdPeriod51 = "05";
        private static int[] cdItems51 = { 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 };

        private static string cdTest6 = "2011-09 SC Physics Unit 1 T2 CUT 91-21";
        private static string cdCampus61 = "BHS";
        private static string cdTeacher61 = "PARKER, WILLIAM";
        private static string cdPeriod61 = "01";
        private static string cdPeriod62 = "02";
        private static string cdPeriod63 = "03";
        private static int[] cdItems61 = { 20 };

        private static string cdTest7 = "2011-10 SS US History Unit 4 CUT 81-41";
        private static string cdCampus71 = "RHS";
        private static string cdTeacher71 = "ESPARZA, DAVID";
        private static string cdPeriod71 = "05";
        private static int[] cdItems71 = { 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40 };

        private static string cdTest8 = "2011-10 SC Grade 7 Sci Unit 2 T2-T3 PreAP CUT 97-33";
        private static string cdCampus81 = "FMJH";
        private static string cdTeacher81 = "CONOVER, AMY";
        private static string cdPeriod81 = "02";
        private static string cdPeriod82 = "03";
        private static string cdPeriod83 = "06";
        private static int[] cdItems81 = { 21, 22, 23 };

        private static string cdTest9 = "2011-11 SC Physics Unit 2 T3-T4 CUT 91-41";
        private static string cdCampus91 = "LHHS";
        private static string cdTeacher91 = "VALADEZ, MATTHEW";
        private static string cdPeriod91 = "01";
        private static string cdPeriod92 = "03";
        private static string cdPeriod93 = "04";
        private static string cdPeriod94 = "06";
        private static string cdPeriod95 = "07";
        private static int[] cdItems91 = { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 };

        #endregion

        #region griddable_nonexact_items


        #endregion


        #endregion

        public static string noAltAnswerValue = "XXXXXXXXXXXXXXXXXXNOALTANSWERXXXXXXXXXXXXXXXXXX";


        public static bool isGriddableNonExactMatch(string testID, string campus, int itemNum)
        {
            foreach (GridNonExactMatch curNonExactMatch in ExceptionData.gridNonExactMatches)
            {
                if (curNonExactMatch.Equals(testID, campus, itemNum))
                    return true;
            }

            return false;
        }


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
                        if (birUtilities.isStringInStringArray(period, exPeriod111))
                                return incAmt;
                    }
                    else if (teacher == exTeacher112)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod112))        
                                return incAmt;
                    }
                    else if (teacher == exTeacher113)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod113))
                                return incAmt;
                    }
                }
                else if (campus == exCampus12)
                {
                    if (teacher == exTeacher121)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod121))
                                return incAmt;
                    }
                    else if (teacher == exTeacher122)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod122))
                                return incAmt;
                    }
                    else if (teacher == exTeacher123)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod123))
                                return incAmt;
                    }
                    else if (teacher == exTeacher124)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod124))
                                return incAmt;
                    }
                }
                else if (campus == exCampus13)
                {
                    if (teacher == exTeacher131)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod131))
                                return incAmt;
                    }
                    else if (teacher == exTeacher132)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod132))
                                return incAmt;
                    }
                    else if (teacher == exTeacher133)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod133))
                                return incAmt;
                    }
                    else if (teacher == exTeacher134)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod134))
                                return incAmt;
                    }
                    else if (teacher == exTeacher135)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod135))
                                return incAmt;
                    }
                    else if (teacher == exTeacher136)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod136))
                                return incAmt;
                    }
                    else if (teacher == exTeacher137)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod137))
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
                        if (birUtilities.isStringInStringArray(period, exPeriod211))
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
                        if (birUtilities.isStringInStringArray(period, exPeriod311))
                                return incAmt;
                    }
                    else if (teacher == exTeacher312)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod312))
                                return incAmt;
                    }
                    else if (teacher == exTeacher313)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod313))
                                return incAmt;
                    }
                    else if (teacher == exTeacher314)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod314))
                                return incAmt;
                    }
                    else if (teacher == exTeacher315)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod315))
                                return incAmt;
                    }
                    else if (teacher == exTeacher316)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod316))
                                return incAmt;
                    }
                    else if (teacher == exTeacher317)
                    {
                        if (birUtilities.isStringInStringArray(period, exPeriod317))
                                return incAmt;
                    }
                }
            }
            else if (testID == exTest4)
            {
                if (campus == exCampus41)
                    if (teacher == exTeacher411)
                        if (birUtilities.isStringInStringArray(period, exPeriod411))
                            return incAmt * 2;
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
            else if (testID == ddTest3)
            {
                // for all campuses
                return ddItems31;
            }
            else if (testID == ddTest4)
            {
                return ddItems41;
            }
            else if (testID == ddTest5)
            {
                return ddItems51;
            }
            else if (testID == ddTest6)
            {
                return ddItems61;
            }
            else if (testID == ddTest7)
            {
                return ddItems71;
            }
            else if (testID == ddTest8)
            {
                return ddItems81;
            }
            else if (testID == ddTest9)
            {
                return ddItems91;
            }
            else if (testID == ddTest10)
            {
                return ddItems101;
            }
            else if (testID == ddTest11)
            {
                return ddItems111;
            }
            else if (testID == ddTest12)
            {
                return ddItems121;
            }
            else if (testID == ddTest13)
            {
                return ddItems131;
            }
            else if (testID == ddTest14)
            {
                return ddItems141;
            }
            else if (testID == ddTest15)
            {
                if (campus == ddCampus151 || campus == ddCampus152)
                    return ddItems151;
            }
            else if (testID == ddTest16)
            {
                return ddItems161;
            }

            return null;
        }


        //**********************************************************************//
        //** return an array of integers representing the item numbers on
        //** a campus answer key to drop, i.e. not count, for the 
        //** specified test and campus
        //**
        public static int[] getCampusItemDropList(string testID, string campus, string teacher, string period)
        {





            if (testID == cdTest1)
            {
                if (campus == cdCampus11)
                    return cdItems11;
                else if (campus == cdCampus12)
                    return cdItems12;
            }
            else if (testID == cdTest2)
            {
                if (campus == cdCampus21)
                    return cdItems21;
            }
            else if (testID == cdTest3)
            {
                if (campus == cdCampus31)
                {
                    if (teacher == cdTeacher31)
                    {
                        if (period == cdPeriod31)
                            return cdItems31;
                    }
                }
            }
            else if (testID == cdTest4)
            {
                if (campus == cdCampus41)
                    return cdItems41;
                else if (campus == cdCampus42)
                    return cdItems42;
                else if (campus == cdCampus43)
                    return cdItems43;
                else if (campus == cdCampus44)
                    return cdItems44;
                else if (campus == cdCampus45)
                    return cdItems45;
            }
            else if (testID == cdTest5)
            {
                if (campus == cdCampus51)
                    if (teacher == cdTeacher51)
                        if (period == cdPeriod51)
                            return cdItems51;
            }
            else if (testID == cdTest6)
            {
                if (campus == cdCampus61)
                    if (teacher == cdTeacher61)
                        if (period == cdPeriod61 || period == cdPeriod62 || period == cdPeriod63)
                            return cdItems61;
            }
            else if (testID == cdTest7)
            {
                if (teacher == cdTeacher71)
                    if (period == cdPeriod71)
                        return cdItems71;
            }
            else if (testID == cdTest8)
            {
                if (campus == cdCampus81)
                    if (teacher == cdTeacher81)
                        if (period == cdPeriod81 || period == cdPeriod82 || period == cdPeriod83)
                            return cdItems81;
            }
            else if (testID == cdTest9)
            {
                if (campus == cdCampus91)
                    if (teacher == cdTeacher91)
                        if (period == cdPeriod91 || period == cdPeriod92 || period == cdPeriod93 ||
                            period == cdPeriod94 || period == cdPeriod95)
                            return cdItems91;
            }
            return null;
        }



        public static string getAlternateAnswer(string testID, int curItemNum, string campus = "")
        {
            foreach (AlternateAnswer curAltAnswer in ExceptionData.alternateAnswers)
            {
                if (curAltAnswer.Equals(testID, campus, curItemNum))
                    return curAltAnswer.AltAnswer;
            }

            return noAltAnswerValue;
        }




    }

}