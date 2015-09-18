using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SedcServer;
using SedcServer.ActionManagers;

namespace SqlDataProvider
{
    public class TableActionManager : IActionManager
    {
        public string ActionName
        {
            get { return "table"; }
        }

        public ProcessResult Process(RequestParser parser)
        {
            var tableName = parser.Parameter;
            var tableNames = DataProvider.GetTableNames();
            if (!tableNames.Contains(tableName))
            {
                string badResponse = "<h1>Please don't do that. There is not a table named " + tableName + "</h1>";
                badResponse += "<p>There is a action listTables that lists all available tables</p>";
                var badByteResponse = Encoding.UTF8.GetBytes(badResponse);

                return new ProcessResult
                {
                    StatusCode = StatusCode.NotFound,
                    Bytes = badByteResponse,
                    ContentType = "text/html"
                };
            }

            var data = DataProvider.GetTableData(tableName);
            var columns = DataProvider.GetTableColumns(tableName);

            StringBuilder response = new StringBuilder();

            response.Append("[");
            foreach (var row in data)
            {
                response.Append("{");
                foreach (var column in columns)
                {
                    var name = column.Value;
                    var value = row[column.Key - 1];
                    response.AppendFormat("\"{0}\":\"{1}\",", name, value);
                }
                response.Append("},");
            }
            response.Append("]");
            var byteResponse = Encoding.UTF8.GetBytes(response.ToString());
            return new ProcessResult
            {
                StatusCode = StatusCode.OK,
                Bytes = byteResponse,
                ContentType = "application/json"
            };
        }
    }
}
