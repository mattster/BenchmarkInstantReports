using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    public class ScanReportData : DataItemCollection<ScanReportItem>
    {
        public ScanReportData() : base() { }

        public ScanReportData(List<ScanReportItem> items) : base(items) { }

        public void AddOrMerge(ScanReportItem item)
        {
            int curNumScanned = new int();
            int curNumQueried = new int();

            curNumScanned = item.NumScanned;
            curNumQueried = item.NumQueried;

            // do we already have this row in the results table?
            List<ScanReportItem> foundItems = this.GetItemsWhere(i =>
                    i.Campus == item.Campus &&
                    i.TestID == item.TestID &&
                    i.Teacher == item.Teacher &&
                    i.Period == item.Period).ToList();

            if (foundItems.Count > 0)
            {
                for (int i = 0; i < foundItems.Count; i++)
                {
                    // there are rows here - get the values then delete it
                    curNumScanned += foundItems[i].NumScanned;
                    curNumQueried += foundItems[i].NumQueried;
                    this.Remove(foundItems[i]);
                }
            }
            else
            {
                // this row is not yet here - we don't need to do anything
            }

            // add the row to the results table
            ScanReportItem newItem = new ScanReportItem();
            newItem.Campus = item.Campus;
            newItem.TestID = item.TestID;
            newItem.Teacher = item.Teacher;
            newItem.Period = item.Period;
            newItem.NumScanned = curNumScanned;
            newItem.NumQueried = curNumQueried;

            this.Add(newItem);

            return;

        }
    }
}