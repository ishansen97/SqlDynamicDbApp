using Microsoft.Data.SqlClient;
using SqlDynamicDbApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDynamicDbApp.Connection
{
    public class DatabaseConnection
    {
        private static DatabaseConnection _databaseConnection;
        private static SqlConnection _sqlConnection;
        private static string _masterConnectionString = "Data Source=DESKTOP-VCM7FP3;Initial Catalog=MasterDB;Integrated Security=True;Pooling=False";
        private Dictionary<int, SqlConnection> _connectionPool;

        private DatabaseConnection()
        {
            _connectionPool = new Dictionary<int, SqlConnection>();
        }

        public static DatabaseConnection GetInstance()
        {
            if (_databaseConnection == null)
            {
                lock (typeof(DatabaseConnection))
                {
                    if (_databaseConnection == null)
                    {
                        if (_sqlConnection == null)
                        {
                            _sqlConnection = new SqlConnection(_masterConnectionString);
                        }
                        _databaseConnection = new DatabaseConnection();
                    }
                }
            }

            return _databaseConnection;
        }

        public SqlConnection SqlConnection 
        { 
            get
            {
                return _sqlConnection;
            } 
        }

        public SqlConnection GetUserDBConnection(ConnectionModel model)
        {
            if (!_connectionPool.ContainsKey(model.Id)) {
                _connectionPool.Add(model.Id, new SqlConnection(model.ConnectionString));
            }

            return _connectionPool[model.Id];
        }

        public string GetUserConnectionString(string name) => $"Data Source=DESKTOP-VCM7FP3;Initial Catalog={name};Integrated Security=True;Pooling=False";
    }
}
