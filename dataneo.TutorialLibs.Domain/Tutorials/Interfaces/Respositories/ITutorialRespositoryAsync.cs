using dataneo.SharedKernel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Tutorials
{
    public interface ITutorialRespositoryAsync : IAsyncRepository<Tutorial>
    {
        Task<IReadOnlyList<TutorialHeaderDto>> GetAllTutorialHeadersDtoAsync(CancellationToken cancellationToken = default);
        Task UpdateEpisodeAsync(Episode episode, CancellationToken cancellationToken = default);
    }
}
