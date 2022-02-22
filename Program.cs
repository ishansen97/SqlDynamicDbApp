using Microsoft.Data.SqlClient;
using SqlDynamicDbApp.Connection;
using SqlDynamicDbApp.DatabaseOperations;
using SqlDynamicDbApp.StoredProcedures;
using System;
using System.Collections.Generic;
using System.Data;

class Program
{
    static void Main()
    {
        Console.WriteLine("===== Creating databases =====");
        Console.WriteLine("Input the Database name: ");
        string dbName = Console.ReadLine();

        if (dbName != null)
        { 
            Console.WriteLine("Database has started creating.....");
            Operations.CreateDB(dbName);
        }

        
        //TestMethod();
    }

    private static void TestMethod()
    {
        //string connectionString =
        //    "Data Source=DESKTOP-VCM7FP3;Initial Catalog=MasterDB;Integrated Security=True;Pooling=False";
        string connectionString =
            "Data Source=DESKTOP-VCM7FP3;Initial Catalog=MasterDB;Integrated Security=True;Pooling=False";

        string commandString = "INSERT INTO Main(Name, Age, Description) VALUES ('Nimesh', 26, 'Hello Nimes')";

        Dictionary<int, string> connectionStrings = new Dictionary<int, string>();


        // Create and open the connection in a using block. This
        // ensures that all resources will be closed and disposed
        // when the code exits.
        using (var connection =
            new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(Procedures.GetDetailsStoredProcedure, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@Id", 1));
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                connectionStrings.Add(reader.GetInt32(0), reader.GetString(2));
            }
            //for (int i = 0; i < 10; i++)
            //{
            //    string queryString = $"CREATE DATABASE {"Ishan" + i}";
            //    SqlCommand command = new SqlCommand(queryString, connection);
            //    command.ExecuteNonQuery();
            //}
        }

        if (connectionStrings.TryGetValue(1, out var conStr))
        {
            using (var conn2 = new SqlConnection(conStr))
            {
                conn2.Open();
                SqlCommand command2 = new SqlCommand(commandString, conn2);
                int res = command2.ExecuteNonQuery();

                if (res > 0) Console.WriteLine("Success");
            }
        }
    }
}