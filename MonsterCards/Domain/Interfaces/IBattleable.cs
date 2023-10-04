
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
        public void battleTwoCards(Card myCard, Card cardOpponent,ref  Deck myUserDeck,ref Deck OpponentDeck);
        public int battleEffectiveDamage(ref Card myCard);
        public int battleNotEffectiveDamage(ref Card myCard);
        public void battleEffective(Card winerCard, Card lostCard, ref Deck myUserDeck, ref Deck OpponentDeck);
        public void battleNoEffective(Card card1, Card card2, ref Deck myUserDeck, ref Deck OpponentDeck);

    }
}
