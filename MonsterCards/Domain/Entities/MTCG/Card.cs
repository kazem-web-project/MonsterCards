using MonsterCards.Domain.Enums.MTCG;
using System.Text.Json.Serialization;

namespace MonsterCards.Domain.Entities.MTCG

{
    [Serializable]
    public  class Card
    {
        public Card()
        {
        }
        [JsonConstructor]
        public Card(string id, string name, double damage)
        {
            Name = name;
            Damage = damage;
            // ElementType = ElementType.FIRE;
            Id = id;
        }
        /*
        public Card(string name, int damage)
        {
            Name = name;
            Damage = damage;
        }
        */

        protected Card(string name, double damage, ElementType elementType)
        {
            Name = name;
            Damage = damage;
            ElementType = elementType;
        }
        protected Card(string id, string name, double damage, ElementType elementType )
        {
            Name = name;
            Damage = damage;
            ElementType = elementType;
            Id = id;
        }
        public string Name { get; set; }
        public double Damage { get; set; }
        public ElementType ElementType { get; set; }
        public string Id { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Card card &&
                   Name == card.Name &&
                   Damage == card.Damage &&
                   ElementType == card.ElementType &&
                   Id == card.Id;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return "Card Name: " + Name + " Damage: " + Damage +" Id: " + Id+ " Element type: " + ElementType;
        }

    }
}