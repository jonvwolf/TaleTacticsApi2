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

        public static string GetFileImageApiPath(string filename)
        {
            return FileImageApiPathWithVars.Replace("{filename}", filename);
        }

        public static string GetFileAudioApiPath(string filename)
        {
            return FileAudioApiPathWithVars.Replace("{filename}", filename);
        }
    }
}
