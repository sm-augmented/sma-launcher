// Decompiled with JetBrains decompiler
// Type: SMClient.Data.Managers.IntegrationManagers.SteamManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Steamworks;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMClient.Data.Managers.IntegrationManagers
{
  public class SteamManager
  {
    protected static SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

    public static bool Initialized { get; protected set; }

    protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText) => Logger.LogWarning(pchDebugText.ToString());

    public static event EventHandler<bool> SteamInitialized;

    public static void Initialize()
    {
      try
      {
        if (!Packsize.Test())
          throw new Exception("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");
        if (!DllCheck.Test())
          throw new Exception("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
        SteamManager.Initialized = SteamAPI.Init();
        if (!SteamManager.Initialized)
        {
          EventHandler<bool> steamInitialized = SteamManager.SteamInitialized;
          if (steamInitialized != null)
            steamInitialized((object) null, false);
          throw new Exception("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.");
        }
        if (SteamManager.m_SteamAPIWarningMessageHook == null)
        {
          SteamManager.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
          SteamClient.SetWarningMessageHook(SteamManager.m_SteamAPIWarningMessageHook);
        }
        EventHandler<bool> steamInitialized1 = SteamManager.SteamInitialized;
        if (steamInitialized1 != null)
          steamInitialized1((object) null, true);
        Task.Run((Func<Task>) (() =>
        {
          while (true)
          {
            SteamAPI.RunCallbacks();
            Thread.Sleep(TimeSpan.FromSeconds(1.0));
          }
        }));
      }
      catch (Exception ex)
      {
        Logger.LogWarning("Steam initialization exception: " + ex.Message);
      }
    }

    public static void Shutdown()
    {
      if (!SteamManager.Initialized)
        return;
      SteamAPI.Shutdown();
    }
  }
}
