using EPIC.MedicalControls.Macros;

namespace EPIC.MedicalControls.Themes
{
    public partial class ClearView
    {

        private void ContentControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var contentControl = sender as System.Windows.Controls.ContentControl;
            if (contentControl == null) return;

            var Window = System.Windows.Application.Current.MainWindow;
            var backgroundLayer = contentControl.Template.FindName("BackgroundLayer", contentControl) as System.Windows.FrameworkElement;

            if (backgroundLayer != null)
            {
                Mica.SubscribeFuzz(Window, backgroundLayer, contentControl);
            }
        }
        //private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    Navigation.CloseTab(sender);
        //}
    }
}
