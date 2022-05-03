// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.PlayerInfo
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SMClient.Controls
{
  public class PlayerInfo : UserControl, IComponentConnector
  {
    internal Label Username;
    internal Label Branch;
    internal Label Status;
    private bool _contentLoaded;

    public PlayerInfo(string username, string branch, string status)
    {
      this.InitializeComponent();
      this.Username.Content = (object) (username ?? "");
      this.Branch.Content = (object) ("In " + branch);
      this.Status.Content = (object) (status ?? "");
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/SMClient;component/controls/components/playerinfo.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.Username = (Label) target;
          break;
        case 2:
          this.Branch = (Label) target;
          break;
        case 3:
          this.Status = (Label) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
