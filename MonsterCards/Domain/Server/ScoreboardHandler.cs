using MonsterCards.Application;
using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Entities.Server;
using MonsterCards.Domain.Interfaces.Server;
using MonsterCards.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Server
{
    public class ScoreboardHandler : IHttpEndpoint
    {
        static readonly object _lock = new object();
        public bool HandleRequest(HttpRequest rq, HttpResponse rs, Battle battle)
        {
            PackageRepository packageRepository = new PackageRepository();
            UserRepository userRepository = new UserRepository();
            if (rq.Headers.ContainsKey("Authorization"))
            {
                string result = retrieveUserSessionFromHeader(rq.Headers["Authorization"]);
                string username = result.Split('-')[0];
                User user = userRepository.retrieveUserInfoFromUsername(username);
                if (packageRepository.isUserLogedIn(retrieveUserSessionFromHeader(rq.Headers["Authorization"])))
                {

                    rs.ResponseCode = 200;
                    lock (_lock)
                    {                        
                        rs.ResponseMessage = battle.retrieveScoreBoard();
                    }
                }
            }
            return true;
        }
        public string retrieveUserSessionFromHeader(string headerValue)
        {
            int cnt = headerValue.Length;
            return headerValue.Substring(headerValue.IndexOf(' ') + 1);
        }
    }
}
