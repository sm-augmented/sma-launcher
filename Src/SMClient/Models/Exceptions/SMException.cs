// Decompiled with JetBrains decompiler
// Type: SMClient.Models.Exceptions.SMException
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;

namespace SMClient.Models.Exceptions
{
  public class SMException : Exception
  {
    public SMException()
    {
    }

    public SMException(string message)
      : base(message)
    {
    }

    public SMException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
