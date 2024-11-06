using CodebridgeTestApp.DataDbContext;
using CodebridgeTestApp.Dtos;
using CodebridgeTestApp.Enum;
using CodebridgeTestApp.Models;
using CodebridgeTestApp.Parameters;
using CodebridgeTestApp.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CodebridgeTestApp.UnitTests.Repository
{
    public class DogRepositoryTests
    {
        private readonly DogRepository _repository;
        private readonly ApplicationDbContext _context;

        public DogRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DogDatabase")
                .Options;
            _context = new ApplicationDbContext(options);
            _repository = new DogRepository(_context);

            if (_context.Set<Dog>().Count() > 0)
            {
                _context.Set<Dog>().RemoveRange(_context.Set<Dog>());
                _context.SaveChanges();
            }
        }


        /*
        GetAsync_ShouldReturnAllDogs
        GetAsync_WithPredicate_ShouldReturnDogs
        GetAsync_WithSortAscending_ShouldReturnDogs
        GetAsync_WithSortDescending_ShouldReturnDogs
        GetAsync_WithValidPagination_ShouldReturnDogs
        GetAsync_WithInvalidPagination_ShouldThrowArgumentException
        */


        [Fact]
        public async Task GetAsync_ShouldReturnAllDogs()
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>() { 
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 }};

            listDogs.OrderBy(x => x.name);

            _context.Set<Dog>().AddRange(listDogs);
            await _context.SaveChangesAsync();

            // Act
            var resultDogs = await _repository.GetAsync();

            // Assert
            resultDogs.Should().BeOfType<List<Dog>>();
            resultDogs.Should().BeEquivalentTo(listDogs);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(21)]
        [InlineData(22)]

        public async Task GetAsync_WithPredicate_ShouldReturnDogs(int tailLength)
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>() {
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 },
                new Dog { name = "Marina", color = "brown", tail_length = 2, weight = 5 }};

            listDogs.OrderBy(x => x.name);


            _context.Set<Dog>().AddRange(listDogs);
            await _context.SaveChangesAsync();

            Func<Dog, bool> predicate = d => d.tail_length > tailLength;
            Expression<Func<Dog, bool>> exprPredicate = d => d.tail_length > tailLength;
            // Act
            var resultDogs = await _repository.GetAsync(exprPredicate);
            // Assert
            resultDogs.Should().BeOfType<List<Dog>>();
            resultDogs.Should().BeEquivalentTo(listDogs.Where(predicate));
        }

        [Fact]
        public async Task GetAsync_WithSortAscending_ShouldReturnDogs()
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>() {
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 },
                new Dog { name = "Marina", color = "brown", tail_length = 2, weight = 5 }};

            listDogs.OrderBy(x => x.name);

            _context.Set<Dog>().AddRange(listDogs);
            await _context.SaveChangesAsync();

            Func<Dog, object> sortBy = d => d.tail_length;
            Expression<Func<Dog, object>> exprSortBy = d => d.tail_length;
            SortDirection sortDirection = SortDirection.Asc;

            // Act
            var result = await _repository.GetAsync(sortBy: exprSortBy, sortDirection: sortDirection );
            // Assert
            result.Should().BeOfType<List<Dog>>();
            result.Should().BeEquivalentTo(listDogs.OrderBy(sortBy));
        }

        [Fact]
        public async Task GetAsync_WithSortDescending_ShouldReturnDogs()
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>() {
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 },
                new Dog { name = "Marina", color = "brown", tail_length = 2, weight = 5 }};

            listDogs.OrderBy(x => x.name);

            _context.Set<Dog>().AddRange(listDogs);
            await _context.SaveChangesAsync();

            Func<Dog, object> sortBy = d => d.tail_length;
            Expression<Func<Dog, object>> exprSortBy = d => d.tail_length;
            SortDirection sortDirection = SortDirection.Desc;

            // Act
            var result = await _repository.GetAsync(sortBy: exprSortBy, sortDirection: sortDirection);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Dog>>();
            result.Should().BeEquivalentTo(listDogs.OrderByDescending(sortBy));
        }

        [Theory]
        [InlineData(1,1)]
        [InlineData(2,1)]
        [InlineData(1,2)]
        [InlineData(2,2)]

        public async Task GetAsync_WithValidPagination_ShouldReturnDogs(int pageNumber, int pageSize)
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>() 
            {
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 },
                new Dog { name = "Marina", color = "brown", tail_length = 2, weight = 5 },
                new Dog { name = "Betty", color = "black", tail_length = 1977, weight = 232 }
            };

            _context.Set<Dog>().AddRange(listDogs);
            await _context.SaveChangesAsync();

            PageParameter pm = new PageParameter(pageNumber, pageSize);

            // Act
            var result = await _repository.GetAsync(pageParams: pm);

            // Assert
            listDogs = listDogs.Skip((pm.PageNumber - 1) * pm.PageSize).Take(pm.PageSize).ToList();

            result.Should().NotBeNull();
            result.Should().BeOfType<List<Dog>>();
            result.Should().HaveSameCount(listDogs);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, -1)]
        [InlineData(-1, 0)]
        [InlineData(1, -1)]
        [InlineData(-1, 1)]
        public async Task GetAsync_WithInvalidPagination_ShouldThrowArgumentException(int pageNumber, int pageSize)
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>() {
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 },
                new Dog { name = "Marina", color = "brown", tail_length = 2, weight = 5 }};

            listDogs.OrderBy(x => x.name);

            _context.Set<Dog>().AddRange(listDogs);
            await _context.SaveChangesAsync();

            PageParameter pm = new PageParameter(pageNumber, pageSize);

            // Act
            Func<Task> act = async () => await _repository.GetAsync(pageParams: pm);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid pagination parameter");
        }

        /*
         
        CreateAsync_WithValidModel_ShouldEntityExistsInDb
        CreateAsync_WithNameAlreadyExist_ShouldThrowException
        CreateAsync_WithInvalidModel_ShouldThrowException
         */

        [Fact]
        public async Task CreateAsync_WithValidModel_ShouldEntityAddedToDb()
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>() {
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 },
                new Dog { name = "Marina", color = "brown", tail_length = 2, weight = 5 }};

            _context.Set<Dog>().AddRange(listDogs);
            await _context.SaveChangesAsync();

            Dog newDog = new Dog() { name = "Betty", color = "black", tail_length = 1977, weight = 232 };

            // Act
            _repository.Create(newDog);
            await _repository.SaveAsync();

            var result = await _repository.GetAsync();
            // Assert
            result.Should().Contain(newDog);
        }

        [Theory]
        [InlineData("", "black", 1977, 232)]
        [InlineData("Betty", "", 1977, 232)]
        [InlineData("Betty", "black", -1, 232)]
        [InlineData("Betty", "black", 1977, -1)]
        public async Task Create_WithInvalidModel_ShouldThrowArgumentExceptionAndNewEntityIsNotAdded(string name, string color, int tailLength, int weight)
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>() {
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 },
                new Dog { name = "Marina", color = "brown", tail_length = 2, weight = 5 }};

            _context.Set<Dog>().AddRange(listDogs);
            await _context.SaveChangesAsync();

            Dog newDog = new Dog() { name = name, color = color, tail_length = tailLength, weight = weight };

            // Act
            Action act = () => _repository.Create(newDog);

            var dbState = await _repository.GetAsync();

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Invalid dog model");
            dbState.Should().NotContain(newDog);

        }

        [Fact]
        public async Task CreateAsync_WithNameAlreadyExist_ShouldThrowDbUpdateExceptionAndNewEntityIsNotAdded()
        {
            // Arrange
            List<Dog> listDogs = new List<Dog>() {
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 },
                new Dog { name = "Betty", color = "brown", tail_length = 2, weight = 5 }};

            Dog newDog = new Dog() { name = "Betty", color = "black", tail_length = 1977, weight = 232 };

            _context.Set<Dog>().AddRange(listDogs);
            await _context.SaveChangesAsync();

            // Act
            Action act = () => _repository.Create(newDog);

            var dbState = await _repository.GetAsync();

            // Assert
            act.Should().Throw<InvalidOperationException>();
            dbState.Should().NotContain(newDog);
        }
    }
}
