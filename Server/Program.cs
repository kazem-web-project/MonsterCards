using Server.Entities;
using System;
using System.Linq.Expressions;
using System.Net;
using Server.Entities;
using MonsterCards.Domain.Entities;
namespace Program
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // continue; f
            HttpServer httpServer = new HttpServer(IPAddress.Any, 10001);
            // httpServer.RegisterEndpoint("users", new User());
            httpServer.Run();
        }
    }
}