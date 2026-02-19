using EPIC.ClearView.Utilities.Macros;
using System.Windows;
using System.Windows.Controls;

namespace EPIC.ClearView.Pages.Patient
{
    // Token: 0x02000020 RID: 32
    public partial class Add : Page
    {
        // Token: 0x17000062 RID: 98
        // (get) Token: 0x060001AF RID: 431 RVA: 0x0001126C File Offset: 0x0000F46C
        // (set) Token: 0x060001B0 RID: 432 RVA: 0x0001128E File Offset: 0x0000F48E
        public DataLayer.Entities.Patient Patient
        {
            get
            {
                return (DataLayer.Entities.Patient)GetValue(Add.PatientProperty);
            }
            set
            {
                SetValue(Add.PatientProperty, value);
            }
        }

        // Token: 0x060001B1 RID: 433 RVA: 0x000113EC File Offset: 0x0000F5EC
        public Add()
        {
            Patient = new DataLayer.Entities.Patient();
            this.InitializeComponent();
            Navigation.InsertRibbon(this);
        }

        // Token: 0x060001B5 RID: 437 RVA: 0x00011488 File Offset: 0x0000F688
        private void BirthDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            DateTime today = DateTime.Today;
            if (this.BirthDate.SelectedDate != null)
            {
                int num = today.Year - this.BirthDate.SelectedDate.Value.Year;
                if (this.BirthDate.SelectedDate.Value > today.AddYears(-num))
                {
                    num--;
                }
                this.Age.Text = num.ToString(CultureInfo.InvariantCulture);
            }
            */
        }

        // Token: 0x040000ED RID: 237
        public static readonly DependencyProperty PatientProperty = DependencyProperty.Register("Patient", typeof(DataLayer.Entities.Patient), typeof(Add), new PropertyMetadata(new DataLayer.Entities.Patient()));

    }
}
