using System;
using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Grading
{
    public class StudentData
    {
        /// <summary>
        /// returns a set of data ready for grading and processing for reports
        /// includes student roster data and student scan data
        /// </summary>
        /// <param name="dataservice">IRepoService access to data</param>
        /// <param name="tests">list of Test items to use</param>
        /// <param name="schools">list of School items to use</param>
        /// <returns>DataToGradeItemCollection structure of a collection of DataToGradeItem's</returns>
        public static DataToGradeItemCollection GetStudentDataToGrade(IRepoService dataservice,
            List<Test> tests, List<School> schools)
        {
            DataToGradeItemCollection finalData = new DataToGradeItemCollection();

            foreach (Test curTest in tests)
            {
                foreach (School curSchool in schools)
                {
                    // get a set of the student scans for this test & campus
                    IQueryable<Scan> scannedItems = GetScannedData(dataservice, curTest, curSchool);

                    // get a set of the preslugged data for this test & campus
                    PreslugData preslugged = GetPreslugData(dataservice, curTest, curSchool);

                    // find data that is both preslugged and scanned
                    var scannedItemsNotPreslugged = new HashSet<Scan>();
                    foreach (Scan scan in scannedItems)
                    {
                        var foundPreslugs = preslugged.GetItemsWhere(i => Int32.Parse(i.StudentID) == scan.StudentID);
                        if (foundPreslugs.Count() > 0)
                        {
                            DataToGradeItem newDataToGradeItem = new DataToGradeItem();
                            newDataToGradeItem.StudentID = scan.StudentID.ToString();
                            newDataToGradeItem.StudentName = foundPreslugs.First().StudentName;
                            newDataToGradeItem.TeacherName = foundPreslugs.First().TeacherName;
                            newDataToGradeItem.Period = foundPreslugs.First().Period;
                            newDataToGradeItem.CourseID = foundPreslugs.First().CourseID;
                            newDataToGradeItem.Campus = foundPreslugs.First().Campus;
                            newDataToGradeItem.TestID = scan.TestID;
                            newDataToGradeItem.ScanItem = scan;
                            finalData.Add(newDataToGradeItem);
                        }
                        else
                        {
                            scannedItemsNotPreslugged.Add(scan);
                        }
                    }


                    // match the students with scans that are not preslugged to a teacher/period/course
                    if (scannedItemsNotPreslugged.Count > 0)
                    {
                        foreach (Scan scan in scannedItemsNotPreslugged)
                        {
                            // does this student match a teacher who already has preslugged data?
                            var thisStudentsRoster = dataservice.RosterRepo.FindByStudentID(scan.StudentID.ToString());
                            foreach (var rosterItem in thisStudentsRoster)
                            {
                                var foundPreslugged = preslugged.GetItemsWhere(p => p.TeacherName == rosterItem.TeacherName &&
                                    p.Period == rosterItem.Period);
                                if (foundPreslugged.Count() > 0)
                                {
                                    // found a match
                                    DataToGradeItem newItem = new DataToGradeItem();
                                    newItem.StudentID = scan.StudentID.ToString();
                                    newItem.StudentName = rosterItem.StudentName;
                                    newItem.TeacherName = rosterItem.TeacherName;
                                    newItem.Period = rosterItem.Period;
                                    newItem.CourseID = rosterItem.CourseID;
                                    newItem.Campus = rosterItem.CourseCampus;
                                    newItem.TestID = scan.TestID;
                                    newItem.ScanItem = scan;
                                    finalData.Add(newItem);
                                }

                                // if all else fails, put as Unknown Teacher
                                else
                                {
                                    var rosterData = dataservice.RosterRepo.FindByStudentID(scan.StudentID.ToString()).First();

                                    DataToGradeItem newItem = new DataToGradeItem();
                                    newItem.StudentID = scan.StudentID.ToString();
                                    newItem.StudentName = rosterData.StudentName;
                                    newItem.TeacherName = Constants.UnknownTeacherName;
                                    newItem.Period = Constants.UnknownPeriod;
                                    newItem.CourseID = Constants.UnknownCourseID;
                                    newItem.Campus = rosterData.HomeCampus;
                                    newItem.TestID = scan.TestID;
                                    newItem.ScanItem = scan;
                                    finalData.Add(newItem);
                                }
                            }
                        }
                    }
                }
            }

            return finalData;
        }


        /// <summary>
        /// returns "Preslugged" data for a specific test and campus
        /// data includes the fields that are used in setting up a test on the 
        /// Lexmark system
        /// </summary>
        /// <param name="dataservice">IRepoService access to data</param>
        /// <param name="test">Test object to use</param>
        /// <param name="school">School object to use</param>
        /// <returns>PreslugData structure of a collection of preslug data items</returns>
        public static PreslugData GetPreslugData(IRepoService dataservice, Test test, School school)
        {
            PreslugData finalData = new PreslugData();

            // get the CUSTOM_QUERY defined in the database
            string rawCustomQuery = test.CustomQuery;

            // put the correct school in the query
            string tempschoolCustomQuery = rawCustomQuery.Replace("@school", "'" + school.Abbr + "'");
            string schoolCustomQuery = tempschoolCustomQuery.Replace("\n", " ");

            // change the teacher name-number part: we only need the name
            schoolCustomQuery = schoolCustomQuery.Replace(Constants.TeacherNameNumFieldName, Constants.TeacherNameFieldNameR);

            // run the query
            var rosterstudents = dataservice.RosterRepo.ExecuteTestQuery(schoolCustomQuery);

            foreach (var student in rosterstudents)
                finalData.Add(new PreslugItem(student));

            return finalData;
        }


        /// <summary>
        /// return a set of the latest (most recent) scanned items for a given test and school
        /// </summary>
        /// <param name="dataservice">IRepoService access to data</param>
        /// <param name="test">Test to use</param>
        /// <param name="school">School to use</param>
        /// <returns>IQueryable-Scan- set of Scan objects</returns>
        public static IQueryable<Scan> GetScannedData(IRepoService dataservice, Test test, School school)
        {
            IQueryable<Scan> scannedItemsAll = dataservice.ScanRepo.FindScansForTestCampus(test.TestID, school.Abbr);
            IQueryable<Scan> scannedItems = GetLatestScans(scannedItemsAll);

            return scannedItems;
        }








        /// <summary>
        /// return only the most recent scans from the list of all scans, assuming one common TestID
        /// </summary>
        /// <param name="allScannedItems">IQueryable-scan- set of scanned items, including duplicates</param>
        /// <returns>IQueryable-scan- set of only the most recent scans in the set</returns>
        private static IQueryable<Scan> GetLatestScans(IQueryable<Scan> allScannedItems)
        {
            HashSet<Scan> finalData = new HashSet<Scan>();

            foreach (int thisStudentID in allScannedItems.Select(s => s.StudentID).Distinct())
            {
                var scans = allScannedItems.Where(i => i.StudentID == thisStudentID);
                if (scans.Count() > 1)
                {
                    Scan latestScan = new Scan();
                    foreach (Scan scan in scans)
                    {
                        if (scan.DateScanned > latestScan.DateScanned)
                            latestScan = scan;
                    }
                    finalData.Add(latestScan);
                }
                else
                {
                    finalData.Add(scans.First());
                }
            }

            return finalData.AsQueryable();
        }

    }
}