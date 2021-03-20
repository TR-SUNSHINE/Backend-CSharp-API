using LambdaCSharpWebAPI.Data;
using LambdaCSharpWebAPI.Logging;
using LambdaCSharpWebAPI.Models;
using System.Collections;

namespace LambdaCSharpWebAPI.Services
{
    public class WalkService : IWalkService
    {
        private IDatabase database;

        public WalkService(IDatabase database)
        {
            Logger.LogDebug("Setting the database.", "WalkService", "WalkService");
            this.database = database;
        }

        public void AddWalk(WalkModel walk)
        {
            Logger.LogDebug("Calling AddWalk", "AddWalk", "WalkService");
            database.AddWalk(walk);
        }
        public ArrayList GetWalks(string walkId)
        {
            Logger.LogDebug("Calling GetWalks", "GetWalks", "WalkService");
            return database.GetWalks(walkId);
        }

        public ArrayList GetWalksByUserId(string userId)
        {
            Logger.LogDebug("Calling GetWalksByUserId", "GetWalksByUserId", "WalkService");
            return database.GetWalksByUserId(userId);
        }

        public ArrayList GetWalkMonthlyRating(string walkId)
        {
            Logger.LogDebug("Calling GetWalkMonthlyRating", "GetWalkMonthlyRating", "WalkService");
            return database.GetWalkMonthlyRating(walkId);
        }

    }
}
