using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public class ScanHelper
    {
        public static PreslugData ReturnPreslugData(IRepoService dataservice, string testid, string campus)
        {
            PreslugData finalData = new PreslugData();

            // get the CUSTOM_QUERY defined in the database
            //string rawCustomQuery = TestHelper.ReturnRawCustomQuery(testid);
            var test = dataservice.TestRepo.FindByTestID(testid);
            string rawCustomQuery = test.CustomQuery;

            // put the correct school in the query
            string curSchoolAbbrevList = SchoolHelper.ConvertCampusList(dataservice, campus);
            string tempschoolCustomQuery = rawCustomQuery.Replace(" = @school", " in (" + curSchoolAbbrevList + ")");
            string schoolCustomQuery = tempschoolCustomQuery.Replace("\n", " ");

            // change the teacher name-number part: we only need the name
            schoolCustomQuery = schoolCustomQuery.Replace(Constants.TeacherNameNumFieldName, Constants.TeacherNameFieldNameR);

            // run the query
            var rosterstudents = dataservice.RosterRepo.ExecuteTestQuery(schoolCustomQuery);
            //return DBIOWorkaround.ReturnExecutedCustomQuery(schoolCustomQuery);

            foreach (var student in rosterstudents)
                finalData.Add(new PreslugItem(student));

            return finalData;
        }




        public static List<StudentListItem> GetStudentScanListData(string testid, string campus,
                                                                   string teacher, string periodList)
        {
            string customQuery = TestHelper.ReturnRawCustomQuery(testid);
            customQuery = customQuery.Replace("@school", "\'" + campus + "\'");

            string qs = Queries.GetStudentScansFromTestQuery.Replace("@testId", testid);
            qs = qs.Replace("@query", customQuery);
            string qTeacher = teacher.Replace("'", "''");
            qs = qs.Replace("@teacherQuery", "teacher_name = \'" + qTeacher + "\' ");
            qs = qs.Replace("@periodQuery", "period in (" + periodList + ") ");

            return DBIOWorkaround.ReturnStudentScanDataItemsFromQ(qs);
        }

        public static List<StudentListItem> GetStudentScanListData(string testid, string campus)
        {
            string customQuery = TestHelper.ReturnRawCustomQuery(testid);

            if (campus == Constants.DispAllElementary || campus == Constants.DispAllSecondary)
            {
                customQuery = customQuery.Replace("AND R.SCHOOL2 = @school", " ");
                customQuery = customQuery.Replace("AND SCHOOL2 = @school", " ");
                customQuery = customQuery.Replace("AND SCHOOL_ABBR = @school", " ");
                customQuery = customQuery.Replace("AND school_abbr = @school", " ");
                customQuery = customQuery.Replace("and school_abbr = @school", " ");
            }
            else
            {
                customQuery = customQuery.Replace("@school", "\'" + campus + "\'");
            }

            string qs = Queries.GetStudentScansFromTestQuery.Replace("@testId", testid);
            qs = qs.Replace("@query", customQuery);
            qs = qs.Replace("@teacherQuery", "1=1 ");
            qs = qs.Replace("@periodQuery", "1=1 ");

            return DBIOWorkaround.ReturnStudentScanDataItemsFromQ(qs);
        }

    }
}