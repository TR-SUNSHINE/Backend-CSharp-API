using LambdaCSharpWebAPI.Data;
using LambdaCSharpWebAPI.Models;
using System.Collections;

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
        public ArrayList GetWalks(string walkId)
        {
            return database.GetWalks(walkId);
        }

    }
}
