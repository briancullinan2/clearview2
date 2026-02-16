using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace EPIC.ClearView.User
{
	// Token: 0x02000049 RID: 73
	public partial class Login : Page
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600026C RID: 620 RVA: 0x00014458 File Offset: 0x00012658
		// (remove) Token: 0x0600026D RID: 621 RVA: 0x00014494 File Offset: 0x00012694
		public event Login.LoggedInHandler LoggedIn;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600026E RID: 622 RVA: 0x000144D0 File Offset: 0x000126D0
		// (remove) Token: 0x0600026F RID: 623 RVA: 0x0001450C File Offset: 0x0001270C
		public event Login.LoginCanceledHandler LoginCanceled;

		// Token: 0x06000270 RID: 624 RVA: 0x00014548 File Offset: 0x00012748
		public Login()
		{
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00014654 File Offset: 0x00012854
		private void ListServers()
		{
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00014820 File Offset: 0x00012A20
		private void Login_Click(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00014950 File Offset: 0x00012B50
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			this.Username.Text = "";
			this.Password.Password = "";
			if (this.LoginCanceled != null)
			{
				this.LoginCanceled();
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x000149D8 File Offset: 0x00012BD8
		private void Servers_TextChanged(object sender, TextChangedEventArgs e)
		{
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00014BD0 File Offset: 0x00012DD0
		private void ListDatabases(string serverName, ComboBox source)
		{
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00014DA4 File Offset: 0x00012FA4
		private void Connection_Loaded(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00014E88 File Offset: 0x00013088
		private void Databases_TextChanged(object sender, TextChangedEventArgs e)
		{
			//this._builder.InitialCatalog = this.Databases.Text;
			//this.Connection.Text = this._builder.ToString();
			this.Databases.SelectedItem = this.Databases.Items.OfType<string>().FirstOrDefault((string x) => x == this.Databases.Text);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00014EF4 File Offset: 0x000130F4
		private void Connection_TextChanged(object sender, TextChangedEventArgs e)
		{
			/*
			this._builder = new SqlConnectionStringBuilder(this.Connection.Text);
			this.Servers.Text = this._builder.DataSource;
			this.Databases.Text = this._builder.InitialCatalog;
			this.DbUser.Text = this._builder.UserID;
			this.DbPass.Password = this._builder.Password;
			*/
		}


		// Token: 0x04000146 RID: 326
		//private SqlConnectionStringBuilder _builder;

		// Token: 0x0200004A RID: 74
		// (Invoke) Token: 0x06000286 RID: 646
		public delegate void LoggedInHandler(/* DataLayer.Entities.User user */);

		// Token: 0x0200004B RID: 75
		// (Invoke) Token: 0x0600028A RID: 650
		public delegate void LoginCanceledHandler();
	}
}
