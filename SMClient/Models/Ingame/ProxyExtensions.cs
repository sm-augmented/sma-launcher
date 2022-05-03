// Decompiled with JetBrains decompiler
// Type: SMClient.Models.Ingame.ProxyExtensions
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMA.Core.Models.Dictionaries;
using SMA.Core.Models.Ingame;
using SMA.Core.Models.Ingame.Proxy;
using SMClient.Data.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMClient.Models.Ingame
{
  public static class ProxyExtensions
  {
    public static Loadout Build(this LoadoutProxy proxy, string branch)
    {
      List<Wargear> wargears = ProfileManager.Wargears;
      Wargear wargear1 = wargears.FirstOrDefault<Wargear>((Func<Wargear, bool>) (x =>
      {
        if ((int) x.Value != (int) proxy.Melee)
          return false;
        return x.Branch == null || x.Branch.Contains(branch);
      }));
      Wargear wargear2 = wargears.FirstOrDefault<Wargear>((Func<Wargear, bool>) (x =>
      {
        if ((int) x.Value != (int) proxy.Pistol)
          return false;
        return x.Branch == null || x.Branch.Contains(branch);
      }));
      Wargear wargear3 = wargears.FirstOrDefault<Wargear>((Func<Wargear, bool>) (x =>
      {
        if ((int) x.Value != (int) proxy.Ranged)
          return false;
        return x.Branch == null || x.Branch.Contains(branch);
      }));
      Wargear wargear4 = wargears.FirstOrDefault<Wargear>((Func<Wargear, bool>) (x =>
      {
        if ((int) x.Value != (int) proxy.Ranged2)
          return false;
        return x.Branch == null || x.Branch.Contains(branch);
      }));
      Wargear wargear5 = wargears.FirstOrDefault<Wargear>((Func<Wargear, bool>) (x =>
      {
        if ((int) x.Value != (int) proxy.Equipment)
          return false;
        return x.Branch == null || x.Branch.Contains(branch);
      }));
      Wargear wargear6 = wargears.FirstOrDefault<Wargear>((Func<Wargear, bool>) (x =>
      {
        if ((int) x.Value != (int) proxy.CoreAbility)
          return false;
        return x.Branch == null || x.Branch.Contains(branch);
      }));
      Wargear wargear7 = wargears.FirstOrDefault<Wargear>((Func<Wargear, bool>) (x =>
      {
        if ((int) x.Value != (int) proxy.Evade)
          return false;
        return x.Branch == null || x.Branch.Contains(branch);
      }));
      Wargear wargear8 = wargears.FirstOrDefault<Wargear>((Func<Wargear, bool>) (x =>
      {
        if ((int) x.Value != (int) proxy.FreeSlot1)
          return false;
        return x.Branch == null || x.Branch.Contains(branch);
      }));
      Wargear wargear9 = wargears.FirstOrDefault<Wargear>((Func<Wargear, bool>) (x =>
      {
        if ((int) x.Value != (int) proxy.FreeSlot2)
          return false;
        return x.Branch == null || x.Branch.Contains(branch);
      }));
      Wargear wargear10 = wargears.FirstOrDefault<Wargear>((Func<Wargear, bool>) (x =>
      {
        if ((int) x.Value != (int) proxy.FreeSlot3)
          return false;
        return x.Branch == null || x.Branch.Contains(branch);
      }));
      Wargear wargear11 = wargears.FirstOrDefault<Wargear>((Func<Wargear, bool>) (x =>
      {
        if ((int) x.Value != (int) proxy.FreeSlot4)
          return false;
        return x.Branch == null || x.Branch.Contains(branch);
      }));
      Wargear wargear12 = wargears.FirstOrDefault<Wargear>((Func<Wargear, bool>) (x =>
      {
        if ((int) x.Value != (int) proxy.FreeSlot5)
          return false;
        return x.Branch == null || x.Branch.Contains(branch);
      }));
      Wargear wargear13 = wargears.FirstOrDefault<Wargear>((Func<Wargear, bool>) (x =>
      {
        if ((int) x.Value != (int) proxy.FreeSlot6)
          return false;
        return x.Branch == null || x.Branch.Contains(branch);
      }));
      Wargear wargear14 = wargears.FirstOrDefault<Wargear>((Func<Wargear, bool>) (x =>
      {
        if ((int) x.Value != (int) proxy.Perk1)
          return false;
        return x.Branch == null || x.Branch.Contains(branch);
      }));
      Wargear wargear15 = wargears.FirstOrDefault<Wargear>((Func<Wargear, bool>) (x =>
      {
        if ((int) x.Value != (int) proxy.Perk2)
          return false;
        return x.Branch == null || x.Branch.Contains(branch);
      }));
      Class @class = ProfileManager.Classes.FirstOrDefault<Class>((Func<Class, bool>) (x => (int) x.Value == (int) proxy.Class));
      return new Loadout()
      {
        Class = @class,
        Melee = wargear1,
        Pistol = wargear2,
        Ranged = wargear3,
        Ranged2 = wargear4,
        Equipment = wargear5,
        CoreAbility = wargear6,
        Evade = wargear7,
        FreeSlot1 = wargear8,
        FreeSlot2 = wargear9,
        FreeSlot3 = wargear10,
        FreeSlot4 = wargear11,
        FreeSlot5 = wargear12,
        FreeSlot6 = wargear13,
        Perk1 = wargear14,
        Perk2 = wargear15
      };
    }

    public static UserProfile Build(
      this ProfileProxy proxy,
      string username,
      string branch)
    {
      return new UserProfile()
      {
        ID = new UserProfile.ProfileID()
        {
          Branch = branch,
          Username = username
        },
        Experience = proxy.Experience,
        Loadouts = proxy.Loadouts.Select<LoadoutProxy, Loadout>((Func<LoadoutProxy, Loadout>) (x => x.Build(branch))).ToList<Loadout>()
      };
    }
  }
}
