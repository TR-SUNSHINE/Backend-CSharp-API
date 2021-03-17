using LambdaCSharpWebAPI.Data;
using LambdaCSharpWebAPI.Models;
using LambdaCSharpWebAPI.Services;
using Moq;
using System.Collections;
using Xunit;

namespace LambdaCSharpWebAPI.Tests
{
    public class WalkControllerTests
    {
        /*
         * At this stage the following files are created, but not in use:
         * WalkController-GetWalk.json
         * WalkController-PutWalk.json
         */
        private WalkService walkService = null;
        private Mock<IDatabase> databaseMock = null;

        [Fact]
        public void TestGetWalkReturnsValues()
        {
            ArrayList arr = new ArrayList();
            arr.Add("TestVal1");
            arr.Add("TestVal2");
            databaseMock = new Mock<IDatabase>();

            //arrange
            databaseMock.Setup(p => p.GetWalks(It.IsAny<string>())).Returns(arr);
            //act
            walkService = new WalkService(databaseMock.Object);
            var result = walkService.GetWalks("randomValue");
            //assert
            Assert.NotEmpty(result);
        }
        [Fact]
        public void TestGetWalkReturnsNoValues()
        {
            ArrayList arr = new ArrayList();
            arr.Add("TestVal1");
            arr.Add("TestVal2");
            databaseMock = new Mock<IDatabase>();

            //arrange
            databaseMock.Setup(p => p.GetWalks("1234")).Returns(arr);
            //act
            walkService = new WalkService(databaseMock.Object);
            var result = walkService.GetWalks("randomValue");
            //assert
            Assert.Null(result);
        }
        [Fact]
        public void TestAddWalk()
        {
            databaseMock = new Mock<IDatabase>()
            {
                DefaultValue = DefaultValue.Mock
            };

            //arrange
            WalkModel walk;
            walk = new WalkModel
            {
                Id = "abc1234",
                Routes = null,
                UserID = "def1234",
                WalkName = "TestWalk"
            };

            //act
            walkService = new WalkService(databaseMock.Object);
            walkService.AddWalk(walk);
            //assert
            databaseMock.Verify(x => x.AddWalk(walk));
        }
    }
}
