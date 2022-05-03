// Decompiled with JetBrains decompiler
// Type: SMClient.Models.Exceptions.UnableToRestoreException
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;

namespace SMClient.Models.Exceptions
{
  public class UnableToRestoreException : SMException
  {
    public UnableToRestoreException()
    {
    }

    public UnableToRestoreException(string message)
      : base(message)
    {
    }

    public UnableToRestoreException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
