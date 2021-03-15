using System;

namespace LambdaCSharpWebAPI.Models
{
    public class RatingModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string WalkId { get; set; }
        public int WalkRating { get; set; }
        public DateTime RatingTime { get; set; }
    }
}
