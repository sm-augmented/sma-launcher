// Decompiled with JetBrains decompiler
// Type: SMClient.Data.Tasks.TokenRefreshTask
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Api;
using SMClient.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SMClient.Data.Tasks
{
  public static class TokenRefreshTask
  {
    private static CancellationTokenSource tokenSource;

    public static Task RefreshToken()
    {
      if (TokenRefreshTask.tokenSource != null)
        TokenRefreshTask.tokenSource.Cancel();
      TokenRefreshTask.tokenSource = new CancellationTokenSource();
      CancellationTokenSource ts = TokenRefreshTask.tokenSource;
      CancellationToken ct = ts.Token;
      return Task.Run((Func<Task>) (async () =>
      {
        while (!ts.IsCancellationRequested)
        {
          ct.WaitHandle.WaitOne(TimeSpan.FromMinutes(55.0));
          try
          {
            User user = await AccountApi.Authenticate(Settings.Instance.Login, Settings.Instance.Password, true);
          }
          catch (Exception ex)
          {
            Logger.LogError(ex);
          }
        }
      }));
    }

    public static void Stop()
    {
      if (TokenRefreshTask.tokenSource == null)
        return;
      TokenRefreshTask.tokenSource.Cancel();
    }
  }
}
