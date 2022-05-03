// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.LauncherWindow.OnlineBlock
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Data.Managers;
using SMClient.Data.Tasks;
using SMClient.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SMClient.Controls.LauncherWindow
{
  public class OnlineBlock : UserControl, IComponentConnector
  {
    internal StackPanel playerList;
    private bool _contentLoaded;

    public OnlineBlock()
    {
      this.InitializeComponent();
      OnlineManager.OnlineChecked += new OnlineCounterTask.OnlineChecked(this.OnlineManager_OnlineChecked);
    }

    private void OnlineManager_OnlineChecked(int total, Dictionary<string, UserPing> players) => this.Dispatcher.Invoke((Action) (() =>
    {
      this.playerList.Children.Clear();
      foreach (KeyValuePair<string, UserPing> player in players)
      {
        UIElementCollection children = this.playerList.Children;
        children.Add((UIElement) new PlayerInfo(player.Value.ID, player.Value.Branch, "")
        {
          Margin = new Thickness(0.0, 1.0, 0.0, 1.0)
        });
      }
    }));

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/SMClient;component/controls/components/onlineblock.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.playerList = (StackPanel) target;
      else
        this._contentLoaded = true;
    }
  }
}
