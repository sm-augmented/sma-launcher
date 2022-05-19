// Decompiled with JetBrains decompiler
// Type: SMClient.Models.UserPing
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;

namespace SMClient.Models
{
  public class UserPing
  {
    public string ID { get; set; }

    public string Branch { get; set; }

    public string Status { get; set; }

    public DateTime Pinged { get; set; }

    public DateTime Created { get; set; }

    public int PingsCount { get; set; }
  }
}
