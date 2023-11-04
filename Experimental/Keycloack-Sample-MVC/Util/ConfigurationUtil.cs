using Microsoft.Extensions.Configuration;

namespace Keycloack_Sample_MVC.Util
{
    public static class ConfigurationUtil
    {
        private static IConfiguration Configuration;

        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration GetConfigurations() => Configuration;

        public static IConfigurationSection GetSection(string section) => Configuration.GetSection(section);

        // Value must not be an object or array otherwise use GetSection function
        public static T GetValue<T>(string path) => Configuration.GetValue<T>(path);
    }
}
