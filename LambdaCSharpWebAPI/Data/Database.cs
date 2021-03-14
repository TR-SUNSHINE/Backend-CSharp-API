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
        private enum Models
        {
            TaskListModel,
            RatingModel
        }
        public Database()
        {
            Initialize();
        }
        public void AddRating(RatingModel rating)
        {
            string queryStatement =
                "INSERT INTO " +
                "   rating" +
                "(" +
                "   userID," +
                "   walkID," +
                "   walkRating," +
                "   walkTime" +
                ") " +
                "VALUES " +
                "(" +
                "   @userID, " +
                "   @walkID, " +
                "   @walkRating, " +
                "   SYSDATE()" +
                ")";
            MySqlParameter[] dbParams = {
                new MySqlParameter("@userID",rating.UserId),
                new MySqlParameter("@walkID",rating.WalkId),
                new MySqlParameter("@walkRating",rating.WalkRating)
             };
            this.InsertData(queryStatement, dbParams);
        }
        public void AddTask(TaskListModel task)
        {
            string queryStatement =
                "INSERT INTO " +
                "   task " +
                "(" +
                "   taskId," +
                "   userId," +
                "   description," +
                "   completed" +
                ")" +
                "VALUES " +
                "(" +
                "   UUID(), " +
                "   @userId, " +
                "   @description, " +
                "   @completed" +
                ")";
            MySqlParameter[] dbParams = {
                new MySqlParameter("@userId",task.UserId),
                new MySqlParameter("@description",task.Description),
                new MySqlParameter("@completed",task.Completed)
             };
            this.InsertData(queryStatement, dbParams);
        }
        public void UpdateTask(TaskListModel task)
        {
            string queryStatement = "" +
                "UPDATE " +
                "   task " +
                "SET " +
                "   userId = @userId, " +
                "   description = @description, " +
                "   completed = @completed " +
                "WHERE " +
                "   taskId = @taskId";
            MySqlParameter[] dbParams = {
                new MySqlParameter("@taskId",task.TaskId),
                new MySqlParameter("@userId",task.UserId),
                new MySqlParameter("@description",task.Description),
                new MySqlParameter("@completed",task.Completed)
             };
            this.UpdateData(queryStatement, dbParams);
        }
        public void DeleteTask(string taskId)
        {
            string queryStatement = "" +
                "DELETE FROM " +
                "   task " +
                "WHERE " +
                "   taskId = @taskId";
            MySqlParameter[] dbParams = {
                new MySqlParameter("@taskId",taskId)
             };
            this.DeleteData(queryStatement, dbParams);
        }
        public ArrayList GetTasks()
        {
            string queryStatement = "" +
                "SELECT " +
                "   * " +
                "FROM " +
                "   task";
            return this.GetData(queryStatement, null, Models.TaskListModel);
        }
        public ArrayList GetTasks(string taskId)
        {
            string queryStatement = "" +
                "SELECT " +
                "   * " +
                "FROM " +
                "   task " +
                "WHERE " +
                "   taskId = @taskId";
            MySqlParameter[] dbParams = {
                new MySqlParameter("@taskId",taskId)
            };
            return this.GetData(queryStatement, dbParams, Models.TaskListModel);
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
        private void OpenConnection()
        {
            try
            {
                connection.Open();
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
            }
        }
        private void CloseConnection()
        {
            try
            {
                connection.Close();
            }
            catch (MySqlException ex)
            {
                //LambdaLogger.Log(ex.Message);
            }
        }
        private void InsertData(string queryStatement, MySqlParameter[] dbParams)
        {
            try
            {
                this.OpenConnection();
                MySqlCommand command = new MySqlCommand(queryStatement, connection);
                if (dbParams != null) command.Parameters.AddRange(dbParams);

                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                //LambdaLogger.Log(ex.Message);

            }
            finally
            {
                this.CloseConnection();
            }
        }
        private void UpdateData(string queryStatement, MySqlParameter[] dbParams)
        {
            try
            {
                this.OpenConnection();
                MySqlCommand command = new MySqlCommand(queryStatement, connection);
                if (dbParams != null) command.Parameters.AddRange(dbParams);

                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                //LambdaLogger.Log(ex.Message);
            }
            finally
            {
                this.CloseConnection();
            }
        }
        private void DeleteData(string queryStatement, MySqlParameter[] dbParams)
        {
            try
            {
                this.OpenConnection();
                MySqlCommand command = new MySqlCommand(queryStatement, connection);
                if (dbParams != null) command.Parameters.AddRange(dbParams);

                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                //LambdaLogger.Log(ex.Message);
                this.CloseConnection();
            }
            finally
            {
            }
        }
        private ArrayList GetData(string queryStatement, MySqlParameter[] dbParams, Models objectModelType)
        {
            ArrayList data = new ArrayList();
            object obj;

            try
            {
                this.OpenConnection();
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


                            case Models.RatingModel:
                                obj = new RatingModel
                                {
                                    UserId = dbReader.GetString("userID"),
                                    WalkId = dbReader.GetString("walkID"),
                                    WalkRating = dbReader.GetInt32("walkRating"),
                                    WalkTime = dbReader.GetDateTime("WalkTime")
                                };
                                data.Add(obj);
                                break;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                //LambdaLogger.Log(ex.Message);
                this.CloseConnection();
            }
            finally
            {
                dbReader.Close();
                this.CloseConnection();
            }
            return data;
        }
    }
}
