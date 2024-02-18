
using MonsterCards.Application;
using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Enums.MTCG;

using NUnit.Framework;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Threading;


namespace MonsterCarssUnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            

        }

        [Test]

        public void water_againts_fire_cards_involved()
        {
            Card m1 = new MonsterCard("FireTroll", 10, ElementType.FIRE);
            Card m2 = new MonsterCard("WaterSpell", 20, ElementType.WATER);

            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 2);
            Assert.AreEqual(user1.deck.Count, 0);

        }
        [Test]

        

        public void mixed_fights_equal_damage_numbers()
        {
            //Monster Fights (= round with only monster cards involved):
            Card m1 = new SpellCard("FireSpell", 10, ElementType.FIRE);
            Card m2 = new MonsterCard("WaterGoblin", 10, ElementType.WATER);
            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 2);
            Assert.AreEqual(user1.deck.Count, 0);
        }
        [Test]

        public void mixed_fights_waterSpell_againt_WaterGoblin_equal_damage_numbers()
        {
            //Monster Fights (= round with only monster cards involved):
            Card m1 = new SpellCard("WaterSpell", 10, ElementType.WATER);
            Card m2 = new MonsterCard("WaterGoblin", 10, ElementType.WATER);

            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 1);
            Assert.AreEqual(user1.deck.Count, 1);

        }
        [Test]

        public void mixed_fights_RegularSpell_againt_WaterGoblin_equal_damage_numbers()
        {
            //Monster Fights (= round with only monster cards involved):
            Card m1 = new SpellCard("RegularSpell", 10, ElementType.WATER);
            Card m2 = new MonsterCard("WaterGoblin", 10, ElementType.WATER);

            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 1);
            Assert.AreEqual(user1.deck.Count, 1);

        }
        [Test]

        public void RegularSpell_againts_RegularSpell()
        {
            //Monster Fights (= round with only monster cards involved):
            Card m1 = new SpellCard("RegularSpell", 10, ElementType.WATER);
            Card m2 = new MonsterCard("Knight", 15, ElementType.WATER);

            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 2);
            Assert.AreEqual(user1.deck.Count, 00);
        }
        [Test]

        public void FireTroll_againt_WaterGoblin()
        {
            //Monster Fights (= round with only monster cards involved):

            // test2 : Spell fights:
            Card m1 = new MonsterCard("FireTroll", 15, ElementType.FIRE);
            Card m2 = new MonsterCard("WaterGoblin", 10, ElementType.WATER);
            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 0);
            Assert.AreEqual(user1.deck.Count, 2);

        }
        [Test]

        public void FireSpell_againt_WaterSpell_water_with_more_damage()
        {

            // test3
            Card m1= new SpellCard("FireSpell", 10, ElementType.FIRE);
            Card m2= new SpellCard("WaterSpell", 20, ElementType.WATER);
            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 2);
            Assert.AreEqual(user1.deck.Count, 0);
        }
        [Test]

        public void FireSpell_agiant_WaterSpell_FireSpell_with_more_damage()
        {

            // test4
            Card m1 = new SpellCard("FireSpell", 20, ElementType.FIRE);
            Card m2 = new SpellCard("WaterSpell", 5, ElementType.WATER);
            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 1);
            Assert.AreEqual(user1.deck.Count, 1);
        }

        [Test]
        public void water_againts_fire_card_normal_damage()
        {
            Card m1 = new MonsterCard("FireTroll", 15, ElementType.FIRE);
            Card m2 = new MonsterCard("WaterGoblin", 10, ElementType.WATER);
            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            user1.battle2(user1, user2, 2);


            Assert.AreEqual(user2.deck.Count, 0);

        }
        [Test]

        public void FireSpell_agiant_WaterSpell_fire_with_more_damage_huge_damage_num()
        {

            // test5
            Card m1 = new SpellCard("FireSpell", 90, ElementType.FIRE);
            Card m2 = new SpellCard("WaterSpell", 5, ElementType.WATER);
            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 0);
            Assert.AreEqual(user1.deck.Count, 2);
        }
        [Test]

        public void FireSpell_againt_WaterGoblin_same_damage()
        {

            // test6
            Card m1 = new SpellCard("FireSpell", 10, ElementType.FIRE);
            Card m2 = new MonsterCard("WaterGoblin", 10, ElementType.WATER);
            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 2);
            Assert.AreEqual(user1.deck.Count, 0);

        }
        [Test]

        public void RegularSpell_against_WaterGoblin_same_damage()
        {
            // test7
            Card m1= new SpellCard("RegularSpell", 10, ElementType.NORMAL);
            Card m2 = new MonsterCard("WaterGoblin", 10, ElementType.WATER);
            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 0);
            Assert.AreEqual(user1.deck.Count, 2);

        }
        [Test]

        public void RegularSpell_against_Knight_RegularSpell_with_more_damage()
        {// test9
            Card m1 = new SpellCard("RegularSpell", 20, ElementType.NORMAL);
            Card m2= new MonsterCard("Knight", 15, ElementType.NORMAL);
            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 0);
            Assert.AreEqual(user1.deck.Count, 2);

        }
        [Test]

        public void FireTroll_against_WaterGoblin_with_less_damge()
        {
            //Monster Fights (= round with only monster cards involved):
            Card m1 = new MonsterCard("FireTroll", 10, ElementType.FIRE);
            Card m2 = new MonsterCard("WaterGoblin", 25, ElementType.WATER);
            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            //Battle testBattle = new MonsterCards.Application.Battle(users);
            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 2);
        }
        [Test]

        public void battleNotEffectiveDamage()
        {
            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            Assert.True(20 == user1.battleEffectiveDamage(10));
        }
        [Test]

        public void battleEffectiveDamage_test()
        {
            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            Assert.True(20== user1.battleEffectiveDamage(10));
        }
        [Test]

        public void water_againts_fire_cards_involved2_small_MaxValue_numbers()
        {
            //Monster Fights (= round with only monster cards involved):
            Card m1 = new MonsterCard("FireSpell", Double.MaxValue, ElementType.FIRE);
            Card m2 = new MonsterCard("WaterSpell", 07, ElementType.WATER);

            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);

            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 0);
            Assert.AreEqual(user1.deck.Count, 2);
        }
        [Test]

        public void spell_fights_normal_against_normal()
        {

            // test4
            Card m1 = new SpellCard("FireSpell", 20, ElementType.NORMAL);
            Card m2 = new SpellCard("WaterSpell", 5, ElementType.WATER);
            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 0);
        }
        [Test]

        public void spell_water_against_water()
        {
            Card m1 = new SpellCard("WaterSpell", 20, ElementType.WATER);
            Card m2 = new SpellCard("WaterSpell", 5, ElementType.WATER);
            User user1 = new MonsterCards.Domain.Entities.MTCG.User();
            User user2 = new MonsterCards.Domain.Entities.MTCG.User();
            List<User> users = new List<User>() { user1, user2 };
            user1.deck.Add(m1);
            user2.deck.Add(m2);
            user1.battle2(user1, user2, 2);
            /*
            //this.assertEquals([message,] expected, actual)
            */

            Assert.AreEqual(user2.deck.Count, 0);
        }
        [Test]

        public void game_simulation()
        {
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

             
            Assert.AreEqual(playerA.stack, player1Stack);  

        }
       
    }
}