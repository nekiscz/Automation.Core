using Screna;
using Screna.Avi;
using Screna.FFMpeg;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                return Environment.TestPath + string.Format(@ConfigurationManager.AppSettings["screencapturename"],
                    DateTime.Now.ToString(Environment.DateFormat),
                    Environment.TestName);
            }
        }

        public static string FullName { get { return FullPath + FullName; }}
        private static Area CaptureArea
        {
            get
            {
                return EnumHelper.Parse<Area>(ConfigurationManager.AppSettings["screencapturerectangle"]);
            }
        }
        /// <summary>
        /// Instance of WindowProvider
        /// </summary>
        public static WindowProvider WinwProvider { get; set; }
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
        /// Rectangle to capture
        /// </summary>
        public static Rectangle Rect { get; set; } = new Rectangle(0, 0, 1920, 1080);
        /// <summary>
        /// Frame rate of video
        /// </summary>
        public static int FrameRate { get; set; } = 30;

        /// <summary>
        /// Capture for desktop in 30fps
        /// </summary>
        static ScreenCapture()
        {
            if (!Directory.Exists(FullPath))
                Directory.CreateDirectory(FullPath);

            WinwProvider = new WindowProvider(Window.DesktopWindow);
            RgnProvider = new RegionProvider(Rect, null);

            Writer = new AviWriter("output.avi", AviCodec.MotionJpeg);

            switch (CaptureArea) 
            {
                case Area.All:
                    Rec = new Recorder(Writer, WinwProvider, FrameRate);
                    break;
                case Area.Rectangle:
                    Rec = new Recorder(Writer, RgnProvider, FrameRate);
                    break;
            }
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
    }
}
