using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Metadata
{
    public class Subject : TestMetadataItem
    {
        private static string baseRegExPattern1 = @"[0-9]{4}-[0-9]{2} [ES]";
        private static string baseRegExPattern2 = @".* \d{1,2}-\d{2}$";
        public override string RegEx
        {
            get { return baseRegExPattern1 + CodeAbbr + baseRegExPattern2; }
        }
        public string CurricApplicable { get; set; }

        public Subject(string name, string dispabbr, string codeabbr, string elemsec, string curricapplicable)
            : base(name, dispabbr, codeabbr, elemsec)
        {
            CurricApplicable = curricapplicable;
        }
    }


    public static partial class AllTestMetadata
    {
        public static Subject[] AllSubjects = 
        {
            // Science
            new Subject("7th Grade Science", "Grade 7 Science", "C.*(Grade 7)", "S", "Science"),
            new Subject("8th Grade Science", "Grade 8 Science", "C.*(Grade 8)", "S", "Science"),
            new Subject("Biology", "Biology", "C.*(Biology)", "S", "Science"),
            new Subject("Chemistry", "Chemistry", "C.*(Chemistry)", "S", "Science"),
            new Subject("IPC", "IPC", "C.*(IPC)", "S", "Science"),
            new Subject("Physics", "Physics", "C.*(Physics)", "S", "Science"),
            new Subject("Environmental Science", "Environ. Science", "C.*(Environmental)", "S", "Science"),
            
            // Social Studies
            new Subject("Texas History", "Grade 7 - TX Hist", "S.*(TX Hist)", "S", "Social Studies"),
            new Subject("American History", "Grade 8 - Amer. Hist", "S.*(Grade 8)", "S", "Social Studies"),
            new Subject("World Geography", "World Geography", "S.*(World Geography)", "S", "Social Studies"),
            new Subject("World History", "World History", "S.*(World History)", "S", "Social Studies"),
            new Subject("US History", "US History", "S.*(US History)", "S", "Social Studies"),
            new Subject("Human Geography", "Human Geography", "S.*(Human Geography)", "S", "Social Studies"),
            new Subject("European History", "European History", "S.*(European History)", "S", "Social Studies"),

            // Math
            new Subject("7th Grade Math", "Grade 7 Math", "M.*(Grade 7)", "S", "Math"),
            new Subject("8th Grade Math", "Grade 8 Math", "M.*(Grade 8)", "S", "Math"),
            new Subject("Algebra", "Algebra", "M.*(Algebra)", "S", "Math"),
            new Subject("Geometry", "Geometry", "M.*(Geometry)", "S", "Math"),
            new Subject("Math Models", "Math Models", "M.*(Math Models)", "S", "Math"),

            // ELA
            new Subject("7th Grade ELA", "Grade 7 ELA", "ELA.*(Grade 7)", "S", "ELA"),
            new Subject("8th Grade ELA", "Grade 8 ELA", "ELA.*(Grade 8)", "S", "ELA"),
            new Subject("English I", "English I", "ELA.*(English I)[^I]", "S", "ELA"),
            new Subject("English II", "English II", "ELA.*(English II)[^I]", "S", "ELA"),
            new Subject("English III", "English III", "ELA.*(English III)", "S", "ELA"),

            // LOTE
            new Subject("French", "French", "LOTE.*(French)", "S", "LOTE"),
            new Subject("German", "German", "LOTE.*(German)", "S", "LOTE"),
            new Subject("Japanese", "Japanese", "LOTE.*(Japanese)", "S", "LOTE"),
            new Subject("Latin", "Latin", "LOTE.*(Latin)", "S", "LOTE"),
            new Subject("Spanish", "Spanish", "LOTE.*(Spanish)", "S", "LOTE"),
        };
    }
}