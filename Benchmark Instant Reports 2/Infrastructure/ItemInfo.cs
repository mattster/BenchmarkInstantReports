
namespace Benchmark_Instant_Reports_2.Infrastructure
{
    /// <summary>
    /// used to store info about a set of responses (items);
    /// e.g. a list of student responses
    /// </summary>
    /// <typeparam name="T">type of info stored - string, bool, int, etc.</typeparam>
    public class ItemInfo<T>
    {
        public int ItemNum { get; set; }
        public T Info { get; set; }


        /// <summary>
        /// default constructor - default / 0 values
        /// </summary>
        public ItemInfo()
        {
            ItemNum = 0;
            Info = default(T);
        }


        /// <summary>
        /// constructor to set the to specific values
        /// </summary>
        /// <param name="itemnum">Item number</param>
        /// <param name="info">Info data of type T</param>
        public ItemInfo(int itemnum, T info)
        {
            ItemNum = itemnum;
            Info = info;
        }

    }
}