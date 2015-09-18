using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataProvider
{
    internal class DataProvider
    {
        public static IEnumerable<string> GetTableNames()
        {
            string localConnectionString = ConfigurationManager.ConnectionStrings["local"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(localConnectionString))
            {
                connection.Open();
                List<string> result = new List<string>();
                var schema = connection.GetSchema("Tables");
                foreach (DataRow row in schema.Rows)
                {
                    var type = (string)row["TABLE_TYPE"];
                    if (type == "BASE TABLE")
                    {
                        var tableName = (string)row["TABLE_NAME"];
                        if (tableName == "sysdiagrams") continue;
                        result.Add(tableName);
                    }
                }
                return result;
            }
        }

        public static Dictionary<int, string> GetTableColumns(string tableName)
        {
            string localConnectionString = ConfigurationManager.ConnectionStrings["local"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(localConnectionString))
            {
                connection.Open();

                var result = new Dictionary<int, string>();
                var schema = connection.GetSchema("Columns");
                foreach (DataRow row in schema.Rows)
                {
                    if ((string)row["TABLE_NAME"] != tableName)
                        continue;
                    result.Add((int)row["ordinal_position"], (string)row["Column_NAME"]);
                }
                return result;
            }
        }

        internal static IEnumerable<object[]> GetTableData(string tableName)
        {
            string localConnectionString = ConfigurationManager.ConnectionStrings["local"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(localConnectionString))
            {
                connection.Open();
                var cmd = new SqlCommand("select * from [" + tableName+"]", connection);
                var reader = cmd.ExecuteReader();
                var result = new List<object[]>();
                while (reader.Read())
                {
                    var rowValues = new object[reader.FieldCount];
                    reader.GetValues(rowValues);
                    result.Add(rowValues);
                }
                return result;
            }

        }
    }
}
