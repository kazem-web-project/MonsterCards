using MonsterCards.Application;
using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Entities.Server;
using MonsterCards.Domain.Enums.MTCG;
using MonsterCards.Domain.Interfaces.Persistance;
using MonsterCards.Domain.Interfaces.Server;
using MonsterCards.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Server
{
    public class DeckHandler : IHttpEndpoint
    {
        static readonly object _lock = new object();

        public bool HandleRequest(HttpRequest rq, HttpResponse rs, Battle battle)
        {
            PackageRepository packageRepository = new PackageRepository();
            if (rq.Method == Enums.Server.HttpMethod.GET)
            {
                if (rq.Headers.ContainsKey("Authorization"))
                {
                    string result = retrieveUserSessionFromHeader(rq.Headers["Authorization"]);
                    string username = result.Split('-')[0];
                    if (packageRepository.isUserLogedIn(retrieveUserSessionFromHeader(rq.Headers["Authorization"])))
                    {
                        string result1 = "";
                        List<Card> cardstoshow = packageRepository.retrieveUserDeckCards(username);
                        if (cardstoshow.Count == 0)
                        {
                            rs.ResponseCode = 200;
                            rs.ResponseMessage = "There is no card in the deck.";
                            return true;
                        }
                        foreach (Card card in cardstoshow)
                        {
                            if (rq.QueryParams.Count != 0 && rq.QueryParams["format"] == "plain")
                            {
                                result1 += card.Name + " with damage " + card.Damage + " is in the deck</br>";
                            }
                            else
                            {
                                result1 += card.ToString();
                            }
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
            else if (rq.Method == Enums.Server.HttpMethod.PUT)
            {
                if (rq.Headers.ContainsKey("Authorization"))
                {
                    string result = retrieveUserSessionFromHeader(rq.Headers["Authorization"]);
                    string username = result.Split('-')[0];
                    if (packageRepository.isUserLogedIn(retrieveUserSessionFromHeader(rq.Headers["Authorization"])))
                    {
                        List<string>? cardToCheck =
                                            JsonSerializer.Deserialize<List<string>>(rq.Content);

                        UserRepository userRepository = new UserRepository();
                        User user = userRepository.retrieveUserInfoFromUsername(username);
                        //  rq.Content
                        if (cardToCheck.Count == 4)
                        {
                            rs.ResponseMessage = packageRepository.insertCardsInDeck(username, cardToCheck);
                            lock (_lock)
                            {

                                if (battle.users.Contains(user))
                                {
                                    StackRepository stackRepository = new StackRepository();

                                    List<Card> cards = stackRepository.retrieveUserCardsByIds(user, cardToCheck);
                                    foreach (var userIt in battle.users)
                                    {
                                        if (user.name == userIt.name)
                                        {
                                            userIt.deck.AddRange(cards);

                                        }// add to the battle´.user

                                    }
                                    user.deck.AddRange(cards);
                                    rs.ResponseCode = 200;
                                }
                                else
                                {
                                    rs.ResponseMessage = "I hope this is not a deadlock ;)";
                                    rs.ResponseCode = 200;
                                }
                            }

                        }
                        else
                        {
                            rs.ResponseCode = 200;
                            rs.ResponseMessage = "Dear " + username + ", Please insert 4 cards into your deck!";
                        }
                    }
                }
                else
                {
                    rs.ResponseCode = 200;
                    rs.ResponseMessage = "Please login!";
                }
                return true;

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
