namespace Application.Features.Appsettings.v1.DTOs
{
    public class UpdateAppSettingDto
    {
        public string Group { get; set; } = default!;
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
