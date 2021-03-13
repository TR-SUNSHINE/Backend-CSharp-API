//using Amazon.Lambda.Core;
using MySql.Data.MySqlClient;

namespace LambdaCSharpWebAPI.Data
{
    public class Database
    {
        private MySqlConnection connection;
        private string dbHost;
        private string dbPort;
        private string dbName;
        private string dbUser;
        private string dbPassword;
        private string connectionString;

        public Database()
        {
            Initialize();
        }
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                //LambdaLogger.Log(ex.Message);
                return false;
            }
        }
        public void Insert(string query)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
        public void Update(string query)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;

                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
        public void Delete(string query)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
        public MySqlDataReader Select(string query, MySqlParameter[] dbParamArray)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand myCommand = new MySqlCommand(query, connection);
                myCommand.CommandText = query;
                myCommand.Parameters.AddRange(dbParamArray);
                return myCommand.ExecuteReader();
            }
            else
            {
                return null;
            }

        }
        private void Initialize()
        {
            dbHost = System.Environment.GetEnvironmentVariable("DB_HOST");
            dbPort = System.Environment.GetEnvironmentVariable("DB_PORT");
            dbName = System.Environment.GetEnvironmentVariable("DB_NAME");
            dbUser = System.Environment.GetEnvironmentVariable("DB_USER");
            dbPassword = System.Environment.GetEnvironmentVariable("DB_PASSWORD");

            connectionString =
                "SERVER=" + dbHost + ";" +
                "DATABASE=" + dbName + ";" +
                "PORT=" + dbPort + ";" +
                "USER=" + dbUser + ";" +
                "PASSWORD=" + dbPassword + ";";

            try
            {
                connection = new MySqlConnection(connectionString);
            }
            catch (MySqlException ex)
            {
                //LambdaLogger.Log("ERROR - Creating mySQL connection: " + ex.Message);
            }
        }
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        //LambdaLogger.Log("ERROR - Issue connecting to the server: " + ex.Message);
                        break;

                    case 1045:
                        //LambdaLogger.Log("ERROR - Invalid username or password: " + ex.Message);
                        break;
                    default:
                        //LambdaLogger.Log("ERROR - " + ex.Message);
                        break;
                }
                return false;
            }
        }
    }
}
