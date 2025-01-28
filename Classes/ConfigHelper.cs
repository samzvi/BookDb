using Microsoft.Extensions.Configuration;
using System.IO;

namespace BookDb
{
    public static class ConfigHelper
    {
        private static IConfigurationRoot _configuration;

        static ConfigHelper()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public static string GetConnectionString()
        {
            string baseConnectionString = _configuration.GetConnectionString("DefaultConnection");
            string dbPath = GetDbPath();

            return baseConnectionString.Replace("{DbPath}", dbPath);
        }

        public static string GetDbPath()
        {
            string relativePath = _configuration["DatabaseSettings:DbPath"];
            return Path.GetFullPath(relativePath, AppDomain.CurrentDomain.BaseDirectory);
        }

        public static string GetDbInitPath()
        {
            return _configuration["DatabaseSettings:DbInitPath"];
        }
    }
}
