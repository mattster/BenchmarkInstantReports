using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    /// <summary>
    /// Answer Key data item;
    /// inherit from IComparable in order to implement CompareTo, for easy sorting
    /// </summary>
    public class AnswerKeyItem : IComparable<AnswerKeyItem>
    {
        public string TestID { get; set; }      // key
        public int ItemNum { get; set; }        // key
        public string Campus { get; set; }
        public string Answer { get; set; }
        public int Category { get; set; }
        public string TEKS { get; set; }
        public double Weight { get; set; }

        /// <summary>
        /// default constructor - empty / blank / zero items
        /// </summary>
        public AnswerKeyItem()
        {
            TestID = "";
            ItemNum = 0;
            Answer = "";
            Category = 0;
            TEKS = "";
            Weight = 1;
        }

        /// <summary>
        /// constructor to create a new AnswerKeyItem from an AnswerKey object
        /// </summary>
        /// <param name="districtAnsKey">AnswerKey object, e.g. a district key item</param>
        public AnswerKeyItem(AnswerKey districtAnsKey)
        {
            TestID = districtAnsKey.TestID;
            ItemNum = districtAnsKey.ItemNum;
            Answer = districtAnsKey.Answer;
            Category = districtAnsKey.Objective;
            TEKS = districtAnsKey.TEKS;
            Weight = districtAnsKey.Weight;
        }

        /// <summary>
        /// constructor to create a new AnswerKeyCampus Item from an AnswerKeyCampus object
        /// </summary>
        /// <param name="campusAnsKey">AnswerKeyCampus object, e.g. a campus key item</param>
        public AnswerKeyItem(AnswerKeyCampus campusAnsKey)
        {
            TestID = campusAnsKey.TestID;
            ItemNum = campusAnsKey.ItemNum;
            Answer = campusAnsKey.Answer;
            Category = campusAnsKey.Objective;
            TEKS = campusAnsKey.TEKS;
            Weight = campusAnsKey.Weight;
        }

        // implement IComparable.CompareTo in order to Sort a list
        /// <summary>
        /// implements IComparable.CompareTo in order to Sort on ItemNum
        /// </summary>
        /// <param name="other">an AnserKeyItem to compare against this one</param>
        /// <returns>less than zero if this ItemNum is less that the compared ItemNum
        ///          0 if the ItemNum of this and the compared items are equal
        ///          greater than zero if this ItemNum is greater than the compared ItemNum</returns>
        public int CompareTo(AnswerKeyItem other)
        {
            if (other == null) return 1;

            return ItemNum.CompareTo(other.ItemNum);
        }
    }


    /// <summary>
    /// the multi-part key to be used in a hashtable
    /// </summary>
    public class AnswerKeyItemKey : ClassKey<AnswerKeyItem>
    {
        /// <summary>
        /// create a hash key based on an instance of AnswerKeyItem
        /// </summary>
        /// <param name="ClassReference">instance of an AnswerKeyItem</param>
        public AnswerKeyItemKey(AnswerKeyItem ClassReference) : base(ClassReference) { }

        /// <summary>
        /// return the list of values in the key
        /// </summary>
        /// <returns>an array of objects that contain the values of the actual fields used in the key</returns>
        public override object[] GetKeyValues()
        {
            return new object[] 
            {
                ClassReference.TestID,
                ClassReference.ItemNum
            };
        }
    }


}