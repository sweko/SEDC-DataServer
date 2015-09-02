using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SedcServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ushort port = 8081;
            WebServer ws = new WebServer(port);
            ws.Start();
            ws.Start();
        }
    }
}
