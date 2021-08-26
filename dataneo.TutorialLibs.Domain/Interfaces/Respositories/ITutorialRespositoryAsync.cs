using dataneo.SharedKernel;
using dataneo.TutorialLibs.Domain.DTO;
using dataneo.TutorialLibs.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Interfaces.Respositories
{
    public interface ITutorialRespositoryAsync : IAsyncRepository<Tutorial>
    {
        Task<IReadOnlyList<TutorialHeaderDto>> GetAllTutorialHeadersDtoAsync(CancellationToken cancellationToken = default);
        Task UpdateEpisodeAsync(Episode episode, CancellationToken cancellationToken = default);
    }
}
