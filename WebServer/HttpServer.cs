using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
namespace WebServer
{
    class HttpServer
    {
        public const string VERSION = "HTTP/1.1";
        public const string NAME = "Hacked Server Version 2.2";
        public const string MSG_DIR = "/root/msg/";
        public const string WEB_DIR = "/root/web";

        public bool running;
        private TcpListener listener;
        public HttpServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            running = false;
        }
        public void Start()
        {
            Thread serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
        }
        public void Run()
        {
            running = true;
            listener.Start();
            while (running){
                Console.WriteLine("Waiting for connection");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Connected");
                HandleClient(client);
            }
            //running = false;
            //listener.Stop();
        }
        public void HandleClient(TcpClient client)
        {
            StreamReader reader = new System.IO.StreamReader(client.GetStream());
            string msg = "";
            if(reader.Peek()!=-1)
            {
                msg += reader.ReadLine()+"\n";
            }
            Console.WriteLine("Request "+msg);
            Request request = Request.GetRequest(msg);
            Response response = Response.From(request);
            response.Post(client.GetStream());
            client.Close();
        }
    }
}
