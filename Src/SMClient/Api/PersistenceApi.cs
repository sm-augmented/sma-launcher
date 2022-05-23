// Decompiled with JetBrains decompiler
// Type: SMClient.Api.PersistenceApi
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Newtonsoft.Json;
using SMClient.Data.Managers;
using SMClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMClient.Api
{
  public class PersistenceApi
  {
    public static async Task<bool> CheckServerStatus() => (await BaseApi.GetAsync("api/persistence/checkStatus")).IsSuccessStatusCode;

    public static async Task<bool> UnregisterPlayerIngame() => (await BaseApi.PostAsync("persistence/unregisterUserIngame?uid=" + OnlineManager.GetUsername(true))).IsSuccessStatusCode;

    public static async Task<bool> RegisterPlayerIngame(string branch) => (await BaseApi.PostAsync("persistence/registerUserIngame?branch=" + branch + "&uid=" + OnlineManager.GetUsername(true))).IsSuccessStatusCode;

    public static async Task<Dictionary<string, UserPing>> GetUsersDetails() => JsonConvert.DeserializeObject<Dictionary<string, UserPing>>(await BaseApi.GetStringAsync("persistence/getUsersDetails"));
  }
}
