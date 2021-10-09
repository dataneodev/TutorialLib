using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using dataneo.SharedKernel;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite
{
    public class EfRepositoryDetached<T> : IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
    {
        protected readonly IApplicationDbContext _applicationDbContext;

        public EfRepositoryDetached(IApplicationDbContext dbContext)
        {
            _applicationDbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var keyValues = new object[] { id };
            using var dbContext = _applicationDbContext.GetApplicationDbContext();
            return await dbContext.Set<T>().FindAsync(keyValues, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            using var dbContext = _applicationDbContext.GetApplicationDbContext();
            return await dbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            using var dbContext = _applicationDbContext.GetApplicationDbContext();
            var specificationResult = ApplySpecification(spec, dbContext);
            return await specificationResult.ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            using var dbContext = _applicationDbContext.GetApplicationDbContext();
            var specificationResult = ApplySpecification(spec, dbContext);
            return await specificationResult.CountAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            using var dbContext = _applicationDbContext.GetApplicationDbContext();
            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            using var dbContext = _applicationDbContext.GetApplicationDbContext();
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            using var dbContext = _applicationDbContext.GetApplicationDbContext();
            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<T> FirstAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            using var dbContext = _applicationDbContext.GetApplicationDbContext();
            var specificationResult = ApplySpecification(spec, dbContext);
            return await specificationResult.FirstAsync(cancellationToken);
        }

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            using var dbContext = _applicationDbContext.GetApplicationDbContext();
            var specificationResult = ApplySpecification(spec, dbContext);
            return await specificationResult.FirstOrDefaultAsync(cancellationToken);
        }

        protected IQueryable<T> ApplySpecification(ISpecification<T> spec, DbContext dbContext)
        {
            var evaluator = new SpecificationEvaluator();
            return evaluator.GetQuery(dbContext.Set<T>().AsQueryable(), spec);
        }
    }
}
