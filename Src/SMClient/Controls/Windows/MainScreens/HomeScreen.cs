using System.Windows.Controls;
using System.Windows.Markup;

namespace SMClient.Controls.LauncherWindow
{
    public partial class HomeScreen : UserControl, IComponentConnector
    {
        public HomeScreen()
        {
            this.InitializeComponent();
        }

        public void FillNews(string changelog)
        {
            this.ctrlChangelog.Text = changelog;
        }
    }
}
