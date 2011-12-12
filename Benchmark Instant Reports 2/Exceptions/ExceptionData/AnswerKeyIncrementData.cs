using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.References;


namespace Benchmark_Instant_Reports_2.Exceptions
{
    public static partial class ExceptionData
    {
        public static List<AnswerKeyIncrement> additionalAnswerKeys = new List<AnswerKeyIncrement> 
            {
                // 2010-2011 Tests
                new AnswerKeyIncrement("2010-12 SC Biology SEM 22-24", "LHFC", "LANG, LUCIANA", new string[] { "03", "04", "06", "07" }),
                new AnswerKeyIncrement("2010-12 SC Biology SEM 22-24", "LHFC", "ALTIMORE, MICHELLE", new string[] { "02", "03" }),
                new AnswerKeyIncrement("2010-12 SC Biology SEM 22-24", "LHFC", "FLETCHER, COLLEEN", new string[] { "02", "03" }),
                new AnswerKeyIncrement("2010-12 SC Biology SEM 22-24", "PHS", "ANDREWS, SUSAN", 
                    new string[] { "01", "02", "03", "04", "06", "07" }),
                new AnswerKeyIncrement("2010-12 SC Biology SEM 22-24", "PHS", "BODNER, KATHERINE", 
                    new string[] { "01", "05", "06" }),
                new AnswerKeyIncrement("2010-12 SC Biology SEM 22-24", "PHS", "SCHOENROCK, DEEDRA", "07"),
                new AnswerKeyIncrement("2010-12 SC Biology SEM 22-24", "PHS", "WALDERMAN, KRISTIANA", "04"),
                new AnswerKeyIncrement("2010-12 SC Biology SEM 22-24", "BHS", "MIKESELL, WILLIAM", new string[] { "01", "05", "07" }),
                new AnswerKeyIncrement("2010-12 SC Biology SEM 22-24", "BHS", "GILL, STEVEN", new string[] { "02", "03", "06" }),
                new AnswerKeyIncrement("2010-12 SC Biology SEM 22-24", "BHS", "DUTTON, TANJA", new string[] { "04", "06" }),
                new AnswerKeyIncrement("2010-12 SC Biology SEM 22-24", "BHS", "GIRON, JESUSITO", new string[] { "06", "07" }),
                new AnswerKeyIncrement("2010-12 SC Biology SEM 22-24", "BHS", "GRINSFELDER, LISA", new string[] { "02", "06", "07" }),
                new AnswerKeyIncrement("2010-12 SC Biology SEM 22-24", "BHS", "GALVAN, CARRIE", new string[] { "02", "04", "05", "07" }),
                new AnswerKeyIncrement("2010-12 SC Biology SEM 22-24", "BHS", "JACKSON, LETREANNA", "05"),
                new AnswerKeyIncrement("2010-12 SC Chemistry SEM 22-26", "BHS", "GILLIAM, LEONA", new string[] { "01", "02", "04", "07" }),
                new AnswerKeyIncrement("2010-12 SC Biology-M SEM 22-36", "BHS", "MIKESELL, WILLIAM", new string[] { "01", "05", "07" }),
                new AnswerKeyIncrement("2010-12 SC Biology-M SEM 22-36", "BHS", "GILL, STEVEN", new string[] { "02", "03", "06" }),
                new AnswerKeyIncrement("2010-12 SC Biology-M SEM 22-36", "BHS", "DUTTON, TANJA", new string[] { "04", "06" }),
                new AnswerKeyIncrement("2010-12 SC Biology-M SEM 22-36", "BHS", "GIRON, JESUSITO", new string[] { "06", "07" }),
                new AnswerKeyIncrement("2010-12 SC Biology-M SEM 22-36", "BHS", "GRINSFELDER, LISA", new string[] { "02", "06", "07" }),
                new AnswerKeyIncrement("2010-12 SC Biology-M SEM 22-36", "BHS", "GALVAN, CARRIE", new string[] { "02", "04", "05", "07" }),
                new AnswerKeyIncrement("2010-12 SC Biology-M SEM 22-36", "BHS", "JACKSON, LETREANNA", "05"),

                // 2011-2012 tests
                new AnswerKeyIncrement("2011-11 SC Grade 8 Sci Unit 3 T2 CUT 98-41", "PHJH", "ANDERSON, ANGINELL", 
                    new string[] { "01", "05" }, 2 * Constants.defAnsKeyIncrement),

                    // semester exams
                new AnswerKeyIncrement("2011-12 SC Chemistry SEM 15-39", "LHHS", "TRUONG, THANH-THAI", 
                    new string[] { "02", "03", "05", "06" }, 2 * Constants.defAnsKeyIncrement),
                //RHS Pre-AP Chemistry
                //BHS Pre-AP Chemistry

            };
    }
}