using LambdaCSharpWebAPI.Models;
using System.Collections;

namespace LambdaCSharpWebAPI.Data
{
    public interface IDatabase
    {
        //GetWalksByUserId
        ArrayList GetWalksByUserId(string userId);
        ArrayList GetWalks(string walkId);
        void AddWalk(WalkModel walk);
        ArrayList GetTasks();
        ArrayList GetTasks(string taskId);
        void AddTask(TaskListModel task);
        void DeleteTask(string taskId);
        void UpdateTask(TaskListModel task);
        void AddRating(RatingModel rating);
    }
}
