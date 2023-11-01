using System.Data;
using Npgsql;
using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Interfaces.Persistance;
using System;
using MonsterCards.Domain.Exceptions.Server;
using MonsterCards.Domain.Entities.Server;

namespace MonsterCards.Infrastructure.Persistance
{
    [Serializable]
    internal class CredentialRepository
    {
        private string connectionString;

        public CredentialRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int Add(Credential credential)
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();


            command.CommandText = "INSERT INTO  credentials( username, password)  " +
                "VALUES (@username, @password) RETURNING id";


            AddParameterWithValue(command, "username", DbType.String, credential.Username);
            AddParameterWithValue(command, "password", DbType.String, credential.Password);


            //person.Id = (int)(command.ExecuteScalar() ?? 0);
            int result_id = 0;
            try
            {
                result_id = (int)(command.ExecuteScalar() ?? 0);
            }catch(Exception ex)
            {
                return 0;
            }

            return result_id;
        }

        public bool login(Credential credential)
        {
            if (credential == null) { return false; }

            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = @"SELECT username, password FROM credentials WHERE username=@username and password=@password";
            AddParameterWithValue(command, "username", DbType.String, credential.Username);
            AddParameterWithValue(command, "password", DbType.String,  credential.Password);

            using (IDataReader reader = command.ExecuteReader())
                if (reader.Read())
                {
                    return true;
                }
            return false;
        }

        public int Update(Credential credential,HttpRequest rq, HttpResponse rs)
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();


            command.CommandText = "UPDATE credentials set password=@password  " +
                "WHERE username=@username RETURNING id";


            AddParameterWithValue(command, "username", DbType.String, credential.Username);
            AddParameterWithValue(command, "password", DbType.String, credential.Password);


            //person.Id = (int)(command.ExecuteScalar() ?? 0);
            int result_id = 0;
            try
            {
                if (rq.Path[2] ==credential.Username)
                {                    
                    result_id = (int)(command.ExecuteScalar() ?? 0);
                }
                else
                {
                   throw new UnauthorizedError("You cannot change the password of other users!",rq, rs);
                }
            }
            catch (Exception ex)
            {
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
    }
}
