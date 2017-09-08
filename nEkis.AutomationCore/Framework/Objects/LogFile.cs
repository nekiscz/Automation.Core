using nEkis.Automation.Core.Framework.Configuration;

namespace nEkis.Automation.Core.Framework.Objects
{

    /// <summary>
    /// Log file representation
    /// </summary>
    public class LogFile
    {
        private LogElement _file;
        /// <summary>
        /// Different types of logfiles
        /// </summary>
        public enum LogFileType
        {
            /// <summary>
            /// HTML report from nunit
            /// </summary>
            Report,
            /// <summary>
            /// Text log
            /// </summary>
            Log,
            /// <summary>
            /// Viewport screenshots
            /// </summary>
            ScreenShot
        }

        /// <summary>
        /// Constructs file object baed on type
        /// </summary>
        /// <param name="type">Type of the file</param>
        public LogFile(LogFileType type)
        {
            switch (type)
            {
                case LogFileType.Report:
                    _file = LogConfig.Settings.Report;
                    break;
                case LogFileType.Log:
                    _file = LogConfig.Settings.Log;
                    break;
                case LogFileType.ScreenShot:
                    _file = LogConfig.Settings.ScreenShot;
                    break;
            }
        }
        /// <summary>
        /// Path to directory where file should be created
        /// </summary>
        public string Path { get { return _file.Path; } }
        /// <summary>
        /// Name of log file 
        /// </summary>
        public string Name { get { return _file.Name; } }
        /// <summary>
        /// If files should be created 
        /// </summary>
        public bool Create
        {
            get
            {
                if (_file.Create == "true")
                    return true;
                else return false;
            }
        }
    }
}
