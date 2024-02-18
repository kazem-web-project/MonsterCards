using MonsterCards.Application;
using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Entities.Server;
using MonsterCards.Domain.Enums.MTCG;
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
    internal class PackageHandler : IHttpEndpoint
    {
        public bool HandleRequest(HttpRequest rq, HttpResponse rs, Battle battle)
        {
            PackageRepository packageRepository = new PackageRepository();
            // CredentialRepository credentialRepository = new CredentialRepository();

            if (rq.Method.Equals(Enums.Server.HttpMethod.POST))
            {

                if (rq.Headers.ContainsKey("Authorization"))
                {
                    string result = retrieveUserSessionFromHeader(rq.Headers["Authorization"]);
                    // if (packageRepository.isUserLogedIn(retrieveUserSessionFromHeader(rq.Headers["Authorization"])))
                    // {
                    if (rq.Headers["Authorization"] == "Bearer admin-mtcgToken")
                    {
                        List<Card> cards = GetCards(rq);
                        int packageNumber = packageRepository.accuireMaxPackageNumber();
                        rs.ResponseCode = 200;

                        rs.ResponseMessage = packageRepository.addNewPackage(cards, packageNumber + 1);
                    }
                    else
                    {

                    }

                    //}
                }
            }

            return true;
        }

        public List<Card> GetCards(HttpRequest rq)
        {
            // string name = "845f0dc7 - 37d0 - 426e-994e - 43fc3ac83c08";
            List<Card> cardsResult = new List<Card>();
            List<Card>? cardToCheck =
                JsonSerializer.Deserialize<List<Card>>(rq.Content);
            foreach (var item in cardToCheck)
            {
                if (item.Name.ToUpper().Contains("WATER"))
                {
                    item.ElementType = ElementType.WATER;
                    if (item.Name.Contains("Spell"))
                    {
                        //Card cardToAdd = new SpellCard(item.Name, item.Damage, ElementType.WATER);
                        Card cardToAdd = new SpellCard(item.Name, item.Damage, ElementType.WATER, item.Id);
                        cardsResult.Add(cardToAdd);
                    }
                    else
                    {
                        Card cardToAdd = new MonsterCard(item.Name, item.Damage, ElementType.WATER, item.Id);
                        cardsResult.Add(cardToAdd);
                    }
                }
                else if (item.Name.ToUpper().Contains("FIRE"))
                {
                    item.ElementType = ElementType.FIRE;
                    if (item.Name.Contains("Spell"))
                    {
                        Card cardToAdd = new SpellCard(item.Name, item.Damage, ElementType.FIRE, item.Id);
                        cardsResult.Add(cardToAdd);
                    }
                    else
                    {
                        Card cardToAdd = new MonsterCard(item.Name, item.Damage, ElementType.FIRE, item.Id);
                        cardsResult.Add(cardToAdd);
                    }

                }
                else
                {
                    item.ElementType = ElementType.NORMAL;
                    if (item.Name.Contains("Regular"))
                    {
                        Card cardToAdd = new SpellCard(item.Name, item.Damage, ElementType.NORMAL, item.Id);
                        cardsResult.Add(cardToAdd);
                    }
                    else
                    {
                        Card cardToAdd = new MonsterCard(item.Name, item.Damage, ElementType.NORMAL, item.Id);
                        cardsResult.Add(cardToAdd);
                    }
                }
            }
            // StackRepository stackRepository = new StackRepository();
            // {[X-request-ID, 0E-E1-14-F9-CB-87-D4-7E-02-98-8E-42-C3-25-B8-49]}
            return cardsResult;
            // stackRepository.insertNewCardsToUserStack(cardsResult, rq.Headers["X-Request-ID"]);

        }
        public string retrieveUserSessionFromHeader(string headerValue)
        {
            int cnt = headerValue.Length;
            return headerValue.Substring(headerValue.IndexOf(' ') + 1);
        }

    }

}
