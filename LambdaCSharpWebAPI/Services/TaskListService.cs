using LambdaCSharpWebAPI.Data;
using LambdaCSharpWebAPI.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections;

namespace LambdaCSharpWebAPI.Services
{
    public class TaskListService : ITaskListService
    {
        private readonly Database database = null;
        private MySqlDataReader reader = null;
        private ArrayList taskListStorage = null;

        public TaskListService()
        {
            database = new Database();
        }
        public ArrayList GetSingleItemFromTaskList(string taskId)
        {
            try
            {
                taskListStorage = new ArrayList();
                string selectQuery = "SELECT * FROM task WHERE taskId = @userId AND 1=@test";

                MySqlParameter[] dbParams = {
                    new MySqlParameter("@userId",taskId),
                    new MySqlParameter("@test",1)
                 };

                reader = database.Select(selectQuery, dbParams);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TaskListModel task = new TaskListModel
                        {
                            TaskId = reader.GetString("taskId"),
                            UserId = reader.GetString("userId"),
                            Description = reader.GetString("description"),
                            Completed = reader.GetBoolean("completed")
                        };

                        taskListStorage.Add(task);
                    }
                }
                else
                {
                    //LambdaLogger.Log("INFO - No results returned");
                }
                return taskListStorage;
            }
            catch (Exception ex)
            {
                // LambdaLogger.Log("ERROR - Querying database: " + ex.Message);
                return taskListStorage;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (database != null) database.CloseConnection();

            }
        }

        public void AddItemsToTaskList(TaskListModel taskList)
        {

        }

        public void RemoveItem(string taskListName)
        {

        }

    }

}
