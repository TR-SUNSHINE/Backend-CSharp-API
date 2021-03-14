//using Amazon.Lambda.Core;
using LambdaCSharpWebAPI.Models;
using MySql.Data.MySqlClient;
using System.Collections;

namespace LambdaCSharpWebAPI.Data
{
    public class Database
    {
        private MySqlConnection connection;
        private MySqlDataReader dbReader;
        private string dbHost;
        private string dbPort;
        private string dbName;
        private string dbUser;
        private string dbPassword;
        private string connectionString;

        enum Models
        {
            TaskListModel,
            RatingModdel
        }

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

        private void InsertData(string queryStatement, MySqlParameter[] dbParams)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(queryStatement, connection);
                if (dbParams != null) command.Parameters.AddRange(dbParams);

                command.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
        private void UpdateData(string queryStatement, MySqlParameter[] dbParams)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(queryStatement, connection);
                if (dbParams != null) command.Parameters.AddRange(dbParams);

                command.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
        private void DeleteData(string queryStatement, MySqlParameter[] dbParams)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(queryStatement, connection);
                if (dbParams != null) command.Parameters.AddRange(dbParams);

                command.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
        public void AddTask(TaskListModel task)
        {
            string queryStatement = "INSERT INTO task VALUES (@taskId, @userId, @description, @completed)";
            MySqlParameter[] dbParams = {
                new MySqlParameter("@taskId",task.TaskId),
                new MySqlParameter("@userId",task.UserId),
                new MySqlParameter("@description",task.Description),
                new MySqlParameter("@completed",task.Completed)
             };
            InsertData(queryStatement, dbParams);
        }
        public void DeleteTask(string taskId)
        {
            string queryStatement = "DELETE FROM task WHERE taskId = @taskId";
            MySqlParameter[] dbParams = {
                new MySqlParameter("@taskId",taskId)
             };
            DeleteData(queryStatement, dbParams);
        }
        public void UpdateTask(TaskListModel task)
        {
            string queryStatement = "Update task SET userId = @userId, description = @description, completed = @completed WHERE taskId = @taskId";
            MySqlParameter[] dbParams = {
                new MySqlParameter("@taskId",task.TaskId),
                new MySqlParameter("@userId",task.UserId),
                new MySqlParameter("@description",task.Description),
                new MySqlParameter("@completed",task.Completed)
             };
            UpdateData(queryStatement, dbParams);
        }

        public ArrayList GetTasks()
        {
            string queryStatement = "SELECT * FROM task";
            return GetData(queryStatement, null, Models.TaskListModel);
        }
        public ArrayList GetTasks(string taskId)
        {
            string queryStatement = "SELECT * FROM task WHERE taskId = @taskId";
            MySqlParameter[] dbParams = {
                new MySqlParameter("@taskId",taskId)
            };
            return GetData(queryStatement, dbParams, Models.TaskListModel);
        }
        private ArrayList GetData(string queryStatement, MySqlParameter[] dbParams, Models objectModelType)
        {
            ArrayList data = new ArrayList();
            object obj;

            if (this.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(queryStatement, connection);
                command.CommandText = queryStatement;
                if (dbParams != null) command.Parameters.AddRange(dbParams);

                dbReader = command.ExecuteReader();

                if (dbReader.HasRows)
                {
                    while (dbReader.Read())
                    {
                        switch (objectModelType)
                        {
                            case Models.TaskListModel:
                                obj = new TaskListModel
                                {
                                    TaskId = dbReader.GetString("taskId"),
                                    UserId = dbReader.GetString("userId"),
                                    Description = dbReader.GetString("description"),
                                    Completed = dbReader.GetBoolean("completed")
                                };
                                data.Add(obj);
                                break;

                            case Models.RatingModdel:
                                obj = new TaskListModel
                                {
                                    TaskId = dbReader.GetString("taskId"),
                                    UserId = dbReader.GetString("userId"),
                                    Description = dbReader.GetString("description"),
                                    Completed = dbReader.GetBoolean("completed")
                                };
                                data.Add(obj);
                                break;
                        }
                    }
                }
            }
            dbReader.Close();
            this.CloseConnection();
            return data;
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
