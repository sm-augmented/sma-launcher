using Models.Exceptions;
using SMClient.Managers;
using SMClient.Models;
using SMClient.Models.Exceptions;
using SMClient.Tasks;
using SMClient.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace SMClient.Controls.LauncherWindow
{
    public partial class PlayScreen : UserControl, IComponentConnector
    {
        private bool isFirstLaunch;
        private Package launchedWith;
        private Package currentBranch;
        private Package versus;
        private Package exterminatus;
        private List<Package> allPackages;
        private List<Package> selectedPackages;
        public static readonly DependencyProperty IsButtonsEnabledProperty = DependencyProperty.Register(nameof(IsButtonsEnabled), typeof(bool), typeof(PlayScreen));
        private AddonInfo current;

        public event EventHandler IsButtonsEnabledChange;

        public bool IsButtonsEnabled
        {
            get => (bool)this.GetValue(PlayScreen.IsButtonsEnabledProperty);
            set => this.SetValue(PlayScreen.IsButtonsEnabledProperty, (object)value);
        }

        public PlayScreen()
        {
            this.allPackages = new List<Package>();
            this.selectedPackages = new List<Package>();
            this.InitializeComponent();
            this.exBr.IsEnabled = this.versusBr.IsEnabled = false;
        }

        private void UpdatePackagesList() => this.Dispatcher.Invoke((Action)(() =>
       {
           this.addonDescription.Text = "";
           this.packagesPanel.Children.Clear();
           this.selectedPackages.Clear();
           foreach (Package package in this.allPackages.Where<Package>((Func<Package, bool>)(x => x.Name == "Exterminatus-test" && this.currentBranch.Name == "Exterminatus" || x.Name == "Versus-test" && this.currentBranch.Name == "Versus" || x.Dependencies.Contains(this.currentBranch) || x.Dependencies.Count == 0)))
           {
               AddonInfo element = new AddonInfo(package);
               element.Checked += new EventHandler(this.AddInfo_Checked);
               element.Selected += new EventHandler(this.AddInfo_Selected);
               this.packagesPanel.Children.Add((UIElement)element);
               if (package.Name == "NoHUD" && Settings.Instance.UseNoHud)
                   element.checkbox.Enabled_MouseLeftButtonDown((object)null, (MouseButtonEventArgs)null);
           }
       }));

        private void AddInfo_Selected(object sender, EventArgs e)
        {
            AddonInfo ai = (AddonInfo)sender;
            if (this.current != null && this.current != ai)
                this.current.SetSelected(false);
            if (this.current != ai)
            {
                this.current = ai;
                ai.SetSelected(true);
            }
            this.Dispatcher.Invoke((Action)(() => this.addonDescription.Text = ai.Package.Description ?? "No description"));
        }

        private void AddInfo_Checked(object sender, EventArgs e)
        {
            CheckboxEventArgs checkboxEventArgs = (CheckboxEventArgs)e;
            if (checkboxEventArgs.Selected)
                this.selectedPackages.Add(checkboxEventArgs.Package);
            else if (this.selectedPackages.Contains(checkboxEventArgs.Package))
                this.selectedPackages.Remove(checkboxEventArgs.Package);
            if (!(checkboxEventArgs.Package.Name == "NoHUD"))
                return;
            Settings.Instance.UseNoHud = checkboxEventArgs.Selected;
            Settings.Instance.Save();
        }

        public void PackagesDownloaded()
        {
            this.exBr.IsEnabled = this.versusBr.IsEnabled = true;
            this.versus = this.currentBranch = PackageManager.Packages.FirstOrDefault<Package>((Func<Package, bool>)(x => x.Name == "Versus"));
            this.exterminatus = PackageManager.Packages.FirstOrDefault<Package>((Func<Package, bool>)(x => x.Name == "Exterminatus"));
            if (Settings.Instance.LastSelection == "versus")
                this.switchToVersus_MouseLeftButtonDown((object)null, (MouseButtonEventArgs)null);
            else
                this.switchToExt_MouseLeftButtonDown((object)null, (MouseButtonEventArgs)null);
            this.allPackages = PackageManager.Packages.Where<Package>((Func<Package, bool>)(x => x.IsVisible)).ToList<Package>();
            this.UpdatePackagesList();
        }

        private void StartGame(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                Logger.LogInfo("OnGameStart: Preparing data");
                ShowPreparing();

                try
                {
                    launchedWith = selectedPackages
                    .OrderByDescending(x => x.FlatPriority)
                    .FirstOrDefault(x => x.CountedAsBranch)
                    ?? currentBranch;

                    Unlocker.StartGame(currentBranch, launchedWith, selectedPackages);
                    ShowIngame();

                    try
                    {
                        RegisterIngameTask.RegisterPlayerIngame(launchedWith.Name, () => OnlineManager.Initialize());
                        DiscordManager.SetIngame(launchedWith?.Name ?? currentBranch.Name);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(new Exception("Unable to register ingame", ex));
                    }

                    await Unlocker.WaitForGame();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);

                    switch (ex)
                    {
                        case SteamNotRunningException _:
                            MessageBoxHelper.ShowError("Steam is not running!");
                            break;
                        case UnableToRestoreException _:
                            MessageBoxHelper.ShowError("Unable to restore backup! Manually rename -backup folders");
                            break;
                        case UnableToUnpackException _:
                            MessageBoxHelper.ShowError("Unable to unpack data. Check permissions on modview or preview folders");
                            break;
                        default:
                            MessageBoxHelper.ShowError("Unable to start. More info in loglog.log");
                            break;
                    }
                }

                Dispatcher.Invoke(() =>
                {
                    IsButtonsEnabledChange?.Invoke(true, null);
                    launchedWith = null;                    
                    MainWindow.CanBeClosed = true;
                    MainWindow.HideLoading();

                    try
                    {
                        RegisterIngameTask.RegisterPlayerIngame("Launcher", () => OnlineManager.Initialize());
                        DiscordManager.SetInLauncher();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(new Exception("Unable to register ingame", ex));
                    }
                });
            });
        }

        private void ShowIngame()
        {
            Dispatcher.InvokeAsync(() =>
            {
                IsButtonsEnabledChange?.Invoke(false, null);
                MainWindow.ShowLoading("Ingame...");
            });
        }

        private void ShowPreparing()
        {
            Dispatcher.Invoke(() =>
            {
                IsButtonsEnabledChange?.Invoke(false, null);
                MainWindow.ShowLoading("Preparing data...");
            });
        }

        private void switchToVersus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Settings.Instance.LastSelection = "versus";
            Settings.Instance.Save();
            this.currentBranch = this.versus;
            this.versusBr.Foreground = (Brush)new SolidColorBrush(Color.FromRgb((byte)225, (byte)165, (byte)26));
            this.exBr.Foreground = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
            UpdatePackagesList();
            if (e != null)
                e.Handled = true;
        }

        private void switchToExt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Settings.Instance.LastSelection = "exterminatus";
            Settings.Instance.Save();
            this.currentBranch = this.exterminatus;
            this.exBr.Foreground = (Brush)new SolidColorBrush(Color.FromRgb((byte)225, (byte)165, (byte)26));
            this.versusBr.Foreground = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
            UpdatePackagesList();
            if (e != null)
                e.Handled = true;
        }

        private void exBr_MouseEnter(object sender, MouseEventArgs e) => this.exBr.Foreground = (Brush)new SolidColorBrush(Color.FromRgb((byte)225, (byte)165, (byte)26));

        private void exBr_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.currentBranch != this.versus)
                return;
            this.exBr.Foreground = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
        }

        private void versusBr_MouseEnter(object sender, MouseEventArgs e) => this.versusBr.Foreground = (Brush)new SolidColorBrush(Color.FromRgb((byte)225, (byte)165, (byte)26));

        private void versusBr_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.currentBranch != this.exterminatus)
                return;
            this.versusBr.Foreground = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
        }
    }
}
