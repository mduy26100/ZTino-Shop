using Application.Common.Contracts;

namespace Application.Common.Abstractions.ExternalServices
{
    public interface IFileUploadService
    {
        Task<string> UploadAsync(FileUploadRequest request, CancellationToken cancellationToken = default);
    }
}
