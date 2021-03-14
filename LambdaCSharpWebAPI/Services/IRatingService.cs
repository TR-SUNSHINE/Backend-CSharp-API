using LambdaCSharpWebAPI.Models;

namespace LambdaCSharpWebAPI.Services
{
    public interface IRatingService
    {
        void AddRating(RatingModel rating);
    }
}
