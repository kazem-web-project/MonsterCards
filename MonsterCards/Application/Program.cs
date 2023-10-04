using MonsterCards.Domain.Entities;
using MonsterCards.Domain.Enums;

namespace MonsterCards.Application
{
    internal class Program
    {
        static void Main(string[] args)
        {

            /*
            MonsterCard m1 = new MonsterCard("FireTroll", 15, FIRE);

            MonsterCard m2 = new MonsterCard("WaterGoblin", 10, WATER);
            Deck myDeck = new Deck();
            User myUser = new User(3, "df");
            myUser.Name = "dfk";
            Console.WriteLine(myUser.Name);
            Console.WriteLine("Hello, World!");
            */
            //Monster Fights (= round with only monster cards involved):
            Card m1 = new MonsterCard("FireTroll", 15, ElementType.FIRE);
            Card m2 = new MonsterCard("WaterGoblin", 10, ElementType.WATER);

            // Spell Fights (= round with only spell cards involved):
            Card m3 = new MonsterCard("FireSpell", 10, ElementType.FIRE);
            Card m4 = new MonsterCard("WaterSpell", 20, ElementType.WATER);

            Card m5 = new MonsterCard("FireSpell", 20, ElementType.FIRE);
            Card m6 = new MonsterCard("WaterSpell", 05, ElementType.WATER);

            Card m7 = new MonsterCard("FireSpell", 90, ElementType.FIRE);
            Card m8 = new MonsterCard("WaterSpell", 05, ElementType.WATER);

            // Mixed Fights (= round with a spell card vs a monster card):
            Card m9 = new SpellCard("FireSpell", 10, ElementType.FIRE);
            Card m10 = new MonsterCard("WaterGoblin", 10, ElementType.WATER);
            Card m11 = new SpellCard("WaterSpell", 10, ElementType.WATER);
            Card m12 = new MonsterCard("WaterGoblin", 10, ElementType.WATER);

            Card m13 = new SpellCard("RegularSpell", 10, ElementType.WATER);
            Card m14 = new MonsterCard("WaterGoblin", 10, ElementType.WATER);

            Card m15 = new SpellCard("RegularSpell", 10, ElementType.WATER);
            Card m16 = new MonsterCard("Knight", 15, ElementType.WATER);

            // test Stack
            //StackCard stackCard1 = new StackCard();


            List<Card> player1Stack = new List<Card> { m1, m3, m5, m7, m9, m11, m13, m15 };
            List<Card> player2Stack = new List<Card> { m2, m4, m6, m8, m10, m12, m14, m16 };


            User playerA = new User(player1Stack, 100, "PlayerA");
            User playerB = new User(player2Stack, 100, "PlayerB");

            playerA.deck.Add(m1);
            playerA.deck.Add(m3);
            playerA.deck.Add(m5);
            playerA.deck.Add(m7);

            playerB.deck.Add(m2);
            playerB.deck.Add(m4);
            playerB.deck.Add(m6);
            playerB.deck.Add(m8);

            playerA.battle(ref playerA, ref playerB);

            Console.WriteLine("dsf");

            /*
            stackCard1.stack.AddRange( m1);
            stackCard1.stack.AddRange( m1);
            stackCard1.stack.AddRange( m1);
            stackCard1.stack.AddRange( m1);
            stackCard1.stack.AddRange( m1);
            stackCard1.stack.AddRange( m1);
            StackCard stackCard2 = new StackCard();  
            */

            // test battle

        }
    }
}