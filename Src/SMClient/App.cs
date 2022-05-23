// Decompiled with JetBrains decompiler
// Type: SMClient.App
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Hardcodet.Wpf.TaskbarNotification;
using SMClient.Api;
using SMClient.Data.Managers.IntegrationManagers;
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
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(this.App_DispatcherUnhandledException);
            this.appIcon = new BitmapImage(new Uri("pack://application:,,,/SMClient;component/Resources/AppIcon.ico"));
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
            App app = new App();
            app.InitializeComponent();
            app.Run(app.MainWindow);
        }
    }
}
