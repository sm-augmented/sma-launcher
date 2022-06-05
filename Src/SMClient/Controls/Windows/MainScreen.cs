using SMClient.Managers;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace SMClient.Controls.LauncherWindow
{
    public partial class MainScreen : UserControl, IComponentConnector
    {
        public static readonly DependencyProperty StateLabelProperty = DependencyProperty.Register(nameof(StateLabel), typeof(string), typeof(MainScreen));
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress), typeof(double), typeof(MainScreen));
        public static readonly DependencyProperty IsButtonsEnabledProperty = DependencyProperty.Register(nameof(IsButtonsEnabled), typeof(bool), typeof(MainScreen));

        public string StateLabel
        {
            get => (string)this.GetValue(MainScreen.StateLabelProperty);
            set => this.SetValue(MainScreen.StateLabelProperty, (object)value);
        }

        public double Progress
        {
            get => (double)this.GetValue(MainScreen.ProgressProperty);
            set => this.SetValue(MainScreen.ProgressProperty, (object)value);
        }

        public bool IsButtonsEnabled
        {
            get => (bool)this.GetValue(MainScreen.IsButtonsEnabledProperty);
            set => this.SetValue(MainScreen.IsButtonsEnabledProperty, (object)value);
        }

        public MainScreen()
        {
            this.InitializeComponent();

            this.playScreen.IsButtonsEnabledChange += new EventHandler(this.PlayScreen_IsButtonsEnabledChange);
            this.IsButtonsEnabled = false;

            if (!Settings.Instance.GamePathFine)
            {
                this.homeScreen.Visibility = Visibility.Collapsed;
                this.settingsScreen.Visibility = Visibility.Visible;
            }

            this.steamJoin.Foreground = (Brush)new SolidColorBrush(Color.FromRgb((byte)225, (byte)165, (byte)26));
            this.discordJoin.Foreground = (Brush)new SolidColorBrush(Color.FromRgb((byte)225, (byte)165, (byte)26));
        }

        private void PlayScreen_IsButtonsEnabledChange(object sender, EventArgs e) => this.IsButtonsEnabled = (bool)sender;

        public void OnLoggedIn()
        {
            this.header.Username = AuthManager.GetUsername(true);
        }

        public void InitializeProgress()
        {
            header.SetProgress(0, false);
            this.IsButtonsEnabled = false;
        }

        public void FinishProgress()
        {
            header.SetProgress(100, true);
            this.IsButtonsEnabled = true;
        }

        public void DownloadProgress(int percentage)
        {
            Progress = percentage;
            StateLabel = string.Format("{0}%", percentage);
            header.SetProgress(Progress, false);
        }

        public void DownloadComplete(object sender, EventArgs e) => this.Dispatcher.Invoke((Action)(() => this.StateLabel = "Unpacking..."));

        private void Header_HomeButtonClick(object sender, RoutedEventArgs e)
        {
            this.homeScreen.Visibility = Visibility.Visible;
            this.playScreen.Visibility = Visibility.Hidden;
            this.settingsScreen.Visibility = Visibility.Hidden;
            e.Handled = true;
        }

        private void Header_PlayButtonClick(object sender, RoutedEventArgs e)
        {
            this.playScreen.Visibility = Visibility.Visible;
            this.homeScreen.Visibility = Visibility.Hidden;
            this.settingsScreen.Visibility = Visibility.Hidden;
            e.Handled = true;
        }

        private void Header_SettingsButtonClick(object sender, RoutedEventArgs e)
        {
            this.playScreen.Visibility = Visibility.Hidden;
            this.homeScreen.Visibility = Visibility.Hidden;
            this.settingsScreen.Visibility = Visibility.Visible;
            e.Handled = true;
        }

        private void steamJoin_MouseDown(object sender, MouseButtonEventArgs e) => Process.Start("steam://friends/joinchat/103582791464672449");

        private void steamJoin_MouseEnter(object sender, MouseEventArgs e) => this.steamJoin.Foreground = (Brush)new SolidColorBrush(Color.FromRgb((byte)225, (byte)165, (byte)26));

        private void steamJoin_MouseLeave(object sender, MouseEventArgs e) => this.steamJoin.Foreground = (Brush)new SolidColorBrush(Color.FromRgb((byte)232, (byte)140, (byte)0));

        private void discordJoin_MouseDown(object sender, MouseButtonEventArgs e) => Process.Start("https://discord.com/invite/e9Af4gEc2Y");

        private void discordJoin_MouseEnter(object sender, MouseEventArgs e) => this.discordJoin.Foreground = (Brush)new SolidColorBrush(Color.FromRgb((byte)225, (byte)165, (byte)26));

        private void discordJoin_MouseLeave(object sender, MouseEventArgs e) => this.discordJoin.Foreground = (Brush)new SolidColorBrush(Color.FromRgb((byte)232, (byte)140, (byte)0));
    }
}
