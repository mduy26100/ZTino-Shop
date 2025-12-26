namespace Application.Common.Models.Requests
{
    public class FileUploadRequest
    {
        public Stream Content { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public string ContentType { get; set; } = default!;
    }
}
