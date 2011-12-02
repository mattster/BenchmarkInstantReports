using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Exceptions
{
    public class ExceptionItem
    {
        public string TestID { get; set; }
        public string[] Campuses { get; set; }
        public string[] Teachers { get; set; }
        public string[] Periods { get; set; }
        public int[] Items { get; set; }

        #region constructors
        public ExceptionItem(string testid, object campuses, object teachers, object periods, int[] items)
        {
            TestID = testid;

            if (campuses.GetType() == typeof(string[]))
                Campuses = campuses as string[];
            else
                Campuses = new string[] { campuses as string };


            if (teachers.GetType() == typeof(string[]))
                Teachers = teachers as string[];
            else
                Teachers = new string[] { teachers as string };


            if (periods.GetType() == typeof(string[]))
                Periods = periods as string[];
            else Periods = new string[] { periods as string };

            Items = items;
        }

        public ExceptionItem(string testid, object campuses, object teachers, object periods, int item)
        {
            TestID = testid;

            if (campuses.GetType() == typeof(string[]))
                Campuses = campuses as string[];
            else
                Campuses = new string[] { campuses as string };


            if (teachers.GetType() == typeof(string[]))
                Teachers = teachers as string[];
            else
                Teachers = new string[] { teachers as string };


            if (periods.GetType() == typeof(string[]))
                Periods = periods as string[];
            else Periods = new string[] { periods as string };

            Items = new int[] { item };
        }

        public ExceptionItem(string testid, object campuses, object teachers, int[] items)
        {
            TestID = testid;

            if (campuses.GetType() == typeof(string[]))
                Campuses = campuses as string[];
            else
                Campuses = new string[] { campuses as string };


            if (teachers.GetType() == typeof(string[]))
                Teachers = teachers as string[];
            else
                Teachers = new string[] { teachers as string };

            Periods = new string[] { "ALL" };
            Items = items;
        }

        public ExceptionItem(string testid, object campuses, object teachers, int item)
        {
            TestID = testid;

            if (campuses.GetType() == typeof(string[]))
                Campuses = campuses as string[];
            else
                Campuses = new string[] { campuses as string };


            if (teachers.GetType() == typeof(string[]))
                Teachers = teachers as string[];
            else
                Teachers = new string[] { teachers as string };


            Periods = new string[] { "ALL" };
            Items = new int[] { item };
        }

        public ExceptionItem(string testid, object campuses, int[] items)
        {
            TestID = testid;

            if (campuses.GetType() == typeof(string[]))
                Campuses = campuses as string[];
            else
                Campuses = new string[] { campuses as string };

            Teachers = new string[] { "ALL" };
            Periods = new string[] { "ALL" };
            Items = items;
        }

        public ExceptionItem(string testid, object campuses, int item)
        {
            TestID = testid;

            if (campuses.GetType() == typeof(string[]))
                Campuses = campuses as string[];
            else
                Campuses = new string[] { campuses as string };

            Teachers = new string[] { "ALL" };
            Periods = new string[] { "ALL" };
            Items = new int[] { item };
        }

        public ExceptionItem(string testid, int[] items)
        {
            TestID = testid;
            Campuses = new string[] { "ALL" };
            Teachers = new string[] { "ALL" };
            Periods = new string[] { "ALL" };
            Items = items;
        }

        public ExceptionItem(string testid, int item)
        {
            TestID = testid;
            Campuses = new string[] { "ALL" };
            Teachers = new string[] { "ALL" };
            Periods = new string[] { "ALL" };
            Items = new int[] { item };
        }
        #endregion


        #region utilities
        public bool Equals(string testID, string campus, int item)
        {
            if (TestID == TestID)
                if (Campuses.Contains<string>(campus) || Campuses.Contains<string>("ALL"))
                    if (Items.Contains<int>(item))
                        return true;

            return false;
        }

        #endregion
        
    }
}