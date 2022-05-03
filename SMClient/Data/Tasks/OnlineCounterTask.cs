// Decompiled with JetBrains decompiler
// Type: SMClient.Data.Tasks.OnlineCounterTask
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Api;
using SMClient.Data.Managers;
using SMClient.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SMClient.Data.Tasks
{
  public static class OnlineCounterTask
  {
    private static CancellationTokenSource tokenSource;

    public static Task CheckOnlineCount(OnlineCounterTask.OnlineChecked func)
    {
      if (OnlineCounterTask.tokenSource != null)
        OnlineCounterTask.tokenSource.Cancel();
      OnlineCounterTask.tokenSource = new CancellationTokenSource();
      CancellationTokenSource ts = OnlineCounterTask.tokenSource;
      CancellationToken ct = ts.Token;
      return Task.Run((Func<Task>) (async () =>
      {
        while (!ts.IsCancellationRequested)
        {
          try
          {
            int total = await PersistenceApi.GetPlayerCount("");
            if (PackageManager.Packages != null)
            {
              foreach (Package package1 in PackageManager.Packages)
              {
                Package package = package1;
                package.Online = await PersistenceApi.GetPlayerCount(package1.Name);
                package = (Package) null;
              }
            }
            Dictionary<string, UserPing> usersDetails = await PersistenceApi.GetUsersDetails();
            func(total, usersDetails);
          }
          catch (Exception ex)
          {
            Logger.LogError(ex);
          }
          ct.WaitHandle.WaitOne(TimeSpan.FromMinutes(1.1));
        }
      }));
    }

    public delegate void OnlineChecked(int total, Dictionary<string, UserPing> players);
  }
}
