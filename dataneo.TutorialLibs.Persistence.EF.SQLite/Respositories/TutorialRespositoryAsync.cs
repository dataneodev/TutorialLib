using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.DTO;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Interfaces.Respositories;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Context;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories
{
    public class TutorialRespositoryAsync : EfRepository<Tutorial>, ITutorialRespositoryAsync, IDisposable
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public TutorialRespositoryAsync() : base(new ApplicationDbContext())
        { }

        public TutorialRespositoryAsync(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        { }

        public Task<IReadOnlyList<TutorialHeaderDto>> GetTutorialHeadersDtoByIdAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new TutorialHeaderDto[0] as IReadOnlyList<TutorialHeaderDto>);
        }

        public Task<Maybe<TutorialHeaderDto>> GetTutorialHeaderDtoByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Maybe<TutorialHeaderDto>.None);
        }

        public void Dispose()
        {
            this._dbContext.Dispose();
        }
    }
}
