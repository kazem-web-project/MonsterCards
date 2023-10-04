
using MonsterCards.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Interfaces
{
    internal interface IBattleable
    {
        public bool battle(ref User myUser,ref User userOpponent);
        public void battleTwoCards(Card myCard, Card cardOpponent,List<Card> myUserDeck, List<Card> opponentDeck);
        public int battleEffectiveDamage(Card myCard);
        public int battleNotEffectiveDamage(Card myCard);
        public void battleEffective(Card winerCard, Card lostCard, List<Card> myUserDeck, List<Card> OpponentDeck);
        public void battleNoEffective(Card card1, Card card2, List<Card> myUserDeck,  List<Card> OpponentDeck);

    }
}
