using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SedcServer
{
    public class RequestHandler
    {
        public Socket ServerSocket { get; private set; }

        public RequestHandler(Socket socket)
        {
            ServerSocket = socket;
        }

        internal async void AcceptRequest()
        {
            Socket clientSocket = null;
            try
            {
                // Create new thread to handle the request and continue to listen the socket.
                clientSocket = ServerSocket.Accept();
                clientSocket.ReceiveTimeout = ServerSocket.ReceiveTimeout;
                clientSocket.SendTimeout = ServerSocket.SendTimeout;

                await Task.Run(() => HandleRequest(clientSocket));
                //var requestHandler = new Thread(() => HandleTheRequest(clientSocket));
                //requestHandler.Start();
                //HandleTheRequest(clientSocket);
            }
            catch
            {
                Console.WriteLine("Error in accepting client request");
                if (clientSocket != null)
                    clientSocket.Close();
            }
        }

        private void HandleRequest(Socket clientSocket)
        {
            //Thread.Sleep(5000);
            //Console.WriteLine("Server request at {0}", DateTime.UtcNow);
            string requestString = DecodeRequest(clientSocket);
            RequestParser parser = new RequestParser(requestString);

            Console.WriteLine("Received request {0}", requestString);

            RequestProcessor processor = new RequestProcessor(parser);
            var result = processor.ProcessRequest();

            byte[] byteHeader = CreateHeader(result.StatusCode, result.Bytes.Length, result.ContentType);
            clientSocket.Send(byteHeader);
            clientSocket.Send(result.Bytes);
            clientSocket.Close();
        }

        public void StopClientSocket(Socket clientSocket)
        {
            if (clientSocket != null)
                clientSocket.Close();
        }

        private string DecodeRequest(Socket clientSocket)
        {
            var receivedBufferlen = 0;
            var buffer = new byte[10240];
            try
            {
                receivedBufferlen = clientSocket.Receive(buffer);
            }
            catch (Exception)
            {
                Console.WriteLine("Error decoding request");
            }
            return Encoding.UTF8.GetString(buffer, 0, receivedBufferlen);
        }

        private byte[] CreateHeader(StatusCode responseCode, int contentLength, string contentType)
        {
            var statusCode = (int) responseCode;

            return Encoding.UTF8.GetBytes("HTTP/1.1 " + statusCode + "\r\n"
                                  + "Server: SEDC Data Web Server\r\n"
                                  + "Content-Length: " + contentLength + "\r\n"
                                  + "Connection: close\r\n"
                                  + "Content-Type: " + contentType + "\r\n\r\n");
        }
    }
}
