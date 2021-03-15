using LambdaCSharpWebAPI.Data;
using LambdaCSharpWebAPI.Models;
using System.Collections;

namespace LambdaCSharpWebAPI.Services
{
    public class TaskListService : ITaskListService
    {
        private IDatabase database;
        public TaskListService(IDatabase database)
        {
            this.database = database;
        }
        public ArrayList GetTasks(string taskId)
        {
            return database.GetTasks(taskId);
        }
        public ArrayList GetTasks()
        {
            return database.GetTasks();
        }
        public void AddTask(TaskListModel task)
        {
            database.AddTask(task);
        }
        public void DeleteTask(string taskId)
        {
            database.DeleteTask(taskId);
        }
        public void UpdateTask(TaskListModel task)
        {
            database.UpdateTask(task);
        }
    }
}
