using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Strarting the server");
            HttpServer server = new HttpServer(8000);
            server.Start();
        }
    }
}
