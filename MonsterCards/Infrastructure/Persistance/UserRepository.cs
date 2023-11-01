using System.Data;
using Npgsql;
using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Interfaces.Persistance;
using System;

namespace MonsterCards.Infrastructure.Persistance
{
    internal class UserRepository : IUserRepository
    {


        private string connectionString;

        public UserRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }


        bool IUserRepository.Add(User user)
        {
            
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            // insert into users ( name ,deck_id ,stat_id ,coins, credential_id) values ('test', 10,10,10, 10);
            // command.CommandText = "INSERT INTO users ( name ,deck_id ,stat_id ,coins, credential_id) " +
            //     "VALUES (@name, @deck_id, @stat_id, @coins, @credential_id) RETURNING id";

            command.CommandText = "INSERT INTO users ( name ,deck_id ,stat_id ,coins, credential_id) " +
                "VALUES (@name, @deck_id, @stat_id, @coins, @credential_id) RETURNING id";


            AddParameterWithValue(command, "name", DbType.String, user.name);
            //AddParameterWithValue(command, "deck_id", DbType.Int32, user.deck);
            //AddParameterWithValue(command, "stat_id", DbType.Int32, user.deck);
            AddParameterWithValue(command, "coins", DbType.Int32, 10);
            AddParameterWithValue(command, "stat", DbType.Int32, 100);
            // AddParameterWithValue(command, "credential_id", DbType.Int32, user.deck);
            /*

            AddParameterWithValue(command, "name", DbType.String, user.name);
            AddParameterWithValue(command, "deck_id", DbType.Int32, user.deck);
            AddParameterWithValue(command, "stat_id", DbType.Int32, user.deck);
            AddParameterWithValue(command, "coins", DbType.Int32, user.deck);
            AddParameterWithValue(command, "credential_id", DbType.Int32, user.deck);
            */


            // AddParameterWithValue(command, "description", DbType.String, person.Description);
            //person.Id = (int)(command.ExecuteScalar() ?? 0);
            
            return false;
        }

        public static void AddParameterWithValue(IDbCommand command, string parameterName, DbType type, object value)
        {
            /*
            var parameter = command.CreateParameter();
            parameter.DbType = type;
            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
            */
        }
    }
}
