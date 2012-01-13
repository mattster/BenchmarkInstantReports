using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    public class ResultsTableData
    {
        private List<ResultsTableItem> _items;

        public int Count { get { return _items.Count; } }

        public ResultsTableData()
        {
            _items = new List<ResultsTableItem>();
        }

        public ResultsTableData(List<ResultsTableItem> items)
        {
            _items = items;
        }

        public ResultsTableData(IEnumerable<ResultsTableItem> items)
        {
            _items = items.ToList<ResultsTableItem>();
        }

        public ResultsTableItem Idx(int i)
        {
            try
            {
                return _items[0];
            }
            catch
            {
                return null;
            }
        }

        public ResultsTableItem First()
        {
            return Idx(0);
        }

        public void Add(ResultsTableItem item)
        {
            _items.Add(item);
        }

        public void Remove(ResultsTableItem item)
        {
            _items.Remove(item);
        }

        public ResultsTableItem GetItemWhere(Func<ResultsTableItem, bool> predicate)
        {
            return _items.Where(predicate).FirstOrDefault();
        }

        public IEnumerable<ResultsTableItem> GetItems()
        {
            return _items;
        }

        public IEnumerable<ResultsTableItem> GetItemsWhere(Func<ResultsTableItem, bool> predicate)
        {
            return _items.Where(predicate);
        }
    }
}
