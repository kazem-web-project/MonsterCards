﻿using MonsterCards.Domain.Enums.MTCG;

namespace MonsterCards.Domain.Entities.MTCG

{

    internal abstract class Card
    {
        protected Card(string name, int damage, ElementType elementType)
        {
            Name = name;
            Damage = damage;
            ElementType = elementType;
        }

        public string Name { get; set; }
        public int Damage { get; set; }
        public ElementType ElementType { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Card card &&
                   Name == card.Name &&
                   Damage == card.Damage &&
                   ElementType == card.ElementType;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return "Card Name: " + Name + " Damage: " + Damage + " Element type: " + ElementType;
        }
    }
}