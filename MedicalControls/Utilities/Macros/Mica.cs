using EPIC.MedicalControls.Native;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace EPIC.MedicalControls.Utilities.Macros
{
    public static class Mica
    {
        public static BitmapSource CaptureRegion(int x, int y, int width, int height)
        {
            IntPtr hDesk = User32.GetDesktopWindow();
            IntPtr hSrce = User32.GetWindowDC(hDesk);
            IntPtr hDest = Gdi32.CreateCompatibleDC(hSrce);
            IntPtr hBmp = Gdi32.CreateCompatibleBitmap(hSrce, width, height);
            IntPtr hOld = Gdi32.SelectObject(hDest, hBmp);

            // 0x00CC0020 is SRCCOPY
            Gdi32.BitBlt(hDest, 0, 0, width, height, hSrce, x, y, 0x00CC0020);

            // Convert HBITMAP to WPF BitmapSource
            BitmapSource bps = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBmp, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            // Cleanup unmanaged junk
            Gdi32.SelectObject(hDest, hOld);
            Gdi32.DeleteObject(hBmp);
            Gdi32.DeleteDC(hDest);
            User32.ReleaseDC(hDesk, hSrce);

            return bps;
        }


        public static void SubscribeFuzz(Window Window, FrameworkElement WelcomeLeft, FrameworkElement BackgroundLayer)
        {
            Window.LocationChanged += (s, e) => UpdateFuzz(WelcomeLeft, BackgroundLayer);
            Window.SizeChanged += (s, e) => UpdateFuzz(WelcomeLeft, BackgroundLayer);

            // 2. Optional: Small heartbeat for background changes (5 FPS instead of 60)
            var timer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            timer.Tick += (s, e) => UpdateFuzz(WelcomeLeft, BackgroundLayer);
            timer.Start();

            // Only run the timer if the window is in the foreground
            Window.Activated += (s, e) => timer.Start();
            Window.Deactivated += (s, e) => timer.Stop();

            _subscriptions.Add(WelcomeLeft, new Tuple<Window, FrameworkElement, FrameworkElement, DispatcherTimer, DateTime, ImageBrush?>(Window, WelcomeLeft, BackgroundLayer, timer, DateTime.Now, null));
        }

        private static Dictionary<FrameworkElement, Tuple<Window, FrameworkElement, FrameworkElement, DispatcherTimer, DateTime, ImageBrush?>> _subscriptions = new Dictionary<FrameworkElement, Tuple<Window, FrameworkElement, FrameworkElement, DispatcherTimer, DateTime, ImageBrush?>>();

        public static void UpdateFuzz(FrameworkElement WelcomeLeft, FrameworkElement BackgroundLayer)
        {
            // Wrap in a check to prevent crashing during window minimize
            if (Application.Current.MainWindow.WindowState == WindowState.Minimized || WelcomeLeft.ActualWidth <= 0) return;
            var source = PresentationSource.FromVisual(WelcomeLeft);
            if (source == null) return;

            var property = BackgroundLayer.GetType().GetProperty("Background");
            if (property == null)
            {
                return;
            }

            /*
            // this doesn't make any sense because every element has it's own surface, if i implemented my solution from the Appifyer
            // i could optimize by only drawing the windows that overlap under my window, and maximizing capture area to cover a
            // rectangle of all requesting controls in one go, then slicing up from that image only the one i need for backgrounds
            var previous = _subscriptions[WelcomeLeft].Item5;
            var previousBrush = 
            if (previous + 200 < DateTime.Now)
            {
                property.SetValue(BackgroundLayer, brush);
                return; // too soon
            }
            */

            var hwnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            int cloak = DwmApi.DWM_CLOAKED_APP;
            int uncloak = DwmApi.DWM_UNCLOAK;

            // 1. Cloak the window (Invisible to BitBlt)
            //DwmApi.DwmSetWindowAttribute(hwnd, DwmApi.DWMWA_CLOAK, ref cloak, sizeof(int));
            User32.SetWindowDisplayAffinity(hwnd, User32.WDA_EXCLUDEFROMCAPTURE);

            // Map your WPF element "WelcomeLeft" to screen coordinates
            Point screenPos = WelcomeLeft.PointToScreen(new Point(0, 0));

            if (source != null && source.CompositionTarget != null)
            {
                double dpiX = source.CompositionTarget.TransformToDevice.M11; // e.g., 1.5 for 150%
                double dpiY = source.CompositionTarget.TransformToDevice.M22;

                // 2. Scale the coordinates and sizes
                int pxX = (int)(screenPos.X * dpiX);
                int pxY = (int)(screenPos.Y * dpiY);
                int pxWidth = (int)(WelcomeLeft.RenderSize.Width * dpiX);
                int pxHeight = (int)(WelcomeLeft.RenderSize.Height * dpiY);

                // 3. Capture using physical pixel values
                var background = Mica.CaptureRegion(pxX, pxY, pxWidth, pxHeight);
                var brush = new System.Windows.Media.ImageBrush(background);
                brush.Stretch = System.Windows.Media.Stretch.Uniform;
                brush.AlignmentX = System.Windows.Media.AlignmentX.Center;
                brush.AlignmentY = System.Windows.Media.AlignmentY.Center;
                property.SetValue(BackgroundLayer, brush);
            }

            // unlock from cloaking
            //DwmApi.DwmSetWindowAttribute(hwnd, DwmApi.DWMWA_CLOAK, ref uncloak, sizeof(int));
            User32.SetWindowDisplayAffinity(hwnd, User32.WDA_NONE);

            // Apply your custom ShaderEffect
            //BackgroundLayer.Effect = new BlurEffect() { Radius = 20, KernelType = KernelType.Box, RenderingBias = RenderingBias.Quality }; //new MedicalControls.Shaders.InvertColorEffect();
        }
    }

}
