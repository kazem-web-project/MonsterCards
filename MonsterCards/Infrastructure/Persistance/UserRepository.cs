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
