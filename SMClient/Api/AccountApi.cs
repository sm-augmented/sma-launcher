// Decompiled with JetBrains decompiler
// Type: SMClient.Api.AccountApi
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SMClient.Models;
using Steamworks;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SMClient.Api
{
  public class AccountApi
  {
    private static string HashPassword(string password)
    {
      using (SHA512 shA512 = (SHA512) new SHA512Managed())
      {
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        byte[] hash = shA512.ComputeHash(bytes);
        StringBuilder stringBuilder = new StringBuilder(128);
        foreach (byte num in hash)
          stringBuilder.Append(num.ToString("X2"));
        return stringBuilder.ToString();
      }
    }

    public static async Task<bool> Register(string username, string password)
    {
      string hashPass = AccountApi.HashPassword(password);
      bool result;
      bool.TryParse(await BaseApi.PostAsyncForString("api/account/register", (HttpContent) new StringContent(JsonConvert.SerializeObject((object) new JObject()
      {
        {
          nameof (username),
          (JToken) username
        },
        {
          nameof (password),
          (JToken) hashPass
        }
      }))), out result);
      if (result)
      {
        Settings.Instance.Login = username;
        Settings.Instance.Password = hashPass;
        Settings.Instance.Save();
      }
      bool flag = result;
      hashPass = (string) null;
      return flag;
    }

    public static async Task<User> LoginSteam(string ticket)
    {
      JObject jobject = JsonConvert.DeserializeObject<JObject>(await BaseApi.GetStringAsync("api/account/loginSteam?ticket=" + ticket));
      Settings.Instance.AccessToken = jobject.Value<string>((object) "access_token");
      Settings.Instance.Login = (string) null;
      Settings.Instance.Password = (string) null;
      Settings.Instance.Save();
      try
      {
        BaseApi.SetToken();
      }
      catch
      {
      }
      return JsonConvert.DeserializeObject<User>(jobject["user"].ToString());
    }

    public static async Task<User> CheckToken() => JsonConvert.DeserializeObject<User>(await BaseApi.GetStringAsync(string.Format("api/account/checkToken?steamId={0}", (object) SteamUser.GetSteamID())));

    public static async Task<User> Authenticate(
      string username,
      string password,
      bool isHashed)
    {
      string str = !isHashed ? AccountApi.HashPassword(password) : password;
      JObject jobject1 = new JObject();
      jobject1.Add(nameof (username), (JToken) username);
      jobject1.Add(nameof (password), (JToken) str);
      try
      {
        JObject jobject2 = JsonConvert.DeserializeObject<JObject>(await BaseApi.PostAsyncForString("api/account/token", (HttpContent) new StringContent(JsonConvert.SerializeObject((object) jobject1))));
        Settings.Instance.AccessToken = jobject2.Value<string>((object) "access_token");
        Settings.Instance.Login = (string) null;
        Settings.Instance.Password = (string) null;
        Settings.Instance.Save();
        try
        {
          BaseApi.SetToken();
        }
        catch
        {
        }
        return JsonConvert.DeserializeObject<User>(jobject2["user"].ToString());
      }
      catch (Exception ex)
      {
        return (User) null;
      }
    }

    public static async Task SendLogAsync()
    {
      string str1 = File.ReadAllText("loglog.log");
      string str2 = File.ReadAllText("Settings.xml");
      string str3 = "";
      string str4 = "";
      if (File.Exists(Settings.Instance.SMALogPath))
        str3 = File.ReadAllText(Settings.Instance.SMALogPath);
      if (File.Exists(Settings.Instance.GameLogPath))
        str4 = File.ReadAllText(Settings.Instance.GameLogPath);
      HttpResponseMessage httpResponseMessage = await BaseApi.PostAsync("api/data/sendLog", (HttpContent) new StringContent(Settings.Instance.Login + "\n" + str2 + "\n" + str1 + "\n" + str3 + "\n" + str4));
    }
  }
}
