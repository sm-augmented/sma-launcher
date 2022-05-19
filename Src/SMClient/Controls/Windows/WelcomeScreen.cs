// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.LauncherWindow.WelcomeScreen
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Api;
using SMClient.Data.Managers;
using SMClient.Data.Managers.IntegrationManagers;
using SMClient.Data.Tasks;
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
using System.Windows.Markup;

namespace SMClient.Controls.LauncherWindow
{
    public partial class WelcomeScreen : UserControl, IComponentConnector
    {
        private bool steamInitAttempted;

        public event EventHandler LoggedIn;

        public static bool IsSteamRunning => ((IEnumerable<Process>)Process.GetProcessesByName("Steam")).FirstOrDefault<Process>() != null;

        public WelcomeScreen()
        {
            this.InitializeComponent();
            //PersistenceManager.OnlineChecked += new PersistenceTask.AliveChecked(this.PersistenceManager_OnlineChecked);
            SteamManager.SteamInitialized += new EventHandler<bool>(this.SteamManager_SteamInitialized);
        }

        private void SteamManager_SteamInitialized(object sender, bool e) => this.steamInitAttempted = true;

        private void PersistenceManager_OnlineChecked(bool isAlive, bool needUpdate)
        {
            if (OnlineManager.Account != null)
                return;
            this.SetOnline(isAlive);
        }

        public void SetOnline(bool isAlive)
        {
            //if (isAlive)
                
            //else
            //    this.Dispatcher.Invoke((Action)(() =>
            //   {
            //       int num = (int)MessageBoxHelper.ShowError("Cannot connect to master server");
            //       Application.Current.Shutdown();
            //   }));
        }

        private void AfterLogin()
        {
            Settings.Instance.Password = (string)null;
            Settings.Instance.Save();
            EventHandler loggedIn = this.LoggedIn;
            if (loggedIn == null)
                return;
            loggedIn((object)null, (EventArgs)null);
        }

        private async Task WaitForSteamInit()
        {
            do
                ;
            while (!this.steamInitAttempted);
        }

        public async Task ContinueWithoutAuth()
        {
            if (!WelcomeScreen.IsSteamRunning)
            {
                Dispatcher.Invoke((Action)(() =>
                {
                    int num = (int)MessageBoxHelper.ShowError("Steam is not running");
                    Application.Current.Shutdown();
                }));
            }
            else
            {
                MainWindow.ShowLoading("Signing in...");
                await WaitForSteamInit();
                MainWindow.HideLoading();
                AfterLogin();
                // ISSUE: reference to a compiler-generated method
                //welcomeScreen.Dispatcher.Invoke(new Action(welcomeScreen.\u003CAuthorizeFromSettings\u003Eb__12_1));
            }
        }

        public async Task AuthorizeFromSettings()
        {
            WelcomeScreen welcomeScreen = this;
            string login = Settings.Instance.Login;
            string password = Settings.Instance.Password;
            string token = Settings.Instance.AccessToken;
            if (!WelcomeScreen.IsSteamRunning)
            {
                welcomeScreen.Dispatcher.Invoke((Action)(() =>
               {
                   int num = (int)MessageBoxHelper.ShowError("Steam is not running");
                   Application.Current.Shutdown();
               }));
                token = (string)null;
            }
            else
            {
                MainWindow.ShowLoading("Signing in...");
                await welcomeScreen.WaitForSteamInit();
                bool flag = false;
                if (!string.IsNullOrEmpty(token))
                {
                    BaseApi.SetToken();
                    flag = await OnlineManager.AuthenticateByToken();
                }
                if (!flag)
                {
                    int num = await OnlineManager.AuthenticateBySteam() ? 1 : 0;
                }
                MainWindow.HideLoading();
                // ISSUE: reference to a compiler-generated method
                //welcomeScreen.Dispatcher.Invoke(new Action(welcomeScreen.\u003CAuthorizeFromSettings\u003Eb__12_1));
                token = (string)null;
            }
        }
    }
}
