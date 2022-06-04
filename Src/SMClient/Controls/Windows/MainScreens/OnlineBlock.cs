using SMClient.Managers;
using SMClient.Models;
using SMClient.Tasks;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SMClient.Controls.LauncherWindow
{
    public partial class OnlineBlock : UserControl, IComponentConnector
    {
        public OnlineBlock()
        {
            this.InitializeComponent();
            OnlineManager.OnlineChecked += new OnlineCounterTask.OnlineChecked(this.OnlineManager_OnlineChecked);
        }

        private void OnlineManager_OnlineChecked(Dictionary<string, UserPing> players) => this.Dispatcher.Invoke((Action)(() =>
        {
            this.playerList.Children.Clear();
            foreach (KeyValuePair<string, UserPing> player in players)
            {
                UIElementCollection children = this.playerList.Children;
                children.Add((UIElement)new PlayerInfo(player.Value.ID, player.Value.Branch, "")
                {
                    Margin = new Thickness(0.0, 1.0, 0.0, 1.0)
                });
            }
        }));
    }
}
