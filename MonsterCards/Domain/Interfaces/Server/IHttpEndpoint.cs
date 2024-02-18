using MonsterCards.Application;
using MonsterCards.Domain.Entities.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Interfaces.Server
{
    public interface IHttpEndpoint
    {
        bool HandleRequest(HttpRequest rq, HttpResponse rs, Battle battle);
    }
}
