using nEkis.Automation.Core.Framework.Configuration;
using nEkis.Automation.Core.Framework.Exceptions;
using nEkis.Automation.Core.Framework.Objects;

namespace nEkis.Automation.Core.Settings
{
    /// <summary>
    /// Settings for test environment
    /// </summary>
    public class EnvironmentSettings
    {
        /// <summary>
        /// Gets value if user should be authenticated in base url
        /// </summary>
        public static bool Authenticate
        {
            get
            {
                if (EnvironmentConfig.Settings.Authenticate.Value == "true")
                    return true;
                else return false;
            }
        }

        /// <summary>
        /// Gets environment configuration and returnes it as object
        /// </summary>
        /// <param name="key">Key attribute from config</param>
        /// <returns>Base url object</returns>
        public static BaseUrl GetUrl(string key)
        {
            try
            {
                var environment = EnvironmentConfig.Settings.Environments[key];
                if (environment == null)
                    throw new UnknownConfiguration($"Environment with this key '{key}' doesn't exist, or isn't saved correctly in configuration file.");

                return new BaseUrl(environment);
            }
            catch (System.Exception e)
            {
                throw new UnknownConfiguration($"Environment with this key '{key}' doesn't exist, or isn't saved correctly in configuration file.", e);
            }
        }
    }
}
