// Decompiled with JetBrains decompiler
// Type: SMClient.Steam.Matchmaking
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Steamworks;

namespace SMClient.Steam
{
  public class Matchmaking
  {
    protected static CallResult<LobbyMatchList_t> m_CallResultLobbyMatchList = new CallResult<LobbyMatchList_t>();
    protected static CallResult<LobbyCreated_t> m_CallResultLobbyCreated = new CallResult<LobbyCreated_t>();
    protected static CallResult<LobbyEnter_t> m_CallResultLobbyEntered = new CallResult<LobbyEnter_t>();
    protected static Callback<LobbyDataUpdate_t> callbackLobbyDataUpdate;

    public static void GetLobbiesList(
      CallResult<LobbyMatchList_t>.APIDispatchDelegate callback)
    {
      SteamMatchmaking.AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide);
      SteamMatchmaking.AddRequestLobbyListFilterSlotsAvailable(1);
      SteamMatchmaking.AddRequestLobbyListResultCountFilter(1000);
      SteamAPICall_t hAPICall = SteamMatchmaking.RequestLobbyList();
      Matchmaking.m_CallResultLobbyMatchList.Set(hAPICall, callback);
    }

    public static void CreateLobby(
      CallResult<LobbyCreated_t>.APIDispatchDelegate created,
      CallResult<LobbyEnter_t>.APIDispatchDelegate entered)
    {
      SteamAPICall_t lobby = SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 100);
      Matchmaking.m_CallResultLobbyCreated.Set(lobby, created);
      Matchmaking.m_CallResultLobbyEntered.Set(lobby, entered);
    }

    public static void RequestLobbyData(
      CSteamID id,
      Callback<LobbyDataUpdate_t>.DispatchDelegate callback)
    {
      Matchmaking.callbackLobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(callback);
      SteamMatchmaking.RequestLobbyData(id);
    }

    public static void EnterLobby(
      CallResult<LobbyEnter_t>.APIDispatchDelegate callback)
    {
    }

    public static SteamLobby GetLobby(CSteamID lobbyId)
    {
      SteamLobby lobby = new SteamLobby()
      {
        ID = lobbyId
      };
      int lobbyDataCount = SteamMatchmaking.GetLobbyDataCount(lobbyId);
      for (int iLobbyData = 0; iLobbyData < lobbyDataCount; ++iLobbyData)
      {
        string pchKey;
        string pchValue;
        SteamMatchmaking.GetLobbyDataByIndex(lobbyId, iLobbyData, out pchKey, (int) byte.MaxValue, out pchValue, 8192);
        lobby.Metadata.Add(pchKey, pchValue);
      }
      return lobby;
    }
  }
}
