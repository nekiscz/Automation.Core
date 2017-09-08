using System.Configuration;

namespace nEkis.Automation.Core.Framework.Configuration
{
    class LogConfig : ConfigurationSection
    {
        private static LogConfig _logConfig = (LogConfig)ConfigurationManager.GetSection("logSettings");
        public static LogConfig Settings { get { return _logConfig; } }

        [ConfigurationProperty("reportDirectory")]
        public LogElement Report { get { return (LogElement)base["reportDirectory"]; } }

        [ConfigurationProperty("logDirectory")]
        public LogElement Log { get { return (LogElement)base["logDirectory"]; } }

        [ConfigurationProperty("screenShotDirectory")]
        public LogElement ScreenShot { get { return (LogElement)base["screenShotDirectory"]; } }

        [ConfigurationProperty("dateFormats")]
        public DateElement DateFormats { get { return (DateElement)base["dateFormats"]; } }
    }

    class LogElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path { get { return (string)base["path"]; } }

        [ConfigurationProperty("name")]
        public string Name { get { return (string)base["name"]; } }

        [ConfigurationProperty("create", DefaultValue = "true")]
        [RegexStringValidator("(true|false)")]
        public string Create { get { return (string)base["create"]; } }

    }

    class DateElement : ConfigurationElement
    {
        [ConfigurationProperty("shortDate")]
        public ShortDateElement ShortDate { get { return (ShortDateElement)base["shortDate"]; } }

        [ConfigurationProperty("shortDateTime")]
        public ShortDateTimeElement ShortDateTime { get { return (ShortDateTimeElement)base["shortDateTime"]; } }

        [ConfigurationProperty("readableDate")]
        public ReadableDateElement ReadableDate { get { return (ReadableDateElement)base["readableDate"]; } }

        [ConfigurationProperty("readableDateTime")]
        public ReadableDateTimeElement ReadableDateTime { get { return (ReadableDateTimeElement)base["readableDateTime"]; } }

    }

    class ShortDateElement : ConfigurationElement
    {
        [ConfigurationProperty("value", DefaultValue = "yyyyMMdd")]
        public string Value { get { return (string)base["value"]; } }
    }

    class ShortDateTimeElement : ConfigurationElement
    {
        [ConfigurationProperty("value", DefaultValue = "yyyyMMdd-HHmmss")]
        public string Value { get { return (string)base["value"]; } }
    }

    class ReadableDateElement : ConfigurationElement
    {
        [ConfigurationProperty("value", DefaultValue = "dd. MM. yyyy")]
        public string Value { get { return (string)base["value"]; } }
    }

    class ReadableDateTimeElement : ConfigurationElement
    {
        [ConfigurationProperty("value", DefaultValue = "dd. MM. yyyy - HH:mm:ss")]
        public string Value { get { return (string)base["value"]; } }
    }

}
