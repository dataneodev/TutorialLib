using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.DTO;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Interfaces.Respositories;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Context;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories
{
    public class TutorialRespositoryAsync : EfRepository<Tutorial>, ITutorialRespositoryAsync
    {
        public TutorialRespositoryAsync(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        { }

        public Task<Maybe<TutorialHeaderDto>> GetTutorialHeaderDtoByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return Maybe<TutorialHeaderDto>.None;
        }
    }
}
