using CodebridgeTestApp.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CodebridgeTestApp.UnitTests.Controller
{
    public class PingControllerTests
    {
        private readonly PingController _pingController;

        public PingControllerTests()
        {
            _pingController = new PingController();
        }

        [Fact]
        public void Ping_ShouldReturnString()
        {
            // Act
            var result = _pingController.Ping();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult?.Value.Should().Be("Dogshouseservice.Version1.0.1");
        }
    }
}
