using CodebridgeTestApp.Dtos;
using CodebridgeTestApp.Enum;
using CodebridgeTestApp.Exceptions;
using CodebridgeTestApp.Mappers;
using CodebridgeTestApp.Models;
using CodebridgeTestApp.Parameters;
using CodebridgeTestApp.Repository;
using CodebridgeTestApp.Service;
using FluentAssertions;
using Moq;
using System;
using System.Linq.Expressions;

namespace CodebridgeTestApp.UnitTests.Service
{
    public class DogServiceTests
    {
        private readonly Mock<IDogRepository> _mockRepository;
        private readonly DogService _dogService;

        public DogServiceTests()
        {
            _mockRepository = new Mock<IDogRepository>();
            _dogService = new DogService(_mockRepository.Object);
        }

        /*
        GetDogAsync_ShouldReturnAllGetDogDtos

        GetDogAsync_WithAttributeSortAscending_ShouldReturnGetDogDtos
        GetDogAsync_WithAttributeSortDescending_ShouldReturnGetDogDtos
        GetDogAsync_WithInvalidAttribute_ShouldThrowArgumentException

        GetDogAsync_WithValidPagination_ShouldReturnGetDogDtos
        GetDogAsync_WithInvalidPagination_ShouldThrowArgumentException

        GetDogByNameAsync_WithEntityExist_ShouldReturnDog
        GetDogByNameAsync_WithEntityNotExist_ShouldReturnNull
        GetDogByNameAsync_WithInvalidName_ShouldThrowArgumentException

        CreateDogAsync_ShouldReturnDogDto
        CreateDogAsync_WithNameIsNotUnique_ShouldThrowEntityAlreadyExistsException
        CreateDogAsync_WithModelInvalid_ShouldThrowEntityAlreadyExistsException
         */


        [Fact] 
        public async Task GetDogAsync_ShouldReturnAllGetDogDtos() 
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>() {
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 },
                new Dog { name = "Marina", color = "brown", tail_length = 2, weight = 5 }};

            _mockRepository.Setup(s => s.GetAsync(
                It.IsAny<Expression<Func<Dog, bool>>>(),
                It.IsAny<PageParameter>(),
                It.IsAny<Expression<Func<Dog, object>>>(),
                It.IsAny<SortDirection>(),    
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(listDogs);

            // Act
            var results = await _dogService.GetDogsAsync();

            //Assert
            results.Should().NotBeNull();
            results.Should().BeOfType<List<DogGetDto>>();
            results.Should().BeEquivalentTo(listDogs);
        }

        [Fact] 
        public async Task GetDogAsync_WithValidAttributeSortAscending_ShouldReturnGetDogDtos() 
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>() {
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 },
                new Dog { name = "Marina", color = "brown", tail_length = 2, weight = 5 }};

            _mockRepository.Setup(s => s.GetAsync(
                It.IsAny<Expression<Func<Dog, bool>>>(),
                It.IsAny<PageParameter>(),
                It.IsAny<Expression<Func<Dog, object>>>(),
                It.IsAny<SortDirection>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(listDogs);

            string attribute = "color";
            SortDirection sortDir = SortDirection.Asc;

            // Act
            var results = await _dogService.GetDogsAsync(attribute: attribute, order: sortDir);

            //Assert
            results.Should().NotBeNull();
            results.Should().BeOfType<List<DogGetDto>>();
            results.Should().BeEquivalentTo(listDogs);
        }

        [Fact]
        public async Task GetDogAsync_WithValidAttributeSortDescending_ShouldReturnGetDogDtos()
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>() 
            {
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 },
                new Dog { name = "Marina", color = "brown", tail_length = 2, weight = 5 }
            };

            _mockRepository.Setup(s => s.GetAsync(
                It.IsAny<Expression<Func<Dog, bool>>>(),
                It.IsAny<PageParameter>(),
                It.IsAny<Expression<Func<Dog, object>>>(),
                It.IsAny<SortDirection>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(listDogs);


            string attribute = "color";
            SortDirection sortDir = SortDirection.Desc;

            // Act
            var results = await _dogService.GetDogsAsync(attribute: attribute, order: sortDir);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeOfType<List<DogGetDto>>(); 
            results.Should().BeEquivalentTo(listDogs.Select(s => s.ToGetDto()).ToList());
        }

        [Fact]
        public async Task GetDogAsync_WithInvalidAttribute_ShouldThrowArgumentException()
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>() {
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 },
                new Dog { name = "Marina", color = "brown", tail_length = 2, weight = 5 }};

            _mockRepository.Setup(s => s.GetAsync(
                It.IsAny<Expression<Func<Dog, bool>>>(),
                It.IsAny<PageParameter>(),
                It.IsAny<Expression<Func<Dog, object>>>(),
                It.IsAny<SortDirection>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(listDogs);


            string attribute = "invalid_attribute";
            // Act
            Func<Task> act = async () => await _dogService.GetDogsAsync(attribute: attribute);

            //Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid attribute parameter");
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 2)]
        public async Task GetDogAsync_WithValidPagination_ShouldReturnGetDogDtos(int pageNumber, int pageSize) 
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>()
            {
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 },
                new Dog { name = "Marina", color = "brown", tail_length = 2, weight = 5 }
            };

            _mockRepository.Setup(s => s.GetAsync(
                It.IsAny<Expression<Func<Dog, bool>>>(),
                It.IsAny<PageParameter>(),
                It.IsAny<Expression<Func<Dog, object>>>(),
                It.IsAny<SortDirection>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(listDogs);

            PageParameter pageParam = new PageParameter(pageNumber, pageSize);

            // Act
            var results = await _dogService.GetDogsAsync(pageParameter: pageParam);

            //Assert
            results.Should().NotBeNull();
            results.Should().BeOfType<List<DogGetDto>>();
            results.Should().BeEquivalentTo(listDogs);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, -1)]
        [InlineData(-1, 0)]
        [InlineData(1, -1)]
        [InlineData(-1, 1)]
        public async Task GetDogAsync_WithInvalidPagination_ShouldThrowArgumentException(int pageNumber, int pageSize)
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>() {
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 },
                new Dog { name = "Marina", color = "brown", tail_length = 2, weight = 5 }};

            _mockRepository.Setup(s => s.GetAsync(
                It.IsAny<Expression<Func<Dog, bool>>>(),
                It.IsAny<PageParameter>(),
                It.IsAny<Expression<Func<Dog, object>>>(),
                It.IsAny<SortDirection>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(listDogs);

            PageParameter pageParam = new PageParameter(pageNumber, pageSize);



            // Act
            Func<Task> act = async () => await _dogService.GetDogsAsync(pageParameter: pageParam);

            //Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid pagination parameter");
        }

        [Fact] 
        public async Task GetDogByNameAsync_WithValidNameAndEntityExist_ShouldReturnDog() 
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>() {
                new Dog { name = "Valid Name", color = "red & amber", tail_length = 22, weight = 32 }
            };

            _mockRepository.Setup(s => s.GetAsync(
                It.IsAny<Expression<Func<Dog, bool>>>(),
                It.IsAny<PageParameter>(),
                It.IsAny<Expression<Func<Dog, object>>>(),
                It.IsAny<SortDirection>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(listDogs);

            string name = "Valid Name";

            // Act
            var results = await _dogService.GetDogByNameAsync(name);

            //Assert
            results.Should().NotBeNull();
            results.Should().BeOfType<DogGetDto>();
            results.Should().BeEquivalentTo(listDogs.SingleOrDefault());

        }

        [Fact]
        public async Task GetDogByNameAsync_WithValidNameAndEntityNotExist_ShouldReturnNull()
        {
            // Arrange
            List<Dog> emptyList = new List<Dog>();
            string name = "Valid Name";

            _mockRepository.Setup(r => r.GetAsync(
                It.IsAny<Expression<Func<Dog, bool>>>(),
                It.IsAny<PageParameter>(),
                It.IsAny<Expression<Func<Dog, object>>>(),
                It.IsAny<SortDirection>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(emptyList);

            // Act
            var results = await _dogService.GetDogByNameAsync(name);

            // Assert
            results.Should().BeNull(); 
        }

        [Fact] 
        public async Task GetDogByNameAsync_WithEmptyName_ShouldThrowArgumentException() 
        {
            // Arrange
            string name = "";

            Func<Task> act = async () => await _dogService.GetDogByNameAsync(name);

            //Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid name parameter");
        }

        [Fact]
        public async Task GetDogByNameAsync_WithNullName_ShouldThrowArgumentException()
        {
            // Arrange
            string name = null;

            Func<Task> act = async () => await _dogService.GetDogByNameAsync(name);

            //Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid name parameter");
        }

        [Fact] 
        public async Task CreateDogAsync_ShouldReturnDogGetDto() 
        {
            // Arrange
            DogCreateDto newDog = new DogCreateDto { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 };

            // Act
            var results = await _dogService.CreateDogAsync(newDog);

            //Assert
            results.Should().NotBeNull();
            results.Should().BeOfType<DogGetDto>();
        }

        [Fact]
        public async Task CreateDogAsync_WithNameIsNotUnique_ShouldThrowEntityAlreadyExistsException()
        {
            // Arrange
            DogCreateDto newDog = new DogCreateDto { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 };

            _mockRepository.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Dog, bool>>>(),
                                                  It.IsAny<PageParameter>(),
                                                  It.IsAny<Expression<Func<Dog, object>>>(),
                                                  It.IsAny<SortDirection>(),
                                                  It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Dog> { new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 } });

            // Act
            Func<Task> act = async () => await _dogService.CreateDogAsync(newDog);

            //Assert
            await act.Should().ThrowAsync<EntityAlreadyExistsException>()
                .WithMessage($"Dog with name {newDog.name} already exists");
        }

        [Theory]
        [InlineData("", "black", 1977, 232)]
        [InlineData("Betty", "", 1977, 232)]
        [InlineData("Betty", "black", -1, 232)]
        [InlineData("Betty", "black", 1977, -1)]
        public async Task CreateDogAsync_WithModelInvalid_ShouldThrowEntityAlreadyExistsException(string name, string color, int tailLength, int weight)
        {
            // Arrange
            DogCreateDto newDog = new DogCreateDto() { name = name, color = color, tail_length = tailLength, weight = weight };

            // Act
            Func<Task> act = async () => await _dogService.CreateDogAsync(newDog);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid dog model");
        }
    }
}
