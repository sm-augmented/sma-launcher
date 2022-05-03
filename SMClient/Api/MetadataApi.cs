// Decompiled with JetBrains decompiler
// Type: SMClient.Api.MetadataApi
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Newtonsoft.Json;
using SMClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMClient.Api
{
  public class MetadataApi
  {
    public static async Task<string> GetBaseMetadata() => await BaseApi.GetStringAsync("changelog.txt");

    public static async Task<List<Package>> LoadMetadata()
    {
      string stringAsync = await BaseApi.GetStringAsync("api/data/branches");
      return string.IsNullOrEmpty(stringAsync) ? (List<Package>) null : JsonConvert.DeserializeObject<List<Package>>(stringAsync);
    }
  }
}
