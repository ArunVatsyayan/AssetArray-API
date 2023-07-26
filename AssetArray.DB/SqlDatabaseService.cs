using System;
using System.Data.SqlClient;
namespace AssetArray.DB
{
    public class SqlDatabaseService : IDisposable
    {
        private readonly string _connectionString;
        private SqlConnection _connection;

        public SqlDatabaseService(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(_connectionString);
        }

        public void OpenConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
            {
                _connection.Close();
            }
        }

        public int ExecuteCommand(string query)
        {
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                return command.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            CloseConnection();
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}