using LambdaCSharpWebAPI.Models;
using System.Collections;

namespace LambdaCSharpWebAPI.Services
{
    public interface IWalkService
    {
        ArrayList GetWalks(string taskId);
        void AddWalk(WalkModel rating);
    }
}
