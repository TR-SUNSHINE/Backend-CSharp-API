using System.Collections.Generic;

namespace LambdaCSharpWebAPI.Models
{
    public class WalkMonthlyRatingModel
    {
        public string Id { get; set; }
        public string UserID { get; set; }
        public string WalkID { get; set; }
        public int MonthR { get; set; }
        public float MonthAveRating { get; set; }
    }
}
