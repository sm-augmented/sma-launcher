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
