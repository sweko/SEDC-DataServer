using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SedcServer
{
    public enum StatusCode
    {
        OK = 200,
        NotFound = 404,
        ServerError = 500,
        NotImplemented = 501,
    }
}
