using nEkis.Automation.Core.Utilities.ScreenCaptureUtilities;
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
            Driver,
            DriverScreen,
            PrimaryScreen,
            Rectangle,
            Window
        }

        private static string FullPath
        {
            get
            {
                return TestEnvironment.TestPath + string.Format(@ConfigurationManager.AppSettings["screencapturedirectory"],
                    DateTime.Now.ToString(TestEnvironment.DateFormat));
            }
        }

        private static string VideoName
        {
            get
            {
                return string.Format(@ConfigurationManager.AppSettings["screencapturename"],
                    DateTime.Now.ToString(TestEnvironment.DateTimeFormat),
                    TestEnvironment.TestName);
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
        public static Rectangle Rect { get; set; }
        /// <summary>
        /// Window to capture
        /// </summary>
        public static Window Win { get; set; } = null;

        /// <summary>
        /// Initializes ScreenCapture parameters
        /// </summary>
        /// <param name="framerate">Framerate of the video</param>
        /// <param name="screenZoom">Zoom of display in percents, used for recording in Driver mode</param>
        public ScreenCapture(int framerate = 10, double screenZoom = 1.0)
        {
            CaptureArea = EnumHelper.Parse<Area>(ConfigurationManager.AppSettings["screencapturerectangle"]);

            if (CaptureArea == Area.Rectangle || CaptureArea == Area.Window)
            {
                Log.WriteLine("Screen capture area '{0}' selected, for this use specific constructor", CaptureArea);
                CaptureArea = Area.PrimaryScreen;
            }

            Log.WriteLine("Screen capture area '{0}' selected", CaptureArea);

            Init(framerate, screenZoom);
        }



        /// <summary>
        /// Initializes ScreenCapture parameters for specific window
        /// </summary>
        /// <param name="window">Window to be captured</param>
        /// <param name="framerate">Framerate of video</param>
        /// <param name="screenZoom">Zoom of screen set in windows</param>
        public ScreenCapture(Window window, int framerate = 10, double screenZoom = 1)
        {
            CaptureArea = Area.Window;

            Log.WriteLine("Screen capture area 'Window' forced, provided object {0}", window.ToString());

            Win = window;
            Init(framerate, screenZoom);
        }

        /// <summary>
        /// Initializes ScreenCapture parameters for specific rectangle
        /// </summary>
        /// <param name="rectangle">Rectangle to be captured</param>
        /// <param name="framerate">Framerate of video</param>
        /// <param name="screenZoom">Zoom of screen set in windows</param>
        public ScreenCapture(Rectangle rectangle, int framerate = 10, double screenZoom = 1)
        {
            CaptureArea = Area.Rectangle;

            Log.WriteLine("Screen capture area 'Rectangle' forced, provided object {0}", rectangle.ToString());

            Rect = rectangle;
            Init(framerate, screenZoom);
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

        private void CreateDirectory()
        {
            if (!Directory.Exists(FullPath))
                Directory.CreateDirectory(FullPath);
        }

        private void Init(int framerate, double screenZoom)
        {
            CreateDirectory();

            Writer = new AviWriter(FullName, AviCodec.MotionJpeg);

            switch (CaptureArea)
            {
                case Area.Driver:
                    Rect = WindowHelpers.GetDriverRectangle(screenZoom);
                    RgnProvider = new RegionProvider(Rect, null);
                    Rec = new Recorder(Writer, RgnProvider, framerate);
                    break;
                case Area.DriverScreen:
                    Rect = WindowHelpers.GetDriverScreenRectangle(screenZoom);
                    RgnProvider = new RegionProvider(Rect, null);
                    Rec = new Recorder(Writer, RgnProvider, framerate);
                    break;
                case Area.PrimaryScreen:
                    Rect = WindowHelpers.GetPrimaryScreenRectangle(screenZoom);
                    RgnProvider = new RegionProvider(Rect, null);
                    Rec = new Recorder(Writer, RgnProvider, framerate);
                    break;
                case Area.Rectangle:
                    RgnProvider = new RegionProvider(Rect, null);
                    Rec = new Recorder(Writer, RgnProvider, framerate);
                    break;
                case Area.Window:
                    WinProvider = new WindowProvider(Win);
                    Rec = new Recorder(Writer, WinProvider, framerate);
                    break;
                default:
                    Rect = WindowHelpers.GetPrimaryScreenRectangle(screenZoom);
                    RgnProvider = new RegionProvider(Rect, null);
                    Rec = new Recorder(Writer, RgnProvider, framerate);
                    break;
            }
        }
    }
}
