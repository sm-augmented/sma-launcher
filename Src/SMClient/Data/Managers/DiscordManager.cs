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
                Client.GetActivityManager().UpdateActivity(activity, r => { });
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
                Client.GetActivityManager().UpdateActivity(activity, r => { });
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
                Client = new Discord.Discord(973279648467267615L, 1UL);
                Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            Client.RunCallbacks();
                        }
                        catch (Exception ex)
                        {

                        }

                        Thread.Sleep(16);
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex.Message);
            }
        }
    }
}
