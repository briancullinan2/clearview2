using EPIC.MedicalControls.Macros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.MedicalControls.Themes
{
    public partial class ClearView
    {
        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Navigation.CloseTab(sender);
        }
    }
}
