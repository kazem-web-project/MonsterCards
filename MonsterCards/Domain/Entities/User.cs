


using MonsterCards.Domain.Enums;
using MonsterCards.Domain.Interfaces;
using System;
using System.IO.IsolatedStorage;
using System.Linq.Expressions;

namespace MonsterCards.Domain.Entities
{
    internal class User : ILoginable,
                    IRegistrable, IAcquireable, ICompareable,
                    IBattleable, ITradable, IBattleResultReceivable,
                    IPlayable, ICardExchangable
    { 

        const int winstatvalue = 3;
        const int lossstatvalue = 0;
        const int startingstatvalue = 100;

        

        public User(int stat, string name)
        {

            deck = new List<Card>();
            this.stat = stat;
            this.name = name;
            isregistered = true;
            
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

        public bool acceptTradeOffer(Card cardToAccept)
        {
            throw new NotImplementedException();
        }

        public List<Card> acquireCards(int coingCount)
        {
            throw new NotImplementedException();
        }

        public bool battle(ref User myUser,ref User userOpponent)
        {
            Deck myUserDeck = new Deck();
            Deck userOpponentDeck = new Deck();

            for (int i = 0; i < myUser.deck.Count; i++)
            {
                var randNumsUsed = new List<int>(4) { 0,1,2,3};
                for (int j = 0; j < userOpponent.deck.Count; j++)
                {
                    int randNum;
                    do
                    {
                        randNum = rnd.Next(0, 4);
                        if(randNumsUsed.Contains(randNum)){
                            randNumsUsed.Remove(randNum);
                            break;
                        }
                    } while (true);
                    //randNumsUsed.Add(randNum);
                    battleTwoCards(myUser.deck[i], userOpponent.deck[randNum], ref myUserDeck, ref userOpponentDeck);

                }
            }
            myUser.deck.Clear();
            myUser.deck.AddRange(myUserDeck.cardsDeck);

            userOpponent.deck.Clear();
            userOpponent.deck.AddRange(userOpponentDeck.cardsDeck);
            
            return true;
        }
        public void battleTwoCards(Card userCard, Card opponentCard,ref Deck myUserDeck,ref Deck OpponentDeck)
        {
            if (userCard is SpellCard)
            {
                
                switch (userCard.ElementType)
                {
                    case ElementType.FIRE:
                        // code block
                        switch (opponentCard.ElementType)
                        {
                            case ElementType.WATER:
                                // WATER IS THE WINNER
                                battleEffective(userCard, opponentCard, ref myUserDeck, ref OpponentDeck);
                                break;
                            case ElementType.NORMAL:
                                // NORMAL IS THE WINNER
                                battleEffective(opponentCard, userCard, ref myUserDeck, ref OpponentDeck);
                                break;
                                
                            default:
                                battleNoEffective(userCard, opponentCard, ref myUserDeck, ref OpponentDeck);
                                break;
                        }
                        break;
                    case ElementType.WATER:
                        // working here
                        switch (opponentCard.ElementType)
                        {
                            case ElementType.FIRE:
                                // WATER IS THE WINNER
                                battleNoEffective(userCard, opponentCard, ref myUserDeck, ref OpponentDeck);

                                //battleEffective(userCard, opponentCard, ref myUserDeck, ref OpponentDeck);
                                break;
                            case ElementType.NORMAL:
                                // NORMAL IS THE WINNER
                                battleEffective(opponentCard, userCard, ref myUserDeck, ref OpponentDeck);
                                break;

                            default:
                                battleNoEffective(userCard, opponentCard, ref myUserDeck, ref OpponentDeck);
                                break;
                        }
                        break;
                    case ElementType.NORMAL:
                        switch (opponentCard.ElementType)
                        {
                            case ElementType.FIRE:
                                
                                battleEffective(opponentCard, userCard, ref myUserDeck, ref OpponentDeck);
                                break;
                            case ElementType.WATER:
                                // NORMAL IS THE WINNER
                                battleNoEffective(userCard, opponentCard, ref myUserDeck, ref OpponentDeck);

                                break;

                            default:
                                battleNoEffective(userCard, opponentCard, ref myUserDeck, ref OpponentDeck);
                                break;
                        }
                        break; 
                    default:
                        // code block
                        break;
                }
            } else
            {
                battleNoEffective(userCard, opponentCard, ref myUserDeck, ref OpponentDeck);
            }

        }

        public int battleEffectiveDamage(ref Card myCard)
        {
            return myCard.Damage *= 2;
        }
        public int battleNotEffectiveDamage(ref Card myCard)
        {
            return myCard.Damage /= 2;
        }
        public void battleEffective(Card winCard, Card lostCard, ref Deck myUserDeck, ref Deck OpponentDeck)
        {
            int userDamege = battleEffectiveDamage(ref lostCard);
            int opponentDamage = battleNotEffectiveDamage(ref winCard);
            if (userDamege > opponentDamage)
            {
                myUserDeck.cardsDeck.Add(winCard);
            }
            else if (userDamege < opponentDamage)
            {
                OpponentDeck.cardsDeck.Add(lostCard);
            }
            return;
        }
        public void battleNoEffective(Card card1, Card card2, ref Deck myUserDeck, ref Deck OpponentDeck)
        {
            if (card1.Damage > card2.Damage)
            {
                myUserDeck.cardsDeck.Add(card1);
            }
            else if (card1.Damage < card2.Damage)
            {
                OpponentDeck.cardsDeck.Add(card2);
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


    }
}
