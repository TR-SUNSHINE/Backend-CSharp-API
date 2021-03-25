using System.Collections.Generic;
using System;

namespace LambdaCSharpWebAPI.Models
{
    public class WalkAvgRatingModel
    {
        public string Id { get; set; }
        public string WalkName { get; set; }
        public string Walktime { get; set; }
        public string YearOf { get; set; }
        public string MonthOf { get; set; }
        public string DayOf { get; set; }
        public string UserID { get; set; }
        public float AveRating { get; set; }
    }
}
