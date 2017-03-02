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
        public static Rectangle GetWindowRectangle()
        {
            Rectangle rect = new Rectangle();
            GetWindowRect(GetDriverWindowHandle(), ref rect);

            return rect;
        }

        public static Rectangle GetPrimaryScreenRectangle()
        {
            var screen = Screen.PrimaryScreen;
            Rectangle rect = screen.Bounds;

            Log.WriteLine("Recording rectangle set to {0}", rect.ToString());

            return rect;
        }

        public static Rectangle GetDriverScreenRectangle()
        {
            return Screen.FromRectangle(GetDriverRectangle()).Bounds;
        }

        public static Rectangle GetDriverRectangle()
        {
            var rect = new Rectangle(Browser.Driver.Manage().Window.Position, Browser.Driver.Manage().Window.Size);
            return rect;
        }


        public static IntPtr GetDriverWindowHandle()
        {
            var process = Process.GetProcesses().FirstOrDefault(x => x.ProcessName.ToLower().Contains("driver"));
            return process.Handle;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);
    }
}
