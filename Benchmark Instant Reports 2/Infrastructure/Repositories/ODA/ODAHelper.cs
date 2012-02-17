using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    /// <summary>
    /// helper methods enabling access to the Oracle Database
    /// </summary>
    public class ODAHelper
    {
        /// <summary>
        /// returns a DataSet using an OracleDataAdapter executing a query string
        /// </summary>
        /// <param name="queryString">SQL query string to execute</param>
        /// <returns>DataSet with the results of the query</returns>
        public static DataSet getDataRows(string queryString)
        {
            DataSet ds = new DataSet();

            string connectionString = ConfigurationManager.ConnectionStrings[DatabaseDefn.databaseName].ConnectionString;
            using (OracleConnection connection =
                new OracleConnection(connectionString))
            {
                OracleDataAdapter adapter = new OracleDataAdapter();
                adapter.SelectCommand = new OracleCommand(queryString, connection);
                adapter.Fill(ds);
                return ds;
            }
        }



        /// <summary>
        /// returns a value from a DataRow at a named column if it exists; returns an empty string otherwise
        /// </summary>
        /// <param name="row">the DataRow to read</param>
        /// <param name="colName">the name of the column to return</param>
        /// <returns>the value in the named column as an object if it exists; an empty string otherwise</returns>
        public static object GetTableValueSafely(DataRow row, string colName)
        {
            if (row.Table.Columns.Contains(colName))
                return row[colName];

            return "";
        }


        /// <summary>
        /// properly formats a student ID string with leading zeroes as needed
        /// </summary>
        /// <param name="id">student ID as an integer</param>
        /// <returns>a string with leading zeroes as needed</returns>
        public static string StudentIDString(int id)
        {
            return string.Format("{0,6:D6}", id);
        }


        /// <summary>
        /// converts an integer student ID to a string with leading zeroes as needed
        /// </summary>
        /// <param name="id">student ID as a string; may or may not have leading zeroes</param>
        /// <returns>a string with leading zeroes as needed</returns>
        public static string StudentIDString(string id)
        {
            return string.Format("{0,6:D6}", id);
        }


        /// <summary>
        /// converts a student ID in a string to an integer
        /// </summary>
        /// <param name="id">student ID as a string</param>
        /// <returns>an integer representing the student ID</returns>
        public static int StudentIDInt(string id)
        {
            return int.Parse(id);
        }

    
    }
}