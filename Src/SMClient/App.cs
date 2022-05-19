// Decompiled with JetBrains decompiler
// Type: SMClient.App
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Hardcodet.Wpf.TaskbarNotification;
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

    public App()
    {
      this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(this.App_DispatcherUnhandledException);
      this.appIcon = new BitmapImage(new Uri("pack://application:,,,/SMClient;component/Resources/AppIcon.ico"));
      new MainWindow().Show();
    }

    private void App_DispatcherUnhandledException(
      object sender,
      DispatcherUnhandledExceptionEventArgs e)
    {
      Logger.LogError(e.Exception);
    }

    [STAThread]
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public static void Main()
    {
        App app = new App();
        app.InitializeComponent();
        app.Run();
    }
    }
}
