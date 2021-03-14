using LambdaCSharpWebAPI.Models;
using LambdaCSharpWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LambdaCSharpWebAPI.Controllers
{
    [Route("v1/walks")]
    public class WalkController : Controller
    {
        private readonly IWalkService walkService;

        public WalkController(IWalkService walkService)
        {
            this.walkService = walkService;
        }
        [HttpPost]
        public IActionResult AddWalk([FromBody]WalkModel walk)
        {
            try
            {
                walkService.AddWalk(walk);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}