using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SedcServer
{
    public class RequestHandler
    {
        public Socket ServerSocket { get; private set; }

        public RequestHandler(Socket socket)
        {
            ServerSocket = socket;
        }

        internal void AcceptRequest()
        {
            Socket clientSocket = null;
            try
            {
                // Create new thread to handle the request and continue to listen the socket.
                clientSocket = ServerSocket.Accept();

                //var requestHandler = new Thread(() =>
                //{
                    clientSocket.ReceiveTimeout = ServerSocket.ReceiveTimeout;
                    clientSocket.SendTimeout = ServerSocket.SendTimeout;
                    HandleTheRequest(clientSocket);
                //});
                //requestHandler.Start();
            }
            catch
            {
                Console.WriteLine("Error in accepting client request");
                Console.ReadLine();
                if (clientSocket != null)
                    clientSocket.Close();
            }
        }

        private void HandleTheRequest(Socket clientSocket)
        {
            string requestString = DecodeRequest(clientSocket);

            string response = "<h1>Hello World!<h1>";
            var byteResponse = Encoding.UTF8.GetBytes(response);
            byte[] byteHeader = CreateHeader("200", byteResponse.Length, "text/html");
            clientSocket.Send(byteHeader);
            clientSocket.Send(byteResponse);
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
                //Console.WriteLine("buffer full");
                Console.ReadLine();
            }
            return Encoding.UTF8.GetString(buffer, 0, receivedBufferlen);
        }

        private byte[] CreateHeader(string responseCode, int contentLength, string contentType)
        {
            return Encoding.UTF8.GetBytes("HTTP/1.1 " + responseCode + "\r\n"
                                  + "Server: SEDC Data Web Server\r\n"
                                  + "Content-Length: " + contentLength + "\r\n"
                                  + "Connection: close\r\n"
                                  + "Content-Type: " + contentType + "\r\n\r\n");
        }
    }
}
