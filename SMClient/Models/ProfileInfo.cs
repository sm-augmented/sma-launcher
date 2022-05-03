// Decompiled with JetBrains decompiler
// Type: SMClient.Models.ProfileInfo
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Data.Managers.IntegrationManagers;
using SMClient.Utils;
using Steamworks;
using System;
using System.IO;
using System.Windows;

namespace SMClient.Models
{
  public class ProfileInfo
  {
    public static void RecreateModProfile(bool force = false)
    {
      if (!SteamManager.Initialized)
        return;
      string pchBuffer;
      SteamUser.GetUserDataFolder(out pchBuffer, 10000);
      if (Path.GetFileName(pchBuffer) == "local")
        pchBuffer = Path.Combine(Path.GetDirectoryName(pchBuffer), "remote");
      if (SteamRemoteStorage.FileExists("profile_info.bin") || File.Exists(Path.Combine(pchBuffer, "profile_info.bin")))
      {
        int fileSize1 = SteamRemoteStorage.GetFileSize("profile_info.bin");
        byte[] pvData = new byte[fileSize1];
        SteamRemoteStorage.FileRead("profile_info.bin", pvData, fileSize1);
        if (force || !SteamRemoteStorage.FileExists("profile_Vers.bin") && !File.Exists(Path.Combine(pchBuffer, "profile_Vers.bin")))
          SteamRemoteStorage.FileWrite("profile_Vers.bin", pvData, fileSize1);
        if (force || !SteamRemoteStorage.FileExists("profile_Exte.bin") && !File.Exists(Path.Combine(pchBuffer, "profile_Exte.bin")))
          SteamRemoteStorage.FileWrite("profile_Exte.bin", pvData, fileSize1);
        if (!SteamRemoteStorage.FileExists("profile_Vers_backup.bin") && !File.Exists(Path.Combine(pchBuffer, "profile_Vers_backup.bin")))
        {
          int fileSize2 = SteamRemoteStorage.GetFileSize("profile_Vers.bin");
          SteamRemoteStorage.FileRead("profile_Vers.bin", pvData, fileSize2);
          SteamRemoteStorage.FileWrite("profile_Vers_backup.bin", pvData, fileSize2);
        }
        if (!SteamRemoteStorage.FileExists("profile_Exte_backup.bin") && !File.Exists(Path.Combine(pchBuffer, "profile_Exte_backup.bin")))
        {
          int fileSize3 = SteamRemoteStorage.GetFileSize("profile_Exte.bin");
          SteamRemoteStorage.FileRead("profile_Exte.bin", pvData, fileSize3);
          SteamRemoteStorage.FileWrite("profile_Exte_backup.bin", pvData, fileSize3);
        }
        if (force || !SteamRemoteStorage.FileExists("profile_Vert.bin") && !File.Exists(Path.Combine(pchBuffer, "profile_Vert.bin")))
        {
          int fileSize4 = SteamRemoteStorage.GetFileSize("profile_Vers.bin");
          SteamRemoteStorage.FileRead("profile_Vers.bin", pvData, fileSize4);
          SteamRemoteStorage.FileWrite("profile_Vert.bin", pvData, fileSize4);
        }
        if (!force && (SteamRemoteStorage.FileExists("profile_Extt.bin") || File.Exists(Path.Combine(pchBuffer, "profile_Extt.bin"))))
          return;
        int fileSize5 = SteamRemoteStorage.GetFileSize("profile_Exte.bin");
        SteamRemoteStorage.FileRead("profile_Exte.bin", pvData, fileSize5);
        SteamRemoteStorage.FileWrite("profile_Extt.bin", pvData, fileSize5);
      }
      else
      {
        int num;
        Application.Current.Dispatcher.Invoke((Action) (() => num = (int) MessageBoxHelper.ShowError("Run the game and play multiplayer at least once!")));
      }
    }
  }
}
