using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nEkis.Automation.Core.Utilities.ScreenCaptureUtilities
{
    class WindowHelpers
    {
        public static Rectangle GetWindowRectangle(double zoom)
        {
            Rectangle rect = new Rectangle();
            GetWindowRect(GetDriverWindowHandle(), ref rect);

            return GetRectangleWithZoom(rect, zoom);
        }

        public static Rectangle GetPrimaryScreenRectangle(double zoom)
        {
            var screen = Screen.PrimaryScreen;
            Rectangle rect = GetRectangleWithZoom(screen.Bounds, zoom);

            Log.WriteLine("Recording rectangle set to {0}", rect.ToString());

            return rect;
        }

        public static Rectangle GetDriverScreenRectangle(double zoom)
        {
            return Screen.FromRectangle(GetDriverRectangle(zoom)).Bounds;
        }

        public static Rectangle GetDriverRectangle(double zoom)
        {
            var rect = new Rectangle(Browser.Driver.Manage().Window.Position, Browser.Driver.Manage().Window.Size);
            return GetRectangleWithZoom(rect, zoom);
        }


        public static IntPtr GetDriverWindowHandle()
        {
            var process = Process.GetProcesses().FirstOrDefault(x => x.ProcessName.ToLower().Contains("driver"));
            return process.Handle;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

        private static Rectangle GetRectangleWithZoom(Rectangle rect, double zoom)
        {
            Rectangle newRect = new Rectangle
            {
                Height = Multiply(rect.Height, zoom),
                Width = Multiply(rect.Width, zoom),
                X = rect.X,
                Y = rect.Y,
                Size = new Size
                {
                    Height = Multiply(rect.Size.Height, zoom),
                    Width = Multiply(rect.Size.Width, zoom)
                },
                Location = new Point
                {
                    X = rect.X,
                    Y = rect.Y
                }
            };

            return newRect;
        }

        private static int Multiply(int px, double zoom)
        {
            var d = Convert.ToDouble(px);
            return Convert.ToInt32(d * zoom);
        }

    }
}
