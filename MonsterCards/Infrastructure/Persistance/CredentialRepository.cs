using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Entities.Server;
using MonsterCards.Application;

using Npgsql;
using System.Data;
using MonsterCards.Domain.Interfaces.Server;
using System.Net;
using System.Security.Cryptography;
using MonsterCards.Application;


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


            command.CommandText = "INSERT INTO  users( username, password)  " +
                "VALUES (@username, @password) RETURNING id";


            AddParameterWithValue(command, "username", DbType.String, credential.username);
            AddParameterWithValue(command, "password", DbType.String, credential.password);


            //person.Id = (int)(command.ExecuteScalar() ?? 0);
            int result_id = 200;
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




        private void createStoreSession(Credential credential, HttpResponse rs)
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


            command.CommandText = "INSERT INTO  sessions( sessionId, username, created_time,is_active,battle_id)  " +
                "VALUES ( @newSession, @username, now(), true, @battle_id) ";


            AddParameterWithValue(command, "username", DbType.String, credential.username);
            AddParameterWithValue(command, "newSession", DbType.String, session_id);

            AddParameterWithValue(command, "battle_id", DbType.Int16, create_battle_id());



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

        private int create_battle_id()
        {
            return retrieveMaxBattleId();
        }

        private int retrieveMaxBattleId()
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();

            command.CommandText = @"SELECT max(battle_id) FROM sessions";


            using (IDataReader reader = command.ExecuteReader())
                if (reader.Read())

                {
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


        }
        bool is_there_two_user_with_max_battle_id()
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
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
                        throw new Exception("There are more than 2 records with the same battle_id in session table");
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

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Path[1] == "users")
            {
                if (rq.Method.Equals(MonsterCards.Domain.Enums.Server.HttpMethod.POST))
                {
                    Credential credential = new Credential();
                    // retrieve username
                    credential.username = rq.Content.Split(',')[0].Split(':')[1];
                    int len_username = credential.username.Length;
                    credential.username = credential.username.Substring(1, len_username - 2);

                    //  retrieve password
                    credential.password = rq.Content.Split(',')[1].Split(':')[1];
                    int len_pass = credential.password.Length;
                    credential.password = credential.password.Substring(1, len_pass - 3);

                    this.Add(credential, rs);

                }
                else if (rq.Method.Equals(MonsterCards.Domain.Enums.Server.HttpMethod.GET) && rq.Headers.ContainsKey("X-Request-ID") && is_loged(rq.Headers["X-Request-ID"], rs))
                {
                    Credential credential = new Credential();
                    credential.username = rq.Path[2];

                    this.login(credential, rs);
                    //this.is_loged(credential,rs);
                }
                else if (rq.Method.Equals(MonsterCards.Domain.Enums.Server.HttpMethod.PUT) && rq.Headers.ContainsKey("X-Request-ID") && is_loged(rq.Headers["X-Request-ID"], rs))
                {
                    Credential credential = new Credential();
                    // retrieve username
                    credential.username = rq.Content.Split(',')[0].Split(':')[1];
                    int len_username = credential.username.Length;
                    credential.username = credential.username.Substring(1, len_username - 2);

                    //  retrieve password
                    credential.password = rq.Content.Split(',')[1].Split(':')[1];
                    int len_pass = credential.password.Length;
                    credential.password = credential.password.Substring(1, len_pass - 3);

                    if (this.is_logged || credential.username == "Admin")
                    {
                        this.Update(credential, rs);
                    }
                    else
                    {
                        rs.ResponseMessage = "Please login to change the user data!";
                        rs.ResponseCode = 411;
                    }
                }


            }
            else if (rq.Path[1] == "sessions")
            {
                Credential credential = new Credential();
                // retrieve username
                credential.username = rq.Content.Split(',')[0].Split(':')[1];
                int len_username = credential.username.Length;
                credential.username = credential.username.Substring(1, len_username - 2);

                //  retrieve password
                credential.password = rq.Content.Split(',')[1].Split(':')[1];
                int len_pass = credential.password.Length;
                credential.password = credential.password.Substring(1, len_pass - 3);
                //this.is_loged(credential,rs); 
                createStoreSession(credential, rs);
                retrieveMaxBattleId();
                is_there_two_user_with_max_battle_id();
                if (startConditionBool && lastBattleId != 0)
                {
                    List<User> users = retriveLoggedUsers(rs);
                    Console.WriteLine("BATTLE BEGINS..............................................");
                    Battle newBattle = new Battle(users);

                    /*
                    User user1 = new User();
                    user1.name = "altenhof";
                    User user2 = new User();
                    user2.name = "kienboec";
                    List<User> users = new List<User>() { user1, user2 };
                    Battle battle = new Battle(users);
                    battle.perpareUser();
                    battle.battleStart();
                    */
                }
            }

            return true;
        }

        private List<User> retriveLoggedUsers(HttpResponse rs)
        {

            List<User> users = new List<User>();

            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = @"SELECT username FROM sessions WHERE battle_id=@battle_id";

            AddParameterWithValue(command, "battle_id", DbType.Int16, lastBattleId);


            using (IDataReader reader = command.ExecuteReader())
                while (reader.Read())
                {
                    User newUser = new User();
                    newUser.name = reader[0].ToString();
                    users.Add(newUser);
                }
                
            return users;

        }
    }
}
