using LambdaCSharpWebAPI.Logging;
using LambdaCSharpWebAPI.Models;
using LambdaCSharpWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LambdaCSharpWebAPI.Controllers
{
    [Route("v1/ratings")]
    public class RatingController : Controller
    {
        private readonly IRatingService ratingService;

        public RatingController(IRatingService ratingService)
        {
            Logger.LogDebug("Setting ratingService", "RatingController", "RatingController");
            this.ratingService = ratingService;
        }
        [HttpPost]
        public IActionResult AddRating([FromBody]RatingModel rating)
        {
            try
            {
                Logger.LogDebug("Calling AddRating", "AddRating", "RatingController");
                ratingService.AddRating(rating);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}