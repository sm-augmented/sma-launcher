// Decompiled with JetBrains decompiler
// Type: SMClient.Utils.CheckboxEventArgs
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Models;
using System;

namespace SMClient.Utils
{
  public class CheckboxEventArgs : EventArgs
  {
    public bool Selected { get; set; }

    public Package Package { get; set; }
  }
}
