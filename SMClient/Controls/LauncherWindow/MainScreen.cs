// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.LauncherWindow.MainScreen
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Controls.Components;
using SMClient.Data.Managers;
using SMClient.Data.Managers.IntegrationManagers;
using SMClient.Data.Tasks;
using SMClient.Models;
using SMClient.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace SMClient.Controls.LauncherWindow
{
  public partial class MainScreen : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty StateLabelProperty = DependencyProperty.Register(nameof (StateLabel), typeof (string), typeof (MainScreen));
    public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof (Progress), typeof (double), typeof (MainScreen));
    public static readonly DependencyProperty IsButtonsEnabledProperty = DependencyProperty.Register(nameof (IsButtonsEnabled), typeof (bool), typeof (MainScreen));
    internal MainScreen root;
    internal Image imgBg;
    internal Grid screenContainer;
    internal HomeScreen homeScreen;
    internal PlayScreen playScreen;
    internal ProfileScreen profileScreen;
    internal SettingsScreen settingsScreen;
    internal TextBlock steamJoin;
    internal TextBlock discordJoin;
    internal Header header;
    private bool _contentLoaded;

    public event EventHandler LoggedOut;

    public string StateLabel
    {
      get => (string) this.GetValue(MainScreen.StateLabelProperty);
      set => this.SetValue(MainScreen.StateLabelProperty, (object) value);
    }

    public double Progress
    {
      get => (double) this.GetValue(MainScreen.ProgressProperty);
      set => this.SetValue(MainScreen.ProgressProperty, (object) value);
    }

    public bool IsButtonsEnabled
    {
      get => (bool) this.GetValue(MainScreen.IsButtonsEnabledProperty);
      set => this.SetValue(MainScreen.IsButtonsEnabledProperty, (object) value);
    }

    public MainScreen()
    {
      this.InitializeComponent();
      this.settingsScreen.LoggedOut += new EventHandler(this.SettingsScreen_LoggedOut);
      this.homeScreen.BranchChanged += new EventHandler(this.HomeScreen_BranchChanged);
      this.playScreen.BranchChanged += new EventHandler(this.PlayScreen_BranchChanged);
      OnlineManager.OnlineChecked += new OnlineCounterTask.OnlineChecked(this.OnlineManager_OnlineChecked);
      this.playScreen.IsButtonsEnabledChange += new EventHandler(this.PlayScreen_IsButtonsEnabledChange);
      this.IsButtonsEnabled = false;
      SteamManager.SteamInitialized += new EventHandler<bool>(this.SteamManager_SteamInitialized);
      Task.Run((Action) (() => this.CheckArchives()));
      if (!Settings.Instance.GamePathFine)
      {
        this.homeScreen.Visibility = Visibility.Collapsed;
        this.settingsScreen.Visibility = Visibility.Visible;
      }
      this.steamJoin.Foreground = (Brush) new SolidColorBrush(Color.FromRgb((byte) 225, (byte) 165, (byte) 26));
      this.discordJoin.Foreground = (Brush) new SolidColorBrush(Color.FromRgb((byte) 225, (byte) 165, (byte) 26));
    }

    private void SteamManager_SteamInitialized(object sender, bool initialized) => this.Dispatcher.Invoke((Action) (() =>
    {
      try
      {
        this.header.Username = OnlineManager.GetUsername(initialized);
      }
      catch (Exception ex)
      {
      }
    }));

    private void PlayScreen_IsButtonsEnabledChange(object sender, EventArgs e) => this.IsButtonsEnabled = (bool) sender;

    private void PlayScreen_BranchChanged(object sender, EventArgs e)
    {
    }

    private void OnlineManager_OnlineChecked(int total, Dictionary<string, UserPing> players) => this.Dispatcher.Invoke((Action) (() => { }));

    public void OnLoggedIn()
    {
      this.homeScreen.OnLoggedIn();
      this.playScreen.OnLoggedIn();
      this.profileScreen.OnLoggedIn();
      this.header.Username = OnlineManager.GetUsername(true);
      RegisterIngameTask.RegisterPlayerIngame("Launcher");
    }

    private async Task CheckArchives()
    {
      MainScreen mainScreen = this;
      try
      {
        while (PackageManager.Packages == null || !Settings.Instance.GamePathFine || !SteamManager.Initialized)
          Thread.Sleep(5000);
        ProfileInfo.RecreateModProfile();
        // ISSUE: reference to a compiler-generated method
        mainScreen.Dispatcher.Invoke(new Action(mainScreen.\u003CCheckArchives\u003Eb__21_0));
        await PackageManager.DownloadPackages(new ProgressChangedEventHandler(mainScreen.DownloadProgress), new EventHandler(mainScreen.DownloadComplete));
        // ISSUE: reference to a compiler-generated method
        mainScreen.Dispatcher.Invoke(new Action(mainScreen.\u003CCheckArchives\u003Eb__21_1));
      }
      catch (Exception ex)
      {
        Logger.LogError(ex);
        int num;
        mainScreen.Dispatcher.Invoke((Action) (() => num = (int) MessageBoxHelper.ShowError(ex.Message)));
      }
    }

    private void DownloadProgress(object sender, ProgressChangedEventArgs e) => this.Dispatcher.Invoke((Action) (() =>
    {
      this.Progress = (double) e.ProgressPercentage;
      this.StateLabel = string.Format("{0}%", (object) e.ProgressPercentage);
      this.header.SetProgress(this.Progress, false);
    }));

    private void DownloadComplete(object sender, EventArgs e) => this.Dispatcher.Invoke((Action) (() => this.StateLabel = "Unpacking..."));

    private void SettingsScreen_LoggedOut(object sender, EventArgs e)
    {
      EventHandler loggedOut = this.LoggedOut;
      if (loggedOut == null)
        return;
      loggedOut((object) null, (EventArgs) null);
    }

    private void HomeScreen_BranchChanged(object sender, EventArgs e)
    {
    }

    private void Header_HomeButtonClick(object sender, RoutedEventArgs e)
    {
      this.homeScreen.Visibility = Visibility.Visible;
      this.playScreen.Visibility = Visibility.Hidden;
      this.profileScreen.Visibility = Visibility.Hidden;
      this.settingsScreen.Visibility = Visibility.Hidden;
      e.Handled = true;
    }

    private void Header_PlayButtonClick(object sender, RoutedEventArgs e)
    {
      this.profileScreen.Visibility = Visibility.Hidden;
      this.playScreen.Visibility = Visibility.Visible;
      this.homeScreen.Visibility = Visibility.Hidden;
      this.settingsScreen.Visibility = Visibility.Hidden;
      e.Handled = true;
    }

    private void Header_SettingsButtonClick(object sender, RoutedEventArgs e)
    {
      this.profileScreen.Visibility = Visibility.Hidden;
      this.playScreen.Visibility = Visibility.Hidden;
      this.homeScreen.Visibility = Visibility.Hidden;
      this.settingsScreen.Visibility = Visibility.Visible;
      e.Handled = true;
    }

    private void Header_ProfileButtonClick(object sender, RoutedEventArgs e)
    {
      this.profileScreen.Visibility = Visibility.Visible;
      this.playScreen.Visibility = Visibility.Hidden;
      this.homeScreen.Visibility = Visibility.Hidden;
      this.settingsScreen.Visibility = Visibility.Hidden;
      e.Handled = true;
    }

    private void steamJoin_MouseDown(object sender, MouseButtonEventArgs e) => Process.Start("steam://friends/joinchat/103582791464672449");

    private void steamJoin_MouseEnter(object sender, MouseEventArgs e) => this.steamJoin.Foreground = (Brush) new SolidColorBrush(Color.FromRgb((byte) 225, (byte) 165, (byte) 26));

    private void steamJoin_MouseLeave(object sender, MouseEventArgs e) => this.steamJoin.Foreground = (Brush) new SolidColorBrush(Color.FromRgb((byte) 232, (byte) 140, (byte) 0));

    private void discordJoin_MouseDown(object sender, MouseButtonEventArgs e) => Process.Start("https://discord.com/invite/nHRraDJ");

    private void discordJoin_MouseEnter(object sender, MouseEventArgs e) => this.discordJoin.Foreground = (Brush) new SolidColorBrush(Color.FromRgb((byte) 225, (byte) 165, (byte) 26));

    private void discordJoin_MouseLeave(object sender, MouseEventArgs e) => this.discordJoin.Foreground = (Brush) new SolidColorBrush(Color.FromRgb((byte) 232, (byte) 140, (byte) 0));

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/SMClient;component/controls/windows/mainscreen.xaml", UriKind.Relative));
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
          this.root = (MainScreen) target;
          break;
        case 2:
          this.imgBg = (Image) target;
          break;
        case 3:
          this.screenContainer = (Grid) target;
          break;
        case 4:
          this.homeScreen = (HomeScreen) target;
          break;
        case 5:
          this.playScreen = (PlayScreen) target;
          break;
        case 6:
          this.profileScreen = (ProfileScreen) target;
          break;
        case 7:
          this.settingsScreen = (SettingsScreen) target;
          break;
        case 8:
          this.steamJoin = (TextBlock) target;
          this.steamJoin.MouseDown += new MouseButtonEventHandler(this.steamJoin_MouseDown);
          this.steamJoin.MouseEnter += new MouseEventHandler(this.steamJoin_MouseEnter);
          this.steamJoin.MouseLeave += new MouseEventHandler(this.steamJoin_MouseLeave);
          break;
        case 9:
          this.discordJoin = (TextBlock) target;
          this.discordJoin.MouseDown += new MouseButtonEventHandler(this.discordJoin_MouseDown);
          this.discordJoin.MouseEnter += new MouseEventHandler(this.discordJoin_MouseEnter);
          this.discordJoin.MouseLeave += new MouseEventHandler(this.discordJoin_MouseLeave);
          break;
        case 10:
          this.header = (Header) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
