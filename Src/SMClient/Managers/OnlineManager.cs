using SMClient.Models;
using SMClient.Tasks;
using System.Collections.Generic;

namespace SMClient.Managers
{
    public static class OnlineManager
  {
    public static Dictionary<string, UserPing> Players { get; set; }

    public static event OnlineCounterTask.OnlineChecked OnlineChecked;

    public static void Initialize() => OnlineCounterTask.CheckOnlineCount(new OnlineCounterTask.OnlineChecked(OnlineManager.Checked));

    private static void Checked(Dictionary<string, UserPing> players)
    {
      Players = players;
      OnlineCounterTask.OnlineChecked onlineChecked = OnlineManager.OnlineChecked;
      if (onlineChecked == null)
        return;
      onlineChecked(players);
    }
  }
}
