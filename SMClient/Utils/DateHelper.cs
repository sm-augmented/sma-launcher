// Decompiled with JetBrains decompiler
// Type: SMClient.Utils.DateHelper
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;

namespace SMClient.Utils
{
  public class DateHelper
  {
    public static double ConvertToUnixTimestamp(DateTime date)
    {
      DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
      return Math.Floor((date.ToUniversalTime() - dateTime).TotalSeconds);
    }
  }
}
