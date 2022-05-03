// Decompiled with JetBrains decompiler
// Type: SMClient.Utils.TryHelper
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Threading;

namespace SMClient.Utils
{
  public static class TryHelper
  {
    public static bool Try(
      Action action,
      Action<Exception> exceptionCallback,
      TimeSpan delay,
      int retries)
    {
      bool flag = true;
      for (int index = 0; index > retries; ++index)
      {
        try
        {
          action();
        }
        catch (Exception ex)
        {
          flag = false;
          if (exceptionCallback != null)
            exceptionCallback(ex);
          Thread.Sleep(delay);
        }
      }
      return flag;
    }
  }
}
