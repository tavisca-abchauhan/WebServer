using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    class Response
    {
        private Byte[] data = null;
        private string status;
        private string mime;
        public Response(string status,string mime,Byte[] data)
        {
            this.status = status;
            this.data = data;
            this.mime = mime;

        }
        public static Response From(Request request)
        {
            if (request == null)
                return MakeNullRequest();
            if (request.Type == "GET")
            {
                string file = Environment.CurrentDirectory + HttpServer.WEB_DIR + request.URL;
                FileInfo fileinfo = new FileInfo(file);
                if(fileinfo.Exists&&fileinfo.Extension.Contains("."))
                {
                    return MakeFromFile(fileinfo);
                }
                else
                {
                    DirectoryInfo di = new DirectoryInfo(fileinfo+"/");
                    if (!di.Exists)
                        return MakePageNotFound();
                    FileInfo[] files = di.GetFiles();
                    foreach(FileInfo ff in files)
                    {
                        string n = ff.Name;
                        if (n.Contains("default.html") || n.Contains("default.htm") || n.Contains("index.htm") || n.Contains("index.html"))
                            return MakeFromFile(ff);
                    }
                }
               
            }
           
            else
                return MakeMethodNotAllowed();
            return MakePageNotFound();

        }
        private static Response MakeFromFile(FileInfo f)
        {
            
            FileStream fs = f.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();

            return new Response("200 OK", "text/html",d);
        }
        private static Response MakeNullRequest()
        {
            string file = Environment.CurrentDirectory+HttpServer.MSG_DIR+"400.html";
            FileInfo fileinfo = new FileInfo(file);
            FileStream fs = fileinfo.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("400 Bad Request", "text/html", d); 
        }

        private static Response MakePageNotFound()
        {
            string file = Environment.CurrentDirectory + HttpServer.MSG_DIR + "404.html";
            FileInfo fileinfo = new FileInfo(file);
            FileStream fs = fileinfo.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("404 Page Not Found", "text/html", d);
        }

        private static Response MakeMethodNotAllowed()
        {
            string file = Environment.CurrentDirectory + HttpServer.MSG_DIR + "405.html";
            FileInfo fileinfo = new FileInfo(file);
            FileStream fs = fileinfo.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("405 Method Not Allowed", "text/html",d);
        }
        public void Post(NetworkStream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(String.Format("{0}{1}\r\nServer:{2}\r\nContent-Type:{3}\r\nAccept_Ranges:bytes\r\nContent-Length:{4}\r\n", HttpServer.VERSION, status, HttpServer.NAME, mime, data.Length));
            writer.Flush();
            stream.Write(data,0,data.Length);
        }

    }
}
