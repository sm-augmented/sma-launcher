﻿// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.LauncherWindow.HomeScreen
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Data.Managers;
using SMClient.Models;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SMClient.Controls.LauncherWindow
{
    public partial class HomeScreen : UserControl, IComponentConnector
    {
        private NewsInfo current;

        public event EventHandler BranchChanged;

        public ImageSource NormalBackground { get; private set; }

        public ImageSource VersusBackground { get; private set; }

        public ImageSource ExtermBackground { get; private set; }

        public ImageSource LastBackground { get; private set; }

        public HomeScreen()
        {
            this.InitializeComponent();
            this.LastBackground = this.NormalBackground = (ImageSource)new BitmapImage(new Uri("pack://application:,,,/SMClient;component/Resources/ImageBackground4.png"));
            this.VersusBackground = (ImageSource)new BitmapImage(new Uri("pack://application:,,,/SMClient;component/Resources/ImageBackground4.png"));
            this.ExtermBackground = (ImageSource)new BitmapImage(new Uri("pack://application:,,,/SMClient;component/Resources/ImageBackground4.png"));
        }

        private async void GetNews()
        {
            HomeScreen homeScreen = this;
            var changelog = await NewsManager.GetChangelog();
            homeScreen.Dispatcher.Invoke((Action)(() =>
           {
               this.ctrlChangelog.Text = changelog;
           }));
        }

        private void NewsInfo_Click(object sender, EventArgs e)
        {
            NewsInfo newsInfo = (NewsInfo)sender;
            this.ctrlChangelog.Text = newsInfo.News.Text;
            if (this.current != null && this.current != newsInfo)
                this.current.SetSelected(false);
            if (this.current == newsInfo)
                return;
            this.current = newsInfo;
            newsInfo.SetSelected(true);
        }

        public void OnLoggedIn()
        {
            this.GetNews();
            PackageManager.Initialize().ContinueWith(new Action<Task>(this.OnMetadataLoaded));
        }

        private void OnMetadataLoaded(Task task)
        {
            OnlineManager.Initialize();
            this.Dispatcher.Invoke((Action)(() => { }));
        }
    }
}
