using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SedcServer;
using SedcServer.ActionManagers;

namespace SqlDataProvider
{
    public class ListTableActionManager : IActionManager
    {
        public string ActionName
        {
            get { return "listTables"; }
        }

        public ProcessResult Process(RequestParser parser)
        {
            var result = DataProvider.GetTableNames();

            //poor-man's json
            var json = string.Format("[\"{0}\"]", string.Join("\", \"", result));
            var byteResponse = Encoding.UTF8.GetBytes(json);

            return new ProcessResult
            {
                StatusCode = StatusCode.OK,
                Bytes = byteResponse,
                ContentType = "application/json"
            };
        }

    }
}
