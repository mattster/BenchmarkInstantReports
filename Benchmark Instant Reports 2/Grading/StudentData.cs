using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Helpers;
using Benchmark_Instant_Reports_2.Infrastructure.DataStruct;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Grading
{
    public class StudentData
    {
        /// <summary>
        /// returns a set of data ready for grading and processing for reports;
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

            // go through each test
            foreach (Test curTest in tests)
            {
                // get all scanned data for this test
                IQueryable<Scan> scanDataCurTest = GetScannedData(dataservice, curTest);
                string semesterForTest = TestHelper.SemesterForTest(curTest);

                // go through each school
                foreach (School curSchool in schools)
                {
                    // get all roster data for this school
                    IQueryable<Roster> rosterData = dataservice.RosterRepo.FindBySchool(curSchool.Abbr);

                    // get a set of the preslugged data for this test & campus
                    PreslugData preslugged = GetPreslugData(dataservice, curTest, curSchool);

                    // get a set of the student scans for this test & campus
                    List<TeacherPeriodItem> teachersperiods = GetTeacherPeriodList(preslugged, semesterForTest);
                    //IQueryable<Scan> scanDataCurTestSch = FilterScans(dataservice, scanDataCurTest, rosterData, teachersperiods);
                    IQueryable<Scan> scanDataCurTestSch = FilterScans(dataservice, scanDataCurTest, rosterData);

                    // find data that is both preslugged and scanned
                    var scannedItemsNotPreslugged = new HashSet<Scan>();
                    foreach (Scan scan in scanDataCurTestSch)
                    {
                        IEnumerable<PreslugItem> foundPreslugs = preslugged.GetItemsWhere(p =>
                            p.StudentID == dataservice.StudentIDString(scan.StudentID));
                        if (foundPreslugs.ToList().Count > 0)
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
                            var thisStudentsRoster = dataservice.RosterRepo.
                                FindByStudentID(dataservice.StudentIDString(scan.StudentID));
                            bool found = false;

                            // does this student match a teacher who already has preslugged data?
                            foreach (var rosterItem in thisStudentsRoster)
                            {
                                var foundPreslugged = preslugged.GetItemsWhere(p => p.TeacherName == rosterItem.TeacherName &&
                                    p.Period == rosterItem.Period);
                                if (foundPreslugged.Count() > 0)
                                {
                                    // found a match
                                    DataToGradeItem newItem = new DataToGradeItem();
                                    newItem.StudentID = dataservice.StudentIDString(scan.StudentID);
                                    newItem.StudentName = rosterItem.StudentName;
                                    newItem.TeacherName = rosterItem.TeacherName;
                                    newItem.Period = rosterItem.Period;
                                    newItem.CourseID = rosterItem.CourseID;
                                    newItem.Campus = curSchool.Abbr;
                                    newItem.TestID = scan.TestID;
                                    newItem.ScanItem = scan;
                                    finalData.Add(newItem);
                                    found = true;
                                }

                                if (found) break;
                                else
                                {
                                    // does this student have any courses in the Preslug data?
                                    var foundPreslugged2 = preslugged.GetItemsWhere(p => p.CourseID == rosterItem.CourseID);
                                    if (foundPreslugged2.Count() > 0)
                                    {
                                        // found a match
                                        DataToGradeItem newItem = new DataToGradeItem();
                                        newItem.StudentID = dataservice.StudentIDString(scan.StudentID);
                                        newItem.StudentName = rosterItem.StudentName;
                                        newItem.TeacherName = rosterItem.TeacherName;
                                        newItem.Period = rosterItem.Period;
                                        newItem.CourseID = rosterItem.CourseID;
                                        newItem.Campus = curSchool.Abbr;
                                        newItem.TestID = scan.TestID;
                                        newItem.ScanItem = scan;
                                        finalData.Add(newItem);
                                        found = true;
                                    }
                                }

                                if (found) break;
                            }

                            if (!found)
                            {
                                // if all else fails, put as Unknown Teacher
                                DataToGradeItem newItem = new DataToGradeItem();
                                newItem.StudentID = scan.StudentID.ToString();
                                newItem.StudentName = thisStudentsRoster.First().StudentName;
                                newItem.TeacherName = Constants.UnknownTeacherName;
                                newItem.Period = Constants.UnknownPeriod;
                                newItem.CourseID = Constants.UnknownCourseID;
                                newItem.Campus = thisStudentsRoster.First().HomeCampus;
                                newItem.TestID = scan.TestID;
                                newItem.ScanItem = scan;
                                finalData.Add(newItem);
                            }
                        } // end foreach Scan not preslugged
                    } // end matching non-preslugged scans
                } // end foreach School
            } // end foreach Test

            return finalData;
        }


        /// <summary>
        /// returns a set of data ready for grading and processing for reports;
        /// includes student roster data and student scan data;
        /// assumes preslug data and scan data is already available
        /// </summary>
        /// <param name="dataservice">RepoService access to data</param>
        /// <param name="test">Test to use</param>
        /// <param name="school">School to use</param>
        /// <param name="preslugdata">collection of preslugged data</param>
        /// <returns>DataToGradeItemCollection structure of a collection of DataToGradeItem's</returns>
        public static DataToGradeItemCollection GetStudentDataToGrade(IRepoService dataservice,
            Test test, School school, PreslugData preslugdata, IQueryable<Scan> scandata)
        {
            DataToGradeItemCollection finalData = new DataToGradeItemCollection();

            string semesterForTest = TestHelper.SemesterForTest(test);

            // get all roster data for this school
            IQueryable<Roster> rosterData = dataservice.RosterRepo.FindBySchool(school.Abbr);

            // get a set of the student scans for this test & campus
            List<TeacherPeriodItem> teachersperiods = GetTeacherPeriodList(preslugdata, semesterForTest);
            IQueryable<Scan> scanDataCurTestSch = FilterScans(dataservice, scandata, rosterData, teachersperiods);

            // find data that is both preslugged and scanned
            var scannedItemsNotPreslugged = new HashSet<Scan>();
            foreach (Scan scan in scanDataCurTestSch)
            {
                IEnumerable<PreslugItem> foundPreslugs = preslugdata.GetItemsWhere(p =>
                    p.StudentID == dataservice.StudentIDString(scan.StudentID));
                if (foundPreslugs.ToList().Count > 0)
                {
                    DataToGradeItem newDataToGradeItem = new DataToGradeItem();
                    newDataToGradeItem.StudentID = dataservice.StudentIDString(scan.StudentID);
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
                    var thisStudentsRoster = dataservice.RosterRepo.
                        FindByStudentID(dataservice.StudentIDString(scan.StudentID));
                    bool found = false;

                    // does this student match a teacher who already has preslugged data?
                    foreach (var rosterItem in thisStudentsRoster)
                    {
                        var foundPreslugged = preslugdata.GetItemsWhere(p => p.TeacherName == rosterItem.TeacherName &&
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
                            newItem.Campus = school.Abbr;// rosterItem.CourseCampus;
                            newItem.TestID = scan.TestID;
                            newItem.ScanItem = scan;
                            finalData.Add(newItem);
                            found = true;
                        }
                        if (found) break;
                    }

                    if (!found)
                    {
                        // if all else fails, put as Unknown Teacher
                        DataToGradeItem newItem = new DataToGradeItem();
                        newItem.StudentID = scan.StudentID.ToString();
                        newItem.StudentName = thisStudentsRoster.First().StudentName;
                        newItem.TeacherName = Constants.UnknownTeacherName;
                        newItem.Period = Constants.UnknownPeriod;
                        newItem.CourseID = Constants.UnknownCourseID;
                        newItem.Campus = thisStudentsRoster.First().HomeCampus;
                        newItem.TestID = scan.TestID;
                        newItem.ScanItem = scan;
                        finalData.Add(newItem);
                    }
                } // end foreach Scan not preslugged
            } // end matching non-preslugged scans

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
                if (finalData.GetItemsWhere(d => d.StudentID == student.StudentID).Count() == 0)
                    finalData.Add(new PreslugItem(student, test.TestID));

            return finalData;
        }


        /// <summary>
        /// return a set of the latest (most recent) scanned items for a Test
        /// </summary>
        /// <param name="dataservice">IRepoService access to data</param>
        /// <param name="test">Test object to use</param>
        /// <returns>IQueryable-Scan- set of data</returns>
        public static IQueryable<Scan> GetScannedData(IRepoService dataservice, Test test)
        {
            IQueryable<Scan> scannedAll = dataservice.ScanRepo.FindScansForTest(test.TestID);
            return GetLatestScans(scannedAll);
        }


        /// <summary>
        /// find specific students (via PreslugData) who are preslugged but not scanned
        /// </summary>
        /// <param name="preslugged">set of preslugged data to search</param>
        /// <param name="scanned">set of scanned data, as a DataToGradeItemCollection</param>
        /// <returns>set of PreslugData containing students who are preslugged but not scanned</returns>
        public static PreslugData GetPresluggedNotScanned(PreslugData preslugged,
            DataToGradeItemCollection scanned)
        {
            PreslugData finalData = new PreslugData();

            foreach (var preslugItem in preslugged.GetItems())
                if (scanned.GetItemsWhere(s => s.StudentID == preslugItem.StudentID).Count() == 0)
                    if (finalData.GetItemsWhere(d => d.StudentID == preslugItem.StudentID).Count() == 0)
                        finalData.Add(preslugItem);

            return finalData;
        }



        #region private

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
                    var latestScan = scans.Where(s => s.DateScanned == scans.Select(ss => ss.DateScanned)
                                                                            .Max())
                                          .First();
                    finalData.Add(latestScan);
                }
                else
                {
                    finalData.Add(scans.First());
                }
            }

            return finalData.AsQueryable();
        }


        /// <summary>
        /// filters a list of scans for only those applicable to a specific school 
        /// and a list of teachers & periods
        /// </summary>
        /// <param name="dataservice">IRepo access to data</param>
        /// <param name="scanDataCurTest">all scans for a test</param>
        /// <param name="rosterData">a set of all roster data for the desired school</param>
        /// <param name="teachersperiods">a list of TeacherPeriod data</param>
        /// <returns>IQueryable-Scan- set of data</returns>
        private static IQueryable<Scan> FilterScans(IRepoService dataservice, IQueryable<Scan> scanDataCurTest,
            IQueryable<Roster> rosterData, List<TeacherPeriodItem> teachersperiods)
        {
            HashSet<Scan> finalData = new HashSet<Scan>();

            #region A_enumerate_scans
            //// get Scan data + Roster data for students that have scans
            //var scanDataWithRosterData =
            //    from scan in scanDataCurTest
            //    join roster in rosterData on dataservice.StudentIDString(scan.StudentID) equals roster.StudentID
            //        into scanRosterData
            //    select new { Scandata = scan, RosterData = scanRosterData };

            //// select just the scan data for students who have the applicable teachers, periods, semester
            //bool found = false;
            //foreach (var scanrosterdataItem in scanDataWithRosterData)
            //{
            //    found = false;
            //    foreach (var rosterItem in scanrosterdataItem.RosterData)
            //    {
            //        if (teachersperiods.Where(tp => tp.Teacher == rosterItem.TeacherName &&
            //                                        tp.Period == rosterItem.Period &&
            //                                        (tp.Semester == rosterItem.Semester ||
            //                                                        rosterItem.Semester == "4"))
            //                           .Count() > 0)
            //        {
            //            finalData.Add(scanrosterdataItem.Scandata);
            //            found = true;
            //        }
            //        if (found) break;
            //    }
            //}
            #endregion

            #region B_enumerate_teachersperiods
            string semester = teachersperiods[0].Semester;
            foreach (string curTeacher in teachersperiods.Select(tp => tp.Teacher).Distinct())
            {
                var curPeriods = teachersperiods.Where(tp => tp.Teacher == curTeacher).Select(tp => tp.Period).Distinct();
                foreach (string curPeriod in curPeriods)
                {
                    // get scan + roster data for students who have this teacher and period and semester
                    var rosterDataCurTchPer = rosterData.Where(rd => rd.TeacherName == curTeacher &&
                                                              rd.Period == curPeriod &&
                                                              (rd.Semester == semester ||
                                                               rd.Semester == "4"));
                    var scanDataMatchingTchPer =
                        from roster in rosterDataCurTchPer
                        join scan in scanDataCurTest
                            on roster.StudentID equals dataservice.StudentIDString(scan.StudentID)
                            into scanRosterData
                        select new { Scans = scanRosterData };

                    // we want each of these scan items
                    foreach (var foundData in scanDataMatchingTchPer)
                    {
                        foreach (Scan scan in foundData.Scans)
                            finalData.Add(scan);
                    }
                }
            }
            #endregion

            return finalData.AsQueryable();
        }



        private static IQueryable<Scan> FilterScans(IRepoService dataservice, IQueryable<Scan> scanDataCurTest,
            IQueryable<Roster> rosterData)
        {
            HashSet<Scan> finalData = new HashSet<Scan>();

            //var scanDataWithRosterData =
            //    from scan in scanDataCurTest
            //    join roster in rosterData on dataservice.StudentIDString(scan.StudentID) equals roster.StudentID
            //        into scanRosterData
            //    select new { Scandata = scan, RosterData = scanRosterData };

            //foreach (var scanrosterdataitem in scanDataWithRosterData)
            //{
            //    if (finalData.Where(fd => fd.StudentID == scanrosterdataitem.Scandata.StudentID).Count() == 0)
            //        finalData.Add(scanrosterdataitem.Scandata);
            //}

            foreach (var scan in scanDataCurTest)
            {
                if (rosterData.Where(rd => rd.StudentID == dataservice.StudentIDString(scan.StudentID)).Count() > 0)
                    finalData.Add(scan);
            }

            return finalData.AsQueryable();
        }



        private static List<TeacherPeriodItem> GetTeacherPeriodList(PreslugData preslugData, string semester)
        {
            List<TeacherPeriodItem> finalData = new List<TeacherPeriodItem>();
            string schoolAbbr = preslugData.Idx(0).Campus;
            foreach (var teacher in preslugData.GetItems().Select(ps => ps.TeacherName).Distinct())
            {
                var preslugDataCurTch = preslugData.GetItemsWhere(ps => ps.TeacherName == teacher);
                foreach (var period in preslugDataCurTch.Select(ps => ps.Period).Distinct())
                {
                    finalData.Add(new TeacherPeriodItem(teacher, period, semester));
                }
            }

            return finalData;
        }



        private class TeacherPeriodItem
        {
            public string Teacher { get; set; }
            public string Period { get; set; }
            public string Semester { get; set; }

            public TeacherPeriodItem(string teacher, string period, string semester)
            {
                Teacher = teacher;
                Period = period;
                Semester = semester;
            }
        }

        #endregion

    }
}