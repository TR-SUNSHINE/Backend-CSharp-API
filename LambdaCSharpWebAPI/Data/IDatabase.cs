using LambdaCSharpWebAPI.Models;
using System.Collections;

namespace LambdaCSharpWebAPI.Data
{
    public interface IDatabase
    {
        ArrayList GetWalkMonthlyRating(string walkId);
        ArrayList GetWalksByUserId(string userId);
        ArrayList GetWalks(string walkId);
        void AddWalk(WalkModel walk);
        void AddRating(RatingModel rating);
    }
}
