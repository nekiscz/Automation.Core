using nEkis.Automation.Core.Framework.Configuration;
using nEkis.Automation.Core.Settings;
using System.Linq;

namespace nEkis.Automation.Core.Framework.Objects
{
    /// <summary>
    /// Environment representation
    /// </summary>
    public class BaseUrl
    {
        private string[] _prefixes = { "http://", "https://" };
        private EnvironmentElement _environment { get; set; }

        /// <summary>
        /// Creates object from configuration file based on key value
        /// </summary>
        /// <param name="name"></param>
        public BaseUrl(string name)
        {
            this._environment = EnvironmentConfig.Settings.Environments[name];
        }

        internal BaseUrl(EnvironmentElement environment)
        {
            this._environment = environment;
        }

        /// <summary>
        /// Name of config record, key attribute
        /// </summary>
        public string Name { get { return this._environment.Key; } }
        /// <summary>
        /// Environment domain
        /// </summary>
        public string Domain
        {
            get
            {
                var url = this._environment.Url;
                foreach (var prefix in _prefixes)
                {
                    url = url.Replace(prefix, "");
                }
                if (url.Contains('/'))
                    url = url.Remove(url.IndexOf('/'));
                return url;
            }
        }
        /// <summary>
        /// Environment protocol, if no protocol is specified 'http://' protocol is set
        /// </summary>
        public string Protocol
        {
            get
            {
                if (this._environment.Url.StartsWith(_prefixes[1]))
                    return _prefixes[1];
                else return _prefixes[0];
            }
        }
        /// <summary>
        /// User name for authentication
        /// </summary>
        public string User { get { return this._environment.UserName; } }
        /// <summary>
        /// Password for authentication
        /// </summary>
        public string Password { get { return this._environment.Password; } }
        /// <summary>
        /// If request should be authenticated
        /// </summary>
        public bool Authenticate
        {
            get
            {
                var auth = this._environment.Authenticate;
                if (auth == "null")
                    return EnvironmentSettings.Authenticate;

                if (auth == "true")
                    return true;
                else return false;
            }
        }
        /// <summary>
        /// Url of environment
        /// </summary>
        public string Url { get
            {
                if (this.Authenticate)
                    return $"{this.Protocol}{this.User}:{this.Password}@{this.Domain}";
                else return $"{this.Protocol}{this.Domain}";
            } }
    }
}
