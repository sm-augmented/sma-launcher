// Decompiled with JetBrains decompiler
// Type: SMClient.Data.Managers.OnlineManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Api;
using SMClient.Data.Managers.IntegrationManagers;
using SMClient.Data.Tasks;
using SMClient.Models;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMClient.Data.Managers
{
  public static class OnlineManager
  {
    public static User Account { get; set; }

    public static Dictionary<string, UserPing> Players { get; set; }

    public static event OnlineCounterTask.OnlineChecked OnlineChecked;

    public static void Initialize() => OnlineCounterTask.CheckOnlineCount(new OnlineCounterTask.OnlineChecked(OnlineManager.Checked));

    private static void Checked(int total, Dictionary<string, UserPing> players)
    {
      OnlineManager.Players = players;
      OnlineCounterTask.OnlineChecked onlineChecked = OnlineManager.OnlineChecked;
      if (onlineChecked == null)
        return;
      onlineChecked(total, players);
    }

    public static string GetUsername(bool useSteam)
    {
      if (useSteam && SteamManager.Initialized)
        return SteamFriends.GetPersonaName();
      return OnlineManager.Account?.Login;
    }

    public static async Task<bool> AuthenticateByToken()
    {
      try
      {
        OnlineManager.Account = await AccountApi.CheckToken();
        return OnlineManager.Account != null;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public static async Task<bool> AuthenticateBySteam()
    {
      byte[] numArray = new byte[1024];
      uint pcbTicket;
      SteamUser.GetAuthSessionTicket(numArray, 1024, out pcbTicket);
      OnlineManager.Account = await AccountApi.LoginSteam(BitConverter.ToString(((IEnumerable<byte>) numArray).Take<byte>((int) pcbTicket).ToArray<byte>()).Replace("-", ""));
      return OnlineManager.Account != null;
    }

    public static async Task<bool> Authenticate(
      string username,
      string password,
      bool isHashed)
    {
      OnlineManager.Account = await AccountApi.Authenticate(username, password, isHashed);
      return OnlineManager.Account != null;
    }

    public static void LogOut() => OnlineManager.Account = (User) null;
  }
}
