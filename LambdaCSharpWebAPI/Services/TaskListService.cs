using LambdaCSharpWebAPI.Data;
using LambdaCSharpWebAPI.Logging;
using LambdaCSharpWebAPI.Models;
using System.Collections;

namespace LambdaCSharpWebAPI.Services
{
    public class TaskListService : ITaskListService
    {
        private IDatabase database;
        public TaskListService(IDatabase database)
        {
            Logger.LogDebug("Setting the database.", "TaskListService", "TaskListService");
            this.database = database;
        }
        public ArrayList GetTasks(string taskId)
        {
            Logger.LogDebug("Calling GetTasks.", "GetTasks", "TaskListService");
            return database.GetTasks(taskId);
        }
        public ArrayList GetTasks()
        {
            Logger.LogDebug("Calling GetTasks.", "GetTasks", "TaskListService");
            return database.GetTasks();
        }
        public void AddTask(TaskListModel task)
        {
            Logger.LogDebug("Calling AddTask.", "AddTask", "TaskListService");
            database.AddTask(task);
        }
        public void DeleteTask(string taskId)
        {
            Logger.LogDebug("Calling DeleteTask.", "DeleteTask", "TaskListService");
            database.DeleteTask(taskId);
        }
        public void UpdateTask(TaskListModel task)
        {
            Logger.LogDebug("Calling UpdateTask.", "UpdateTask", "TaskListService");
            database.UpdateTask(task);
        }
    }
}
