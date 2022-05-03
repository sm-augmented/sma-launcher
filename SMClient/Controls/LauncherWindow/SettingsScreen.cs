// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.LauncherWindow.SettingsScreen
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Api;
using SMClient.Controls.Components;
using SMClient.Data.Managers;
using SMClient.Data.Tasks;
using SMClient.Models;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SMClient.Controls.LauncherWindow
{
  public class SettingsScreen : UserControl, IComponentConnector
  {
    public ImageSource NormalBackground;
    internal PathSelector GameDir;
    internal SMCheckbox cbOldLauchWay;
    internal Button btnResetLoadouts;
    internal Button btnFixProfileFile;
    internal Button btnCheckIntegrity;
    internal Button btnLogOut;
    private bool _contentLoaded;

    public event EventHandler LoggedOut;

    public SettingsScreen()
    {
      this.InitializeComponent();
      this.NormalBackground = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/SMClient;component/Resources/ImageBackground4.png"));
      this.GameDir.OnPathSelected += new EventHandler(this.GameDir_PathSelected);
      this.GameDir.SelectedDirectory = Settings.Instance.GamePath;
      this.cbOldLauchWay.IsChecked = Settings.Instance.OldLaunchWay;
    }

    private void ToggleButtonsEnabled(bool enabled) => this.Dispatcher.Invoke((Action) (() => this.btnCheckIntegrity.IsEnabled = this.btnLogOut.IsEnabled = this.btnResetLoadouts.IsEnabled = enabled));

    private void GameDir_PathSelected(object sender, EventArgs e)
    {
      string str = Settings.Instance.GamePath ?? "";
      Settings.Instance.GamePath = this.GameDir.SelectedDirectory;
      if (Settings.Instance.GamePathFine)
      {
        Settings.Instance.Save();
      }
      else
      {
        Settings.Instance.GamePath = this.GameDir.SelectedDirectory = str;
        int num = (int) MessageBox.Show("Wrong path selected");
      }
    }

    private void BtnSendLog_Click(object sender, RoutedEventArgs e)
    {
      this.ToggleButtonsEnabled(false);
      MainWindow.ShowLoading("Sending...");
      AccountApi.SendLogAsync().ContinueWith((Action<Task>) (t => this.Dispatcher.Invoke((Action) (() =>
      {
        MainWindow.HideLoading();
        this.ToggleButtonsEnabled(true);
      }))));
    }

    private void BtnLogOut_Click(object sender, RoutedEventArgs e)
    {
      this.ToggleButtonsEnabled(false);
      MainWindow.ShowLoading("Logging out...");
      Task.Run((Action) (() =>
      {
        Settings.Instance.AccessToken = (string) null;
        Settings.Instance.Login = (string) null;
        Settings.Instance.Password = (string) null;
        Settings.Instance.Save();
        OnlineManager.LogOut();
        RegisterIngameTask.Cancel();
        PersistenceApi.UnregisterPlayerIngame().Wait();
        MainWindow.HideLoading();
        this.ToggleButtonsEnabled(true);
        this.Dispatcher.Invoke((Action) (() =>
        {
          EventHandler loggedOut = this.LoggedOut;
          if (loggedOut == null)
            return;
          loggedOut((object) null, (EventArgs) null);
        }));
      }));
    }

    private void btnCheckIntegrity_Click(object sender, RoutedEventArgs e)
    {
      this.ToggleButtonsEnabled(false);
      MainWindow.ShowLoading("Processing...");
      Task.Run((Action) (() =>
      {
        try
        {
          PackageManager.ReUnpackStaticArchives();
        }
        catch (Exception ex)
        {
          Logger.LogError(ex);
        }
        MainWindow.HideLoading();
        this.ToggleButtonsEnabled(true);
      }));
    }

    private void btnResetLoadouts_Click(object sender, RoutedEventArgs e)
    {
      this.ToggleButtonsEnabled(false);
      MainWindow.ShowLoading("Processing...");
      Task.Run((Func<Task>) (async () =>
      {
        try
        {
          await ProfileApi.ResetLoadouts();
        }
        catch (Exception ex)
        {
          Logger.LogError(ex);
        }
        MainWindow.HideLoading();
        this.ToggleButtonsEnabled(true);
      }));
    }

    private void GameDir_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private void cbOldLauchWay_Checked(object sender, EventArgs e)
    {
      Settings.Instance.OldLaunchWay = this.cbOldLauchWay.IsChecked;
      Settings.Instance.Save();
    }

    private void btnFixProfileFile_Click(object sender, RoutedEventArgs e) => ProfileInfo.RecreateModProfile(true);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/SMClient;component/controls/windows/mainscreens/settingsscreen.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.GameDir = (PathSelector) target;
          break;
        case 2:
          this.cbOldLauchWay = (SMCheckbox) target;
          break;
        case 3:
          this.btnResetLoadouts = (Button) target;
          this.btnResetLoadouts.Click += new RoutedEventHandler(this.btnResetLoadouts_Click);
          break;
        case 4:
          this.btnFixProfileFile = (Button) target;
          this.btnFixProfileFile.Click += new RoutedEventHandler(this.btnFixProfileFile_Click);
          break;
        case 5:
          this.btnCheckIntegrity = (Button) target;
          this.btnCheckIntegrity.Click += new RoutedEventHandler(this.btnCheckIntegrity_Click);
          break;
        case 6:
          this.btnLogOut = (Button) target;
          this.btnLogOut.Click += new RoutedEventHandler(this.BtnLogOut_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
