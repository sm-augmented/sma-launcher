// Decompiled with JetBrains decompiler
// Type: Discord.Activity
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System.Runtime.InteropServices;

namespace Discord
{
  public struct Activity
  {
    public ActivityType Type;
    public long ApplicationId;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string Name;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string State;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string Details;
    public ActivityTimestamps Timestamps;
    public ActivityAssets Assets;
    public ActivityParty Party;
    public ActivitySecrets Secrets;
    public bool Instance;
  }
}
