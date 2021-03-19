using System.Collections.Generic;

namespace LambdaCSharpWebAPI.Models
{
    public class WalkModel
    {
        public string Id { get; set; }
        public string WalkName { get; set; }
        public string UserID { get; set; }
        public float aveRating { get; set; }
        public List<RouteModel> Routes { get; set; }
    }
}
