using Application.Common.Abstractions.Security;
using Domain.Consts;
using Domain.Models.AppSettings;

namespace Infrastructure.Persistence.Seeds
{
    public static class AppSettingSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, IEncryptionService encryptionService)
        {
            var group = AppSettingConstants.Groups.GeminiAI;
            var key = AppSettingConstants.Keys.GeminiFlashApiKey;

            var isExist = await context.AppSettings
                .AnyAsync(x => x.Group == group && x.Key == key);

            if (!isExist)
            {
                string rawApiKey = "AIzaSy_DUMMY_GEMINI_KEY_2026";
                string encryptedValue = encryptionService.Encrypt(rawApiKey);

                var geminiSetting = new AppSetting
                {
                    Id = Guid.NewGuid(),
                    Group = group,
                    Key = key,
                    Value = encryptedValue,
                    Description = "API Key for Google Gemini 1.5 Flash Model",
                    IsActive = true
                };

                await context.AppSettings.AddAsync(geminiSetting);
                await context.SaveChangesAsync();
            }
        }
    }
}