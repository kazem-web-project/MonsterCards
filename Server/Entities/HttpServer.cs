using Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities
{
    internal class HttpServer
    {
        private readonly int port = 10001;
        private readonly IPAddress ip = IPAddress.Loopback;
        private TcpListener tcpListener;

        public Dictionary<string, IHttpEndpoint> Endpoints { get; private set; } = new Dictionary<string, IHttpEndpoint>();
        public HttpServer(IPAddress ip, int port) {
            this.port = port;
            this.ip = ip;

            tcpListener = new TcpListener(ip, port);
        }

        public void Run()
        {
            tcpListener.Start();
            while (true)
            {
                var clientSocket = tcpListener.AcceptTcpClient();
                var httpProcessor = new HttpProcessor(this, clientSocket);
                httpProcessor.Process();
            }
        }

        public void RegisterEndpoint(string path, IHttpEndpoint endpoint)
        {
            Endpoints.Add(path, endpoint);
        }





    }
}
