using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using EPIC.Utilities.Extensions;
using EPIC.Utilities.Logging;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using ParallelWork;

namespace EPIC.Controls
{
	// Token: 0x02000010 RID: 16
	public partial class LogTextBox : TextBox
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00008B04 File Offset: 0x00006D04
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00008B26 File Offset: 0x00006D26
		public Type[] TypeFilter
		{
			get
			{
				return (Type[])base.GetValue(LogTextBox.TypeFilterProperty);
			}
			set
			{
				base.SetValue(LogTextBox.TypeFilterProperty, value);
				this._filter = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00008B40 File Offset: 0x00006D40
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00008B62 File Offset: 0x00006D62
		public bool SinceLoad
		{
			get
			{
				return (bool)base.GetValue(LogTextBox.SinceLoadProperty);
			}
			set
			{
				base.SetValue(LogTextBox.SinceLoadProperty, value);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00008B78 File Offset: 0x00006D78
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00008B9A File Offset: 0x00006D9A
		public bool ShowTime
		{
			get
			{
				return (bool)base.GetValue(LogTextBox.ShowTimeProperty);
			}
			set
			{
				base.SetValue(LogTextBox.ShowTimeProperty, value);
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00008BAF File Offset: 0x00006DAF
		public LogTextBox()
		{
			this.InitializeComponent();
			this._reading = new Semaphore(1, 1);
			base.Dispatcher.ShutdownStarted += this.TextBox_Unloaded;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00008F8C File Offset: 0x0000718C
		private void LogReaderThread(Stream stream, DateTime? since, bool showTime)
		{
			int num = 0;
			if (this._progress != null)
			{
				this._progress.Dispatcher.Invoke(delegate()
				{
					this._progress.Value = 0.0;
					this._progress.Visibility = Visibility.Visible;
				}, DispatcherPriority.Background);
				NameTable nameTable = new NameTable();
				XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
				XNamespace aw = "http://logging.apache.org/log4net/schemas/log4net-events-1.2";
				xmlNamespaceManager.AddNamespace("log4net", aw.ToString());
				XmlParserContext inputContext = new XmlParserContext(nameTable, xmlNamespaceManager, "", XmlSpace.Default);
				XmlReader xmlReader = XmlReader.Create(stream, new XmlReaderSettings
				{
					ConformanceLevel = ConformanceLevel.Fragment,
					ValidationType = ValidationType.None,
					ValidationFlags = XmlSchemaValidationFlags.None,
					NameTable = nameTable,
					IgnoreWhitespace = true
				}, inputContext);
				try
				{
					while (xmlReader.Read())
					{
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.LocalName == "event")
						{
							int percent = Convert.ToInt32((double)stream.Position / (double)stream.Length * 100.0);
							if (percent != num)
							{
								this._progress.Dispatcher.Invoke<double>(() => this._progress.Value = (double)percent, DispatcherPriority.Background);
								Thread.Sleep(1);
								num = percent;
							}
							try
							{
								using (XmlReader xmlReader2 = xmlReader.ReadSubtree())
								{
									xmlReader2.Read();
									XElement evtEl = XNode.ReadFrom(xmlReader2) as XElement;
									if (evtEl != null)
									{
										bool flag = this._filter == null || !this._filter.Any<Type>();
										XAttribute logger = evtEl.Attribute("logger");
										bool flag2;
										if (!flag && logger != null)
										{
											flag2 = !(from type in this._filter
											let entryType = Assembly.GetAssembly(type).GetType(logger.Value) ?? Type.GetType(logger.Value)
											where type.IsAssignableFrom(entryType)
											select type).Any<Type>();
										}
										else
										{
											flag2 = true;
										}
										if (!flag2)
										{
											flag = true;
										}
										bool flag3 = since == null;
										XAttribute xattribute = evtEl.Attribute("timestamp");
										DateTime time = DateTime.Now;
										if (xattribute != null && DateTime.TryParse(xattribute.Value, out time) && !flag3 && time > since)
										{
											flag3 = true;
										}
										if (flag && flag3)
										{
											base.Dispatcher.Invoke(delegate()
											{
												bool flag4 = this._scroller.ExtentHeight < this._scroller.ViewportHeight || this._scroller.VerticalOffset - this._scroller.ExtentHeight < 10.0;
												LogTextBox <>4__this = this;
												<>4__this.Text = <>4__this.Text + ((this.Text.Length > 0) ? "\n" : "") + (showTime ? (time.ToShortDateString() + time.ToShortTimeString() + " ") : "") + ((string)evtEl.Element(aw + "message")).Trim();
												if (flag4)
												{
													this._scroller.ScrollToBottom();
												}
											}, DispatcherPriority.Background);
										}
									}
									xmlReader2.Close();
								}
							}
							catch (Exception ex)
							{
								LogTextBox.Log.Warn("Error reading event.", ex);
							}
						}
					}
				}
				finally
				{
					this._currentPos = stream.Position;
					xmlReader.Close();
					this._progress.Dispatcher.Invoke<Visibility>(() => this._progress.Visibility = Visibility.Collapsed, DispatcherPriority.Background);
					this._reading.Release();
				}
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00009444 File Offset: 0x00007644
		private void TextBox_Loaded(object sender, RoutedEventArgs e)
		{
			this._load = DateTime.Now;
			try
			{
				this._progress = this.FindChild("PART_Progress");
				this._scroller = this.FindChild("PART_ContentHost");
				AppenderCollection appenders = ((Hierarchy)LogManager.GetRepository()).Root.Appenders;
				FileAppender xmlAppender;
				if ((xmlAppender = appenders.OfType<FileAppender>().FirstOrDefault((FileAppender x) => x.Layout is XmlLayout)) == null)
				{
					AsyncAppender asyncAppender;
					if ((asyncAppender = appenders.OfType<AsyncAppender>().FirstOrDefault<AsyncAppender>()) == null)
					{
						xmlAppender = null;
					}
					else
					{
						xmlAppender = asyncAppender.Appenders.OfType<FileAppender>().FirstOrDefault((FileAppender x) => x.Layout is XmlLayout);
					}
				}
				this._xmlAppender = xmlAppender;
				if (this._xmlAppender != null)
				{
					this._xmlWatcher = new FileSystemWatcher
					{
						Path = Path.GetDirectoryName(this._xmlAppender.File),
						Filter = Path.GetFileName(this._xmlAppender.File),
						NotifyFilter = (NotifyFilters.Size | NotifyFilters.LastWrite),
						EnableRaisingEvents = true
					};
					this._xmlWatcher.Changed += this.StartStreamThread;
					Dispatcher dispatcher = base.Dispatcher;
					Delegate method = new Action<object, FileSystemEventArgs>(this.StartStreamThread);
					DispatcherPriority priority = DispatcherPriority.Background;
					object[] args = new object[2];
					dispatcher.BeginInvoke(method, priority, args);
				}
			}
			catch (Exception ex)
			{
				LogTextBox.Log.Error("Cannot find log file.", ex);
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000968C File Offset: 0x0000788C
		private void StartStreamThread(object sender, FileSystemEventArgs fileSystemEventArgs)
		{
			if (this._xmlWatcher != null && this._xmlWatcher.EnableRaisingEvents && this._reading.WaitOne(0))
			{
				FileStream fileStream = new FileStream(this._xmlAppender.File, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				if (this._currentPos > fileStream.Length)
				{
					LogTextBox.Log.Info("Log truncated, restarting.");
				}
				else
				{
					fileStream.Seek(this._currentPos, SeekOrigin.Begin);
				}
				base.Dispatcher.BeginInvoke(new Action<FileStream>(delegate(FileStream stream)
				{
					DateTime? sinceDate = this.SinceLoad ? new DateTime?(this._load) : null;
					bool showTime = this.ShowTime;
					Start.Work(delegate()
					{
						this.LogReaderThread(fileStream, sinceDate, showTime);
					}, ThreadPriority.Lowest).RunNow();
				}), DispatcherPriority.Background, new object[]
				{
					fileStream
				});
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00009758 File Offset: 0x00007958
		private void TextBox_Unloaded(object sender, EventArgs e)
		{
			if (this._xmlWatcher != null)
			{
				this._xmlWatcher.Changed -= this.StartStreamThread;
				this._xmlWatcher.EnableRaisingEvents = false;
				this._xmlWatcher.Dispose();
				this._xmlWatcher = null;
			}
			this._progress = null;
			this._scroller = null;
		}

		// Token: 0x0400008F RID: 143
		private static readonly ILog Log = LogManager.GetLogger(typeof(LogTextBox));

		// Token: 0x04000090 RID: 144
		private FileSystemWatcher _xmlWatcher;

		// Token: 0x04000091 RID: 145
		private FileAppender _xmlAppender;

		// Token: 0x04000092 RID: 146
		private ProgressBar _progress;

		// Token: 0x04000093 RID: 147
		private ScrollViewer _scroller;

		// Token: 0x04000094 RID: 148
		private long _currentPos;

		// Token: 0x04000095 RID: 149
		private readonly Semaphore _reading;

		// Token: 0x04000096 RID: 150
		private DateTime _load;

		// Token: 0x04000097 RID: 151
		private Type[] _filter;

		// Token: 0x04000098 RID: 152
		public static readonly DependencyProperty TypeFilterProperty = DependencyProperty.Register("TypeFilter", typeof(Type[]), typeof(LogTextBox), new PropertyMetadata(new Type[0]));

		// Token: 0x04000099 RID: 153
		public static readonly DependencyProperty SinceLoadProperty = DependencyProperty.Register("SinceLoad", typeof(bool), typeof(LogTextBox), new PropertyMetadata(false));

		// Token: 0x0400009A RID: 154
		public static readonly DependencyProperty ShowTimeProperty = DependencyProperty.Register("ShowTime", typeof(bool), typeof(LogTextBox), new PropertyMetadata(false));
	}
}
