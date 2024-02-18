using MonsterCards.Application;
using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Entities.Server;
using MonsterCards.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace MonsterCards.Domain.Server
{
    public class HttpProcessor
    {
        private TcpClient clientSocket;
        private HttpServer httpServer;
        //  public Battle battleGame{ get; set; }
        public HttpProcessor(HttpServer httpServer, TcpClient clientSocket)
        {
            this.httpServer = httpServer;
            this.clientSocket = clientSocket;            
        }

        

        public void Process(Battle battle)
        {
            using var reader = new StreamReader(clientSocket.GetStream());
            var rq = new HttpRequest(reader);
            rq.Parse();

            using var writer = new StreamWriter(clientSocket.GetStream()) { AutoFlush = true };
            var rs = new HttpResponse(writer);
            var endpoint = httpServer.Endpoints.ContainsKey(rq.Path[1]) ? httpServer.Endpoints[rq.Path[1]] : null;


            if (endpoint == null || !endpoint.HandleRequest(rq, rs,battle))
            {
                //Thread.Sleep(10000);
                rs.ResponseCode = 404;
                rs.ResponseMessage = "Not Found";
                rs.Content = "<html><body>Not found!</body></html>";
                rs.Headers.Add("Content-Type", "text/html");
            }            
            else 
            {
                if (rs.ResponseCode== 300)
                {                                       
                    rs.Content = "<html><body>" + rs.ResponseMessage + "</body></html>";
                    rs.Headers.Add("Content-Type", "text/html");

                }else if (rs.ResponseCode== 410)
                {
                    rs.Content = "<html><body>" + rs.ResponseMessage + "</body></html>";
                    rs.Headers.Add("Content-Type", "text/html");
                }
                else if (rs.ResponseCode == 200) 
                {
                    rs.Content = "<html><body>" + rs.ResponseMessage + "</body></html>";
                    rs.Headers.Add("Content-Type", "text/html");
                }
                else if (rs.ResponseCode == 413)
                {
                    rs.Content = "<html><body>" + rs.ResponseMessage + "</body></html>";
                    rs.Headers.Add("Content-Type", "text/html");
                }
                else if (rs.ResponseCode == 411)
                {
                    rs.Content = "<html><body>" + rs.ResponseMessage + "</body></html>";
                    rs.Headers.Add("Content-Type", "text/html");
                }
                else if (rs.ResponseCode == 414)
                {
                    rs.Content = "<html><body>" + rs.ResponseMessage + "</body></html>";
                    rs.Headers.Add("Content-Type", "text/html");
                }              

            }



            Console.WriteLine("----------------------------------------");
            // ----- 3. Send the HTTP-Response -----
            rs.Send();
            writer.Flush();

            Console.WriteLine("========================================");

        }
        public Battle ckeckBattleStart()
        {
            
            CredentialRepository credentialRepository = new CredentialRepository();
            List<User> users = credentialRepository.retriveLoggedUsers();
            // public List<User> retriveLoggedUsers(HttpResponse rs)
            if (users.Count == 2)
            {
                return new Battle(users);                
            }
            return null;

        }
    }
}
