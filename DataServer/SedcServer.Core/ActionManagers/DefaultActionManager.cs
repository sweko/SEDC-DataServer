using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SedcServer.ActionManagers
{
    public class DefaultActionManager : IActionManager
    {
        public string ActionName
        {
            get { return string.Empty; }
        }

        public ProcessResult Process(RequestParser parser)
        {
            string response = "<h1>Unsupported action " + parser.Action + "</h1>";
            var byteResponse = Encoding.UTF8.GetBytes(response);
            return new ProcessResult
            {
                StatusCode = StatusCode.OK,
                Bytes = byteResponse,
                ContentType = "text/html"
            };
        }

    }
}
