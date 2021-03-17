using LambdaCSharpWebAPI.Data;
using LambdaCSharpWebAPI.Models;
using LambdaCSharpWebAPI.Services;
using Moq;
using System;
using Xunit;

namespace LambdaCSharpWebAPI.Tests
{
    public class RatingControllerTests
    {

        private RatingService ratingService = null;
        private Mock<IDatabase> databaseMock = null;

        [Fact]
        public void TestAddWalk()
        {
            databaseMock = new Mock<IDatabase>()
            {
                DefaultValue = DefaultValue.Mock
            };

            //arrange
            RatingModel rating;
            rating = new RatingModel
            {
                Id = "abc1234",
                RatingTime = DateTime.Now,
                UserId = "TestUserId",
                WalkId = "TestWalkID",
                WalkRating = 3
            };

            //act
            ratingService = new RatingService(databaseMock.Object);
            ratingService.AddRating(rating);
            //assert
            databaseMock.Verify(x => x.AddRating(rating));
        }
    }
}
