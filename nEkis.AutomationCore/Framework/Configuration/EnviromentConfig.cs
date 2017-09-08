using System.Configuration;

namespace nEkis.Automation.Core.Framework.Configuration
{
    class EnvironmentConfig : ConfigurationSection
    {
        private static EnvironmentConfig _environmentConfig = (EnvironmentConfig)ConfigurationManager.GetSection("environmentSettings");
        public static EnvironmentConfig Settings { get { return _environmentConfig; } }

        [ConfigurationProperty("environments")]
        public EnvironmentsElementCollection Environments { get { return (EnvironmentsElementCollection)base["environments"]; } }

        [ConfigurationProperty("authenticate")]
        public AuthenticateElement Authenticate { get { return (AuthenticateElement)base["authenticate"]; } }
    }


    class EnvironmentsElementCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("environment")]
        public EnvironmentElement Environment { get { return (EnvironmentElement)base["environment"]; } }

        protected override ConfigurationElement CreateNewElement()
        {
            return new EnvironmentElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as EnvironmentElement).Key;
        }

        public EnvironmentElement this[int index]
        {
            get { return (EnvironmentElement)base.BaseGet(index); }
            set
            {
                if (base.BaseGet(index) != null)
                    base.BaseRemoveAt(index);

                base.BaseAdd(index, value);
            }
        }

        new public EnvironmentElement this[string key]
        {
            get { return (EnvironmentElement)base.BaseGet(key); }
        }
    }

    class EnvironmentElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key { get { return (string)base["key"]; } }

        [ConfigurationProperty("url", IsRequired = true)]
        public string Url { get { return (string)base["url"]; } }

        [ConfigurationProperty("user", IsRequired = false)]
        public string UserName { get { return (string)base["user"]; } }

        [ConfigurationProperty("password", IsRequired = false)]
        public string Password { get { return (string)base["password"]; } }

        [ConfigurationProperty("authenticate", DefaultValue = "null", IsRequired = false)]
        [RegexStringValidator("(true|false|null)")]
        public string Authenticate { get { return (string)base["authenticate"]; } }
    }

    class AuthenticateElement : ConfigurationElement
    {
        [ConfigurationProperty("value", DefaultValue = "false", IsRequired = false)]
        [RegexStringValidator("(true|false)")]
        public string Value { get { return (string)base["value"]; } }
    }
}
