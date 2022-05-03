// Decompiled with JetBrains decompiler
// Type: Discord.ActivitySecrets
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System.Runtime.InteropServices;

namespace Discord
{
  public struct ActivitySecrets
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string Match;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string Join;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string Spectate;
  }
}
