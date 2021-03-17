using LambdaCSharpWebAPI.Data;
using LambdaCSharpWebAPI.Logging;
using LambdaCSharpWebAPI.Models;

namespace LambdaCSharpWebAPI.Services
{
    public class RatingService : IRatingService
    {
        private IDatabase database;
        public RatingService(IDatabase database)
        {
            Logger.LogDebug("Setting the database.", "RatingService", "RatingService");
            this.database = database;
        }

        public void AddRating(RatingModel rating)
        {
            Logger.LogDebug("Calling AddRating", "AddRating", "RatingService");
            database.AddRating(rating);
        }

    }
}
