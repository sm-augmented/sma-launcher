// Decompiled with JetBrains decompiler
// Type: SMClient.Api.ProfileApi
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Newtonsoft.Json;
using SMA.Core.Models.Ingame.Proxy;
using System.Net.Http;
using System.Threading.Tasks;

namespace SMClient.Api
{
  public class ProfileApi
  {
    public static async Task<ProfileProxy> GetUserProfile(
      string branch,
      bool noWargear = false)
    {
      return JsonConvert.DeserializeObject<ProfileProxy>(await BaseApi.GetStringAsync(string.Format("api/profile/myProfile?branch={0}&noWargear={1}", (object) branch, (object) noWargear)));
    }

    public static async Task ResetLoadouts()
    {
      HttpResponseMessage async = await BaseApi.GetAsync("api/profile/resetLoadouts");
    }
  }
}
