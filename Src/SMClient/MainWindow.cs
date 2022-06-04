using LoadingIndicators.WPF;
using SMClient.Api;
using Steamworks;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using SMClient.Utils;
using Managers;
using System.Diagnostics;
using SMClient.Managers;
using SMClient.Tasks;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SMClient
{
    public partial class MainWindow : Window, IComponentConnector
    {
        private static MainWindow instance;

        public static MainWindow Instance { get => instance; }

        private static LoadingIndicator LoadingIndicator { get; set; }

        private static Label LoadingIndicatorLabel { get; set; }

        public static void ShowLoading(string message) 
        {
            MainWindow.instance.ShowLoadingInternal(message);
        } 

        private void ShowLoadingInternal(string message) => this.Dispatcher.Invoke((Action)(() =>
       {
           MainWindow.CanBeClosed = false;
           MainWindow.LoadingIndicator.IsActive = true;
           MainWindow.LoadingIndicatorLabel.Visibility = Visibility.Visible;
           MainWindow.LoadingIndicatorLabel.Content = (object)message;
       }));

        public static void HideLoading() => MainWindow.instance.HideLoadingInternal();

        private void HideLoadingInternal() => this.Dispatcher.Invoke((Action)(() =>
       {
           MainWindow.CanBeClosed = true;
           MainWindow.LoadingIndicator.IsActive = false;
           MainWindow.LoadingIndicatorLabel.Visibility = Visibility.Collapsed;
           MainWindow.LoadingIndicatorLabel.Content = (object)"";
       }));

        public static bool CanBeClosed { get; set; }

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                instance = this;
                CanBeClosed = true;
                LoadingIndicator = loadingIndicator;
                LoadingIndicatorLabel = loadingIndicatorLabel;

                InitApplication();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                ShowErrorAndExit("Unable to initialize the launcher!");
            }
        }

        private void ShowErrorAndExit(string message)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBoxHelper.ShowError(message);
                Application.Current.Shutdown();
            });
        }

        private void InitApplication()
        {
            Task.Run(async () =>
            {
                ShowLoading("Initializing...");

                var okay = BaseApi.Initialize();
                if (!okay)
                {
                    ShowErrorAndExit("Unable to init API!");
                    return;
                }

                okay = SteamManager.Initialize();
                if (!okay)
                {
                    ShowErrorAndExit("Steam initialization error!");
                    return;
                }

                okay = ProfileInfoManager.CheckVanillaPlayed();
                if (!okay)
                {
                    ShowErrorAndExit("Play vanilla at least once!");
                    return;
                }

                ShowLoading("Checking for updates...");
                await UpdateManager.CheckAutoupdater();
                var hasUpdate = await UpdateManager.CheckUpdate();
                if (hasUpdate)
                {
                    Process.Start("SMClientUpdater.exe", "");
                    Application.Current.Shutdown();
                    return;
                }

                ShowLoading("Authenticating...");
                okay = await AuthManager.Authenticate();
                if (!okay)
                {
                    ShowErrorAndExit("Authentication error!");
                    return;
                }

                DiscordManager.Initialize();

                HideLoading();
                OnLoggedIn();

                RegisterIngameTask.RegisterPlayerIngame("Launcher", () =>
                {
                    OnlineManager.Initialize();
                });

                DiscordManager.SetInLauncher();

                try
                {
                    var changelog = await NewsManager.GetChangelog();
                    FillChangelog(changelog);
                }
                catch (Exception ex)
                {

                }

                InitializeProgress();

                okay = await PackageManager.Initialize(
                    new ProgressChangedEventHandler(DownloadProgress));
                if (!okay)
                {
                    ShowErrorAndExit("Packages loading error!");
                    return;
                }

                FinishProgress();
            });
        }

        private void OnLoggedIn()
        {
            Dispatcher.Invoke(() =>
            {
                nonAuthorized.Visibility = Visibility.Collapsed;
                authorized.Visibility = Visibility.Visible;
                authorized.OnLoggedIn();
            });
        }

        private void FillChangelog(string changelog)
        {
            Dispatcher.Invoke(() =>
            {
                authorized.homeScreen.FillNews(changelog);
            });
        }

        private void InitializeProgress()
        {
            Dispatcher.Invoke(() =>
            {
                authorized.InitializeProgress();
            });
        }

        private void FinishProgress()
        {
            Dispatcher.Invoke(() =>
            {
                authorized.playScreen.PackagesDownloaded();
                authorized.FinishProgress();
            });
        }

        private void DownloadProgress(object sender, ProgressChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                authorized.DownloadProgress(e.ProgressPercentage);
            });
        }

        private void btnMinimize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) => this.WindowState = WindowState.Minimized;

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex)
            {
            }
        }

        private void NotGrid_MouseDown(object sender, MouseButtonEventArgs e) => e.Handled = true;

        private void btnClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!CanBeClosed)
                return;
            Close();
        }

        private void btnMinimize_MouseEnter(object sender, MouseEventArgs e) => this.btnMinimize.Foreground = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, (byte)185, (byte)0));

        private void btnMinimize_MouseLeave(object sender, MouseEventArgs e) => this.btnMinimize.Foreground = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));

        private void btnClose_MouseEnter(object sender, MouseEventArgs e) => this.btnClose.Foreground = (Brush)new SolidColorBrush(Color.FromRgb((byte)180, (byte)0, (byte)0));

        private void btnClose_MouseLeave(object sender, MouseEventArgs e) => this.btnClose.Foreground = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, (byte)0, (byte)0));
    }
}
