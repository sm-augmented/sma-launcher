using Newtonsoft.Json;
using SMClient.Managers;
using SMClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMClient.Api
{
  public class PersistenceApi
  {
    public static async Task<bool> UnregisterPlayerIngame() => (await BaseApi.PostAsync("persistence/unregisterUserIngame?uid=" + AuthManager.GetUsername(true))).IsSuccessStatusCode;

    public static async Task<bool> RegisterPlayerIngame(string branch) => (await BaseApi.PostAsync("persistence/registerUserIngame?branch=" + branch + "&uid=" + AuthManager.GetUsername(true))).IsSuccessStatusCode;

    public static async Task<Dictionary<string, UserPing>> GetUsersDetails() => JsonConvert.DeserializeObject<Dictionary<string, UserPing>>(await BaseApi.GetStringAsync("persistence/getUsersDetails"));
  }
}
