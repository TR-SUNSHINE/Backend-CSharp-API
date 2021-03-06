using LambdaCSharpWebAPI.Models;
using System.Collections;

namespace LambdaCSharpWebAPI.Services
{
    public interface IWalkService
    {
        ArrayList GetWalksByUserId(string userId);
        ArrayList GetWalks(string taskId);
        ArrayList GetWalkMonthlyRating(string walkId);
        void AddWalk(WalkModel rating);
        void DeleteWalk(string walkId);
        void UpdateWalk(WalkModel walk);
    }
}
