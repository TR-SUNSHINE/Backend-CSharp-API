namespace LambdaCSharpWebAPI.Models
{
    public class TaskListModel
    {
        public string TaskId { get; set; }

        public string UserId { get; set; }

        public string Description { get; set; }

        public bool Completed { get; set; }
    }
}
