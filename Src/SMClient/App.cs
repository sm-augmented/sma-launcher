using Hardcodet.Wpf.TaskbarNotification;
using SMClient.Api;
using SMClient.Managers;
using SMClient.Utils;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SMClient
{
    public partial class App : Application
    {
        private BitmapImage appIcon;

        public static TaskbarIcon TaskbarIcon { get; set; }

        public MainWindow MainWindow { get; set; }

        public App()
        {
            DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            appIcon = new BitmapImage(new Uri("pack://application:,,,/SMClient;component/Resources/AppIcon.ico"));
            MainWindow = new MainWindow();
        }

        private void App_DispatcherUnhandledException(
          object sender,
          DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.LogError(e.Exception);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                Unlocker.CloseNoWait();
            }
            catch (Exception ex)
            {
                Logger.LogError(new Exception("Unable to properly exit", ex));
            }

            try
            {
                PersistenceApi.UnregisterPlayerIngame();
            }
            catch (Exception ex)
            {
                Logger.LogError(new Exception("Unable to properly exit", ex));
            }

            try
            {
                SteamManager.Shutdown();
            }
            catch (Exception ex)
            {
                Logger.LogError(new Exception("Unable to properly exit", ex));
            }

            try
            {
                Logger.Flush();
            }
            catch (Exception ex)
            {
                Logger.LogError(new Exception("Unable to properly exit", ex));
            }

            base.OnExit(e);
        }

        [STAThread]
        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public static void Main()
        {
            try
            {
                Logger.Create();
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError("Unable to init log!");
                return;
            }

            App app = new App();
            app.InitializeComponent();
            app.Run(app.MainWindow);
        }
    }
}
