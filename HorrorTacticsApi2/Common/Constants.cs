using System.Reflection;

namespace HorrorTacticsApi2
{
    public static class Constants
    {
        public const string HUB_PATH = "/game-hub";
        public const string FILE_APPLY_MIGRATIONS = "apply-migrations.txt";
        public const string ENV_PREFIX = "HT_";

        public const string APPSETTINGS_GENERAL_KEY = "General";
        public const string APPSETTINGS_JWTGENERATOR_KEY = "JwtGenerator";
        public const string CONNECTION_STRING_MAIN_KEY = "Main";

        public const string MULTIPART_FORMDATA = "multipart/form-data";
        public const string FORMDATA = "form-data";

        public const string APITESTING_ENV_NAME = "ApiTesting_local";
        public const string APITESTING_DB_REPLACE_VALUE_Key = "ApiTesting_Replace_Db_Value";

        public const string SecuredApiPath = "secured";

        public const string FileImageApiPathWithVars = "images/{filename}";
        public const string FileAudioApiPathWithVars = "audios/{filename}";

        const string DefaultFilePath = "DefaultStoryFiles";
        public const string DefaultDataFile = "data.json";

        public const string AdminUsername = "ht-master";
        public const long AdminUserId = 1;

        public const int PasswordSaltSize = 128;
        public const int PasswordSize = 256;
        public const int PasswordIterations = 10000;

        public const string JwtRoleKey = "role";

        // TODO: this should be in appsettings?
        public const string InitLogFolder = "logs";

        public const int AssetFileCacheInSeconds = 1209600;

        public static string GetFileImageApiPath(string filename)
        {
            return FileImageApiPathWithVars.Replace("{filename}", filename);
        }

        public static string GetFileAudioApiPath(string filename)
        {
            return FileAudioApiPathWithVars.Replace("{filename}", filename);
        }

        public static string GetDefaultFilePath()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string? path = Path.GetDirectoryName(asm.Location);

            if (path == default)
                throw new InvalidOperationException("path is null");

            return Path.Combine(path, DefaultFilePath);
        }
    }
}
