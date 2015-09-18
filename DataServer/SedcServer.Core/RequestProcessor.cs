using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SedcServer.ActionManagers;

namespace SedcServer
{
    public class RequestProcessor
    {
        private RequestParser parser;

        public RequestProcessor(RequestParser parser)
        {
            this.parser = parser;
        }

        internal ProcessResult ProcessRequest()
        {
            byte[] byteResponse = new byte[0];
            StatusCode statusCode = StatusCode.OK;
            string contentType = "text/html";
            switch (parser.Kind)
            {
                case RequestKind.Action:
                {
                    IActionManager manager = ActionManagerFactory.GetManager(parser);
                    return manager.Process(parser);
                }
                case RequestKind.File:
                    if (File.Exists(parser.FileName))
                    {
                        switch (parser.Extension)
                        {
                            case "ico":
                                {
                                    byteResponse = File.ReadAllBytes(parser.FileName);
                                    contentType = "image/ico";
                                    break;
                                }
                            case "html":
                                {
                                    byteResponse = File.ReadAllBytes(parser.FileName);
                                    break;
                                }
                            default:
                                {
                                    byteResponse = File.ReadAllBytes(parser.FileName);
                                    contentType = "text/plain";
                                    break;
                                }
                        }
                    }
                    else
                    {
                        statusCode = StatusCode.NotFound;
                    }
                    break;
                case RequestKind.Error:
                    statusCode = StatusCode.NotImplemented;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            return new ProcessResult
            {
                StatusCode = statusCode,
                Bytes = byteResponse,
                ContentType = contentType
            };
        }
    }
}
