using Application.Common.Models;

namespace Application.Common.Interfaces.Services.FileUpLoad
{
    public interface IFileUploadService
    {
        Task<string> UploadAsync(FileUploadRequest request, CancellationToken cancellationToken = default);
    }
}
