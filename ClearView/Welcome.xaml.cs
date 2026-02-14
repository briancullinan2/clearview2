using EPIC.ClearView.Macros;
using EPIC.ClearView.Native;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Effects;

namespace EPIC.ClearView
{
    // Token: 0x0200006C RID: 108
    public partial class Welcome : Page
    {
        // Token: 0x0600033F RID: 831 RVA: 0x0001AE66 File Offset: 0x00019066
        public Welcome()
        {
            this.InitializeComponent();

            /*
            this.SystemBackdrop = new Microsoft.UI.Xaml.MicaBackdrop();

            // 1. Get the HWND of your WPF Window
            var hwnd = new WindowInteropHelper(this).Handle;

            // 2. Create a Compositor (The engine that "sees" the desktop)
            var _compositor = new Windows.UI.Composition.Compositor();

            // 3. Create the Backdrop Brush (This is the "HBITMAP" pull you asked for)
            // This creates a brush that is essentially a live feed of the desktop behind your window
            Windows.UI.Composition.CompositionBackdropBrush desktopBrush = _compositor.CreateBackdropBrush();

            // Create the effect factory (Requires Win2D)
            var invertEffect = new InvertEffect
            {
                Source = new Windows.UI.Composition.CompositionEffectSourceParameter("SourcePixels")
            };

            var factory = _compositor.CreateEffectFactory(invertEffect);
            var effectBrush = factory.CreateBrush();

            // Point the shader to the "image" behind your app
            effectBrush.SetSourceParameter("SourcePixels", desktopBrush);

            // Apply this to a SpriteVisual and put it in your WPF window
            Windows.UI.Composition.SpriteVisual visual = _compositor.CreateSpriteVisual();
            visual.Brush = effectBrush;
            visual.Size = new System.Numerics.Vector2((float)this.ActualWidth, (float)this.ActualHeight);




            // Hook it into the WPF Visual Tree
            Windows.UI.Xaml.Hosting.ElementCompositionPreview.SetElementChildVisual(WelcomeLeft, visual);
            */


            // 1. Update when the window moves/resizes
            var Window = Application.Current.MainWindow;
            Window.LocationChanged += (s, e) => UpdateFuzz();
            Window.SizeChanged += (s, e) => UpdateFuzz();

            // 2. Optional: Small heartbeat for background changes (5 FPS instead of 60)
            var timer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            timer.Tick += (s, e) => UpdateFuzz();
            timer.Start();

            // Only run the timer if the window is in the foreground
            Window.Activated += (s, e) => timer.Start();
            Window.Deactivated += (s, e) => timer.Stop();

            // hide parent window from all capture?
            //User32.SetWindowDisplayAffinity(hwnd, User32.WDA_EXCLUDEFROMCAPTURE);


        }


        private void UpdateFuzz()
        {
            // Wrap in a check to prevent crashing during window minimize
            if (Application.Current.MainWindow.WindowState == WindowState.Minimized || WelcomeLeft.ActualWidth <= 0) return;
            var source = PresentationSource.FromVisual(WelcomeLeft);
            if (source == null) return;

            var hwnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            int cloak = DwmApi.DWM_CLOAKED_APP;
            int uncloak = DwmApi.DWM_UNCLOAK;

            // 1. Cloak the window (Invisible to BitBlt)
            //DwmApi.DwmSetWindowAttribute(hwnd, DwmApi.DWMWA_CLOAK, ref cloak, sizeof(int));
            User32.SetWindowDisplayAffinity(hwnd, User32.WDA_EXCLUDEFROMCAPTURE);

            // Map your WPF element "WelcomeLeft" to screen coordinates
            Point screenPos = WelcomeLeft.PointToScreen(new Point(0, 0));

            // Grab the pixels behind the element
            var background = Mica.CaptureRegion(
                (int)screenPos.X - 80, (int)screenPos.Y - 80,
                (int)WelcomeLeft.ActualWidth + 160, (int)WelcomeLeft.ActualHeight + 160);

            // Apply to an Image control inside WelcomeLeft
            // 3. FIX: Convert BitmapSource to an ImageBrush
            // This solves the "Cannot implicitly convert" error
            var brush = new System.Windows.Media.ImageBrush(background);

            // Optional: Ensure the background doesn't stretch weirdly
            brush.Stretch = System.Windows.Media.Stretch.None;
            brush.AlignmentX = System.Windows.Media.AlignmentX.Left;
            brush.AlignmentY = System.Windows.Media.AlignmentY.Top;

            // unlock from cloaking
            //DwmApi.DwmSetWindowAttribute(hwnd, DwmApi.DWMWA_CLOAK, ref uncloak, sizeof(int));
            User32.SetWindowDisplayAffinity(hwnd, User32.WDA_NONE);

            // 4. Apply to the Background property
            BackgroundLayer.Background = brush;

            // Apply your custom ShaderEffect
            BackgroundLayer.Effect = new BlurEffect() { Radius = 20, KernelType = KernelType.Box, RenderingBias = RenderingBias.Quality }; //new MedicalControls.Shaders.InvertColorEffect();
        }
    }
}
