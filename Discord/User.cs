// Decompiled with JetBrains decompiler
// Type: Discord.User
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System.Runtime.InteropServices;

namespace Discord
{
  public struct User
  {
    public long Id;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string Username;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
    public string Discriminator;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string Avatar;
    public bool Bot;
  }
}
