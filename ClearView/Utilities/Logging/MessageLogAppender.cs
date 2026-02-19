using EPIC.ClearView.Utilities.Extensions;
using EPIC.DataLayer.Utilities.Extensions;
using log4net.Appender;
using log4net.Core;
using log4net.Util;

namespace EPIC.ClearView.Utilities.Logging
{
    // Token: 0x02000061 RID: 97
    public class MessageLogAppender : IBulkAppender, IAppender, IOptionHandler, IAppenderAttachable
    {
        public string Name { get; set; }

        // Token: 0x17000092 RID: 146
        // (set) Token: 0x0600030A RID: 778 RVA: 0x00019778 File Offset: 0x00017978
        public string ConnectionStringName
        {
            set
            {
                ConnectionString = ClearViewConfiguration.Current.ConnectionStrings[value].ToString();
            }
        }

        public string ConnectionString { get; private set; }

        public void ActivateOptions()
        {
        }

        // Token: 0x060002FF RID: 767 RVA: 0x000193A4 File Offset: 0x000175A4
        public void Close()
        {
            lock (this)
            {
                if (this._mAppenderAttachedImpl != null)
                {
                    this._mAppenderAttachedImpl.RemoveAllAppenders();
                }
            }
        }
        public void DoAppend(LoggingEvent[] loggingEvents)
        {
            for (int i = 0; i < loggingEvents.Length; i++)
            {
                DoAppend(loggingEvents[i]);
            }
        }

        // Token: 0x0600030B RID: 779 RVA: 0x000197C4 File Offset: 0x000179C4
        public void DoAppend(LoggingEvent loggingEvent)
        {
            try
            {
                var context = DataLayer.TranslationContext.Current["Data Source=:memory:"];
                if (loggingEvent.ExceptionObject != null)
                {
                    new DataLayer.Entities.Message
                    {
                        Source = loggingEvent.LoggerName,
                        Title = loggingEvent.ExceptionObject.Message.Limit(DataLayer.EntityMetadata.Message.MaxLength[nameof(DataLayer.Entities.Message.Title)] ?? 1024),
                        Body = loggingEvent.ExceptionObject.StackTrace?.Limit(DataLayer.EntityMetadata.Message.MaxLength[nameof(DataLayer.Entities.Message.Body)] ?? 4096),
                        CreateTime = DateTime.UtcNow,
                        IsActive = true,
                        MessageType = 4
                    }.Save();
                }
                else
                {
                    new DataLayer.Entities.Message
                    {
                        Source = loggingEvent.LoggerName,
                        Title = (loggingEvent.MessageObject?.ToString() ?? loggingEvent.RenderedMessage)?.Limit(DataLayer.EntityMetadata.Message.MaxLength[nameof(DataLayer.Entities.Message.Title)] ?? 1024),
                        Body = new System.Diagnostics.StackTrace(true).ToString().Limit(DataLayer.EntityMetadata.Message.MaxLength[nameof(DataLayer.Entities.Message.Body)] ?? 4096),
                        CreateTime = DateTime.UtcNow,
                        IsActive = true,
                        MessageType = 4
                    }.Save();
                }
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate ()
                    {
                        MainWindow? mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
                        if (mainWindow == null)
                        {
                            return;
                        }
                        mainWindow.UpdateAlerts();
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        // Token: 0x06000303 RID: 771 RVA: 0x000194D0 File Offset: 0x000176D0
        public void AddAppender(IAppender newAppender)
        {
            if (newAppender == null)
            {
                throw new ArgumentNullException("newAppender");
            }
            lock (this)
            {
                if (this._mAppenderAttachedImpl == null)
                {
                    this._mAppenderAttachedImpl = new AppenderAttachedImpl();
                }
                this._mAppenderAttachedImpl.AddAppender(newAppender);
            }
        }

        // Token: 0x17000091 RID: 145
        // (get) Token: 0x06000304 RID: 772 RVA: 0x00019554 File Offset: 0x00017754
        public AppenderCollection Appenders
        {
            get
            {
                AppenderCollection result;
                lock (this)
                {
                    if (this._mAppenderAttachedImpl == null)
                    {
                        result = AppenderCollection.EmptyCollection;
                    }
                    else
                    {
                        result = this._mAppenderAttachedImpl.Appenders;
                    }
                }
                return result;
            }
        }

        // Token: 0x06000305 RID: 773 RVA: 0x000195BC File Offset: 0x000177BC
        public IAppender GetAppender(string name)
        {
            IAppender result;
            lock (this)
            {
                if (this._mAppenderAttachedImpl == null || name == null)
                {
                    result = null;
                }
                else
                {
                    result = this._mAppenderAttachedImpl.GetAppender(name);
                }
            }
            return result;
        }

        // Token: 0x06000306 RID: 774 RVA: 0x00019628 File Offset: 0x00017828
        public void RemoveAllAppenders()
        {
            lock (this)
            {
                if (this._mAppenderAttachedImpl != null)
                {
                    this._mAppenderAttachedImpl.RemoveAllAppenders();
                    this._mAppenderAttachedImpl = null;
                }
            }
        }

        // Token: 0x06000307 RID: 775 RVA: 0x0001968C File Offset: 0x0001788C
        public IAppender RemoveAppender(IAppender appender)
        {
            lock (this)
            {
                if (appender != null && this._mAppenderAttachedImpl != null)
                {
                    return this._mAppenderAttachedImpl.RemoveAppender(appender);
                }
            }
            return null;
        }

        // Token: 0x06000308 RID: 776 RVA: 0x000196F8 File Offset: 0x000178F8
        public IAppender RemoveAppender(string name)
        {
            lock (this)
            {
                if (name != null && this._mAppenderAttachedImpl != null)
                {
                    return this._mAppenderAttachedImpl.RemoveAppender(name);
                }
            }
            return null;
        }

        // Token: 0x04000170 RID: 368
        private AppenderAttachedImpl _mAppenderAttachedImpl;
    }

}
