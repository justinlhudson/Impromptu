using System.Configuration;

namespace Impromptu
{
    public class Configuration
    {
        public static string SettingValue(string name)
        {
            var value = string.Empty;
            if(ConfigurationManager.AppSettings[name] != null)
                value = ConfigurationManager.AppSettings[name];
            return value;
        }
    }
}
