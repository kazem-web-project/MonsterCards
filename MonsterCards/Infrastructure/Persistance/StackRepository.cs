using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Entities.Server;
using MonsterCards.Domain.Interfaces.Server;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Infrastructure.Persistance
{
    [Serializable]
    public class StackRepository 
    {
        private string connectionString;

        public StackRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public StackRepository()
        {
            this.connectionString = "Host=localhost;Database=monster_cards;Username=server;Password=password;Persist Security Info=True";
        }
    
        public void initStackTable()
        {
            List<string> cardTypes = new List<string>() { "WATER", "FIRE", "NORMAL" };
            List<string> cardNames = new List<string>() { "FireTroll" ,"WaterGoblin",
                                                                        "FireSpell",
                                                                        "FireTroll",
                                                                        "WaterSpell",
                                                                        "FireSpell",
                                                                        "WaterSpell",
                                                                        "FireSpell",
                                                                        "WaterSpell",
                                                                        "FireSpell",
                                                                        "WaterGoblin",
                                                                        "WaterSpell",
                                                                        "WaterGoblin",
                                                                        "RegularSpell",
                                                                        "WaterGoblin",
                                                                        "RegularSpell",
                                                                        "Knight",
                                                                        };

            //List<int> ids = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            //for (int i = 0; i < ids.Count; i++)
            int myUserId = 0;
            for (int k = 0; k < cardTypes.Count; k++)
            {
                for (int j = 0; j < cardNames.Count; j++)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        using IDbConnection connection = new NpgsqlConnection(this.connectionString);
                        using IDbCommand command = connection.CreateCommand();
                        connection.Open();


                        command.CommandText = "INSERT INTO  stack( user_id ,card_type ,card_name ,damage)  " +
                        "VALUES (@user_id, @card_type, @card_name, @damage) RETURNING user_id";
                        //AddParameterWithValue(command, "user_id", DbType.Int32, ids.ElementAt(i));
                        AddParameterWithValue(command, "user_id", DbType.Int32, i);
                        AddParameterWithValue(command, "card_name", DbType.String, cardNames.ElementAt(j));
                        AddParameterWithValue(command, "card_type", DbType.String, cardTypes.ElementAt(k));
                        Random rnd = new Random();
                        int rand_num = rnd.Next(10, 50);
                        AddParameterWithValue(command, "damage", DbType.Int32, rand_num);
                        int result_id;
                        try
                        {
                            result_id = (int)(command.ExecuteScalar() ?? 0);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error in initializing the stack.");
                        }
                        //if (k == cardTypes.Count - 1) { k = 0; }
                    }
                }
            }


        }

        public bool insertNewCardsToUserStack(List<Card> cardsToAdd, string sessionId)
        {
            
            
            CredentialRepository credentialRepository = new CredentialRepository();
            string newUsername = credentialRepository.retrieveUsernameFromSessionId(sessionId);
            UserRepository userRepository = new UserRepository();
            User dummyUser =userRepository.retrieveUserInfoFromUsername(newUsername);
            if(dummyUser.coins<20) {
                Console.WriteLine("not enough coins for user " + dummyUser.name + ". He has only " + dummyUser.coins + " coins.");
                return false; 
            }
            else
            {
                Console.WriteLine("User: " + dummyUser.name + " gets his cards!");

            }
            //foreach (var item in cardsToAdd)
            for (int i=0; i<  cardsToAdd.Count;i++)
                {
                using IDbConnection connection = new NpgsqlConnection(this.connectionString);
                using IDbCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = "INSERT INTO  stack( user_id ,card_type ,card_name ,damage)  " +
                "VALUES (@user_id, @card_type, @card_name, @damage) RETURNING user_id";
                //AddParameterWithValue(command, "user_id", DbType.Int32, ids.ElementAt(i));
                AddParameterWithValue(command, "user_id", DbType.Int32, dummyUser.user_id);
                AddParameterWithValue(command, "card_name", DbType.String, cardsToAdd.ElementAt(i).Name);
                AddParameterWithValue(command, "card_type", DbType.String, cardsToAdd.ElementAt(i).ElementType.ToString());
                AddParameterWithValue(command, "damage", DbType.Int32, cardsToAdd.ElementAt(i).Damage);
                int result_id;
                try
                {
                    result_id = (int)(command.ExecuteScalar() ?? 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in initializing the stack.");
                    return false;
                }
            }



            return true;
        }

        public List<Card> retrieveUserCards(User user)
        {
            List<Card> cards = new List<Card>();
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = @"SELECT card_type ,card_name ,damage FROM stack WHERE user_id=@usersId";

            AddParameterWithValue(command, "usersId", DbType.Int16, user.user_id);


            using (IDataReader reader = command.ExecuteReader())
                while (reader.Read())
                {

                    Card card;
                    if (reader[0].ToString().Contains("Spell"))
                    {
                        if (reader[0].ToString() == "FIRE")
                        {
                            card = new SpellCard(reader[1].ToString(), Int32.Parse(reader[2].ToString()), Domain.Enums.MTCG.ElementType.FIRE);

                        }
                        else if (reader[0].ToString() == "WATER")
                        {
                            card = new SpellCard(reader[1].ToString(), Int32.Parse(reader[2].ToString()), Domain.Enums.MTCG.ElementType.WATER);
                        }
                        else
                        {
                            card = new SpellCard(reader[1].ToString(), Int32.Parse(reader[2].ToString()), Domain.Enums.MTCG.ElementType.NORMAL);
                        }
                    }
                    else
                    {
                        if (reader[0].ToString() == "FIRE")
                        {
                            card = new MonsterCard(reader[1].ToString(), Int32.Parse(reader[2].ToString()), Domain.Enums.MTCG.ElementType.FIRE);
                        }
                        else if (reader[0].ToString() == "WATER")
                        {
                            card = new MonsterCard(reader[1].ToString(), Int32.Parse(reader[2].ToString()), Domain.Enums.MTCG.ElementType.WATER);
                        }
                        else
                        {
                            card = new MonsterCard(reader[1].ToString(), Int32.Parse(reader[2].ToString()), Domain.Enums.MTCG.ElementType.NORMAL);
                        }
                    }
                    cards.Add(card);
                }
            user.stack = cards;
            return cards;
        }
        public void initUserStackTable(int user_id)
        {



            List<string> cardTypes = new List<string>() { "WATER", "FIRE", "NORMAL" };
            List<string> cardNames = new List<string>() { "FireTroll" ,"WaterGoblin",
                                                                        "FireSpell",
                                                                        "FireTroll",
                                                                        "WaterSpell",
                                                                        "FireSpell",
                                                                        "WaterSpell",
                                                                        "FireSpell",
                                                                        "WaterSpell",
                                                                        "FireSpell",
                                                                        "WaterGoblin",
                                                                        "WaterSpell",
                                                                        "WaterGoblin",
                                                                        "RegularSpell",
                                                                        "WaterGoblin",
                                                                        "RegularSpell",
                                                                        "Knight",
                                                                        };

            //List<int> ids = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            //for (int i = 0; i < ids.Count; i++)
            int myUserId = 0;
            for (int k = 0; k < cardTypes.Count; k++)
            {
                for (int j = 0; j < cardNames.Count; j++)
                {

                    using IDbConnection connection = new NpgsqlConnection(this.connectionString);
                    using IDbCommand command = connection.CreateCommand();
                    connection.Open();


                    command.CommandText = "INSERT INTO  stack( user_id ,card_type ,card_name ,damage)  " +
                    "VALUES (@user_id, @card_type, @card_name, @damage) RETURNING user_id";
                    //AddParameterWithValue(command, "user_id", DbType.Int32, ids.ElementAt(i));
                    AddParameterWithValue(command, "user_id", DbType.Int32, user_id);
                    AddParameterWithValue(command, "card_name", DbType.String, cardNames.ElementAt(j));
                    AddParameterWithValue(command, "card_type", DbType.String, cardTypes.ElementAt(k));
                    Random rnd = new Random();
                    int rand_num = rnd.Next(10, 50);
                    AddParameterWithValue(command, "damage", DbType.Int32, rand_num);
                    int result_id;
                    try
                    {
                        result_id = (int)(command.ExecuteScalar() ?? 0);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in initializing the stack.");
                    }
                    //if (k == cardTypes.Count - 1) { k = 0; }

                }
            }


        }

        public List<Card> retrieveUserCardsByIds(User user, List<string> cardIds)
        {
            List<Card> cards = new List<Card>();
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            // SELECT   film_id in (1, 2, 3);

            string idsString = "('";
            for (int i = 0; i< cardIds.Count;i++)
            {
                if (i!=cardIds.Count-1)
                {                    
                    idsString += cardIds.ElementAt(i)+ "','";
                }
                else
                {
                    idsString += cardIds.ElementAt(i) + "')";
                }
            }
  
            command.CommandText = @"SELECT name ,id ,damage FROM cards WHERE owner=@userName and Id in " + idsString;

            AddParameterWithValue(command, "userName", DbType.String, user.name);


            using (IDataReader reader = command.ExecuteReader())
                while (reader.Read())
                {

                    Card card;
                    if (reader[0].ToString().Contains("Spell"))
                    {
                        if (reader[0].ToString() == "FIRE")
                        {
                            card = new SpellCard(reader[0].ToString(), Int32.Parse(reader[2].ToString()), Domain.Enums.MTCG.ElementType.FIRE, reader[1].ToString());

                        }
                        else if (reader[0].ToString() == "WATER")
                        {
                            card = new SpellCard(reader[0].ToString(), Double.Parse(reader[2].ToString()), Domain.Enums.MTCG.ElementType.WATER, reader[1].ToString());
                        }
                        else
                        {
                            card = new SpellCard(reader[0].ToString(), Double.Parse(reader[2].ToString()), Domain.Enums.MTCG.ElementType.NORMAL, reader[1].ToString());
                        }
                    }
                    else
                    {
                        if (reader[0].ToString() == "FIRE")
                        {
                            card = new MonsterCard(reader[0].ToString(), Double.Parse(reader[2].ToString()), Domain.Enums.MTCG.ElementType.FIRE, reader[1].ToString());
                        }
                        else if (reader[0].ToString() == "WATER")
                        {
                            card = new MonsterCard(reader[0].ToString(), Double.Parse(reader[2].ToString()), Domain.Enums.MTCG.ElementType.WATER, reader[1].ToString());
                        }
                        else
                        {
                            card = new MonsterCard(reader[0].ToString(), Double.Parse(reader[2].ToString()), Domain.Enums.MTCG.ElementType.NORMAL, reader[1].ToString());
                        }
                    }
                    cards.Add(card);
                }
            return cards;
        }

        public static void AddParameterWithValue(IDbCommand command, string parameterName, DbType type, object value)
        {
            var parameter = command.CreateParameter();
            parameter.DbType = type;
            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }
    }
}
