using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;

namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    public class ODAHelper
    {
        private static string databaseName = "SISPRODOracleDb";             // used in the Connection String

        /// <summary>
        /// returns a DataSet using an OracleDataAdapter executing a query string
        /// </summary>
        /// <param name="queryString">SQL query string to execute</param>
        /// <returns>DataSet with the results of the query</returns>
        public static DataSet getDataRows(string queryString)
        {
            DataSet ds = new DataSet();

            string connectionString = ConfigurationManager.ConnectionStrings[databaseName].ConnectionString;
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
    }
}