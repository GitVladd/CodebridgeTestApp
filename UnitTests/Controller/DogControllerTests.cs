using CodebridgeTestApp.Controllers;
using CodebridgeTestApp.Dtos;
using CodebridgeTestApp.Enum;
using CodebridgeTestApp.Exceptions;
using CodebridgeTestApp.Models;
using CodebridgeTestApp.Parameters;
using CodebridgeTestApp.Service;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CodebridgeTestApp.UnitTests.Controller
{
    public class DogControllerTests
    {
        private readonly DogController _controller;
        private readonly Mock<IDogService> _mockService;

        public DogControllerTests()
        {
            _mockService = new Mock<IDogService>();
            _controller = new DogController(_mockService.Object);
        }

        // GetAsync Tests
        [Fact]
        public async Task GetAsync_ShouldReturnAllDogGetDtosAndOkResult()
        {
            // Arrange
            var dogs = new List<DogGetDto>
            {
                new DogGetDto { name = "Buddy", color = "Brown", tail_length = 10, weight = 20 },
                new DogGetDto { name = "Max", color = "Black", tail_length = 15, weight = 25 }
            };
            var pageParams = new PageParameter();

            //_mockService.Setup(s => s.GetDogAsync(pageParams, null, SortDirection.Asc)).ReturnsAsync(dogs);
            _mockService.Setup(s => s.GetDogsAsync(It.IsAny<PageParameter>(), It.IsAny<string>(), It.IsAny<SortDirection>())).ReturnsAsync(dogs);

            // Act
            var result = await _controller.GetDogsAsync(pageParams, null, SortDirection.Asc);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);

            okResult.Should().NotBeNull();

            var returnedDogs = okResult.Value as List<DogGetDto>;
            returnedDogs.Should().NotBeNull();
            returnedDogs.Should().BeEquivalentTo(dogs);
        }


        [Fact]
        public async Task GetAsync_WhenWithValidAttributeAscending_ShouldReturnDogGetDtosAndOkResult()
        {
            // Arrange
            var collection = new List<DogGetDto>
            {
                new DogGetDto { name = "Buddy", color = "Brown", tail_length = 10, weight = 20 },
                new DogGetDto { name = "Max", color = "Black", tail_length = 15, weight = 23 },
                new DogGetDto { name = "Stas", color = "Black", tail_length = 23, weight = 24 },
                new DogGetDto { name = "Boris", color = "Grey", tail_length = 11, weight = 26 }
            };
            var attribute = "tail_length";
            SortDirection sortDirection = SortDirection.Asc;
            var pageParams = new PageParameter();

            _mockService.Setup(s => s.GetDogsAsync(It.IsAny<PageParameter>(), It.IsAny<string>(), It.IsAny<SortDirection>())).ReturnsAsync(collection);

            // Act
            var result = await _controller.GetDogsAsync(pageParams, attribute, sortDirection);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);


            okResult.Should().NotBeNull();

            var returnedDogs = okResult?.Value as List<DogGetDto>;
            returnedDogs.Should().NotBeNull();
            returnedDogs.Should().HaveCount(collection.Count);
            returnedDogs.Should().BeEquivalentTo(collection);
        }


        [Fact]
        public async Task GetAsync_WhenWithValidAttributeDescending_ShouldReturnDogGetDtosAndOkResult()
        {
            // Arrange
            var collection = new List<DogGetDto>
            {
                new DogGetDto { name = "Buddy", color = "Brown", tail_length = 10, weight = 20 },
                new DogGetDto { name = "Max", color = "Black", tail_length = 15, weight = 23 },
                new DogGetDto { name = "Rap", color = "Black", tail_length = 23, weight = 24 },
                new DogGetDto { name = "Graffity", color = "Grey", tail_length = 11, weight = 26 }
            };
            var attribute = "tail_length";
            SortDirection sortDirection = SortDirection.Desc;
            var pageParams = new PageParameter();

            _mockService.Setup(s => s.GetDogsAsync(It.IsAny<PageParameter>(), It.IsAny<string>(), It.IsAny<SortDirection>())).ReturnsAsync(collection);

            // Act
            var result = await _controller.GetDogsAsync(pageParams, attribute, sortDirection);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);


            okResult.Should().NotBeNull();

            var returnedDogs = okResult.Value as List<DogGetDto>;
            returnedDogs.Should().NotBeNull();
            returnedDogs.Should().HaveCount(collection.Count);
            returnedDogs.Should().BeEquivalentTo(collection);
        }


        [Fact]
        public async Task GetAsync_WhenWithEmptyAttribute_ShouldReturnBadRequestResult()
        {
            // Arrange
            var attribute = "";

            // Act
            var result = await _controller.GetDogsAsync(attribute: attribute);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>(); 
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult?.StatusCode.Should().Be(400); 
        }

        [Fact]
        public async Task GetAsync_WhenWithInvalidAttribute_ShouldThrowArgumentException()
        {
            // Arrange
            var attribute = "InvalidAttribute";
            SortDirection sortDirection = SortDirection.Desc;
            var pageParams = new PageParameter();

            _mockService.Setup(s => s.GetDogsAsync(It.IsAny<PageParameter>(), It.IsAny<string>(), It.IsAny<SortDirection>()))
                .ThrowsAsync(new ArgumentException("Invalid attribute parameter"));

            // Act
            Func<Task> act = async () => await _controller.GetDogsAsync(pageParams, attribute, sortDirection);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid attribute parameter"); 
        }


        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 2)]
        public async Task GetAsync_WithValidPagination_ShouldReturnDogDtosAndOkResult(int pageNumber, int pageSize)
        {
            // Arrange
            var dogs = new List<DogGetDto>
            {
            new DogGetDto { name = "Buddy", color = "Brown", tail_length = 10, weight = 20 },
            new DogGetDto { name = "Max", color = "Black", tail_length = 15, weight = 25 }
            };

            var pageParams = new PageParameter(pageNumber, pageSize);

            _mockService.Setup(s => s.GetDogsAsync(It.IsAny<PageParameter>(), It.IsAny<string>(), It.IsAny<SortDirection>())).ReturnsAsync(dogs);

            // Act
            var result = await _controller.GetDogsAsync(pageParameter: pageParams);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);

            okResult.Should().NotBeNull();
            okResult?.Value.Should().NotBeNull();
            okResult?.Value.Should().BeOfType<List<DogGetDto>>();
            okResult?.Value.Should().BeEquivalentTo(dogs);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, -1)]
        [InlineData(-1, 0)]
        [InlineData(1, -1)]
        [InlineData(-1, 1)]
        public async Task GetAsync_WithInvalidPagination_ShouldReturnBadRequestResult(int pageNumber, int pageSize)
        {

            var pageParams = new PageParameter(pageNumber, pageSize);

            // Act
            var result = await _controller.GetDogsAsync(pageParams);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult?.StatusCode.Should().Be(400);
        }

        //CreateAsync Tests
        [Fact]
        public async Task CreateAsyn_ShouldReturnDogGetDtoAndCreatedResult()
        {
            // Arrange
            var dogCreateDto = new DogCreateDto { name = "Buddy", color = "Brown", tail_length = 10, weight = 20 };
            var dogGetDto = new DogGetDto { name = "Buddy", color = "Brown", tail_length = 10, weight = 20 };

            _mockService.Setup(s => s.CreateDogAsync(It.IsAny<DogCreateDto>())).ReturnsAsync(dogGetDto);

            // Act
            var result = await _controller.CreateDogAsync(dogCreateDto);

            // Assert
            result.Result.Should().BeOfType<CreatedResult>();
            var createdResult = result.Result as CreatedResult;
            createdResult.StatusCode.Should().Be(201);

            createdResult.Should().NotBeNull();
            createdResult.Value.Should().BeEquivalentTo(dogCreateDto);
        }

        [Fact]
        public async Task CreateAsync_WithNameIsAlreadyUsed_ShouldThrowEntityAlreadyExistsException()
        {
            // Arrange
            var dogDto = new DogCreateDto { name = "Buddy", color = "Brown", tail_length = 10, weight = 20 };
            _mockService.Setup(s => s.CreateDogAsync(It.IsAny<DogCreateDto>()))
                        .ThrowsAsync(new EntityAlreadyExistsException($"Dog with name {dogDto.name} already exists"));

            // Act
            Func<Task> act = async () => await _controller.CreateDogAsync(dogDto);

            // Assert
            await act.Should().ThrowAsync<EntityAlreadyExistsException>()
                .WithMessage($"Dog with name {dogDto.name} already exists");
        }

        [Fact]
        public async Task CreateAsync_WithInvalidDogModel_ShouldReturnBadRequestResult()
        {

            // Arrange
            var dogDto = new DogCreateDto { name = "Buddy", color = "Brown", tail_length = 10, weight = -1 };
            _controller.ModelState.AddModelError("ERROR", "Model Error");
            // Act
            var result = await _controller.CreateDogAsync(dogDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult?.StatusCode.Should().Be(400);
        }
    }
}
