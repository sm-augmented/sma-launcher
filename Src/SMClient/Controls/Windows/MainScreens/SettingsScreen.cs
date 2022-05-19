using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using SMClient.Data.Managers;
using SMClient.Models;

namespace SMClient.Controls.LauncherWindow
{
    public partial class SettingsScreen : UserControl, IComponentConnector
    {
        public ImageSource NormalBackground;

        public event EventHandler LoggedOut;

        public SettingsScreen()
        {
            InitializeComponent();
            NormalBackground = new BitmapImage(new Uri("pack://application:,,,/SMClient;component/Resources/ImageBackground4.png"));
            GameDir.OnPathSelected += new EventHandler(this.GameDir_PathSelected);
            GameDir.SelectedDirectory = Settings.Instance.GamePath;
            cbOldLauchWay.IsChecked = Settings.Instance.OldLaunchWay;
            cbWindowed.IsChecked = Settings.Instance.StartWindowed;
        }

        private void ToggleButtonsEnabled(bool enabled) => this.Dispatcher.Invoke((Action)(() => btnCheckIntegrity.IsEnabled = enabled));

        private void GameDir_PathSelected(object sender, EventArgs e)
        {
            string str = Settings.Instance.GamePath ?? "";
            Settings.Instance.GamePath = GameDir.SelectedDirectory;
            if (Settings.Instance.GamePathFine)
            {
                Settings.Instance.Save();
            }
            else
            {
                Settings.Instance.GamePath = GameDir.SelectedDirectory = str;
                MessageBox.Show("Wrong path selected");
            }
        }

        private void btnCheckIntegrity_Click(object sender, RoutedEventArgs e)
        {
            ToggleButtonsEnabled(false);
            MainWindow.ShowLoading("Processing...");
            Task.Run((() =>
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
               ToggleButtonsEnabled(true);
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

        private void cbWindowed_Checked(object sender, EventArgs e)
        {
            Settings.Instance.StartWindowed = this.cbWindowed.IsChecked;
            Settings.Instance.Save();
        }

        private void btnFixProfileFile_Click(object sender, RoutedEventArgs e) => ProfileInfo.RecreateModProfile(true);
    }
}
