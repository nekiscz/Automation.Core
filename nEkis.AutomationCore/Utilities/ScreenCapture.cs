using Screna;
using Screna.Avi;
using Screna.FFMpeg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nEkis.Automation.Core.Utilities
{
    /// <summary>
    /// ScreenCapture object
    /// </summary>
    public class ScreenCapture
    {
        enum Area
        {
            All,
            Driver,
            Rectangle
        }

        private static string FullPath
        {
            get
            {
                return Environment.TestPath + string.Format(@ConfigurationManager.AppSettings["screencapturedirectory"],
                    DateTime.Now.ToString(Environment.DateFormat));
            }
        }

        private static string VideoName
        {
            get
            {
                return string.Format(@ConfigurationManager.AppSettings["screencapturename"],
                    DateTime.Now.ToString(Environment.DateTimeFormat),
                    Environment.TestName);
            }
        }

        private static string FullName { get { return FullPath + VideoName; } }
        private static Area CaptureArea { get; set; }

        /// <summary>
        /// Instance of WindowProvider
        /// </summary>
        public static WindowProvider WinProvider { get; set; }
        /// <summary>
        /// Instance of RegionProvider
        /// </summary>
        public static RegionProvider RgnProvider { get; set; }
        /// <summary>
        /// Instance of AviWriter
        /// </summary>
        public static AviWriter Writer { get; set; }
        /// <summary>
        /// Instance of Recorder
        /// </summary>
        public static Recorder Rec { get; set; }
        /// <summary>
        /// Rectangle to capture by default set to rectangle of 1920x1080px without offset
        /// </summary>
        public static Rectangle Rect { get; set; } = new Rectangle(0, 0, 1920, 1080);
        /// <summary>
        /// Window to capture
        /// </summary>
        public static Window Win { get; set; } = null;

        /// <summary>
        /// Initializes ScreenCapture parameters
        /// </summary>
        /// <param name="framerate">Framerate of the video</param>
        public ScreenCapture(int framerate = 10)
        {
            CaptureArea = EnumHelper.Parse<Area>(ConfigurationManager.AppSettings["screencapturerectangle"]);

            Init(framerate);
        }

        /// <summary>
        /// Initializes ScreenCapture parameters for specific window
        /// </summary>
        /// <param name="window">Window to be captured</param>
        /// <param name="framerate">Framerate of video</param>
        public ScreenCapture(Window window, int framerate = 10)
        {
            CaptureArea = Area.Driver;

            Win = window;
            Init(framerate);
        }

        /// <summary>
        /// Initializes ScreenCapture parameters for specific rectangle
        /// </summary>
        /// <param name="rectangle">Rectangle to be captured</param>
        /// <param name="framerate">Framerate of video</param>
        public ScreenCapture(Rectangle rectangle, int framerate = 10)
        {
            CaptureArea = Area.Rectangle;

            Rect = rectangle;
            Init(framerate);
        }

        /// <summary>
        /// Starts recording
        /// </summary>
        public static void Start()
        {
            Rec.Start();
            Log.WriteLine("Video recording started");
        }

        /// <summary>
        /// Pause recording
        /// </summary>
        public static void Pause()
        {
            Rec.Pause();
            Log.WriteLine("Video recording paused");
        }


        /// <summary>
        /// Stops recording and performs Cleanup
        /// </summary>
        public static void Stop()
        {
            Rec.Stop();
            Log.WriteLine("Video recording stoped");
        }

        private IntPtr GetDriverWindow()
        {
            var process = Process.GetProcesses().FirstOrDefault(x => x.ProcessName.ToLower().Contains("driver"));
            return process.Handle;
        }

        private void CreateDirectory()
        {
            if (!Directory.Exists(FullPath))
                Directory.CreateDirectory(FullPath);
        }

        private void Init(int framerate)
        {
            CreateDirectory();

            Writer = new AviWriter(FullName, AviCodec.MotionJpeg);

            switch (CaptureArea)
            {
                case Area.All:
                    WinProvider = new WindowProvider(Window.DesktopWindow);
                    Rec = new Recorder(Writer, WinProvider, framerate);
                    break;
                case Area.Driver:

                    try
                    {
                        Win.Equals(null);
                    }
                    catch (NullReferenceException)
                    {
                        Win = new Window(GetDriverWindow());
                    }

                    WinProvider = new WindowProvider(Win).;
                    Rec = new Recorder(Writer, WinProvider, framerate);
                    break;
                case Area.Rectangle:
                    RgnProvider = new RegionProvider(Rect, null);
                    Rec = new Recorder(Writer, RgnProvider, framerate);
                    break;
                default:
                    WinProvider = new WindowProvider(Window.DesktopWindow);
                    Rec = new Recorder(Writer, WinProvider, framerate);
                    break;
            }
        }

    }
}
