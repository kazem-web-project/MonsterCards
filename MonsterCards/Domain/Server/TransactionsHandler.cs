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
    internal class TransactionsHandler : IHttpEndpoint
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
                    if (packageRepository.isThereEnoughCardToSell())
                    {
                        if (user.coins >= 5)
                        {

                            rs.ResponseMessage = packageRepository.buyCard(username);
                            lock (_lock)
                            {

                                if (battle.users.Contains(user))
                                {
                                    user.stack.AddRange(packageRepository.retrieveUserCards(username));
                                }
                                rs.ResponseMessage += userRepository.reduceCoinsByFive(username);
                                battle.users.Remove(user);
                                User updatedUser = userRepository.retrieveUserInfoFromUsername(username);
                                updatedUser.stack.AddRange(packageRepository.retrieveUserCards(username));
                                battle.users.Add(updatedUser);
                            }

                            rs.ResponseCode = 200;
                        }
                        else
                        {
                            rs.ResponseCode = 200;
                            rs.ResponseMessage = "The user has not enough coins!";
                        }
                    }
                    else
                    {
                        rs.ResponseCode = 200;
                        rs.ResponseMessage = "There are not enough packet to sell!";
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
