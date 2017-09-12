using nEkis.Automation.Core.Framework.Objects;

namespace nEkis.Automation.Core.Settings
{
    /// <summary>
    /// Settings of log, report and screenshots
    /// </summary>
    public class LogSettings
    {
        /// <summary>
        /// Log config settings
        /// </summary>
        public static LogFile Log { get { return new LogFile(LogFile.LogFileType.Log); } }
        /// <summary>
        /// Report config settings
        /// </summary>
        public static LogFile Report { get { return new LogFile(LogFile.LogFileType.Report); } }
        /// <summary>
        /// Screenshot config settings
        /// </summary>
        public static LogFile ScreenShot { get { return new LogFile(LogFile.LogFileType.ScreenShot); } }
    }
}
