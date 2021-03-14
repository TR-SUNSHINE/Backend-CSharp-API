using LambdaCSharpWebAPI.Models;
using System.Collections;

namespace LambdaCSharpWebAPI.Services
{
    public interface ITaskListService
    {
        ArrayList GetTasks(string taskId);
        ArrayList GetTasks();
        void AddTask(TaskListModel taskList);
        void DeleteTask(string taskId);
        void UpdateTask(TaskListModel taskList);
    }
}
