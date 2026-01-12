namespace Domain.Models.AppSettings
{
    public class AppSetting
    {
        public Guid Id { get; set; }
        public string Group { get; set; } = default!;
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}