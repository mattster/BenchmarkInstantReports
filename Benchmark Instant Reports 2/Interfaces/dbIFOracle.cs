using System;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Web.UI.WebControls;


namespace Benchmark_Instant_Reports_2.Interfaces
{
    public class dbIFOracle
    {
        public static string databaseName = "SISPRODOracleDb";             // used in the Connection String



        //**********************************************************************//
        //** creates an Oracle data adapter to the Oracle database using the 
        //** specified query string; returnes a DataSet populated with the 
        //** results of the query
        //**
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





        //**********************************************************************//
        //** creates a datasource to the Oracle database using the 
        //** specified query string; returnes an SqlDataSource object
        //** ****** used for binding to form controls ******
        //**
        public static SqlDataSource getDataSource(string theQuery)
        {
            try
            {
                // Connect to the database and run the query.           
                SqlDataSource dbConnection = new SqlDataSource();
                dbConnection.ConnectionString = ConfigurationManager.ConnectionStrings[databaseName].ConnectionString;
                dbConnection.ProviderName = ConfigurationManager.ConnectionStrings[databaseName].ProviderName;
                dbConnection.SelectCommand = theQuery;
                dbConnection.DataSourceMode = SqlDataSourceMode.DataReader;

                return (dbConnection);
            }

            catch (Exception ex)
            {
                // The connection failed. Display an error message.
                Console.WriteLine("Unable to connect to the database: {0}\n", ex.ToString());
            }

            return null;
        }
    }
}