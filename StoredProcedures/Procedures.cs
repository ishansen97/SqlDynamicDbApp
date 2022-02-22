using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDynamicDbApp.StoredProcedures
{
    public class Procedures
    {
        public static string InsertStoredProcedure = "MasterDB_InsertStoredProcedure";
        public static string InsertConnectionString = "InsertConnectionString";
        public static string GetAllConnectionStrings = "GetAllConnectionStrings";
        public static string GetDetailsStoredProcedure = "GetUserDetailsProcedure";
        public static string CreateUserDatabase = "CreateUserDatabase";
        public static string CreateUserProductTable = "CreateUserProductTable";

        public static string CreateUserProductTableQuery = "CREATE TABLE [dbo].[Product](" +
        "[Id][int] IDENTITY(1,1) NOT NULL," +
       "[Name] [varchar] (50) NOT NULL," +
        "[Company] [varbinary] (50) NOT NULL," +
         "[Manufactured] [date]" +
        "NOT NULL," +
         "[Expired] [date]" +
        "NOT NULL" +
        ") ON[PRIMARY]";
    }
}
