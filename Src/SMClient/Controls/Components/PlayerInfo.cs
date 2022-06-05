using System.Windows.Controls;
using System.Windows.Markup;

namespace SMClient.Controls
{
  public partial class PlayerInfo : UserControl, IComponentConnector
  {
    public PlayerInfo(string username, string branch, string status)
    {
      this.InitializeComponent();
      this.Username.Content = (object) (username ?? "");
      this.Branch.Content = (object) ("In " + branch);
      this.Status.Content = (object) (status ?? "");
    }
  }
}
