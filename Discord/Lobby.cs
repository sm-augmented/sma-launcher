// Decompiled with JetBrains decompiler
// Type: Discord.Lobby
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System.Runtime.InteropServices;

namespace Discord
{
  public struct Lobby
  {
    public long Id;
    public LobbyType Type;
    public long OwnerId;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string Secret;
    public uint Capacity;
    public bool Locked;
  }
}
