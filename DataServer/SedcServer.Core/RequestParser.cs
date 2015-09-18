using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SedcServer
{
    public enum RequestKind
    {
        Action,
        File,
        Error,
    }

    public class RequestParser
    {
        public string RequestString { get; private set; }

        public List<Header> Headers { get; private set; }
        public string Method { get; private set; }
        public string PathString { get; private set; }
        public string Parameter { get; private set; }
        public string Action { get; private set; }
        public RequestKind Kind { get; private set; }
        public string Extension { get; private set; }
        public string FileName { get; private set; }

        public RequestParser(string requestString)
        {
            RequestString = requestString;
            Kind = RequestKind.Error;
            //parsing the request string
            Parse();
        }

        private void Parse()
        {
            var lines = RequestString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0)
                return;
            Headers = new List<Header>();
            for (int i = 1; i < lines.Length; i++)
            {
                Regex headerRegex = new Regex(@"^([a-zA-Z\-]*):\s(.*)$");
                var match = headerRegex.Match(lines[i]);
                Header header = new Header
                {
                    Name = match.Groups[1].Value,
                    Value = match.Groups[2].Value,
                };
                Headers.Add(header);
            }
            ParseMethod(lines[0]);
        }

        private void ParseMethod(string methodLine)
        {
            //GET /asdfsdaf HTTP/1.1
            Regex methodRegex = new Regex(@"^([A-Z]*)\s(\/.*) HTTP\/1.1$");
            var match = methodRegex.Match(methodLine);
            Method = match.Groups[1].Value;
            PathString = match.Groups[2].Value;

            ParsePathString();
        }

        private void ParsePathString()
        {
            Regex patRegex = new Regex(@"^\/([a-zA-Z-]*)\/?([a-zA-Z0-9-]*)$");
            var match = patRegex.Match(PathString);
            if (match.Success)
            {
                Kind = RequestKind.Action;
                Action = match.Groups[1].Value;
                Parameter = match.Groups[2].Value;
                return;
            }

            patRegex = new Regex(@"^\/(\w+).(\w+)$");
            match = patRegex.Match(PathString);
            if (match.Success)
            {
                Kind = RequestKind.File;
                FileName = string.Format("{0}.{1}", match.Groups[1].Value, match.Groups[2].Value);
                Extension = match.Groups[2].Value;
                return;
            }
            Kind = RequestKind.Error;
        }
    }
}
