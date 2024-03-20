using System;
using System.IO.IsolatedStorage;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System.Numerics;
using System.Collections.Generic;
using MonsterCards.Domain.Interfaces.MTCG;
using MonsterCards.Domain.Enums.MTCG;
using MonsterCards.Domain.Interfaces.Server;
using MonsterCards.Domain.Entities.Server;
using System.Text.Json;
using MonsterCards.Infrastructure.Persistance;

namespace MonsterCards.Domain.Entities.MTCG
{
    public class User : ILoginable,
                    IRegistrable, IAcquireable, ICompareable,
                    IBattleable, ITradable, IBattleResultReceivable,
                    IPlayable, ICardExchangable, Ilogable
    { 

        const int winstatvalue = 3;
        const int lossstatvalue = 0;
        const int startingstatvalue = 100;
        

        public User()
        {

            deck = new List<Card>();
            this.stat = 100;
            this.name = "Test User";
            isregistered = true;
            
        }

        public User(int stat, string name, int user_id)
        {

            deck = new List<Card>();
            this.stat = stat;
            this.name = name;
            isregistered = true;
            this.user_id = user_id;
        }

        public User(List<Card> stack, int stat, string name)
        {
            
            this.stack = stack;
            this.stat = stat;
            this.name = name;
            isregistered = true;
            
        }
        private Random rnd = new Random();
        public Credential UserCredential { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int coins { get; set ; }
        public int stat { get ; set ; }
        public string name { get; set; }
        public bool isregistered { get ; set; }
        public List<Card> stack { get; set; } = new List<Card>();
        public List<Card> deck { get; set; } = new List<Card>();
        public int user_id { get ; set ; }

        public bool acceptTradeOffer(Card cardToAccept)
        {
            throw new NotImplementedException();
        }

        public List<Card> acquireCards(int coingCount)
        {
            throw new NotImplementedException();
        }

        public string battle(User myUser, User userOpponent)
        {
            List<Card> playedCards = new List<Card>();
            string responseMessage = "";
            int myUserDeckCount = myUser.deck.Count;
            int userOpponentDeckCount = userOpponent.deck.Count;
            while (playedCards.Count < (myUserDeckCount + userOpponentDeckCount))           
            { // water
              // 5 has problem
                int myUserRandNum = myUser.deck.Count != 0 ? rnd.Next(0, myUser.deck.Count - 1) : 0;
                int userOpponentRandNum = userOpponent.deck.Count != 0 ? rnd.Next(0, userOpponent.deck.Count - 1) : 0;
                if (myUser.deck.Count==0 || userOpponent.deck.Count==0 )
                {
                    break;
                }
                if (playedCards.Contains(myUser.deck[myUserRandNum]) || playedCards.Contains(userOpponent.deck[userOpponentRandNum])) continue;
                playedCards.Add(myUser.deck[myUserRandNum]);
                playedCards.Add(userOpponent.deck[userOpponentRandNum]);
                Console.WriteLine(myUser.ToString() + " plays against" + userOpponent.ToString() + ":");
                Console.WriteLine(myUser.deck[myUserRandNum].ToString() + " Card is agaist " + userOpponent.deck[userOpponentRandNum].ToString() + "!");
                responseMessage += myUser.ToString() + " plays against" + userOpponent.ToString() + ":";
                responseMessage += myUser.deck[myUserRandNum].ToString() + " Card is agaist " + userOpponent.deck[userOpponentRandNum].ToString() + "!";
                battleTwoCards(myUser.deck[myUserRandNum], userOpponent.deck[userOpponentRandNum], myUser.deck, userOpponent.deck);

                myUser.stat -= (myUserDeckCount - myUser.deck.Count);
                userOpponent.stat -= (userOpponentDeckCount - userOpponent.deck.Count);
            }
            if (myUser.stat> userOpponent.stat)
            {
                responseMessage += myUser.name + " won!!";
            }
            else
            {
                responseMessage += userOpponent.name + " won!!";
            }

            //for (int i = 0; i < myUser.deck.Count; i++)
            //{
            //    var randNumsUsed = new List<int>(4) { 0,1,2,3};
            //    for (int j = 0; j < userOpponent.deck.Count; j++)
            //    {
            //        if (randNumsUsed.Count==0){ return true; }
            //        int randNum;
            //        do
            //        {
            //            randNum = rnd.Next(0, 4);
            //            if(randNumsUsed.Contains(randNum)){
            //                randNumsUsed.Remove(randNum);
            //                break;
            //            }
            //        } while (true);
            //        //randNumsUsed.Add(randNum);
            //        //battleTwoCards(myUser.deck[i], userOpponent.deck[randNum], ref myUserDeck, ref userOpponentDeck);
            //        if(randNum < myUser.deck.Count)
            //        {

            //        }
            //    }
            //}
            // myUser.deck.Clear();
            //myUser.deck.AddRange(myUserDeck.cardsDeck);

            // userOpponent.deck.Clear();
            //userOpponent.deck.AddRange(userOpponentDeck.cardsDeck);

            return responseMessage;
        }
        public string battle2(User myUser, User userOpponent,int cardNum)
        {
            List<Card> playedCards = new List<Card>();
            string responseMessage = "";
   
            // while (playedCards.Count < 20)
            while (playedCards.Count < cardNum)
            { // water
              // 5 has problem
                int myUserRandNum = rnd.Next(0, myUser.deck.Count);
                int userOpponentRandNum = rnd.Next(0, userOpponent.deck.Count);
                if (playedCards.Contains(myUser.deck[myUserRandNum]) || playedCards.Contains(userOpponent.deck[userOpponentRandNum])) continue;
                playedCards.Add(myUser.deck[myUserRandNum]);
                playedCards.Add(userOpponent.deck[userOpponentRandNum]);
                Console.WriteLine(myUser.ToString() + " plays against" + userOpponent.ToString() + ":");
                Console.WriteLine(myUser.deck[myUserRandNum].ToString() + " Card is agaist " + userOpponent.deck[userOpponentRandNum].ToString() + "!");
                responseMessage += myUser.ToString() + " plays against" + userOpponent.ToString() + ":";
                responseMessage += myUser.deck[myUserRandNum].ToString() + " Card is agaist " + userOpponent.deck[userOpponentRandNum].ToString() + "!";
                battleTwoCards(myUser.deck[myUserRandNum], userOpponent.deck[userOpponentRandNum], myUser.deck, userOpponent.deck);

            }

            //for (int i = 0; i < myUser.deck.Count; i++)
            //{
            //    var randNumsUsed = new List<int>(4) { 0,1,2,3};
            //    for (int j = 0; j < userOpponent.deck.Count; j++)
            //    {
            //        if (randNumsUsed.Count==0){ return true; }
            //        int randNum;
            //        do
            //        {
            //            randNum = rnd.Next(0, 4);
            //            if(randNumsUsed.Contains(randNum)){
            //                randNumsUsed.Remove(randNum);
            //                break;
            //            }
            //        } while (true);
            //        //randNumsUsed.Add(randNum);
            //        //battleTwoCards(myUser.deck[i], userOpponent.deck[randNum], ref myUserDeck, ref userOpponentDeck);
            //        if(randNum < myUser.deck.Count)
            //        {

            //        }
            //    }
            //}
            // myUser.deck.Clear();
            //myUser.deck.AddRange(myUserDeck.cardsDeck);

            // userOpponent.deck.Clear();
            //userOpponent.deck.AddRange(userOpponentDeck.cardsDeck);

            return responseMessage;
        }
        public void battleTwoCards(Card myCard, Card cardOpponent, List<Card> myUserDeck, List<Card> opponentDeck)
        {                
            if(myCard is not SpellCard && cardOpponent is SpellCard)
            {                
                battleTwoCards(cardOpponent, myCard, opponentDeck, myUserDeck);
                return;
            }
            if (myCard is SpellCard)
            {
                
                switch (myCard.ElementType)
                {
                    case ElementType.FIRE:
                        // code block
                        switch (cardOpponent.ElementType)
                        {
                            case ElementType.WATER:
                                // WATER IS THE WINNER
                                battleEffective(myCard, cardOpponent, myUserDeck, opponentDeck);
                                break;
                            case ElementType.NORMAL:
                                // NORMAL IS THE WINNER
                                battleEffective(cardOpponent, myCard, myUserDeck, opponentDeck);
                                break;
                                
                            default:
                                battleNoEffective(myCard, cardOpponent, myUserDeck, opponentDeck);
                                break;
                        }
                        break;
                    case ElementType.WATER:
                        // working here
                        switch (cardOpponent.ElementType)
                        {
                            case ElementType.FIRE:
                                // WATER IS THE WINNER
                                battleNoEffective(myCard, cardOpponent,  myUserDeck,  opponentDeck);

                                //battleEffective(userCard, opponentCard, ref myUserDeck, ref OpponentDeck);
                                break;
                            case ElementType.NORMAL:
                                // NORMAL IS THE WINNER
                                battleEffective(cardOpponent, myCard,  myUserDeck,  opponentDeck);
                                break;

                            default:
                                battleNoEffective(myCard, cardOpponent,  myUserDeck,  opponentDeck);
                                break;
                        }
                        break;
                    case ElementType.NORMAL:
                        switch (cardOpponent.ElementType)
                        {
                            case ElementType.FIRE:
                                
                                battleEffective(cardOpponent, myCard,  myUserDeck, opponentDeck);
                                break;
                            case ElementType.WATER:
                                // NORMAL IS THE WINNER
                                battleEffective(cardOpponent, myCard , opponentDeck,myUserDeck );

                                // battleNoEffective(myCard, cardOpponent,  myUserDeck, opponentDeck);

                                break;

                            default:
                                battleNoEffective(myCard, cardOpponent,  myUserDeck, opponentDeck);
                                break;
                        }
                        break; 
                    default:
                        // code block
                        break;
                }
            } else
            {
                battleNoEffective(myCard, cardOpponent, myUserDeck, opponentDeck);
            }

        }

        public int battleEffectiveDamage( int myCardDamage)
        {
            return myCardDamage *= 2;
        }
        public int battleNotEffectiveDamage( int myCardDamage)
        {
            return myCardDamage /= 2;
        }
        public void battleEffective(Card winerCard, Card lostCard, List<Card> myUserDeck, List<Card> OpponentDeck)
            // (Card winerCard, Card lostCard, List<Card> myUserDeck, List<Card> OpponentDeck)
        {
            int userDamege = battleEffectiveDamage((int)lostCard.Damage);
            int opponentDamage = battleNotEffectiveDamage((int)winerCard.Damage);
            if (userDamege > opponentDamage)
            {
                Log(">>>>>>" +  lostCard.Name + " with damage: " + userDamege+ " Has won " + winerCard.Name + " with damage: " + opponentDamage +  "<<<<<<<");
                OpponentDeck.Add(winerCard);
                myUserDeck.Remove(winerCard);
                // OpponentDeck.Remove(lostCard);
                // Console.WriteLine("done!");
            }
            else if (userDamege < opponentDamage)
            {
                // Log(">>>>>>" + winerCard.Name + " Has won " + lostCard.Name + "<<<<<<<");
                Log(">>>>>>" + winerCard.Name + " with damage: " + opponentDamage  + " Has won " + lostCard.Name + " with damage: " + userDamege + "<<<<<<<");

                myUserDeck.Add(lostCard);
                OpponentDeck.Remove(lostCard);

            }
        }
        public void battleNoEffective(Card card1, Card card2, List<Card> myUserDeck, List<Card> OpponentDeck)            
        {
            if (card1.Damage > card2.Damage)
            {
                if(myUserDeck.Contains(card1) && OpponentDeck.Contains(card2))
                {
                    myUserDeck.Add(card2);
                    for (int i = 0; i < OpponentDeck.Count; i++)
                    {
                        if (card2 == OpponentDeck[i])
                        {
                            Log(">>>>>>" +  card1.Name + " with damage: " + card1.Damage+ " Has won " + OpponentDeck[i].Name +" with damage: " + OpponentDeck[i].Damage + "<<<<<<<");
                            OpponentDeck.Remove(OpponentDeck[i]);

                        }
                    }
                }else if (OpponentDeck.Contains(card1) && myUserDeck.Contains(card2))
                {
                    OpponentDeck.Add(card1);
                    for (int i = 0; i < myUserDeck.Count; i++)
                    {
                        if (card1 == myUserDeck[i])
                        {
                            Log(">>>>>>" +  card1.ToString() + " Has won " + myUserDeck[i].ToString() + "<<<<<<<");
                            myUserDeck.Remove(myUserDeck[i]);

                        }
                    }

                }
                // foreach (Card myCard in OpponentDeck)
                
            }
            else if (card1.Damage < card2.Damage)
            {
                OpponentDeck.Add(card1);
                myUserDeck.Remove(card1);
            }
            else
            {
                Log(">>>>>>" + card1.Name +" with damage: "+ card1.Damage + " draws " + card2.Name + " with damage: "+ card2.Damage+ "<<<<<<<");                
            }

        }
        public string compare()
        {
            throw new NotImplementedException();
        }

        public bool login()
        {
            throw new NotImplementedException();
        }

        public string receiveBattleResult()
        {
            throw new NotImplementedException();
        }

        public bool register(Credential credential)
        {
            throw new NotImplementedException();
        }

        public bool requestTrade(Card cardToTrade)
        {
            throw new NotImplementedException();
        }

        public Card trade(Card acceptedCard)
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return this.name + " Stat: " + this.stat+
                " Coins:" + this.coins + "      " ;
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }
        /*
        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {


            if (rq.Method.ToString() == "POST")
            {
                Credential? credentialToCheck =
                    JsonSerializer.Deserialize<Credential>(rq.Content);
                CredentialRepository credentialRepo = new CredentialRepository("Host=localhost;Database=mydb;Username=postgres;Password=postgres;Persist Security Info=True");
                int new_user_id = credentialRepo.Add(credentialToCheck, rs);
                if (new_user_id > 0)
                {
                    Console.WriteLine("New user created with the id: " + credentialToCheck);
                }
                else
                {
                    Console.WriteLine("Could not create new user!");
                }
            }
            else if (rq.Method.ToString() == "PUT")
            {
                // implement /users/...
                Credential? credentialToCheck =
                    JsonSerializer.Deserialize<Credential>(rq.Content);
                CredentialRepository credentialRepo = new CredentialRepository("Host=localhost;Database=mydb;Username=postgres;Password=postgres;Persist Security Info=True");


                int new_user_id = credentialRepo.Update(credentialToCheck, rs);
                if (new_user_id > 0)
                {
                    Console.WriteLine("The user with the id: " + credentialToCheck + " changed his password with the id: ");
                }
                else
                {
                    Console.WriteLine("Could not edit the user password!");
                }

            }

            //Console.WriteLine(rq);
            // Console.WriteLine("inside user handle");
            //throw new NotImplementedException();
            return true;
            Console.WriteLine(rq);
            Console.WriteLine("inside user handle");
            throw new NotImplementedException();

        }
        */

        public void loadDeckCardsFromStack()
        {
            this.deck.Clear();
            for (int i = 0; i < 20; i++)
            {
                if (i >= this.stack.Count - 1) return;
                this.deck.Add(this.stack.ElementAt(i));
            }
            
        }

        public override bool Equals(object? obj)
        {
            return obj is User user &&
                   user_id == user.user_id;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(rnd);
            hash.Add(UserCredential);
            hash.Add(coins);
            hash.Add(stat);
            hash.Add(name);
            hash.Add(stack);
            hash.Add(deck);
            hash.Add(user_id);
            return hash.ToHashCode();
        }
        public string gewiner()
        {
            return "";
        }
    }
}
