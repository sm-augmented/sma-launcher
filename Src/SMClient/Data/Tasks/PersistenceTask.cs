// Decompiled with JetBrains decompiler
// Type: SMClient.Data.Tasks.PersistenceTask
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Api;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SMClient.Data.Tasks
{
  public static class PersistenceTask
  {
    private static CancellationTokenSource tokenSource;
    private static bool firstTime = true;

    public static Task CheckAlive(PersistenceTask.AliveChecked func)
    {
      if (PersistenceTask.tokenSource != null)
        PersistenceTask.tokenSource.Cancel();
      PersistenceTask.tokenSource = new CancellationTokenSource();
      CancellationTokenSource ts = PersistenceTask.tokenSource;
      CancellationToken token = ts.Token;
      return Task.Run((Func<Task>) (async () =>
      {
        while (!ts.IsCancellationRequested)
        {
          try
          {
            bool isAlive = await PersistenceApi.CheckServerStatus();
            bool needUpdate = false;
            if (isAlive && PersistenceTask.firstTime)
            {
              PersistenceTask.firstTime = false;
              needUpdate = !await DataApi.UpdateClient();
            }
            func(isAlive, needUpdate);
          }
          catch (Exception ex)
          {
            Logger.LogError(ex);
            func(false, false);
          }
          Thread.Sleep(TimeSpan.FromMinutes(1.0));
        }
      }));
    }

    public delegate void AliveChecked(bool isAlive, bool needUpdate);
  }
}
