//using Amazon.Lambda.Core;
using LambdaCSharpWebAPI.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace LambdaCSharpWebAPI.Data
{
    public class Database : IDatabase
    {
        private MySqlConnection connection;
        private MySqlDataReader dbReader;
        private MySqlTransaction dbTrans;
        private string dbHost;
        private string dbPort;
        private string dbName;
        private string dbUser;
        private string dbPassword;
        private string connectionString;
        private enum Models
        {
            TaskListModel,
            RatingModel,
            WalkModel,
            RouteModel
        }
        public Database()
        {
            Initialize();
        }
        public void AddWalk(WalkModel walk)
        {
            try
            {
                var guid = Guid.NewGuid();
                string queryStatementWalk =
                    "INSERT INTO " +
                    "   walk" +
                    "(" +
                    "   id," +
                    "   walkName," +
                    "   userID" +
                    ") " +
                    "VALUES " +
                    "(" +
                    "   @id, " +
                    "   @walkName, " +
                    "   @userID" +
                    ")";
                MySqlParameter[] dbParamsWalk = {
                    new MySqlParameter("@id",guid),
                    new MySqlParameter("@walkName",walk.WalkName),
                    new MySqlParameter("@userID",walk.UserID)
                };

                this.OpenConnection();
                this.BeginTransaction();
                this.InsertData(queryStatementWalk, dbParamsWalk);

                foreach (RouteModel route in walk.Routes)
                {
                    string queryStatementRoute =
                        "INSERT INTO " +
                        "   route" +
                        "(" +
                        "   id," +
                        "   sequence," +
                        "   coords," +
                        "   walkID" +
                        ") " +
                        "VALUES " +
                        "(" +
                        "   UUID(), " +
                        "   @sequence, " +
                        "   POINT(@lat, @lng)," +
                        "   @walkID" +
                        ")";
                    MySqlParameter[] dbParamsRoute = {
                        new MySqlParameter("@sequence",route.Sequence),
                        new MySqlParameter("@lat",route.Lat),
                        new MySqlParameter("@lng",route.Lng),
                        new MySqlParameter("@walkID",guid)
                    };
                    this.InsertData(queryStatementRoute, dbParamsRoute);
                }
                this.CommitTransaction();
            }
            catch (MySqlException ex)
            {
                //LambdaLogger.Log(ex.Message);
                this.RollbackTransaction();
                throw new Exception("ERROR: AddWalk: ", ex);
            }
            finally
            {
                this.CloseConnection();
            }
        }
        public void AddRating(RatingModel rating)
        {
            string queryStatement =
                "INSERT INTO " +
                "   rating" +
                "(" +
                "   id," +
                "   userID," +
                "   walkID," +
                "   walkRating," +
                "   ratingTime" +
                ") " +
                "VALUES " +
                "(" +
                "   UUID(), " +
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
            this.OpenConnection();
            this.InsertData(queryStatement, dbParams);
            this.CloseConnection();
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
            this.OpenConnection();
            this.InsertData(queryStatement, dbParams);
            this.CloseConnection();
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
            this.OpenConnection();
            this.UpdateData(queryStatement, dbParams);
            this.CloseConnection();
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
            this.OpenConnection();
            this.DeleteData(queryStatement, dbParams);
            this.CloseConnection();
        }
        public ArrayList GetTasks()
        {
            ArrayList data = null;

            string queryStatement = "" +
                "SELECT " +
                "   * " +
                "FROM " +
                "   task";

            this.OpenConnection();
            data = this.GetData(queryStatement, null, Models.TaskListModel);
            this.CloseConnection();

            return data;
        }
        public ArrayList GetTasks(string taskId)
        {
            ArrayList data = null;

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

            this.OpenConnection();
            data = this.GetData(queryStatement, dbParams, Models.TaskListModel);
            this.CloseConnection();

            return data;
        }
        public ArrayList GetWalks(string walkId)
        {
            ArrayList dataWalk = null;
            ArrayList dataRoute = null;

            string queryStatementWalk =
               "SELECT " +
               "   id," +
               "   walkName," +
               "   userID " +
               "FROM " +
               "   walk " +
               "WHERE " +
                "  id = @walkId";
            MySqlParameter[] dbParamsWalk = {
                new MySqlParameter("@walkId",walkId)
            };

            string queryStatementRoute =
            "SELECT " +
            "    id," +
            "    sequence," +
            "    X(coords) AS lat," +
            "    Y(coords) AS lng," +
            "    walkID " +
            "FROM " +
            "    route  " +
            "WHERE " +
             "   walkID = @walkId";
            MySqlParameter[] dbParamsRoute = {
                new MySqlParameter("@walkId",walkId)
            };

            this.OpenConnection();
            dataWalk = this.GetData(queryStatementWalk, dbParamsWalk, Models.WalkModel);
            dataRoute = this.GetData(queryStatementRoute, dbParamsRoute, Models.RouteModel);
            this.CloseConnection();

            if (dataWalk.Count > 0)
            {
                WalkModel walk = (WalkModel)dataWalk[0];
                walk.Routes = new List<RouteModel>();

                foreach (RouteModel route in dataRoute)
                {
                    walk.Routes.Add(route);
                }
            }
            return dataWalk;
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
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
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
        private void BeginTransaction()
        {
            try
            {
                dbTrans = connection.BeginTransaction();
            }
            catch (MySqlException ex)
            {
                //LambdaLogger.Log(ex.Message);
            }
        }
        private void CommitTransaction()
        {
            try
            {
                dbTrans.Commit();
            }
            catch (MySqlException ex)
            {
                //LambdaLogger.Log(ex.Message);
            }
        }
        private void RollbackTransaction()
        {
            try
            {
                dbTrans.Rollback();
            }
            catch (MySqlException ex)
            {
                //LambdaLogger.Log(ex.Message);
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
            MySqlCommand command = new MySqlCommand(queryStatement, connection);
            if (dbParams != null) command.Parameters.AddRange(dbParams);

            command.ExecuteNonQuery();
        }
        private void UpdateData(string queryStatement, MySqlParameter[] dbParams)
        {
            MySqlCommand command = new MySqlCommand(queryStatement, connection);
            if (dbParams != null) command.Parameters.AddRange(dbParams);

            command.ExecuteNonQuery();
        }
        private void DeleteData(string queryStatement, MySqlParameter[] dbParams)
        {
            MySqlCommand command = new MySqlCommand(queryStatement, connection);
            if (dbParams != null) command.Parameters.AddRange(dbParams);

            command.ExecuteNonQuery();
        }
        private ArrayList GetData(string queryStatement, MySqlParameter[] dbParams, Models objectModelType)
        {
            ArrayList data = new ArrayList();
            object obj;

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

                        case Models.WalkModel:
                            obj = new WalkModel
                            {
                                Id = dbReader.GetString("id"),
                                WalkName = dbReader.GetString("walkName"),
                                UserID = dbReader.GetString("userID")
                            };
                            data.Add(obj);
                            break;
                        case Models.RouteModel:
                            obj = new RouteModel
                            {
                                Id = dbReader.GetString("id"),
                                Sequence = dbReader.GetInt32("sequence"),
                                Lat = dbReader.GetDouble("lat"),
                                Lng = dbReader.GetDouble("lng"),
                                WalkId = dbReader.GetString("walkID")
                            };
                            data.Add(obj);
                            break;

                            /*
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
                                */
                    }
                }
            }
            dbReader.Close();
            return data;
        }
    }
}
