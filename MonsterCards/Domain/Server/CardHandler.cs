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
    public class CardHandler : IHttpEndpoint
    {
        public bool HandleRequest(HttpRequest rq, HttpResponse rs, Battle battle)
        {
            PackageRepository packageRepository = new PackageRepository();

            if (rq.Headers.ContainsKey("Authorization"))
            {
                string result = retrieveUserSessionFromHeader(rq.Headers["Authorization"]);
                string username = result.Split('-')[0];
                if (packageRepository.isUserLogedIn(retrieveUserSessionFromHeader(rq.Headers["Authorization"])))
                {
                    string result1 = "";
                    List<Card> cardstoshow = packageRepository.retrieveUserCards(username);
                    foreach (Card card in cardstoshow)
                    {

                        result1 += card.ToString();
                        result1 += "</br>";
                    }
                    rs.ResponseCode = 200;
                    rs.ResponseMessage = result1;
                }
            }
            else
            {
                rs.ResponseCode = 200;
                rs.ResponseMessage = "Please login!";
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
