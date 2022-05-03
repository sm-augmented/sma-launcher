// Decompiled with JetBrains decompiler
// Type: SMClient.Data.Tasks.RegisterIngameTask
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Api;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SMClient.Data.Tasks
{
  public class RegisterIngameTask
  {
    private static CancellationTokenSource tokenSource;
    private static bool firstTime = true;

    public static void Cancel()
    {
      if (RegisterIngameTask.tokenSource == null)
        return;
      RegisterIngameTask.tokenSource.Cancel();
    }

    public static Task RegisterPlayerIngame(string branch, Action registered = null)
    {
      if (RegisterIngameTask.tokenSource != null)
        RegisterIngameTask.tokenSource.Cancel();
      RegisterIngameTask.tokenSource = new CancellationTokenSource();
      CancellationTokenSource ts = RegisterIngameTask.tokenSource;
      CancellationToken ct = ts.Token;
      return Task.Run((Func<Task>) (async () =>
      {
        RegisterIngameTask.firstTime = true;
        while (!ts.IsCancellationRequested)
        {
          try
          {
            int num = await PersistenceApi.RegisterPlayerIngame(branch) ? 1 : 0;
          }
          catch (Exception ex)
          {
            Logger.LogError(ex);
          }
          if (RegisterIngameTask.firstTime)
          {
            Action action = registered;
            if (action != null)
              action();
            RegisterIngameTask.firstTime = false;
          }
          ct.WaitHandle.WaitOne(TimeSpan.FromMinutes(15.0));
        }
      }));
    }
  }
}
