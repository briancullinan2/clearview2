using EPIC.ClearView.Utilities.Macros;
using System.Windows.Controls;
using System.Windows.Media;

namespace EPIC.ClearView.Pages.User
{
    // Token: 0x0200006A RID: 106
    public partial class Search : Page
    {
        public ImageBrush RibbonBackground { get; private set; }

        // Token: 0x06000335 RID: 821 RVA: 0x0001AB30 File Offset: 0x00018D30
        public Search()
        {
            this.InitializeComponent();
            Navigation.InsertRibbon(this);
        }
    }
}
