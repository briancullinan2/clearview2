using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using EPICClearView.Macros;
using EPICClearView.Utilities;
using EPICDataLayer;

namespace EPICClearView.Patient
{
	// Token: 0x02000020 RID: 32
	public partial class Add : Page
	{
        // Token: 0x17000062 RID: 98
        // (get) Token: 0x060001AF RID: 431 RVA: 0x0001126C File Offset: 0x0000F46C
        // (set) Token: 0x060001B0 RID: 432 RVA: 0x0001128E File Offset: 0x0000F48E
        public PatientEntity Patient
        {
            get
            {
                return (PatientEntity)GetValue(Add.PatientProperty);
            }
            set
            {
                SetValue(Add.PatientProperty, value);
            }
        }

        // Token: 0x060001B1 RID: 433 RVA: 0x000113EC File Offset: 0x0000F5EC
        public Add()
		{
            Patient = new PatientEntity();
            this.InitializeComponent();
            Navigation.InsertRibbon(this);
        }

        // Token: 0x060001B2 RID: 434 RVA: 0x00011425 File Offset: 0x0000F625
        private void OnChanged(object sender, RoutedEventArgs routedEventArgs)
		{
			this.Save.IsEnabled = true;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00011435 File Offset: 0x0000F635
		private void OnUnchanged(object sender, RoutedEventArgs routedEventArgs)
		{
			this.Save.IsEnabled = false;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00011448 File Offset: 0x0000F648
		private void Save_Click(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00011488 File Offset: 0x0000F688
		private void BirthDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
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
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0001151E File Offset: 0x0000F71E
		private void SaveClose_Click(object sender, RoutedEventArgs e)
		{
			this.Save_Click(sender, e);
			Navigation.CloseTab(this);
		}

		// Token: 0x040000ED RID: 237
		public static readonly DependencyProperty PatientProperty = DependencyProperty.Register("Patient", typeof(PatientEntity), typeof(Add), new PropertyMetadata(new PatientEntity()));
	}
}
