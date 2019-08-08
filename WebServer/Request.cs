using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    class Request
    {
        public string Type { get; set; }
        public string URL { get; set; }
        public string Host{ get; set; }
        public Request(string type,string url,string host)
        {
            Type = type;
            URL = url;
            Host = host;
        }
        public static Request GetRequest(string request)
        {
            if(string.IsNullOrEmpty(request))
            {
                return null;

            }
            string[] tokens = request.Split(' ');
            string type = tokens[0];
            string url = tokens[1];
            string host = "localhost";

            return new Request(type,url,host);
        }
        
    }
}
