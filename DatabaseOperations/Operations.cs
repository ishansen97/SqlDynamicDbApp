using Microsoft.Data.SqlClient;
using SqlDynamicDbApp.Connection;
using SqlDynamicDbApp.Models;
using SqlDynamicDbApp.StoredProcedures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDynamicDbApp.DatabaseOperations
{
    public class Operations
    {
        public static bool CreateDB(string name)
        {
            var defaultConnection = DatabaseConnection.GetInstance();
            decimal? insertedId = null;

            using (var connection = defaultConnection.SqlConnection)
            {
                connection.Open();
                // create database
                SqlCommand createDBCommand = new SqlCommand(Procedures.CreateUserDatabase, connection);
                createDBCommand.CommandType = CommandType.StoredProcedure;
                createDBCommand.Parameters.Add(new SqlParameter("@dbName", name));
                createDBCommand.ExecuteNonQuery();
                Console.WriteLine("===== Database created =====");

                using (SqlTransaction transaction = connection.BeginTransaction())
                {

                    // insert new db connection to master.
                    SqlCommand insertCommand = new SqlCommand(Procedures.InsertConnectionString, connection);
                    insertCommand.Transaction = transaction;
                    insertCommand.CommandType = CommandType.StoredProcedure;
                    insertCommand.Parameters.Add(new SqlParameter("@name", name));
                    insertCommand.Parameters.Add(new SqlParameter("@connectionString", defaultConnection.GetUserConnectionString(name)));
                    insertCommand.Parameters.Add(new SqlParameter("@description", string.Empty));
                    
                    using (SqlDataReader reader = insertCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //insertedId = reader.GetInt32("Id");
                            insertedId = reader.GetDecimal("Id");
                        }
                    }
                    Console.WriteLine("===== finished inserting connection string =====");

                    transaction.Commit();
                }

                Console.WriteLine("===== finished creating product table =====");
            }

            // creating the user product table
            if (insertedId.HasValue && insertedId != 0)
            {
                var conModel = new ConnectionModel
                {
                    Id = Convert.ToInt32(insertedId.Value),
                    ConnectionString = defaultConnection.GetUserConnectionString(name)
                };

                using (var userConnection = defaultConnection.GetUserDBConnection(conModel))
                {
                    userConnection.Open();

                    using (var usertransaction = userConnection.BeginTransaction())
                    {
                        SqlCommand command = new SqlCommand(Procedures.CreateUserProductTableQuery, userConnection);
                        command.Transaction = usertransaction;
                        //command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();

                        usertransaction.Commit();
                    }
                }
            }

            return true;
        }
    }
}
