using MonsterCards.Domain.Entities;
using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Enums.MTCG;
using MonsterCards.Domain.Interfaces;
namespace MonsterCards.Application
{
    public static class BusinessLogic
    {
        
        public static void playGame()
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
            // Card m3 = new MonsterCard("FireSpell", 10, ElementType.FIRE);
            Card m3 = new MonsterCard("FireTroll", 10, ElementType.FIRE);
            Card m4 = new MonsterCard("WaterSpell", 20, ElementType.WATER);

            Card m5 = new MonsterCard("FireSpell", 20, ElementType.FIRE);
            Card m6 = new MonsterCard("WaterSpell", 05, ElementType.WATER);

            Card m7 = new MonsterCard("FireSpell", 90, ElementType.FIRE);
            Card m8 = new MonsterCard("WaterSpell", 07, ElementType.WATER);

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

            // test1
            //playerA.deck.Add(m1);
            //playerB.deck.Add(m2);


            // test2 : Spell fights:
            //Card t21 = new MonsterCard("FireTroll", 15, ElementType.FIRE);
            //Card t22 = new MonsterCard("WaterGoblin", 10, ElementType.WATER);
            //playerA.deck.Add(t21);
            //playerB.deck.Add(t22);

            // test3
            //Card t31= new SpellCard("FireSpell", 10, ElementType.FIRE);
            //Card t32= new SpellCard("WaterSpell", 20, ElementType.WATER);
            //playerA.deck.Add(t31);
            //playerB.deck.Add(t32);



            // test4
            //Card t41 = new SpellCard("FireSpell", 20, ElementType.FIRE);
            //Card t42 = new SpellCard("WaterSpell", 5, ElementType.WATER);
            //playerA.deck.Add(t41);
            //playerB.deck.Add(t42);

            // test5
            //Card t51 = new SpellCard("FireSpell", 90, ElementType.FIRE);
            //Card t52 = new SpellCard("WaterSpell", 5, ElementType.WATER);
            //playerA.deck.Add(t51);
            //playerB.deck.Add(t52);

            // test6
            //Card t61 = new SpellCard("FireSpell", 10, ElementType.FIRE);
            //Card t62 = new MonsterCard("WaterGoblin", 10, ElementType.WATER);
            //playerA.deck.Add(t61);
            //playerB.deck.Add(t62);


            // test6
            //Card t71 = new SpellCard("WaterSpell", 10, ElementType.WATER);
            //Card t72 = new MonsterCard("WaterGoblin", 10, ElementType.WATER);
            //playerA.deck.Add(t71);
            //playerB.deck.Add(t72);

            // test7
            //Card t81 = new SpellCard("RegularSpell", 10, ElementType.NORMAL);
            //Card t82 = new MonsterCard("WaterGoblin", 10, ElementType.WATER);
            //playerA.deck.Add(t81);
            //playerB.deck.Add(t82);
            
            // test9
            Card t91 = new SpellCard("RegularSpell", 10, ElementType.NORMAL);
            Card t92 = new MonsterCard("Knight", 15, ElementType.NORMAL);
            playerA.deck.Add(t92);
            playerB.deck.Add(t91);

            //playerA.deck.Add(m3);
            //playerA.deck.Add(m5);
            //playerA.deck.Add(m7);

            //playerB.deck.Add(m4);
            //playerB.deck.Add(m6);
            //playerB.deck.Add(m8);



            //playerA.deck.Add(playerB.deck[3]);
            //playerB.deck.Remove(playerB.deck[3]);

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
        internal static bool endGame(User playerA, User playerB)
        {
            return playerA.stack.Count==0 || playerB.stack.Count==0;
        }
    }
}