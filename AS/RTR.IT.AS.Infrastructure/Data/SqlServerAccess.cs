using RTR.IT.AS.Core.Entities.Api.Result;
using RTR.IT.AS.Core.Entities.App.Base;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace RTR.IT.AS.Infrastructure.Data
{
    public class SqlServerAccess<T> : IDisposable
    {
        private IDbConnection connection;
        private IDbCommand command;
        private readonly string connectionStringName;
#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
        public delegate BaseResult<T> MapEntity<T>(IDataReader reader) where T : BaseDomain;
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type

        public SqlServerAccess(string name)
        {
            connectionStringName = name;
        }

        private void createCommand()
        {
            if (command == null)
            {
                if (connection == null)
                    createConnection();

                command = connection.CreateCommand();
            }
            if (connection != null && string.IsNullOrEmpty(connection.ConnectionString))
            {
                createConnection();
                command = connection.CreateCommand();
            }
        }

        private void createConnection()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
        }

        private void openConnection(string procedure)
        {
            command.CommandText = procedure;
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
        }

#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
        public BaseResult<T> ExcecuteProcedure<T>(string procedure, MapEntity<T> mapEntity) where T : BaseDomain
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
        {
            BaseResult<T> items = new BaseResult<T>();
            openConnection(procedure);
            using (var reader = command.ExecuteReader())
            {
                items = mapEntity(reader);
            }
            connection.Dispose();
            return items;
        }

        public void ExcecuteProcedure(string procedure)
        {
            openConnection(procedure);
            command.ExecuteNonQuery();
            connection.Dispose();
        }

        public void AddParameter(string name, object value)
        {
            createCommand();
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
