using CodebridgeTestApp.Dtos;
using CodebridgeTestApp.Enum;
using CodebridgeTestApp.Parameters;

namespace CodebridgeTestApp.Service
{
    public interface IDogService
    {
        Task<IEnumerable<DogGetDto>> GetDogsAsync(
            PageParameter? pageParameter = null,
            string attribute = "name",
            SortDirection order = SortDirection.Asc);
        Task<DogGetDto?> GetDogByNameAsync(string name);
        Task<DogGetDto> CreateDogAsync(DogCreateDto dogDto);
    }
}
