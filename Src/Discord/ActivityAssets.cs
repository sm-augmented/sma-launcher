// Decompiled with JetBrains decompiler
// Type: Discord.ActivityAssets
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System.Runtime.InteropServices;

namespace Discord
{
  public struct ActivityAssets
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string LargeImage;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string LargeText;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string SmallImage;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string SmallText;
  }
}
