using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace SignalRChatPage.LibFiles
{
    public class SqlOperation
    {
        protected SqlConnection connection;
        private SqlCommand sqlCommand;
        private SqlDataReader reader;
        //public string sqlPath = @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=ChatDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";        
        private string sqlPath = "Server=(localdb)\\mssqllocaldb;Database=ChatRepo;Trusted_Connection=True;MultipleActiveResultSets=true";      
        public bool OpenConnection()
        {
            connection = new SqlConnection(sqlPath);
            try
            {
                bool result = true;
                if (connection.State.ToString() != "Open")
                {
                    connection.Open();
                }
                return result;
            }
            catch (SqlException ex)
            {
                string temp = ex.ToString();
                return false;
            }

        }

        public bool CloseConnection()
        {
            try
            {
                if (connection != null && connection.State.ToString() != "Closed")
                {
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                string temp = ex.ToString();
                return false;
            }
        }

        public SqlDataReader ExecuteQuery(string sqlQuery)
        {
            sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = sqlQuery;
            reader = sqlCommand.ExecuteReader();
            return reader;
        }
    }
}
