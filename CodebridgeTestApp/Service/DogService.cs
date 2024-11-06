using CodebridgeTestApp.Dtos;
using CodebridgeTestApp.Enum;
using CodebridgeTestApp.Exceptions;
using CodebridgeTestApp.Mappers;
using CodebridgeTestApp.Models;
using CodebridgeTestApp.Parameters;
using CodebridgeTestApp.Repository;
using System.Linq.Expressions;

namespace CodebridgeTestApp.Service
{
    public class DogService : IDogService
    {
        private readonly IDogRepository _dogRepository;

        public DogService(IDogRepository dogRepository)
        {
            _dogRepository = dogRepository;
        }

        public async Task<IEnumerable<DogGetDto>> GetDogsAsync(
            PageParameter? pageParameter = null,
            string attribute = "name",
            SortDirection order = SortDirection.Asc)
        {
            if (pageParameter is not null && !pageParameter.IsValid())
                throw new ArgumentException("Invalid pagination parameter");

            if (string.IsNullOrEmpty(attribute))
                throw new ArgumentException("Invalid attribute parameter");

            var keySelector = CreateDogSortExpression(attribute);

            var dogs = await _dogRepository.GetAsync(
                pageParameter: pageParameter,
                sortBy: keySelector,
                sortDirection: order
            );

            return dogs.Select(d => d.ToGetDto()).ToList();
        }

        public async Task<DogGetDto?> GetDogByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Invalid name parameter");

            var entity = (await _dogRepository.GetAsync(predicate: d => d.name == name))?.SingleOrDefault();

            return entity?.ToGetDto();
        }
        public async Task<DogGetDto> CreateDogAsync(DogCreateDto dogDto)
        {

            var entity = dogDto.ToEntity();
            if (!entity.IsValid()) throw new ArgumentException("Invalid dog model");

            if (await GetDogByNameAsync(entity.name) is not null)
                throw new EntityAlreadyExistsException($"Dog with name {entity.name} already exists");

            _dogRepository.Create(entity);
            await _dogRepository.SaveAsync();
            return entity.ToGetDto();
        }
        private Expression<Func<Dog, object>> CreateDogSortExpression(string attribute)
        {
            if (string.IsNullOrEmpty(attribute)) return null;

            var parameter = Expression.Parameter(typeof(Dog), "d");

            var propertyInfo = typeof(Dog).GetProperty(attribute);
            if (propertyInfo is null)
            {
                throw new ArgumentException("Invalid attribute parameter");
            }

            var property = Expression.Property(parameter, attribute);
            
            return Expression.Lambda<Func<Dog, object>>(Expression.Convert(property, typeof(object)), parameter);
        }
    }
}
