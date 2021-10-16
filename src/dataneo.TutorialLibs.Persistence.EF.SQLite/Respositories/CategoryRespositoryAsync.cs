using Ardalis.Specification;
using dataneo.TutorialLibs.Domain.Categories;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories
{
    public class CattegoryRespositoryAsync : EfRepository<Category>, ICategoryRespositoryAsync
    {
        public CattegoryRespositoryAsync(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        { }

        public override async Task<Category> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _dbContext.Categories
                        .Include("_tutorials")
                        .FirstOrDefaultAsync(f => f.Id == id, cancellationToken)
                        .ConfigureAwait(false);

        public async Task<IReadOnlyList<Category>> ListAllAsync(CancellationToken cancellationToken = default)
            => await _dbContext.Categories
                        .Include("_tutorials")
                        .ToListAsync(cancellationToken)
                        .ConfigureAwait(false);

        public async Task<IReadOnlyList<Category>> ListAsync(ISpecification<Category> spec, CancellationToken cancellationToken = default)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult
                            .Include("_tutorials")
                            .ToListAsync(cancellationToken);
        }
    }
}
