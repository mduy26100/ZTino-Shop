namespace Domain.Consts
{
    public static class AppSettingConstants
    {
        public static class AIProviders
        {
            public const string Gemini = "Gemini";
            public const string GPT = "GPT";
        }

        public static class System
        {
            public const string Group = "System";

            public static class Keys
            {
                public const string ActiveAIProvider = "ActiveAIProvider";
            }
        }

        public static class Gemini
        {
            public const string Group = "GeminiAI";
            public const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-3-flash-preview:generateContent";
            public static class Keys
            {
                public const string FlashApiKey = "FlashApiKey";
            }
        }
    }
}