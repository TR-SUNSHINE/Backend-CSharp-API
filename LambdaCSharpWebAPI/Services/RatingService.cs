using LambdaCSharpWebAPI.Data;
using LambdaCSharpWebAPI.Models;

namespace LambdaCSharpWebAPI.Services
{
    public class RatingService : IRatingService
    {
        private readonly Database database = null;
        public RatingService()
        {
            database = new Database();
        }

        public void AddRating(RatingModel rating)
        {
            database.AddRating(rating);
        }

    }
}
