// Decompiled with JetBrains decompiler
// Type: Discord.ResultException
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;

namespace Discord
{
  public class ResultException : Exception
  {
    public readonly Result Result;

    public ResultException(Result result)
      : base(result.ToString())
    {
    }
  }
}
