using LambdaCSharpWebAPI.Data;
using LambdaCSharpWebAPI.Models;

namespace LambdaCSharpWebAPI.Services
{
    public class WalkService : IWalkService
    {
        private readonly Database database = null;
        public WalkService()
        {
            database = new Database();
        }

        public void AddWalk(WalkModel walk)
        {
            database.AddWalk(walk);
        }

    }
}
