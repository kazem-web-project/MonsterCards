using System.Data;
using Npgsql;
using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Interfaces.Persistance;
using System;
using System.Net;
using static System.Net.Mime.MediaTypeNames;
using MonsterCards.Domain.Enums.MTCG;

namespace MonsterCards.Infrastructure.Persistance
{
    internal class UserRepository : IUserRepository
    {


        private string connectionString;

        public UserRepository(string connectionString)
        {
            //this.connectionString = connectionString;
            this.connectionString = connectionString;
        }
        public UserRepository()
        {
            //this.connectionString = connectionString;
            this.connectionString = "Host=localhost;Database=monster_cards;Username=server;Password=password;Persist Security Info=True";
        }


        bool IUserRepository.Add(User user)
        {
            /*
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();

            command.CommandText = "INSERT INTO users (name, age, description) " +
                "VALUES (@name, @age, @description) RETURNING id";
            AddParameterWithValue(command, "name", DbType.String, person.Name);
            AddParameterWithValue(command, "age", DbType.Int32, person.Age);
            AddParameterWithValue(command, "description", DbType.String, person.Description);
            person.Id = (int)(command.ExecuteScalar() ?? 0);
            */
            return false;
        }

        public static void AddParameterWithValue(IDbCommand command, string parameterName, DbType type, object value)
        {

            var parameter = command.CreateParameter();
            parameter.DbType = type;
            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);

        }
        public void retrieveUserInfo(User user)
        {
            if (user == null) { return; }

            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();

            command.CommandText = @"SELECT  id , username  , stack_id , coins , stat from users WHERE username=@usernameInput";
            AddParameterWithValue(command, "usernameInput", DbType.String, user.name);


            using (IDataReader reader = command.ExecuteReader())
                while (reader.Read())
                {
                    try
                    {
                        int user_id = Int32.Parse(reader[0].ToString());
                        string username = reader[1].ToString();
                        // string stack_id= reader[1].ToString();
                        int coins = Int32.Parse(reader[3].ToString());
                        int stat = Int32.Parse(reader[4].ToString());

                        user.coins = coins;
                        user.stat = stat;
                        user.user_id = user_id;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error fetching data from user db");
                    }
                }
        }
        public User retrieveUserInfoFromUsername(string username)
        {
            if (username == null) { return null; }
            User user = new User();
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();

            command.CommandText = @"SELECT  id , username  , stack_id , coins , stat from users WHERE username=@usernameInput";
            AddParameterWithValue(command, "usernameInput", DbType.String, username);
            

            using (IDataReader reader = command.ExecuteReader())
                while (reader.Read())
                {
                    try
                    {
                        int user_id = Int32.Parse(reader[0].ToString());
                        string myUsername = reader[1].ToString();
                        // string stack_id= reader[1].ToString();
                        int coins = Int32.Parse(reader[3].ToString());
                        int stat = Int32.Parse(reader[4].ToString());

                        user.coins = coins;
                        user.stat = stat;
                        user.user_id = user_id;
                        user.name  = reader[1].ToString();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error fetching data from user db");
                    }
                }
            
            return user;
        }

        public string reduceCoinsByFive(string username)
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();


            command.CommandText = "UPDATE users set coins=coins - 5  " +
                "WHERE username=@username RETURNING id";


            AddParameterWithValue(command, "username", DbType.String, username);


            int result_id = 0;
            try
            {
                result_id = (int)(command.ExecuteScalar() ?? 0);
                return "Coins from user " + username + " reduced by 5";
                
            }
            catch (Exception ex)
            {
                return "Coins from user " + username + " cannot be reduced";
            }            
        }
        public string retrieveUserInfoFromUsernameForUser(string username)
        {
            if (username == null) { return null; }
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();

            command.CommandText = @"SELECT  username  , coins , stat, bio, name, image from users WHERE username=@usernameInput";
            AddParameterWithValue(command, "usernameInput", DbType.String, username);


            using (IDataReader reader = command.ExecuteReader())
                while (reader.Read())
                {
                    try
                    {
                        //int user_id = Int32.Parse(reader[0].ToString());
                        string myUsername = reader[0].ToString();
                        // string stack_id= reader[1].ToString();
                        int coins = Int32.Parse(reader[1].ToString());
                        int stat = Int32.Parse(reader[2].ToString());
                        string bio = reader[3].ToString();
                        string name = reader[4].ToString();
                        string image = reader[5].ToString();
                        return "The user information: </br> Name: " + name + "</br> " + " has " + coins.ToString() + " coins and stat: " + stat.ToString() +
                            " </br>bio: " + bio + "</br> Image : " + image; 
                        
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error fetching data from user db");
                    }
                }
            return "No such information!";
        }

        internal string editUserInfoFromUsernameForUser(string username, UserDTO? userData)
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();


            command.CommandText = "UPDATE users set name=@userGivenName,image=@userImage, bio=@userBio " +
                "WHERE username=@username RETURNING id";


            AddParameterWithValue(command, "username", DbType.String, username);
            AddParameterWithValue(command, "userGivenName", DbType.String, userData.Name);
            AddParameterWithValue(command, "userImage", DbType.String, userData.Image);
            AddParameterWithValue(command, "userBio", DbType.String, userData.Bio);


            //person.Id = (int)(command.ExecuteScalar() ?? 0);
            int result_id = 0;
            try
            {
                result_id = (int)(command.ExecuteScalar() ?? 0);
                return "The username " + username + " has changed his/her bio!";
            }
            catch (Exception ex)
            {
                 return "The username " + username + " cannot changed his/her information data!";

            }            
        }
    }
}
