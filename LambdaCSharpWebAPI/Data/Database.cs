//using Amazon.Lambda.Core;
using LambdaCSharpWebAPI.Logging;
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
            WalkAvgRatingModel,
            RouteModel
        }
        public Database()
        {
            Logger.LogDebug("Calling Initialize", "Database", "Database");
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

                Logger.LogDebug("Performing DB operations", "AddWalk", "Database");
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
                this.CloseConnection();
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue adding walk to the DB", "AddWalk", "Database", ex.Message);

                this.RollbackTransaction();
                this.CloseConnection();
                throw new Exception(ex.Message);
            }
        }
        public void AddRating(RatingModel rating)
        {
            try
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
                Logger.LogDebug("Performing DB operations", "AddRating", "Database");
                this.OpenConnection();
                this.BeginTransaction();
                this.InsertData(queryStatement, dbParams);
                this.CommitTransaction();
                this.CloseConnection();
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue adding rating to the DB", "AddRating", "Database", ex.Message);

                this.RollbackTransaction();
                this.CloseConnection();
                throw new Exception(ex.Message);
            }
        }
        public void AddTask(TaskListModel task)
        {
            try
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
                Logger.LogDebug("Performing DB operations", "AddTask", "Database");
                this.OpenConnection();
                this.BeginTransaction();
                this.InsertData(queryStatement, dbParams);
                this.CommitTransaction();
                this.CloseConnection();
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue adding task to the DB", "AddTask", "Database", ex.Message);

                this.RollbackTransaction();
                this.CloseConnection();
                throw new Exception(ex.Message);
            }

        }
        public void UpdateTask(TaskListModel task)
        {
            try
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
                Logger.LogDebug("Performing DB operations", "UpdateTask", "Database");
                this.OpenConnection();
                this.BeginTransaction();
                this.UpdateData(queryStatement, dbParams);
                this.CommitTransaction();
                this.CloseConnection();
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue updating task to the DB", "UpdateTask", "Database", ex.Message);

                this.RollbackTransaction();
                this.CloseConnection();
                throw new Exception(ex.Message);
            }
        }
        public void DeleteTask(string taskId)
        {
            try
            {
                string queryStatement = "" +
                "DELETE FROM " +
                "   task " +
                "WHERE " +
                "   taskId = @taskId";
                MySqlParameter[] dbParams = {
                new MySqlParameter("@taskId",taskId)
             };
                Logger.LogDebug("Performing DB operations", "DeleteTask", "Database");
                this.OpenConnection();
                this.BeginTransaction();
                this.DeleteData(queryStatement, dbParams);
                this.CommitTransaction();
                this.CloseConnection();
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue deleting task to the DB", "DeleteTask", "Database", ex.Message);

                this.RollbackTransaction();
                this.CloseConnection();
                throw new Exception(ex.Message);
            }
        }
        public ArrayList GetTasks()
        {
            try
            {
                ArrayList data = null;

                string queryStatement = "" +
                    "SELECT " +
                    "   * " +
                    "FROM " +
                    "   task";

                Logger.LogDebug("Performing DB operations", "GetTasks", "Database");
                this.OpenConnection();
                data = this.GetData(queryStatement, null, Models.TaskListModel);
                this.CloseConnection();

                return data;
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue getting tasks from the DB", "GetTasks", "Database", ex.Message);
                this.CloseConnection();
                throw new Exception(ex.Message);
            }

        }
        public ArrayList GetTasks(string taskId)
        {
            try
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

                Logger.LogDebug("Performing DB operations", "GetTasks", "Database");
                this.OpenConnection();
                data = this.GetData(queryStatement, dbParams, Models.TaskListModel);
                this.CloseConnection();

                return data;
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue getting tasks from the DB", "GetTasks", "Database", ex.Message);
                this.CloseConnection();
                throw new Exception(ex.Message);
            }
        }
        public ArrayList GetWalks(string walkId)
        {
            try
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

                Logger.LogDebug("Performing DB operations", "GetWalks", "Database");
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
            catch (MySqlException ex)
            {
                Logger.LogError("Issue getting walks from the DB", "GetWalks", "Database", ex.Message);
                this.CloseConnection();
                throw new Exception(ex.Message);
            }
        }
        // Get Walks for a User using userID
        public ArrayList GetWalksByUserId(string userId)
        {
            try
            {
                ArrayList dataWalk = null;
                string queryStatementWalk = "SELECT " +
                   "   id," +
                   "   walkName," +
                   "   userID, " +
                   "IFNULL((SELECT " +
                   "   AVG(rating.walkrating) " +
                   "FROM " +
                   "   rating " +
                   "WHERE " +
                    "  walk.id = rating.walkID " +
                    "GROUP BY walkID),0) " +
                   "as AveRating " +
                   "FROM " +
                   "   walk " +
                   "WHERE " +
                    "  walk.userID = @userId";

                MySqlParameter[] dbParamsWalk = {
                new MySqlParameter("@userId",userId)
            };



                Logger.LogDebug("Performing DB operations", "GetWalksByUserId", "Database");
                this.OpenConnection();
                dataWalk = this.GetData(queryStatementWalk, dbParamsWalk, Models.WalkAvgRatingModel);
                this.CloseConnection();

                return dataWalk;
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue getting walks from the DB", "GetWalksByUserId", "Database", ex.Message);
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
                Logger.LogDebug("Creating MySQL connection", "Initialize", "Database");
                connection = new MySqlConnection(connectionString);
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue initializing connection to the DB", "Initialize", "Database", ex.Message);
                throw new Exception(ex.Message);
            }
        }
        private void OpenConnection()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    Logger.LogDebug("Opening connection", "OpenConnection", "Database");
                    connection.Open();
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Logger.LogError("Issue connecting to the server", "OpenConnection", "Database", ex.Message);
                        break;
                    case 1045:
                        Logger.LogError("Invalid username or password", "OpenConnection", "Database", ex.Message);
                        break;
                    default:
                        Logger.LogError("Issue opening connection", "OpenConnection", "Database", ex.Message);
                        break;
                }
                throw new Exception(ex.Message);
            }
        }
        private void BeginTransaction()
        {
            try
            {
                Logger.LogDebug("Beggining transaction", "BeginTransaction", "Database");
                dbTrans = connection.BeginTransaction();
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue beggining transaction", "BeginTransaction", "Database", ex.Message);
                throw new Exception(ex.Message);
            }
        }
        private void CommitTransaction()
        {
            try
            {
                Logger.LogDebug("Commiting transaction", "CommitTransaction", "Database");
                dbTrans.Commit();
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue commiting transaction", "CommitTransaction", "Database", ex.Message);
                throw new Exception(ex.Message);
            }
        }
        private void RollbackTransaction()
        {
            try
            {
                Logger.LogDebug("Rolling back transaction", "RollbackTransaction", "Database");
                dbTrans.Rollback();
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue rolling back transaction", "RollbackTransaction", "Database", ex.Message);
                throw new Exception(ex.Message);
            }
        }
        private void CloseConnection()
        {
            try
            {
                Logger.LogDebug("Closing connection", "CloseConnection", "Database");
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue closing connection", "CloseConnection", "Database", ex.Message);
                throw new Exception(ex.Message);
            }
        }
        private void InsertData(string queryStatement, MySqlParameter[] dbParams)
        {
            MySqlCommand command = new MySqlCommand(queryStatement, connection);
            if (dbParams != null) command.Parameters.AddRange(dbParams);

            Logger.LogDebug("Inserting data", "InsertData", "Database");
            Logger.LogDebug("queryStatement = " + queryStatement, "InsertData", "Database");
            command.ExecuteNonQuery();
        }
        private void UpdateData(string queryStatement, MySqlParameter[] dbParams)
        {
            MySqlCommand command = new MySqlCommand(queryStatement, connection);
            if (dbParams != null) command.Parameters.AddRange(dbParams);

            Logger.LogDebug("Updating data", "UpdateData", "Database");
            Logger.LogDebug("queryStatement = " + queryStatement, "UpdateData", "Database");
            command.ExecuteNonQuery();
        }
        private void DeleteData(string queryStatement, MySqlParameter[] dbParams)
        {
            MySqlCommand command = new MySqlCommand(queryStatement, connection);
            if (dbParams != null) command.Parameters.AddRange(dbParams);

            Logger.LogDebug("Deleting data", "DeleteData", "Database");
            Logger.LogDebug("queryStatement = " + queryStatement, "DeleteData", "Database");
            command.ExecuteNonQuery();
        }
        private ArrayList GetData(string queryStatement, MySqlParameter[] dbParams, Models objectModelType)
        {
            ArrayList data = new ArrayList();
            object obj;

            MySqlCommand command = new MySqlCommand(queryStatement, connection);
            command.CommandText = queryStatement;
            if (dbParams != null) command.Parameters.AddRange(dbParams);

            Logger.LogDebug("Getting data", "GetData", "Database");
            Logger.LogDebug("queryStatement = " + queryStatement, "GetData", "Database");
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
                        case Models.WalkAvgRatingModel:
                            obj = new WalkAvgRatingModel
                            {
                                Id = dbReader.GetString("id"),
                                WalkName = dbReader.GetString("walkName"),
                                UserID = dbReader.GetString("userID"),
                                AveRating = dbReader.GetFloat("AveRating")
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
