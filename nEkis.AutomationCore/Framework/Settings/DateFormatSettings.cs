using nEkis.Automation.Core.Framework.Configuration;

namespace nEkis.Automation.Core.Settings
{
    /// <summary>
    /// Settings of date formats 
    /// </summary>
    public class DateFormatSettings
    {
        /// <summary>
        /// Short date format for ordering and file naming mainly
        /// </summary>
        public static string ShortDate { get { return LogConfig.Settings.DateFormats.ShortDate.Value; } }
        /// <summary>
        /// Short date and time format for ordering and file naming mainly
        /// </summary>
        public static string ShortDateTime { get { return LogConfig.Settings.DateFormats.ShortDateTime.Value; } }
        /// <summary>
        /// Readable by human eyes date format
        /// </summary>
        public static string ReadableDate { get { return LogConfig.Settings.DateFormats.ReadableDate.Value; } }
        /// <summary>
        /// Readable by human eyes date and time format
        /// </summary>
        public static string ReadableDateTime { get { return LogConfig.Settings.DateFormats.ReadableDateTime.Value; } }
    }
}
