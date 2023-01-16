using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SoccerScoresApi.Extension
{
    public class DbConnector
    {
        protected static string Password = "Blackdog2020$";
        protected static string Username = "Golohas";
        protected static int Port = 3306;
        protected static string Endpoint = "soccerfixtures.chzcbnejugbt.us-east-1.rds.amazonaws.com";
        protected static string Database = "soccer_fixtures";

        public MySqlConnection Connection;

        public async Task<bool> IsConnected()
        {
            string connectionString = $"Server={Endpoint}; database={Database}; UID={Username}; password={Password}";
            Connection = new MySqlConnection(connectionString);
            await Connection.OpenAsync();
            return Connection.State == System.Data.ConnectionState.Open;
        }

        public async Task Disconnect()
        {
            await Connection.CloseAsync();
        }
    }
}
