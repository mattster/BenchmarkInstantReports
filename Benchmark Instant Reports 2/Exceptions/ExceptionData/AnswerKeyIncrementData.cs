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
                        //LHHS Pre-AP Chemistry
                new AnswerKeyIncrement("2011-12 SC Chemistry SEM 15-39", "LHHS", "TRUONG, THANH-THAI", 
                    new string[] { "02", "03", "05", "06" }, 2 * Constants.defAnsKeyIncrement),
                        
                        //BHS Pre-AP Chemistry
                new AnswerKeyIncrement("2011-12 SC Chemistry SEM 15-39", "BHS", "MILLER, SCOTT", new string[] { "06", "07" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Chemistry SEM 15-39", "BHS", "UMEAKU, ADAOBI", new string[] { "04", "05" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Chemistry SEM 15-39", "BHS", "WOLF, JAKOBUS", new string[] { "01", "03" },
                    2 * Constants.defAnsKeyIncrement),

                        //PHS Pre-AP Bio
                new AnswerKeyIncrement("2011-12 SC Biology SEM 15-37", "PHS", "ANDREWS, SUSAN", new string[] { "04", "06", "07" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Biology SEM 15-37", "PHS", "BODNER, KATHERINE", new string[] { "01", "05", "07" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Biology SEM 15-37", "PHS", "ERICKSON, CINDY", new string[] { "02", "03", "04" },
                    2 * Constants.defAnsKeyIncrement),

                        //PreAP Grade 8 Science
                new AnswerKeyIncrement("2011-12 SC Grade 8 Sci SEM 15-36", "LJH", "ROSS, GARY", new string[] { "02", "03" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Grade 8 Sci SEM 15-36", "LJH", "SCHOTTLAENDER, ALAYNA", new string[] { "01", "07" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Grade 8 Sci SEM 15-36", "PHJH", "WELCH, STACEY", new string[] { "01", "05", "07" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Grade 8 Sci SEM 15-36", "PHJH", "WILSON, KAREN", new string[] { "02", "04", "06" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Grade 8 Sci SEM 15-36", "RWJH", "HANEY, TODD", new string[] { "01", "02", "04", "05", "06" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Grade 8 Sci SEM 15-36", "RWJH", "SMITH, KELLIE", "07",
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Grade 8 Sci SEM 15-36", "FMJH", "THOMAS, JESSICA", new string[] { "03", "07" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Grade 8 Sci SEM 15-36", "FMJH", "MEGISON, STEPHEN", new string[] { "02", "05" },
                    2 * Constants.defAnsKeyIncrement),

                        //PreAP Grade 7 Science
                new AnswerKeyIncrement("2011-12 SC Grade 7 Sci SEM 15-35", "PHJH", "LYONS, PAUL", "02",
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Grade 7 Sci SEM 15-35", "PHJH", "MCWHERTER, MARGARETTE", new string[] { "02", "04", "05", "06", "07" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Grade 7 Sci SEM 15-35", "LJH", "FICK, DENISE", new string[] { "05", "06", "07" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Grade 7 Sci SEM 15-35", "LHJH", "DERBIN, ALEXIS", new string[] { "04", "06", "07" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Grade 7 Sci SEM 15-35", "LHJH", "HOLMES, KATHRYN", new string[] { "02", "05" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Grade 7 Sci SEM 15-35", "FMJH", "REX, MOLLY", new string[] { "01", "05" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SC Grade 7 Sci SEM 15-35", "FMJH", "CONOVER, AMY", new string[] { "02", "03", "06" },
                    2 * Constants.defAnsKeyIncrement),

                        //PreAP Grade 8 ELA
                new AnswerKeyIncrement("2011-12 SELA Grade 8 ELA SEM 13-02", "LHJH", "BALL, LAKISHA", new string[] { "01", "05", "06" },
                    2 * Constants.defAnsKeyIncrement),
                new AnswerKeyIncrement("2011-12 SELA Grade 8 ELA SEM 13-02", "LHJH", "LANE, KATHERINE", new string[] { "03", "07" },
                    2 * Constants.defAnsKeyIncrement),


            };
    }
}