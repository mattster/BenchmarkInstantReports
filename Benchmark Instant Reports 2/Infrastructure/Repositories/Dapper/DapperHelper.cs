using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OracleClient;
using System.Data.SqlClient;
using Benchmark_Instant_Reports_2.References;
using Dapper;
using Microsoft.CSharp.RuntimeBinder;

namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    public class DapperHelper
    {
        public static IEnumerable<T> DQuery<T>(string qs, object parms = null, SqlTransaction transaction = null,
            bool buffered = true)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[DatabaseDefn.databaseName].ConnectionString;
            using (OracleConnection connection =
                new OracleConnection(connectionString))
            {
                connection.Open();
                var results = connection.Query<T>(qs, parms, transaction, buffered);
                return results;
            }
        }



        public static IEnumerable<dynamic> DQuery(string qs, object parms = null, SqlTransaction transaction = null,
            bool buffered = true)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[DatabaseDefn.databaseName].ConnectionString;
            using (OracleConnection connection =
                new OracleConnection(connectionString))
            {
                connection.Open();
                var results = connection.Query(qs, parms, transaction, buffered);
                return results;
            }
        }



        public static void AssignPropertySafely(object itemTo, dynamic itemFrom, string fieldName)
        {
            //var t = itemTo.GetType();
            //itemTo = GetPropertySafely(itemFrom, fieldName, itemTo.GetType());

            //try
            //{
            //    //var prop = (itemTo.GetType()).GetProperty(fieldName);
            //    var x = (itemTo.GetType()).GetProperties();
            //    var value = prop.GetValue(itemFrom, null);
            //    itemTo = value;
            //}
            ////catch (RuntimeBinderException) { itemTo = null; }
            //catch (NullReferenceException) { itemTo = null; }
            //catch { itemTo = null; }

            var props = (itemTo.GetType()).GetProperties();
            foreach (var prop in props)
            {
                if (prop.Name == fieldName)
                {
                    itemTo = prop.GetValue(itemFrom, null);
                    return;
                }
            }

            itemTo = null;
            return;
        }






        private static object GetPropertySafely(object o, string fieldname, Type T)
        {
            try
            {
                // this field is present - return its 
                var prop = T.GetProperty(fieldname);
                var value = prop.GetValue(o, null);
                return value;
            }
            catch (Exception ex)
            {
                if (ex is RuntimeBinderException ||
                    ex is NullReferenceException)
                {
                    // field is not present - return the default for type T
                    if (T.IsValueType)
                        return Activator.CreateInstance(T);
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }



    }
}