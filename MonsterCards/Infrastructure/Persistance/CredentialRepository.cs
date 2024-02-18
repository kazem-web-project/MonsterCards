using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Entities.Server;
using MonsterCards.Application;

using Npgsql;
using System.Data;
using MonsterCards.Domain.Interfaces.Server;
using System.Net;
using System.Security.Cryptography;
using MonsterCards.Application;
using System.Text.Json;
using MonsterCards.Domain.Enums.MTCG;
using MonsterCards.Domain.Server;


namespace MonsterCards.Infrastructure.Persistance
{
    [Serializable]
    internal class CredentialRepository : IHttpEndpoint
    {
        private string connectionString;    

        public bool is_logged { get; set; } = false;
        public bool startConditionBool { get; set; } = false;
        public int lastBattleId { get; set; } = 0;
        public CredentialRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public CredentialRepository()
        {
            this.connectionString = "Host=localhost;Database=monster_cards;Username=server;Password=password;Persist Security Info=True";

        }

        public int Add(Credential credential, HttpResponse response)
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();


            command.CommandText = "INSERT INTO  users( username, password, coins, stat)  " +
                "VALUES (@username, @password, 20, 80) RETURNING id";


            AddParameterWithValue(command, "username", DbType.String, credential.username);
            AddParameterWithValue(command, "password", DbType.String, credential.password);


            //person.Id = (int)(command.ExecuteScalar() ?? 0);
            int result_id = 0;
            try
            {
                result_id = (int)(command.ExecuteScalar() ?? 0);
                response.ResponseMessage = "User " + credential.username.ToString() + "created successfully.";
                response.ResponseCode = result_id;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 300;
                response.ResponseMessage = "Cannot create the new user!";
            }

            return result_id;
        }

        public bool login(Credential credential, HttpResponse rs)
        {
            if (credential == null) { return false; }

            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            // command.CommandText = @"SELECT username, password FROM users WHERE username=@username and password=@password";
            command.CommandText = @"SELECT username, password FROM users WHERE username=@username";
            AddParameterWithValue(command, "username", DbType.String, credential.username);
            // AddParameterWithValue(command, "password", DbType.String, credential.password);

            using (IDataReader reader = command.ExecuteReader())
                if (reader.Read())
                {
                    rs.ResponseMessage = "The username " + credential.username + " exists!";
                    rs.ResponseCode = 200;
                    return true;
                }
                else
                {
                    rs.ResponseCode = 410;
                    rs.ResponseMessage = "The username you entered does not exists!";
                }
            return false;
        }
        public string retrieveUsernameFromSessionId(string userSession)
        {
            if (userSession == null) { return null; }

            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = @"SELECT username FROM sessions WHERE sessionid=@session";

            AddParameterWithValue(command, "session", DbType.String, userSession);


            using (IDataReader reader = command.ExecuteReader())
                if (reader.Read())
                {
                    try
                    {
                        return reader[0].ToString();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error reading from session table");
                    }
                }
                else
                {

                }
            return null;
        }


        public bool is_loged(string userSession, HttpResponse rs)
        {
            if (userSession == null) { return false; }

            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = @"SELECT sessionid FROM sessions WHERE sessionid=@session";

            AddParameterWithValue(command, "session", DbType.String, userSession);


            using (IDataReader reader = command.ExecuteReader())
                if (reader.Read())
                {
                    is_logged = true;

                    return true;
                }
                else
                {
                    rs.ResponseMessage = "Bad credentials!";
                    rs.ResponseCode = 414;
                    is_logged = false;
                }
            return false;
        }

        public bool disableNotUsedSessions()
        {

            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = @"update sessions set is_active=false where EXTRACT(EPOCH FROM (current_timestamp-created_time))/3600 >1";


            using (IDataReader reader = command.ExecuteReader())
                if (reader.Read())
                {
                    is_logged = true;

                    return true;
                }
                else
                {
                    is_logged = false;
                }
            return false;
        }
        public bool hasUserSession_is(Credential credential, HttpResponse rs)
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();

            // command.CommandText = @"SELECT max(battle_id) FROM sessions where is_active=true";
            command.CommandText = @"SELECT is_active FROM sessions where username=@myUsername";

            AddParameterWithValue(command, "myUsername", DbType.String, credential.username);

            using (IDataReader reader = command.ExecuteReader())
                if (reader.Read())

                {
                    // Console.WriteLine(reader[0].ToString()+",");
                    return reader[0].ToString() == "True";
                }
                else
                {
                    return false;
                }
        }

        public void createStoreSession(Credential credential, HttpResponse rs)
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();

            var bytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            // and if you need it as a string...
            string session_id = BitConverter.ToString(bytes);


            connection.Open();


            command.CommandText = "INSERT INTO  sessions(  username, is_active, session_pass)  " +
                "VALUES (  @username, true, @new_session_pass) ";


            AddParameterWithValue(command, "username", DbType.String, credential.username);
            // AddParameterWithValue(command, "newSession", DbType.String, session_id);
            AddParameterWithValue(command, "new_session_pass", DbType.String, credential.username+ "-mtcgToken");
            // AddParameterWithValue(command, "battle_id", DbType.Int16, create_battle_id());



            //person.Id = (int)(command.ExecuteScalar() ?? 0);
            int result_id = 200;
            try
            {
                command.ExecuteNonQuery();
                rs.ResponseMessage = "User session for user: " + credential.username.ToString() + " created successfully.";
                rs.ResponseCode = result_id;
            }
            catch (Exception ex)
            {
                rs.ResponseCode = 300;
                rs.ResponseMessage = "Cannot create a session for the user!";
            }

        }
        public void setIsActiveTrue(Credential credential, HttpResponse rs)
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = @"update sessions set is_active=true where username=@myUsername";

            AddParameterWithValue(command, "myUsername", DbType.String, credential.username);

            using (IDataReader reader = command.ExecuteReader())
                if (reader.Read())
                {
                    return;
                }
                else
                {
                    //is_logged = false;
                }
        }

        private int create_battle_id()
        {
            return retrieveMaxBattleId();
        }

        public int retrieveMaxBattleId()
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();

            // command.CommandText = @"SELECT max(battle_id) FROM sessions where is_active=true";
            command.CommandText = @"SELECT max(battle_id) FROM sessions ";


            using (IDataReader reader = command.ExecuteReader())
                if (reader.Read())
                {
                    /*
                    if (is_there_two_user_with_max_battle_id())
                    {
                        lastBattleId = Int32.Parse(reader[0].ToString());
                        return Int32.Parse(reader[0].ToString()) + 1;
                    }
                    return Int32.Parse(reader[0].ToString());
                }
                else
                {
                    return -1;
                }
                    */
                    return -1;


                }
            return -1;

        }
        public bool is_there_two_user_with_max_battle_id()
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            //command.CommandText = @"SELECT count(battle_id) FROM sessions where battle_id = (select max(battle_id) FROM sessions) and is_active=True";
            command.CommandText = @"SELECT count(battle_id) FROM sessions where battle_id = (select max(battle_id) FROM sessions)";


            using (IDataReader reader = command.ExecuteReader())
                if (reader.Read())
                {
                    if (Int32.Parse(reader[0].ToString()) >= 2)
                    {
                        startConditionBool = true;

                        return true;
                    }
                    else if (Int32.Parse(reader[0].ToString()) == 1)
                    {
                        startConditionBool = false;
                        return false;
                    }
                    else
                    {
                        //throw new Exception("There are more than 2 records with the same battle_id in session table");
                        startConditionBool = false;
                        return false;
                    }

                }
                else
                {
                    throw new Exception("Error reading battle_id from session table");
                }

        }
        public int Update(Credential credential, HttpResponse rs)
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();


            command.CommandText = "UPDATE users set password=@password  " +
                "WHERE username=@username RETURNING id";


            AddParameterWithValue(command, "username", DbType.String, credential.username);
            AddParameterWithValue(command, "password", DbType.String, credential.password);


            //person.Id = (int)(command.ExecuteScalar() ?? 0);
            int result_id = 0;
            try
            {
                result_id = (int)(command.ExecuteScalar() ?? 0);
                rs.ResponseMessage = "The username " + credential.username + " has changed the password!";
                rs.ResponseCode = 200;
            }
            catch (Exception ex)
            {
                rs.ResponseMessage = "The username " + credential.username + " cannot change the password to: " + credential.password;
                rs.ResponseCode = 413;
                return 0;
            }

            return result_id;
        }




        public static void AddParameterWithValue(IDbCommand command, string parameterName, DbType type, object value)
        {
            var parameter = command.CreateParameter();
            parameter.DbType = type;
            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }

        public bool HandleRequest(HttpRequest rq, HttpResponse rs, Battle battle)
        {

            if (rq.Path[1] == "users")
            {
                UserHandler userHandler = new UserHandler();
                userHandler.HandleRequest(rq, rs, battle);

            }
            else if (rq.Path[1] == "sessions")
            { }
            else if (rq.Path[1] == "packages")
            {
                //rq.Content.

                /*
                string input = rq.Content;
                //"\"[{\\\"Id\\\":\\\"845f0dc7-37d0-426e-994e-43fc3ac83c08\\\", \\\"Name\\\":\\\"WaterGoblin\\\", \\\"Damage\\\": 10.0}, {\\\"Id\\\":\\\"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\\\", \\\"Name\\\":\\\"Dragon\\\", \\\"Damage\\\": 50.0}, {\\\"Id\\\":\\\"e85e3976-7c86-4d06-9a80-641c2019a79f\\\", \\\"Name\\\":\\\"WaterSpell\\\", \\\"Damage\\\": 20.0}, {\\\"Id\\\":\\\"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\\\", \\\"Name\\\":\\\"Ork\\\", \\\"Damage\\\": 45.0}, {\\\"Id\\\":\\\"dfdd758f-649c-40f9-ba3a-8657f4b3439f\\\", \\\"Name\\\":\\\"FireSpell\\\",    \\\"Damage\\\": 25.0}]\"";
                var data = (JObject)JsonConvert.DeserializeObject(json);
                JObject obj = JObject.Parse(json);
                string name = (string) obj["Name"];
                Console.WriteLine(input);
                */
                List<Card> cardsResult = new List<Card>();
                List<Card>? cardToCheck =
                    JsonSerializer.Deserialize<List<Card>>(rq.Content);
                foreach (var item in cardToCheck)
                {
                    if (item.Name.ToUpper().Contains("WATER"))
                    {
                        item.ElementType = Domain.Enums.MTCG.ElementType.WATER;
                        if (item.Name.Contains("Spell"))
                        {
                            Card cardToAdd = new SpellCard(item.Name, item.Damage, ElementType.WATER);
                            cardsResult.Add(cardToAdd);
                        }
                        else
                        {
                            Card cardToAdd = new MonsterCard(item.Name, item.Damage, ElementType.WATER);
                            cardsResult.Add(cardToAdd);
                        }
                    }
                    else if (item.Name.ToUpper().Contains("FIRE"))
                    {
                        item.ElementType = Domain.Enums.MTCG.ElementType.FIRE;
                        if (item.Name.Contains("Spell"))
                        {
                            Card cardToAdd = new SpellCard(item.Name, item.Damage, ElementType.FIRE);
                            cardsResult.Add(cardToAdd);
                        }
                        else
                        {
                            Card cardToAdd = new MonsterCard(item.Name, item.Damage, ElementType.FIRE);
                            cardsResult.Add(cardToAdd);
                        }

                    }
                    else
                    {
                        item.ElementType = Domain.Enums.MTCG.ElementType.NORMAL;
                        if (item.Name.Contains("Spell"))
                        {
                            Card cardToAdd = new SpellCard(item.Name, item.Damage, ElementType.NORMAL);
                            cardsResult.Add(cardToAdd);
                        }
                        else
                        {
                            Card cardToAdd = new MonsterCard(item.Name, item.Damage, ElementType.NORMAL);
                            cardsResult.Add(cardToAdd);
                        }
                    }
                }
                StackRepository stackRepository = new StackRepository();
                // {[X-request-ID, 0E-E1-14-F9-CB-87-D4-7E-02-98-8E-42-C3-25-B8-49]}
                stackRepository.insertNewCardsToUserStack(cardsResult, rq.Headers["X-Request-ID"]);

            }

            return true;
        }

        public List<User> retriveLoggedUsers()
        {

            List<User> users = new List<User>();

            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = @"SELECT username FROM sessions WHERE is_active=true";

            AddParameterWithValue(command, "battle_id", DbType.Int16, lastBattleId);


            using (IDataReader reader = command.ExecuteReader())
                while (reader.Read())
                {
                    User newUser = new User();
                    if (reader[0].ToString()== "admin") { continue; }
                    newUser.name = reader[0].ToString();
                    users.Add(newUser);
                }

            return users;
        }
    }
}
