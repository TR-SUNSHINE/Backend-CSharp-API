using LambdaCSharpWebAPI.Data;
using LambdaCSharpWebAPI.Models;
using System.Collections;

namespace LambdaCSharpWebAPI.Services
{
    public class TaskListService : ITaskListService
    {
        private readonly Database database = null;

        public TaskListService()
        {
            database = new Database();
        }
        public ArrayList GetTasks(string taskId)
        {
            return database.GetTasks(taskId);
        }
        public ArrayList GetTasks()
        {
            return database.GetTasks();
        }

        public void AddTask(TaskListModel taskList)
        {
            database.AddTask(taskList);
        }

        public void DeleteTask(string taskId)
        {
            database.DeleteTask(taskId);
        }

        public void UpdateTask(TaskListModel taskList)
        {
            database.UpdateTask(taskList);
        }

    }

}
