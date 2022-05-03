// Decompiled with JetBrains decompiler
// Type: SMClient.Data.Managers.ProfileManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Newtonsoft.Json;
using SMA.Core.Models.Dictionaries;
using SMA.Core.Models.Ingame;
using SMClient.Api;
using SMClient.Models;
using SMClient.Models.Ingame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SMClient.Data.Managers
{
  public class ProfileManager
  {
    public static string DataVersion => Settings.Instance.DataVersion;

    public static List<Wargear> Wargears { get; set; }

    public static List<Class> Classes { get; set; }

    public static List<UserProfile> Profiles { get; set; }

    public static event EventHandler Initialized;

    public static async Task Initialize()
    {
      ProfileManager.Profiles = new List<UserProfile>();
      string version = await DataApi.GetDataVersion();
      if (ProfileManager.DataVersion != version || !File.Exists("wCache"))
      {
        ProfileManager.Wargears = await DataApi.GetWargearData();
        Settings.Instance.DataVersion = version;
        Settings.Instance.Save();
        StreamWriter text = File.CreateText("wCache");
        text.Write(JsonConvert.SerializeObject((object) ProfileManager.Wargears));
        text.Close();
      }
      else
      {
        StreamReader streamReader = File.OpenText("wCache");
        string end = streamReader.ReadToEnd();
        streamReader.Close();
        ProfileManager.Wargears = JsonConvert.DeserializeObject<List<Wargear>>(end);
      }
      ProfileManager.Classes = new List<Class>()
      {
        new Class()
        {
          ID = "tactical",
          Name = "Tactical",
          Value = (byte) 0
        },
        new Class()
        {
          ID = "devastator",
          Name = "Devastator/Havoc",
          Value = (byte) 1
        },
        new Class()
        {
          ID = "assault",
          Name = "Jump Pack Assault/Raptor",
          Value = (byte) 2
        },
        new Class()
        {
          ID = "ground_assault",
          Name = "Ground Assault",
          Value = (byte) 2
        }
      };
      await Task.WhenAll(PackageManager.Packages.Select<Package, Task>((Func<Package, Task>) (p =>
      {
        if (!p.CountedAsBranch && !(p.Name == "Versus") && !(p.Name == "Exterminatus") && !(p.Name == "Base"))
          return;
        ProfileManager.Profiles.Add((await ProfileApi.GetUserProfile(p.Name, p.Name == "Base")).Build(OnlineManager.Account.Login, p.Name));
      })));
      EventHandler initialized = ProfileManager.Initialized;
      if (initialized == null)
      {
        version = (string) null;
      }
      else
      {
        initialized((object) null, (EventArgs) null);
        version = (string) null;
      }
    }
  }
}
