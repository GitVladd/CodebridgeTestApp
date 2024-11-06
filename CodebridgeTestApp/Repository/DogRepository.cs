using CodebridgeTestApp.DataDbContext;
using CodebridgeTestApp.Enum;
using CodebridgeTestApp.Models;
using CodebridgeTestApp.Parameters;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CodebridgeTestApp.Repository
{
    public class DogRepository : IDogRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DogRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Dog>> GetAsync(
            Expression<Func<Dog, bool>>? predicate = null,
            PageParameter? pageParams = null,
            Expression<Func<Dog, object>>? sortBy = null,
            SortDirection sortDirection = SortDirection.Asc,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<Dog>().AsQueryable();

            if (predicate is not null)
                query = query.Where(predicate);

            if (sortBy is not null) 
                query = sortDirection == SortDirection.Asc? query.OrderBy(sortBy) : query.OrderByDescending(sortBy);

            if (pageParams is not null)
            {
                if(!pageParams.IsValid()) throw new ArgumentException("Invalid pagination parameter");

                int take = pageParams.PageSize;
                int skip = (pageParams.PageNumber - 1) * pageParams.PageSize;
                query = query.Skip(skip).Take(take);
            }

            return await query.ToListAsync(cancellationToken);

        }

        public void Create(Dog dog)
        {
            if (!dog.IsValid()) throw new ArgumentException("Invalid dog model");
            _dbContext.Set<Dog>().Add(dog);
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
