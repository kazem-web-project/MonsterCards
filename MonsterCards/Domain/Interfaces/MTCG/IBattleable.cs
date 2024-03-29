﻿using MonsterCards.Domain.Entities.MTCG;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Interfaces.MTCG
{
    internal interface IBattleable
    {
        public string battle( User myUser,  User userOpponent);
        public void battleTwoCards(Card myCard, Card cardOpponent, List<Card> myUserDeck, List<Card> opponentDeck);
        public int battleEffectiveDamage(int myCardDamage);
        public int battleNotEffectiveDamage(int myCardDamage);
        public void battleEffective(Card winerCard, Card lostCard, List<Card> myUserDeck, List<Card> OpponentDeck);
        public void battleNoEffective(Card card1, Card card2, List<Card> myUserDeck, List<Card> OpponentDeck);

    }
}
