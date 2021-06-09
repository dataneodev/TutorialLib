using dataneo.SharedKernel;
using dataneo.TutorialLibs.Domain.DTO;
using dataneo.TutorialLibs.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Interfaces.Respositories
{
    public interface ITutorialRespositoryAsync : IAsyncRepository<Tutorial>
    {
        Task<TutorialHeaderDto> GetTutorialHeaderDtoByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
