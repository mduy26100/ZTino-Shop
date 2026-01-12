using Application.Common.Abstractions.Security;
using Domain.Consts;
using Domain.Models.AppSettings;

namespace Infrastructure.Persistence.Seeds
{
    public static class AppSettingSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, IEncryptionService encryptionService)
        {
            await SeedGeminiKeyAsync(context, encryptionService);

            await SeedSystemActiveProviderAsync(context, encryptionService);

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedGeminiKeyAsync(ApplicationDbContext context, IEncryptionService encryptionService)
        {
            var group = AppSettingConstants.Gemini.Group;
            var key = AppSettingConstants.Gemini.Keys.FlashApiKey;

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
            }
        }

        private static async Task SeedSystemActiveProviderAsync(ApplicationDbContext context, IEncryptionService encryptionService)
        {
            var group = AppSettingConstants.System.Group;
            var key = AppSettingConstants.System.Keys.ActiveAIProvider;

            var isExist = await context.AppSettings
                .AnyAsync(x => x.Group == group && x.Key == key);

            if (!isExist)
            {
                string defaultValue = AppSettingConstants.AIProviders.Gemini;

                string encryptedValue = encryptionService.Encrypt(defaultValue);

                var systemSetting = new AppSetting
                {
                    Id = Guid.NewGuid(),
                    Group = group,
                    Key = key,
                    Value = encryptedValue,
                    Description = "The AI ​​Model configuration is currently enabled system-wide.",
                    IsActive = true
                };

                await context.AppSettings.AddAsync(systemSetting);
            }
        }
    }
}