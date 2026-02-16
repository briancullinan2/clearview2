using System.Windows.Controls;

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

            // hide parent window from all capture?
            //User32.SetWindowDisplayAffinity(hwnd, User32.WDA_EXCLUDEFROMCAPTURE);

        }


    }
}
