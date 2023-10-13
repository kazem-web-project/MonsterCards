using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using static Server.Router;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SWEN1_Server_HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Our first simplte HTTP-Server");

            //Creates Server on Port 8080, loopback for localhost
            var httpServer = new TcpListener(IPAddress.Loopback, 10001);
            httpServer.Start();

            while (true)
            {
                //Creates Socket and initializes Streams
                var clientSocket = httpServer.AcceptTcpClient();
                using var writer = new StreamWriter(clientSocket.GetStream()) { AutoFlush = true };
                using var reader = new StreamReader(clientSocket.GetStream());
                // uri
                string uri = "";

                //reading the requests
                // string? line;
                bool isBody = false;
                int content_length = 0;
                bool finishRequest = false;
                //while ((line = reader.ReadLine()) != null)
                while (!finishRequest)
                {
                    string line = reader.ReadLine();
                    if (null == line || "" == line) break;

                    var firstLine = line.Split(" ");

                    // string method = line.Split(" ")[0];
                    string authorizaiton = "";
                    string contentType = "";
                    var data = new StringBuilder(200);
                    switch (firstLine[0])
                    {
                        case "POST":
                            
                            finishRequest=post(reader, firstLine[1]);
                            break;
                            
                        case "GET":                            
                            finishRequest = get(reader, firstLine[1]);
                            break;
                        case "PUT":
                            
                            finishRequest = put(reader, firstLine[1]);
                            break;



                        case "DELETE":
                            finishRequest = del(reader, firstLine[1]);

                            break;


                        default:
                            // code block                            
                            continue;

                    }
                    // Console.WriteLine("URI ------------------------" + uri);
                    // Console.WriteLine(line);

                    //write the HTTP response
                    //writer.WriteLine("HTTP/1.1 200 OK");

                    //writer.WriteLine("Content-Type: text/html; charset=utf-8");
                    //writer.WriteLine();
                    //writer.WriteLine("<html><body><h1>Hello World!</h1></html></body>");

                }



            }

        }

        static bool get(StreamReader reader, string geturi)
        {
            // code block
            string authorizaiton = "";
            string? getLine;
            
            while ((getLine = reader.ReadLine()) != null)
            {
                if (getLine == "") break;
                // Console.WriteLine(line);
                string[] header = getLine.Split(": ");
                if (header[0] == "Authorization")
                {
                    authorizaiton = header[1];
                    break;
                }
            }

            Server.Router.getRoute(geturi, authorizaiton);
            return true;
        }

        static bool post(StreamReader reader, string pushuri)
        {

            // code block
            string authorizaiton = "";
            string contentType = "";
            var data = new StringBuilder(200);
            bool isBody = false;
            int content_length = 0;            
            Console.WriteLine("POST REQUEST");
            
            string? postLine;
            while ((postLine = reader.ReadLine()) != null)
            {
                //postLine = reader.ReadLine();
                if (null == postLine || "" == postLine) break;
                //Console.WriteLine(postLine);
                string[] header = postLine.Split(": ");
                if (header[0] == "Authorization")
                {
                    authorizaiton = header[1];
                    //break;
                }
                //Console.WriteLine(postLine);

                // check the body:
                if (header[0] == "Content-Type")
                {
                    contentType = header[1];
                    //break;
                }

                if (!isBody)
                {
                    var parts = postLine.Split(':');
                    if (parts.Length == 2 && parts[0] == "Content-Length")
                    {
                        //Console.WriteLine($"Host: {parts[1]}");
                        content_length = int.Parse(parts[1].Trim());
                        if (content_length == 0) break;
                    }
                }
                //read body if existing
                if (content_length > 0)
                {
                    char[] chars = new char[1024];
                    int bytesReadTotal = 0;
                    while (bytesReadTotal < content_length)
                    {
                        var bytesRead = reader.Read(chars, 0, chars.Length);
                        bytesReadTotal += bytesRead;
                        if (bytesRead == 0)
                            break;
                        data.Append(chars, 0, bytesRead);
                    }

                    //Console.WriteLine(data.ToString());
                    break;
                }
                //break;

            }
            Server.Router.postRoute(pushuri, authorizaiton, contentType, data);
            return true;
        }

        static bool put(StreamReader reader, string pushuri)
        {
            // code block
            //Console.WriteLine("PUT REQUEST");
            //uri = firstLine[1];
            //finishRequest = true;

            //break;
            string authorizaiton = "";
            string contentType = "";
            var data = new StringBuilder(200);
            bool isBody = false;
            int content_length = 0;
            // code block
            Console.WriteLine("PUT REQUEST");
            string? putLine;
            while ((putLine = reader.ReadLine()) != null)
            {
                //postLine = reader.ReadLine();
                if (null == putLine || "" == putLine) break;
                //Console.WriteLine(putLine);
                string[] header = putLine.Split(": ");
                if (header[0] == "Authorization")
                {
                    authorizaiton = header[1];
                    //break;
                }
                //Console.WriteLine(putLine);

                // check the body:
                if (header[0] == "Content-Type")
                {
                    contentType = header[1];
                    //break;
                }

                if (!isBody)
                {
                    var parts = putLine.Split(':');
                    if (parts.Length == 2 && parts[0] == "Content-Length")
                    {
                        //Console.WriteLine($"Host: {parts[1]}");
                        content_length = int.Parse(parts[1].Trim());

                    }
                }
                //read body if existing
                if (content_length > 0)
                {
                    char[] chars = new char[1024];
                    int bytesReadTotal = 0;
                    while (bytesReadTotal < content_length)
                    {
                        var bytesRead = reader.Read(chars, 0, chars.Length);
                        bytesReadTotal += bytesRead;
                        if (bytesRead == 0)
                            break;
                        data.Append(chars, 0, bytesRead);
                    }

                    //Console.WriteLine(data.ToString());
                    break;
                }
                //break;

            }
            Server.Router.putRoute(pushuri, authorizaiton, contentType, data);
            return true;
        }
        static bool del(StreamReader reader, string deluri)
        {
            
            string authorizaiton = "";
            string? delLine;
            while ((delLine = reader.ReadLine()) != null)
            {
                // Console.WriteLine(line);
                string[] header = delLine.Split(": ");
                if (header[0] == "Authorization")
                {
                    authorizaiton = header[1];
                    break;
                }
                if (header[0] == "Content-Type")
                {
                    authorizaiton = header[1];
                    break;
                }
            }
            Server.Router.deleteRoute(deluri, authorizaiton);
            return true;
            // continue; f
        }
    }
}