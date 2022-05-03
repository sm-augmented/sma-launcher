// Decompiled with JetBrains decompiler
// Type: SMClient.Data.Managers.DiscordManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Discord;
using SMClient.Models.Exceptions;
using SMClient.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SMClient.Data.Managers
{
  public class DiscordManager
  {
    private static Discord.Discord Client;
    private static ActivityManager ActivityManager;

    public static void SetInLauncher()
    {
      try
      {
        Activity activity = new Activity()
        {
          State = "In Launcher",
          Timestamps = {
            Start = Convert.ToInt64(DateHelper.ConvertToUnixTimestamp(DateTime.Now))
          },
          Assets = new ActivityAssets()
        };
        activity.Assets.SmallImage = "sma_icon";
        activity.Assets.LargeImage = "sma_icon";
        activity.Assets.LargeText = "In Launcher";
        DiscordManager.ActivityManager.UpdateActivity(activity, (ActivityManager.UpdateActivityHandler) (r => { }));
      }
      catch (Exception ex)
      {
        Logger.LogWarning("Discord not running: " + ex.Message);
      }
    }

    public static void SetIngame(string branch)
    {
      try
      {
        Activity activity = new Activity()
        {
          State = "In " + branch,
          Timestamps = {
            Start = Convert.ToInt64(DateHelper.ConvertToUnixTimestamp(DateTime.Now))
          },
          Assets = new ActivityAssets()
        };
        activity.Assets.SmallImage = "sma_icon";
        activity.Assets.LargeImage = "sma_icon";
        activity.Assets.LargeText = "In " + branch;
        DiscordManager.ActivityManager.UpdateActivity(activity, (ActivityManager.UpdateActivityHandler) (r => { }));
      }
      catch (Exception ex)
      {
        Logger.LogWarning("Discord not running: " + ex.Message);
      }
    }

    public static void SetInVersus()
    {
      try
      {
        Activity activity = new Activity()
        {
          State = "In Versus",
          Timestamps = {
            Start = Convert.ToInt64(DateHelper.ConvertToUnixTimestamp(DateTime.Now))
          },
          Assets = new ActivityAssets()
        };
        activity.Assets.SmallImage = "sma_icon";
        activity.Assets.LargeImage = "sma_icon";
        activity.Assets.LargeText = "In Versus";
        DiscordManager.ActivityManager.UpdateActivity(activity, (ActivityManager.UpdateActivityHandler) (r => { }));
      }
      catch (Exception ex)
      {
        Logger.LogWarning("Discord not running: " + ex.Message);
      }
    }

    public static void SetInExterminatus()
    {
      try
      {
        Activity activity = new Activity()
        {
          State = "In Exterminatus",
          Timestamps = {
            Start = Convert.ToInt64(DateHelper.ConvertToUnixTimestamp(DateTime.Now))
          },
          Assets = new ActivityAssets()
        };
        activity.Assets.SmallImage = "sma_icon";
        activity.Assets.LargeImage = "sma_icon";
        activity.Assets.LargeText = "In Exterminatus";
        DiscordManager.ActivityManager.UpdateActivity(activity, (ActivityManager.UpdateActivityHandler) (r => { }));
      }
      catch (Exception ex)
      {
        Logger.LogWarning("Discord not running: " + ex.Message);
      }
    }

    public static void Initialize()
    {
      try
      {
        DiscordManager.Client = new Discord.Discord(593378535834648577L, 1UL);
        DiscordManager.Client.SetLogHook(Discord.LogLevel.Debug, (Discord.Discord.SetLogHookHandler) ((level, message) =>
        {
          switch (level)
          {
            case Discord.LogLevel.Error:
              Logger.LogError((Exception) new DiscordException(message));
              break;
            case Discord.LogLevel.Warn:
              Logger.LogWarning(message);
              break;
            case Discord.LogLevel.Info:
              Logger.LogInfo(message);
              break;
            case Discord.LogLevel.Debug:
              Logger.LogDebug(message);
              break;
          }
        }));
        DiscordManager.ActivityManager = DiscordManager.Client.GetActivityManager();
        DiscordManager.SetInLauncher();
        Task.Run((Func<Task>) (() =>
        {
          while (true)
          {
            DiscordManager.Client.RunCallbacks();
            Thread.Sleep(16);
          }
        }));
      }
      catch (Exception ex)
      {
        Logger.LogWarning(ex.Message);
      }
    }
  }
}
