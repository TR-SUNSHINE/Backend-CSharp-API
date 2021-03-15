using LambdaCSharpWebAPI.Data;
using LambdaCSharpWebAPI.Models;
using System.Collections;

namespace LambdaCSharpWebAPI.Services
{
    public class WalkService : IWalkService
    {
        private IDatabase database;

        public WalkService(IDatabase database)
        {
            this.database = database;
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
