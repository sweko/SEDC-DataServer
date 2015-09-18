using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SedcServer
{
    public class ProcessResult
    {
        public StatusCode StatusCode { get; set; }

        public byte[] Bytes { get; set; }

        public string ContentType { get; set; }
    }
}
