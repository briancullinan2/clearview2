using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using EPICClearView.Macros;
using EPICClearView.Utilities.Extensions;
using EPICDataLayer;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;

namespace EPIC.Patient
{
	// Token: 0x02000069 RID: 105
	public partial class Search : Page
	{
		// Token: 0x06000329 RID: 809 RVA: 0x0001A3D3 File Offset: 0x000185D3
		public Search()
		{
			this.InitializeComponent();
			//this.Patients.ItemsSource = new LinqMetaData().Patient.ToList<PatientEntity>();
			Navigation.InsertRibbon(this);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0001A407 File Offset: 0x00018607
		private void NewPatient_Click(object sender, RoutedEventArgs e)
		{
			Navigation.ShowTab(new Uri("/Patient/Add.xaml", UriKind.Relative), true);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0001A41C File Offset: 0x0001861C
		private void Search_TextChanged(object sender, TextChangedEventArgs e)
		{
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0001A608 File Offset: 0x00018808
		private void CreatePatientContacts(List<PatientEntity> patients)
		{
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0001A79C File Offset: 0x0001899C
		private void CopyToDestination(string fromFile, string toFile)
		{
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0001A87C File Offset: 0x00018A7C
		private void Export_Click(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0001A994 File Offset: 0x00018B94
		private void NewCapture_Click(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x0400017E RID: 382
		private string _saveLocation;

		// Token: 0x0400017F RID: 383
		private string _outputFile;

		// Token: 0x04000180 RID: 384
		private string _tempPath;
	}
}
