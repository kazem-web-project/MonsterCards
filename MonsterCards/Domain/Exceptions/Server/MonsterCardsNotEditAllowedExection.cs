using MonsterCards.Domain.Entities.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Exceptions.Server
{
    internal class UnauthorizedError : Exception
    {
       

        public UnauthorizedError(string? message) : base(message)
        {
            // 
        }

        public UnauthorizedError(string? message, HttpRequest rq, HttpResponse rs) : this(message)
        {
            rs.ResponseCode = 401;
            rs.ResponseMessage = "Not Found";
            rs.Content = "<html><body>"+message+"</body></html>";
            rs.Headers.Add("Content-Type", "text/html");
        }
    }
}
