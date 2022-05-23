// Decompiled with JetBrains decompiler
// Type: SMClient.MainWindow
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using LoadingIndicators.WPF;
using SMClient.Api;
using SMClient.Controls.LauncherWindow;
using SMClient.Data.Managers;
using SMClient.Data.Managers.IntegrationManagers;
using SMClient.Data.Tasks;
using Steamworks;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace SMClient
{
    public partial class MainWindow : Window, IComponentConnector
    {
        private static MainWindow instance;
        private static bool Once;
        private bool SteamInitializedFlag;

        private static LoadingIndicator LoadingIndicator { get; set; }

        private static Label LoadingIndicatorLabel { get; set; }

        public static void ShowLoading(string message) => MainWindow.instance.ShowLoadingInternal(message);

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
                this.InitializeComponent();

                Application.Current.MainWindow = (Window)this;
                MainWindow.instance = this;
                MainWindow.CanBeClosed = true;
                MainWindow.LoadingIndicator = this.loadingIndicator;
                MainWindow.LoadingIndicatorLabel = this.loadingIndicatorLabel;

                this.InitApplication();
                Task.Run((Action)(() => nonAuthorized.ContinueWithoutAuth()));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        private void InitApplication()
        {
            PersistenceManager.OnlineChecked += new PersistenceTask.AliveChecked(this.PersistenceManager_OnlineChecked);
            this.nonAuthorized.LoggedIn += new EventHandler(this.OnLoggedIn);
            this.authorized.LoggedOut += new EventHandler(this.Authorized_LoggedOut);
            SteamManager.SteamInitialized += new EventHandler<bool>(this.SteamManager_SteamInitialized);
            this.Oof();
            BaseApi.Initialize();
            Logger.Clear();
            //  Server integration disabled
            PersistenceManager.Initialize();

            //PersistenceManager.Initialize();
            DiscordManager.Initialize();
            SteamManager.Initialize();
            this.SteamInitializedFlag = false;
            if (!SteamManager.Initialized || this.SteamInitializedFlag)
                return;
            this.SteamManager_SteamInitialized((object)null, true);
        }

        private void SteamManager_SteamInitialized(object sender, bool e)
        {
            if (!SteamManager.Initialized)
                return;
            this.SteamInitializedFlag = true;
            SteamFriends.GetFriendGamePlayed(new CSteamID(76561198005900799UL), out FriendGameInfo_t _);
            bool flag1 = SteamRemoteStorage.FileExists("profile_Vers.bin");
            bool flag2 = SteamRemoteStorage.FileExists("profile_Exte.bin");
            if (!flag1 || !flag2)
            {
                int fileSize = SteamRemoteStorage.GetFileSize("profile_info.bin");
                byte[] pvData = new byte[fileSize];
                if (SteamRemoteStorage.FileRead("profile_info.bin", pvData, fileSize) > 0)
                {
                    if (!flag1)
                        SteamRemoteStorage.FileWrite("profile_Vers.bin", pvData, fileSize);
                    if (!flag2)
                        SteamRemoteStorage.FileWrite("profile_Exte.bin", pvData, fileSize);
                }
            }
            List<CSteamID> csteamIdList = new List<CSteamID>()
              {
                new CSteamID(76561198051576888UL),
                new CSteamID(76561198066546642UL),
                new CSteamID(76561198340333505UL)
              };
            if (MainWindow.Once || !csteamIdList.Contains(SteamUser.GetSteamID()))
                return;
            MainWindow.Once = true;
            int num = (int)MessageBox.Show("WHAT R U CASUL? LMAO GIT GUD");
            Dispatcher.CurrentDispatcher.Invoke((Action)(() => Application.Current.Shutdown()));
        }

        private void Authorized_LoggedOut(object sender, EventArgs e)
        {
            this.nonAuthorized.Visibility = Visibility.Visible;
            this.authorized.Visibility = Visibility.Collapsed;
        }

        private void OnLoggedIn(object sender, EventArgs args)
        {
            Dispatcher.Invoke(() =>
            {
                this.nonAuthorized.Visibility = Visibility.Collapsed;
                this.authorized.Visibility = Visibility.Visible;
                this.authorized.OnLoggedIn();

                DiscordManager.SetInLauncher();
            });            
        }

        private void PersistenceManager_OnlineChecked(bool isAlive, bool needUpdate)
        {
            if (!needUpdate)
                return;
            this.Dispatcher.Invoke((Action)(() => this.Close()));
        }

        private void Oof()
        {
            foreach (string enumerateFile in Directory.EnumerateFiles("."))
            {
                if (Path.GetExtension(enumerateFile) == ".pdb" && Path.GetFileNameWithoutExtension(enumerateFile) == "SMClientUpdater")
                    File.Delete(enumerateFile);
            }
        }

        private void btnMinimize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) => this.WindowState = WindowState.Minimized;

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception ex)
            {
            }
        }

        private void NotGrid_MouseDown(object sender, MouseButtonEventArgs e) => e.Handled = true;

        private void btnClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!MainWindow.CanBeClosed)
                return;
            this.Close();
        }

        private void btnMinimize_MouseEnter(object sender, MouseEventArgs e) => this.btnMinimize.Foreground = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, (byte)185, (byte)0));

        private void btnMinimize_MouseLeave(object sender, MouseEventArgs e) => this.btnMinimize.Foreground = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));

        private void btnClose_MouseEnter(object sender, MouseEventArgs e) => this.btnClose.Foreground = (Brush)new SolidColorBrush(Color.FromRgb((byte)180, (byte)0, (byte)0));

        private void btnClose_MouseLeave(object sender, MouseEventArgs e) => this.btnClose.Foreground = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, (byte)0, (byte)0));
    }
}
