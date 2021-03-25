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
            RatingModel,
            WalkModel,
            WalkAvgRatingModel,
            WalkMonthlyRatingModel,
            RouteModel
        }
        public Database()
        {
            Logger.LogDebug("Calling Initialize", "Database", "Database");
            Initialize();
        }
        public void DeleteWalk(string walkId)
        {
            try
            {
                string queryStatementRating =
                    "DELETE FROM " +
                    "   rating " +
                    "WHERE " +
                    "   walkId = @walkId";
                MySqlParameter[] dbParamsRating = {
                    new MySqlParameter("@walkId",walkId)
                };
                string queryStatementRoute =
                    "DELETE FROM " +
                    "   route " +
                    "WHERE " +
                    "   walkId = @walkId";
                MySqlParameter[] dbParamsRoute = {
                    new MySqlParameter("@walkId",walkId)
                };
                string queryStatementWalk =
                    "DELETE FROM " +
                    "   walk " +
                    "WHERE " +
                    "   id = @walkId";
                MySqlParameter[] dbParamsWalk = {
                    new MySqlParameter("@walkId",walkId)
                };
                Logger.LogDebug("Performing DB operations", "DeleteWalk", "Database");
                this.OpenConnection();
                this.BeginTransaction();
                this.DeleteData(queryStatementRating, dbParamsRating);
                this.DeleteData(queryStatementRoute, dbParamsRoute);
                this.DeleteData(queryStatementWalk, dbParamsWalk);
                this.CommitTransaction();
                this.CloseConnection();
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue deleting walk from the DB", "DeleteWalk", "Database", ex.Message);

                this.RollbackTransaction();
                this.CloseConnection();
                throw new Exception(ex.Message);
            }
        }
        public void UpdateWalk(WalkModel walk)
        {
            try
            {
                string queryStatementWalk =
                   "UPDATE " +
                    "   walk " +
                    "SET " +
                    "   walkName = @walkName " +
                    "WHERE " +
                    "   id = @walkId";
                MySqlParameter[] dbParamsWalk = {
                    new MySqlParameter("@walkId",walk.Id),
                     new MySqlParameter("@walkName",walk.WalkName)
                };
                Logger.LogDebug("Performing DB operations", "UpdateWalk", "Database");
                this.OpenConnection();
                this.BeginTransaction();
                this.UpdateData(queryStatementWalk, dbParamsWalk);
                this.CommitTransaction();
                this.CloseConnection();
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue updating walk from the DB", "UpdateWalk", "Database", ex.Message);

                this.RollbackTransaction();
                this.CloseConnection();
                throw new Exception(ex.Message);
            }
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
                    "   userID," +
                    "   walkTime" +
                    ") " +
                    "VALUES " +
                    "(" +
                    "   @id, " +
                    "   @walkName, " +
                    "   @userID," +
                    "   Now()" +
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
                 "   walkID = @walkId " +
                 "ORDER BY" +
                 "  sequence";
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
                this.CloseConnection();
                throw new Exception(ex.Message);
            }
        }

        public ArrayList GetWalkMonthlyRating(string walkId)
        {
            try
            {
                ArrayList dataWalk = null;
                string queryStatementWalk = "SELECT 'Jan' as 'MonthR', " +
                "(SELECT IFNULL(AVG(rating.walkrating),0) FROM rating where rating.walkID = @walkId AND MONTH(ratingtime) = '1') " +
                "as 'MonthAveRating' " +
                "UNION SELECT 'Feb' as 'MonthR', " +
                "(SELECT IFNULL(AVG(rating.walkrating),0) FROM rating where rating.walkID = @walkId AND MONTH(ratingtime) = '2') " +
                "as 'MonthAveRating' " +
                "UNION SELECT 'Mar' as 'MonthR', " +
                "(SELECT IFNULL(AVG(rating.walkrating),0) FROM rating where rating.walkID = @walkId AND MONTH(ratingtime) = '3') " +
                "as 'MonthAveRating' " +
                "UNION SELECT 'Apr' as 'MonthR', " +
                "(SELECT IFNULL(AVG(rating.walkrating),0) FROM rating where rating.walkID = @walkId AND MONTH(ratingtime) = '4') " +
                "as 'MonthAveRating' " +
                "UNION SELECT 'May' as 'MonthR', " +
                "(SELECT IFNULL(AVG(rating.walkrating),0) FROM rating where rating.walkID = @walkId AND MONTH(ratingtime) = '5') " +
                "as 'MonthAveRating' " +
                "UNION SELECT 'Jun' as 'MonthR', " +
                "(SELECT IFNULL(AVG(rating.walkrating),0) FROM rating where rating.walkID = @walkId AND MONTH(ratingtime) = '6') " +
                "as 'MonthAveRating' " +
                "UNION SELECT 'Jul' as 'MonthR', " +
                "(SELECT IFNULL(AVG(rating.walkrating),0) FROM rating where rating.walkID = @walkId AND MONTH(ratingtime) = '7') " +
                "as 'MonthAveRating' " +
                "UNION SELECT 'Aug' as 'MonthR', " +
                "(SELECT IFNULL(AVG(rating.walkrating),0) FROM rating where rating.walkID = @walkId AND MONTH(ratingtime) = '8') " +
                "as 'MonthAveRating' " +
                "UNION SELECT 'Sep' as 'MonthR', " +
                "(SELECT IFNULL(AVG(rating.walkrating),0) FROM rating where rating.walkID = @walkId AND MONTH(ratingtime) = '9') " +
                "as 'MonthAveRating' " +
                "UNION SELECT 'Oct' as 'MonthR', " +
                "(SELECT IFNULL(AVG(rating.walkrating),0) FROM rating where rating.walkID = @walkId AND MONTH(ratingtime) = '10') " +
                "as 'MonthAveRating' " +
                "UNION SELECT 'Nov' as 'MonthR', " +
                "(SELECT IFNULL(AVG(rating.walkrating),0) FROM rating where rating.walkID = @walkId AND MONTH(ratingtime) = '11') " +
                "as 'MonthAveRating' " +
                "UNION SELECT 'Dec' as 'MonthR', " +
                "(SELECT IFNULL(AVG(rating.walkrating),0) FROM rating where rating.walkID = @walkId AND MONTH(ratingtime) = '12') " +
                "as 'MonthAveRating' " +
                 "UNION SELECT 'Dummy' as 'MonthR', " +
                "(SELECT 0 FROM rating where rating.walkID = @walkId AND MONTH(ratingtime) = '12') " +
                "as 'MonthAveRating' " +

                   "FROM " +
                   "   rating " +
                   "WHERE " +
                    "  rating.walkID = @walkId ";

                MySqlParameter[] dbParamsWalk = {
                new MySqlParameter("@walkId",walkId)
            };

                Logger.LogDebug("Performing DB operations", "GetWalkMonthlyRating", "Database");
                this.OpenConnection();
                dataWalk = this.GetData(queryStatementWalk, dbParamsWalk, Models.WalkMonthlyRatingModel);
                this.CloseConnection();

                return dataWalk;
            }
            catch (MySqlException ex)
            {
                Logger.LogError("Issue getting walks from the DB", "GetWalkMonthlyRating", "Database", ex.Message);
                this.CloseConnection();
                throw new Exception(ex.Message);
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
            if (dbParams != null)
            {
                command.Parameters.AddRange(dbParams);

                Logger.LogDebug("Inserting data", "InsertData", "Database");
                Logger.LogDebug("queryStatement = " + queryStatement, "InsertData", "Database");
                Logger.LogDebug("Looping parameters", "GetData", "Database");
                foreach (var param in dbParams)
                {
                    Logger.LogDebug($"dbParam name = {param.ParameterName} | Value = {param.Value}", "GetData", "Database");
                }
            }
            command.ExecuteNonQuery();
        }
        private void UpdateData(string queryStatement, MySqlParameter[] dbParams)
        {
            MySqlCommand command = new MySqlCommand(queryStatement, connection);
            if (dbParams != null)
            {
                command.Parameters.AddRange(dbParams);

                Logger.LogDebug("Updating data", "UpdateData", "Database");
                Logger.LogDebug("queryStatement = " + queryStatement, "UpdateData", "Database");
                Logger.LogDebug("Looping parameters", "GetData", "Database");
                foreach (var param in dbParams)
                {
                    Logger.LogDebug($"dbParam name = {param.ParameterName} | Value = {param.Value}", "GetData", "Database");
                }
            }
            command.ExecuteNonQuery();
        }
        private void DeleteData(string queryStatement, MySqlParameter[] dbParams)
        {
            MySqlCommand command = new MySqlCommand(queryStatement, connection);
            if (dbParams != null)
            {
                command.Parameters.AddRange(dbParams);

                Logger.LogDebug("Deleting data", "DeleteData", "Database");
                Logger.LogDebug("queryStatement = " + queryStatement, "DeleteData", "Database");
                Logger.LogDebug("Looping parameters", "GetData", "Database");
                foreach (var param in dbParams)
                {
                    Logger.LogDebug($"dbParam name = {param.ParameterName} | Value = {param.Value}", "GetData", "Database");
                }
            }
            command.ExecuteNonQuery();
        }
        private ArrayList GetData(string queryStatement, MySqlParameter[] dbParams, Models objectModelType)
        {
            ArrayList data = new ArrayList();
            object obj;

            MySqlCommand command = new MySqlCommand(queryStatement, connection);
            command.CommandText = queryStatement;
            if (dbParams != null)
            {
                command.Parameters.AddRange(dbParams);

                Logger.LogDebug("Getting data", "GetData", "Database");
                Logger.LogDebug("queryStatement = " + queryStatement, "GetData", "Database");
                Logger.LogDebug("Looping parameters", "GetData", "Database");
                foreach (var param in dbParams)
                {
                    Logger.LogDebug($"dbParam name = {param.ParameterName} | Value = {param.Value}", "GetData", "Database");
                }
            }
            dbReader = command.ExecuteReader();

            if (dbReader.HasRows)
            {
                while (dbReader.Read())
                {
                    switch (objectModelType)
                    {
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
                        case Models.WalkMonthlyRatingModel:
                            obj = new WalkMonthlyRatingModel
                            {
                                MonthR = dbReader.GetString("MonthR"),
                                MonthAveRating = dbReader.GetFloat("MonthAveRating")
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
                    }
                }
            }
            dbReader.Close();
            return data;
        }
    }
}
