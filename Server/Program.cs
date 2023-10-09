using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SWEN1_Server_HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Our first simplte HTTP-Server");

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
                string uri="";

                //reading the requests
                string? line;
                bool isBody = false;
                int content_length = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    var firstLine = line.Split(" ");
                    // string method = line.Split(" ")[0];
                   
                    switch (firstLine[0])
                    {
                        case "POST":
                            // code block
                            Console.WriteLine("POST REQUEST");
                            uri = firstLine[1];
                            break;
                        case "GET":
                            // code block
                            Console.WriteLine("Get REQUEST");
                            uri = firstLine[1];
                            if (uri.Contains('?')) { 
                                Console.WriteLine("THERE ARE SOME PARAMETERS");
                                var parameterkey = uri.Split("?")[1].Split("=");
                            }

                            break;
                        case "PUT":
                            // code block
                            Console.WriteLine("PUT REQUEST");
                            uri = firstLine[1];

                            break;
                        case "DELETE":
                            // code block
                            Console.WriteLine("DELETE REQUEST");
                            uri = firstLine[1];

                            break;

                        default:
                            // code block                            
                            break;
                        
                    }
                    Console.WriteLine("URI ------------------------"+uri);
                    // Console.WriteLine(line);
                    if (line == "")
                    {
                        isBody = true;
                        break;
                    }

                    if (!isBody)
                    {
                        var parts = line.Split(':');
                        if (parts.Length == 2 && parts[0] == "Content-Length")
                        {
                            Console.WriteLine($"Host: {parts[1]}");
                            content_length = int.Parse(parts[1].Trim());
                            
                        }
                    }
                    else
                    {

                    }
                }

                //read body if existing
                if (content_length > 0)
                {
                    var data = new StringBuilder(200);
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
                    Console.WriteLine(data.ToString());
                }


                //write the HTTP response
                writer.WriteLine("HTTP/1.1 200 OK");

                writer.WriteLine("Content-Type: text/html; charset=utf-8");
                writer.WriteLine();
                writer.WriteLine("<html><body><h1>Hello World!</h1></html></body>");
            }

        }
    }
}