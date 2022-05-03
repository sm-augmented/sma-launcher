// Decompiled with JetBrains decompiler
// Type: SMClient.Steam.SteamLobby
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Steamworks;
using System.Collections.Generic;

namespace SMClient.Steam
{
  public class SteamLobby
  {
    public CSteamID ID { get; set; }

    public Dictionary<string, string> Metadata { get; set; }

    public SteamLobby() => this.Metadata = new Dictionary<string, string>();
  }
}
