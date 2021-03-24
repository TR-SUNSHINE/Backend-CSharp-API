using LambdaCSharpWebAPI.Logging;
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
            Logger.LogDebug("Setting walkService", "WalkController", "WalkController");
            this.walkService = walkService;
        }
        [HttpPost]
        public IActionResult AddWalk([FromBody] WalkModel walk)
        {
            try
            {
                Logger.LogDebug("Calling AddWalk", "AddWalk", "WalkController");
                walkService.AddWalk(walk);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpDelete]
        public IActionResult DeleteWalk([FromBody]WalkModel walk)
        {
            try
            {
                Logger.LogDebug("Calling DeleteWalk", "DeleteWalk", "WalkController");
                walkService.DeleteWalk(walk.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpPut]
        public IActionResult UpdateWalk([FromBody]WalkModel walk)
        {
            try
            {
                Logger.LogDebug("Calling UpdateWalk", "UpdateWalk", "WalkController");
                walkService.UpdateWalk(walk);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpGet("{walkId}")]
        public IActionResult GetWalk(string walkId)
        {
            try
            {
                Logger.LogDebug("Calling GetWalks", "GetWalk", "WalkController");
                var result = walkService.GetWalks(walkId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetWalksByUserId(string userId)
        {
            try
            {
                Logger.LogDebug("Calling GetWalksByUserId", "GetWalksByUserId", "WalkController");
                var result = walkService.GetWalksByUserId(userId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("rating/{walkId}")]
        public IActionResult GetWalkMonthlyRating(string walkId)
        {
            try
            {
                Logger.LogDebug("Calling GetWalkMonthlyRating", "GetWalkMonthlyRating", "WalkController");
                var result = walkService.GetWalkMonthlyRating(walkId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}