// Decompiled with JetBrains decompiler
// Type: Discord.Utility
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Runtime.InteropServices;

namespace Discord
{
  internal class Utility
  {
    internal static IntPtr Retain<T>(T value) => GCHandle.ToIntPtr(GCHandle.Alloc((object) value, GCHandleType.Normal));

    internal static void Release(IntPtr ptr) => GCHandle.FromIntPtr(ptr).Free();
  }
}
