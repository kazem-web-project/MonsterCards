using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Entities.Server;
using MonsterCards.Domain.Enums.MTCG;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Infrastructure.Persistance
{
    internal class PackageRepository
    {

        private string connectionString;
        public PackageRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public PackageRepository()
        {
            this.connectionString = "Host=localhost;Database=monster_cards;Username=server;Password=password;Persist Security Info=True";

        }
        public bool isUserLogedIn(string userSessionFromPath)
        {
            if (userSessionFromPath == null) { return false; }

            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = @"SELECT session_pass FROM sessions WHERE session_pass=@session";

            AddParameterWithValue(command, "session", DbType.String, userSessionFromPath);


            using (IDataReader reader = command.ExecuteReader())
                if (reader.Read())
                {
                    try
                    {

                        if (userSessionFromPath == reader[0].ToString())
                        {

                            return true;
                        }
                        return false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error reading from session table");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
        }
        public string addNewPackage(List<Card> newCards, int packageNumber)
        {
            string result = "";
            foreach (Card card in newCards)
            {
                result += addNewCard(card, packageNumber) + "       </br>";
            }
            return result;
        }

        public string addNewCard(Card newCard, int packageNumber)
        {

            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();

            //  id | name | damage | owner | in_deck
            command.CommandText = "INSERT INTO  cards( id, name, damage,in_deck, packet_number)  " +
                "VALUES (@new_id, @new_name, @new_damage, false, @new_packet_number) RETURNING id";


            AddParameterWithValue(command, "new_id", DbType.String, newCard.Id);
            AddParameterWithValue(command, "new_name", DbType.String, newCard.Name);
            AddParameterWithValue(command, "new_damage", DbType.Double, newCard.Damage);
            AddParameterWithValue(command, "new_packet_number", DbType.Int16, packageNumber);



            //person.Id = (int)(command.ExecuteScalar() ?? 0);
            //int result_id = 0;
            try
            {
                //result_id = (int)(command.ExecuteScalar() ?? 0);
                command.ExecuteReader();
                return "The new card(" + newCard.Name + ") created successfuly!";

            }
            catch (Exception ex)
            {
                return "Cannot create the new card(" + newCard.Name + ")!";
            }


        }

        public static void AddParameterWithValue(IDbCommand command, string parameterName, DbType type, object value)
        {
            var parameter = command.CreateParameter();
            parameter.DbType = type;
            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }

        public string buyCard(string buyerUsername)
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();


            command.CommandText = "UPDATE cards set owner=@newBuyer  " +
                "WHERE id in (select id from cards where owner is null limit 5) and owner is null";


            AddParameterWithValue(command, "newBuyer", DbType.String, buyerUsername);


            //person.Id = (int)(command.ExecuteScalar() ?? 0);
            int result_id = 0;
            try
            {
                command.ExecuteReader();
                return "User " + buyerUsername + " has bought the a package successfuly!";
            }
            catch (Exception ex)
            {
                return "User " + buyerUsername + " cannot buy the package!";

            }
        }

        public int accuireMaxPackageNumber()
        {

            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = @"SELECT max(packet_number) FROM cards";



            using (IDataReader reader = command.ExecuteReader())
                if (reader.Read())
                {
                    try
                    {
                        return (int)reader[0];
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error reading from cards table");
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
        }
        public bool isThereEnoughCardToSell()
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = @"SELECT count(packet_number) FROM cards where owner is null";


            using (IDataReader reader = command.ExecuteReader())
                if (reader.Read())
                {
                    try
                    {
                        if (((int.Parse(reader[0].ToString())) >= 5))
                        {
                            return true;
                        }

                        return false;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error reading from cards table");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
        }
        public List<Card> retrieveUserCards(string username)
        {
            List<Card> cardsResult = new List<Card>();

            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            // id                  |     name     | damage |  owner   | in_deck | packet_number
            command.CommandText = @"SELECT  id, name, damage ,owner ,in_deck ,packet_number FROM cards WHERE owner=@username";
            AddParameterWithValue(command, "username", DbType.String, username);
            // AddParameterWithValue(command, "password", DbType.String, credential.password);


            using (IDataReader reader = command.ExecuteReader())
                while (reader.Read())
                {

                    Card cardToAdd;
                    string nameCol = reader[1].ToString().ToUpper();
                    if (nameCol.Contains("WATER"))
                    {
                        if (reader[1].ToString().Contains("Spell"))
                        {
                            //Card cardToAdd = new SpellCard(item.Name, item.Damage, ElementType.WATER);
                            cardToAdd = new SpellCard(reader[1].ToString(), Double.Parse(reader[2].ToString()), ElementType.WATER, reader[0].ToString());
                            cardsResult.Add(cardToAdd);
                        }
                        else
                        {
                            cardToAdd = new MonsterCard(reader[1].ToString(), Double.Parse(reader[2].ToString()), ElementType.WATER, reader[0].ToString());
                            cardsResult.Add(cardToAdd);
                        }
                    }
                    else if (nameCol.Contains("FIRE"))
                    {
                        if (reader[1].ToString().Contains("Spell"))
                        {
                            cardToAdd = new SpellCard(reader[1].ToString(), Double.Parse(reader[2].ToString()), ElementType.FIRE, reader[0].ToString());
                            cardsResult.Add(cardToAdd);
                        }
                        else
                        {
                            cardToAdd = new MonsterCard(reader[1].ToString(), Double.Parse(reader[2].ToString()), ElementType.FIRE, reader[0].ToString());

                            cardsResult.Add(cardToAdd);
                        }

                    }
                    else
                    {
                        if (nameCol.ToUpper().Contains("REGULAR"))
                        {
                            cardToAdd = new SpellCard(reader[1].ToString(), Double.Parse(reader[2].ToString()), ElementType.NORMAL, reader[0].ToString());
                            cardsResult.Add(cardToAdd);
                        }
                        else
                        {
                            cardToAdd = new MonsterCard(reader[1].ToString(), Double.Parse(reader[2].ToString()), ElementType.NORMAL, reader[0].ToString());
                            cardsResult.Add(cardToAdd);
                        }

                    }


                }
            return cardsResult;
        }

        public List<Card> retrieveUserDeckCards(string username)
        {
            List<Card> cardsResult = new List<Card>();

            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            // id                  |     name     | damage |  owner   | in_deck | packet_number
            command.CommandText = @"SELECT  id, name, damage ,owner ,in_deck ,packet_number FROM cards WHERE owner=@username and in_deck=true";
            AddParameterWithValue(command, "username", DbType.String, username);
            // AddParameterWithValue(command, "password", DbType.String, credential.password);


            using (IDataReader reader = command.ExecuteReader())
                while (reader.Read())
                {

                    Card cardToAdd;
                    string nameCol = reader[1].ToString().ToUpper();
                    if (nameCol.Contains("WATER"))
                    {
                        if (reader[1].ToString().Contains("Spell"))
                        {
                            //Card cardToAdd = new SpellCard(item.Name, item.Damage, ElementType.WATER);
                            cardToAdd = new SpellCard(reader[1].ToString(), Double.Parse(reader[2].ToString()), ElementType.WATER, reader[0].ToString());
                            cardsResult.Add(cardToAdd);
                        }
                        else
                        {
                            cardToAdd = new MonsterCard(reader[1].ToString(), Double.Parse(reader[2].ToString()), ElementType.WATER, reader[0].ToString());
                            cardsResult.Add(cardToAdd);
                        }
                    }
                    else if (nameCol.Contains("FIRE"))
                    {
                        if (reader[1].ToString().Contains("Spell"))
                        {
                            cardToAdd = new SpellCard(reader[1].ToString(), Double.Parse(reader[2].ToString()), ElementType.FIRE, reader[0].ToString());
                            cardsResult.Add(cardToAdd);
                        }
                        else
                        {
                            cardToAdd = new MonsterCard(reader[1].ToString(), Double.Parse(reader[2].ToString()), ElementType.FIRE, reader[0].ToString());

                            cardsResult.Add(cardToAdd);
                        }

                    }
                    else
                    {
                        if (nameCol.ToUpper().Contains("REGULAR"))
                        {
                            cardToAdd = new SpellCard(reader[1].ToString(), Double.Parse(reader[2].ToString()), ElementType.NORMAL, reader[0].ToString());
                            cardsResult.Add(cardToAdd);
                        }
                        else
                        {
                            cardToAdd = new MonsterCard(reader[1].ToString(), Double.Parse(reader[2].ToString()), ElementType.NORMAL, reader[0].ToString());
                            cardsResult.Add(cardToAdd);
                        }

                    }


                }
            return cardsResult;
        }

        public string insertCardsInDeck(string username,List<string> deckIdCardsListString)
        {
            string result = "";
            foreach (var cardToInsertDeck in deckIdCardsListString)
            {
                using IDbConnection connection = new NpgsqlConnection(connectionString);
                using IDbCommand command = connection.CreateCommand();
                connection.Open();


                command.CommandText = "UPDATE cards set in_deck=true  " +
                    "WHERE owner=@player and id=@cardId";


                AddParameterWithValue(command, "player", DbType.String, username);
                AddParameterWithValue(command, "cardId", DbType.String, cardToInsertDeck);


                //person.Id = (int)(command.ExecuteScalar() ?? 0);
                int result_id = 0;
                try
                {
                    command.ExecuteReader();
                    result +="User " + username + " inserted the card +" + cardToInsertDeck+ " into his deck successfuly!</br>";
                }
                catch (Exception ex)
                {
                    result += "User " + username + " cannot insert the card +" + cardToInsertDeck + " into his deck!</br>";
                }
            }
            return result;
           
        }

    }


}
