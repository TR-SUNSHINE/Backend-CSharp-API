namespace LambdaCSharpWebAPI.Models
{
    public class RouteModel
    {
        public string Id { get; set; }
        public int Sequence { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string WalkId { get; set; }
    }
}
