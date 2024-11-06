using CodebridgeTestApp.Enum;
using CodebridgeTestApp.Models;
using CodebridgeTestApp.Parameters;
using System.Linq.Expressions;

namespace CodebridgeTestApp.Repository
{
    public interface IDogRepository
    {
        Task<IEnumerable<Dog>> GetAsync(
            Expression<Func<Dog, bool>>? predicate = null,
            PageParameter? pageParameter = null,
            Expression<Func<Dog, object>>? sortBy = null,
            SortDirection sortDirection = SortDirection.Asc,
            CancellationToken cancellationToken = default);
        void Create(Dog dog);
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}
