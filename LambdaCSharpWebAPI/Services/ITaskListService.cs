using LambdaCSharpWebAPI.Models;
using System.Collections;

namespace LambdaCSharpWebAPI.Services
{
    public interface ITaskListService
    {
        ArrayList GetSingleItemFromTaskList(string taskId);
        void AddItemsToTaskList(TaskListModel taskList);
        void RemoveItem(string name);
    }
}
