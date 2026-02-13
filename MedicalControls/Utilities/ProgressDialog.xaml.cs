using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shell;

namespace EPIC.Controls
{
    // Token: 0x02000014 RID: 20
    public partial class ProgressDialog : Window
    {
        // Token: 0x06000124 RID: 292 RVA: 0x0000A9E0 File Offset: 0x00008BE0
        static ProgressDialog()
        {
            ProgressDialog.ResetTask.Elapsed += ProgressDialog.ResetTaskOnElapsed;
        }

        // Token: 0x06000125 RID: 293 RVA: 0x0000AA97 File Offset: 0x00008C97
        private static void ResetTaskOnElapsed(object? sender, ElapsedEventArgs elapsedEventArgs)
        {
            Application.Current.Dispatcher.Invoke(delegate ()
            {
                Application.Current.MainWindow.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
                Application.Current.MainWindow.TaskbarItemInfo.ProgressValue = 0.0;
                Application.Current.MainWindow.TaskbarItemInfo.Changed -= ProgressDialog.TaskbarItemInfoOnChanged;
            });
        }

        // Token: 0x06000126 RID: 294 RVA: 0x0000AC04 File Offset: 0x00008E04
        public static ProgressDialog Show(string titleMessage, params string[] stati)
        {
            ProgressDialog progress = new ProgressDialog();
            Application.Current.Dispatcher.Invoke(delegate ()
            {
                progress.Title = titleMessage;
                progress.Owner = (Application.Current.Windows.OfType<Window>().FirstOrDefault((Window x) => x.IsActive) ?? Application.Current.MainWindow);
                foreach (string content in stati)
                {
                    progress.Stati.Children.Add(new Label
                    {
                        Content = content
                    });
                    progress.Stati.Children.Add(new Label());
                    progress.Stati.Children.Add(new ProgressBar
                    {
                        Height = 15.0
                    });
                }
                progress.Show();
            });
            return progress;
        }

        // Token: 0x06000127 RID: 295 RVA: 0x0000ADD4 File Offset: 0x00008FD4
        public void Update(params Tuple<string, double>[] stati)
        {
            Application.Current.Dispatcher.Invoke(delegate ()
            {
                int num = 0;
                foreach (Tuple<string, double> tuple in stati)
                {
                    Label? label = this.Stati.Children.OfType<Label>().Skip(num * 2 + 1).FirstOrDefault<Label>();
                    if (label != null)
                    {
                        label.Content = tuple.Item1;
                    }
                    ProgressBar? progressBar = this.Stati.Children.OfType<ProgressBar>().Skip(num).FirstOrDefault<ProgressBar>();
                    if (progressBar != null)
                    {
                        progressBar.Value = tuple.Item2;
                    }
                    num++;
                }
                ProgressBar? progressBar2 = this.Stati.Children.OfType<ProgressBar>().LastOrDefault<ProgressBar>();
                if (progressBar2 != null)
                {
                    progressBar2.IsIndeterminate = false;
                    var color = (System.Windows.Media.Brush?)new BrushConverter().ConvertFromString("#FF01D328");
                    if (color != null) progressBar2.Foreground = color;
                    else progressBar2.ClearValue(ProgressBar.ForegroundProperty);
                    Application.Current.MainWindow.TaskbarItemInfo.ProgressState = (progressBar2.IsIndeterminate ? TaskbarItemProgressState.Indeterminate : TaskbarItemProgressState.Normal);
                    Application.Current.MainWindow.TaskbarItemInfo.ProgressValue = progressBar2.Value;
                    Application.Current.MainWindow.TaskbarItemInfo.Changed += ProgressDialog.TaskbarItemInfoOnChanged;
                    ProgressDialog.ResetTask.Enabled = true;
                }
            });
        }

        // Token: 0x06000128 RID: 296 RVA: 0x0000AE13 File Offset: 0x00009013
        private static void TaskbarItemInfoOnChanged(object? sender, EventArgs eventArgs)
        {
            ProgressDialog.ResetTask.Stop();
            Application.Current.MainWindow.TaskbarItemInfo.Changed -= ProgressDialog.TaskbarItemInfoOnChanged;
        }

        // Token: 0x06000129 RID: 297 RVA: 0x0000AE50 File Offset: 0x00009050
        private ProgressDialog()
        {
            // Show() can start from any thread and this has to end up back on STA
            Dispatcher.BeginInvoke(() => this.InitializeComponent());
            base.Closed += delegate (object? sender, EventArgs args)
            {
                ProgressDialog.ResetTask.Enabled = true;
            };
        }

        // Token: 0x0600012A RID: 298 RVA: 0x0000AF01 File Offset: 0x00009101
        public void Pause()
        {
            Application.Current.Dispatcher.Invoke(delegate ()
            {
                ProgressBar? progressBar = this.Stati.Children.OfType<ProgressBar>().LastOrDefault<ProgressBar>();
                if (progressBar != null)
                {
                    progressBar.IsIndeterminate = true;
                    progressBar.Foreground = System.Windows.Media.Brushes.Yellow;
                }
                Application.Current.MainWindow.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Paused;
                Application.Current.MainWindow.TaskbarItemInfo.Changed += ProgressDialog.TaskbarItemInfoOnChanged;
            });
        }

        // Token: 0x0600012B RID: 299 RVA: 0x0000AF2C File Offset: 0x0000912C
        public new void Close()
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(delegate ()
                {
                    base.Close();
                });
            }
            else
            {
                base.Close();
            }
        }

        // Token: 0x0600012C RID: 300 RVA: 0x0000B009 File Offset: 0x00009209
        public void Error(string message = "")
        {
            Application.Current.Dispatcher.Invoke(delegate ()
            {
                ProgressBar? progressBar = this.Stati.Children.OfType<ProgressBar>().LastOrDefault<ProgressBar>();
                if (progressBar != null)
                {
                    progressBar.IsIndeterminate = false;
                    progressBar.Foreground = System.Windows.Media.Brushes.Red;
                    progressBar.Value = 100.0;
                }
                Application.Current.MainWindow.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Error;
                Application.Current.MainWindow.TaskbarItemInfo.Changed += ProgressDialog.TaskbarItemInfoOnChanged;
                ProgressDialog.ResetTask.Enabled = true;
            });
        }

        // Token: 0x0600012D RID: 301 RVA: 0x0000B028 File Offset: 0x00009228
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            base.DialogResult = null;
            this.Close();
        }

        // Token: 0x040000AD RID: 173
        private static readonly System.Timers.Timer ResetTask = new System.Timers.Timer
        {
            AutoReset = true,
            Interval = 2000.0,
            Enabled = false
        };
    }
}
