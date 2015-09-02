using System;

namespace SedcServer
{
    public class WebServer
    {
        public ushort Port { get; private set; }

        public WebServer(ushort port)
        {
            Port = port;
        }

        public string Start()
        {
            InitializeSocket();
            return string.Empty;
        }

        private void InitializeSocket()
        {
            throw new NotImplementedException();
        }
    }
}