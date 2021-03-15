using LambdaCSharpWebAPI.Data;
using LambdaCSharpWebAPI.Models;

namespace LambdaCSharpWebAPI.Services
{
    public class RatingService : IRatingService
    {
        private IDatabase database;
        public RatingService(IDatabase database)
        {
            this.database = database;
        }

        public void AddRating(RatingModel rating)
        {
            database.AddRating(rating);
        }

    }
}
