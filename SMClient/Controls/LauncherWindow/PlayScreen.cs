// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.LauncherWindow.PlayScreen
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Data.Managers;
using SMClient.Data.Managers.DataManagers;
using SMClient.Models;
using SMClient.Models.Exceptions;
using SMClient.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SMClient.Controls.LauncherWindow
{
  public class PlayScreen : UserControl, IComponentConnector
  {
    public ImageSource VersusBackground;
    public ImageSource ExterminatusBackground;
    private bool isFirstLaunch;
    private Package launchedWith;
    private Package currentBranch;
    private Package versus;
    private Package exterminatus;
    private List<Package> allPackages;
    private List<Package> selectedPackages;
    public static readonly DependencyProperty IsButtonsEnabledProperty = DependencyProperty.Register(nameof (IsButtonsEnabled), typeof (bool), typeof (PlayScreen));
    private AddonInfo current;
    private bool PackagesDownloadedFlag;
    internal PlayScreen root;
    internal Label versusBr;
    internal Label exBr;
    internal TextBlock addonDescription;
    internal StackPanel packagesPanel;
    private bool _contentLoaded;

    public event EventHandler BranchChanged;

    public event EventHandler IsButtonsEnabledChange;

    public bool IsButtonsEnabled
    {
      get => (bool) this.GetValue(PlayScreen.IsButtonsEnabledProperty);
      set => this.SetValue(PlayScreen.IsButtonsEnabledProperty, (object) value);
    }

    public PlayScreen()
    {
      this.allPackages = new List<Package>();
      this.selectedPackages = new List<Package>();
      this.InitializeComponent();
      Unlocker.OnException += new EventHandler(this.Unlocker_OnException);
      Unlocker.OnGameExit += new EventHandler(this.Unlocker_OnGameExit);
      Unlocker.OnGameStarted += new EventHandler(this.Unlocker_OnGameStarted);
      this.BranchChanged += new EventHandler(this.PlayScreen_BranchChanged);
      this.VersusBackground = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/SMClient;component/Resources/ImageBackground1.png"));
      this.ExterminatusBackground = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/SMClient;component/Resources/ImageBackground3.png"));
      this.exBr.IsEnabled = this.versusBr.IsEnabled = false;
      this.PackagesDownloadedFlag = false;
      PackageManager.PackagesDownloaded += new EventHandler(this.PackageManager_PackagesDownloaded);
      if (!PackageManager.IsPackagesDownloaded || this.PackagesDownloadedFlag)
        return;
      this.PackageManager_PackagesDownloaded((object) null, (EventArgs) null);
    }

    private void PlayScreen_BranchChanged(object sender, EventArgs e) => this.UpdatePackagesList();

    private void UpdatePackagesList() => this.Dispatcher.Invoke((Action) (() =>
    {
      this.addonDescription.Text = "";
      this.packagesPanel.Children.Clear();
      this.selectedPackages.Clear();
      foreach (Package package in this.allPackages.Where<Package>((Func<Package, bool>) (x => x.Name == "Exterminatus-test" && this.currentBranch.Name == "Exterminatus" || x.Name == "Versus-test" && this.currentBranch.Name == "Versus" || x.Dependencies.Contains(this.currentBranch) || x.Dependencies.Count == 0)))
      {
        AddonInfo element = new AddonInfo(package);
        element.Checked += new EventHandler(this.AddInfo_Checked);
        element.Selected += new EventHandler(this.AddInfo_Selected);
        this.packagesPanel.Children.Add((UIElement) element);
        if (package.Name == "NoHUD" && Settings.Instance.UseNoHud)
          element.checkbox.Enabled_MouseLeftButtonDown((object) null, (MouseButtonEventArgs) null);
      }
    }));

    private void AddInfo_Selected(object sender, EventArgs e)
    {
      AddonInfo ai = (AddonInfo) sender;
      if (this.current != null && this.current != ai)
        this.current.SetSelected(false);
      if (this.current != ai)
      {
        this.current = ai;
        ai.SetSelected(true);
      }
      this.Dispatcher.Invoke((Action) (() => this.addonDescription.Text = ai.Package.Description ?? "No description"));
    }

    private void AddInfo_Checked(object sender, EventArgs e)
    {
      CheckboxEventArgs checkboxEventArgs = (CheckboxEventArgs) e;
      if (checkboxEventArgs.Selected)
        this.selectedPackages.Add(checkboxEventArgs.Package);
      else if (this.selectedPackages.Contains(checkboxEventArgs.Package))
        this.selectedPackages.Remove(checkboxEventArgs.Package);
      if (!(checkboxEventArgs.Package.Name == "NoHUD"))
        return;
      Settings.Instance.UseNoHud = checkboxEventArgs.Selected;
      Settings.Instance.Save();
    }

    private void PackageManager_PackagesDownloaded(object sender, EventArgs e)
    {
      this.PackagesDownloadedFlag = true;
      this.Dispatcher.Invoke((Action) (() => this.exBr.IsEnabled = this.versusBr.IsEnabled = true));
      this.versus = this.currentBranch = PackageManager.Packages.FirstOrDefault<Package>((Func<Package, bool>) (x => x.Name == "Versus"));
      this.exterminatus = PackageManager.Packages.FirstOrDefault<Package>((Func<Package, bool>) (x => x.Name == "Exterminatus"));
      this.Dispatcher.Invoke((Action) (() =>
      {
        if (Settings.Instance.LastSelection == null)
          return;
        if (Settings.Instance.LastSelection == "versus")
          this.switchToVersus_MouseLeftButtonDown((object) null, (MouseButtonEventArgs) null);
        else
          this.switchToExt_MouseLeftButtonDown((object) null, (MouseButtonEventArgs) null);
      }));
      this.allPackages = PackageManager.Packages.Where<Package>((Func<Package, bool>) (x => x.IsVisible)).ToList<Package>();
      this.UpdatePackagesList();
    }

    private void OnGameStart(object sender, EventArgs e)
    {
      MainWindow.CanBeClosed = false;
      EventHandler buttonsEnabledChange = this.IsButtonsEnabledChange;
      if (buttonsEnabledChange != null)
        buttonsEnabledChange((object) false, (EventArgs) null);
      MainWindow.ShowLoading("Preparing data...");
      Logger.LogInfo("OnGameStart: Preparing data");
      try
      {
        this.launchedWith = this.selectedPackages.OrderByDescending<Package, int>((Func<Package, int>) (x => x.FlatPriority)).FirstOrDefault<Package>((Func<Package, bool>) (x => x.CountedAsBranch)) ?? this.currentBranch;
        Task.Run((Action) (() =>
        {
          Unlocker.StartGame(this.currentBranch, this.launchedWith, this.selectedPackages);
          DiscordManager.SetIngame(this.launchedWith?.Name ?? this.currentBranch.Name);
        }));
      }
      catch (Exception ex)
      {
        Logger.LogError(ex);
        this.Unlocker_OnException((object) ex, (EventArgs) null);
      }
    }

    private void Unlocker_OnGameStarted(object sender, EventArgs e) => this.Dispatcher.Invoke((Action) (() =>
    {
      EventHandler buttonsEnabledChange = this.IsButtonsEnabledChange;
      if (buttonsEnabledChange != null)
        buttonsEnabledChange((object) false, (EventArgs) null);
      MainWindow.ShowLoading("Ingame");
    }));

    private void Unlocker_OnGameExit(object sender, EventArgs e) => this.GameExited();

    private void UnableToRestore(UnableToRestoreException ex)
    {
      if (MessageBoxHelper.ShowError(ex.Message + ". Try again?", type: MessageBoxButton.YesNo) != MessageBoxResult.Yes)
        return;
      try
      {
        UnpackManager.RestoreData();
      }
      catch (Exception ex1)
      {
        UnableToRestoreException ex2 = new UnableToRestoreException("Unable to restore backup data", ex1);
        Logger.LogError((Exception) ex2);
        this.UnableToRestore(ex2);
      }
    }

    private void Unlocker_OnException(object sender, EventArgs e)
    {
      Exception ex = (Exception) sender;
      switch (ex)
      {
        case SteamNotRunningException _:
          int num;
          this.Dispatcher.Invoke((Action) (() => num = (int) MessageBoxHelper.ShowError("Steam is not running!")));
          break;
        case UnableToRestoreException _:
          this.UnableToRestore((UnableToRestoreException) ex);
          break;
      }
      this.GameExited();
    }

    private void GameExited()
    {
      this.Dispatcher.Invoke((Action) (() =>
      {
        EventHandler buttonsEnabledChange = this.IsButtonsEnabledChange;
        if (buttonsEnabledChange == null)
          return;
        buttonsEnabledChange((object) true, (EventArgs) null);
      }));
      this.launchedWith = (Package) null;
      DiscordManager.SetInLauncher();
      MainWindow.CanBeClosed = true;
      MainWindow.HideLoading();
    }

    public void OnLoggedIn()
    {
    }

    private void switchToVersus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Settings.Instance.LastSelection = "versus";
      Settings.Instance.Save();
      this.currentBranch = this.versus;
      this.versusBr.Foreground = (Brush) new SolidColorBrush(Color.FromRgb((byte) 225, (byte) 165, (byte) 26));
      this.exBr.Foreground = (Brush) new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
      EventHandler branchChanged = this.BranchChanged;
      if (branchChanged != null)
        branchChanged((object) null, (EventArgs) null);
      if (e == null)
        return;
      e.Handled = true;
    }

    private void switchToExt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Settings.Instance.LastSelection = "exterminatus";
      Settings.Instance.Save();
      this.currentBranch = this.exterminatus;
      this.exBr.Foreground = (Brush) new SolidColorBrush(Color.FromRgb((byte) 225, (byte) 165, (byte) 26));
      this.versusBr.Foreground = (Brush) new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
      EventHandler branchChanged = this.BranchChanged;
      if (branchChanged != null)
        branchChanged((object) null, (EventArgs) null);
      if (e == null)
        return;
      e.Handled = true;
    }

    private void exBr_MouseEnter(object sender, MouseEventArgs e) => this.exBr.Foreground = (Brush) new SolidColorBrush(Color.FromRgb((byte) 225, (byte) 165, (byte) 26));

    private void exBr_MouseLeave(object sender, MouseEventArgs e)
    {
      if (this.currentBranch != this.versus)
        return;
      this.exBr.Foreground = (Brush) new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
    }

    private void versusBr_MouseEnter(object sender, MouseEventArgs e) => this.versusBr.Foreground = (Brush) new SolidColorBrush(Color.FromRgb((byte) 225, (byte) 165, (byte) 26));

    private void versusBr_MouseLeave(object sender, MouseEventArgs e)
    {
      if (this.currentBranch != this.exterminatus)
        return;
      this.versusBr.Foreground = (Brush) new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/SMClient;component/controls/windows/mainscreens/playscreen.xaml", UriKind.Relative));
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
          this.root = (PlayScreen) target;
          break;
        case 2:
          this.versusBr = (Label) target;
          this.versusBr.MouseEnter += new MouseEventHandler(this.versusBr_MouseEnter);
          this.versusBr.MouseLeave += new MouseEventHandler(this.versusBr_MouseLeave);
          this.versusBr.MouseLeftButtonDown += new MouseButtonEventHandler(this.switchToVersus_MouseLeftButtonDown);
          break;
        case 3:
          this.exBr = (Label) target;
          this.exBr.MouseEnter += new MouseEventHandler(this.exBr_MouseEnter);
          this.exBr.MouseLeave += new MouseEventHandler(this.exBr_MouseLeave);
          this.exBr.MouseLeftButtonDown += new MouseButtonEventHandler(this.switchToExt_MouseLeftButtonDown);
          break;
        case 4:
          this.addonDescription = (TextBlock) target;
          break;
        case 5:
          this.packagesPanel = (StackPanel) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
