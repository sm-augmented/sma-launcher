using Discord;
using SMClient.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SMClient.Managers
{
    public class DiscordManager
    {
        public static bool Initialized { get; protected set; }

        private static Discord.Discord Client;

        public static void SetInLauncher()
        {
            //if (Initialized)
            //{
            //    try
            //    {
            //        Activity activity = new Activity()
            //        {
            //            State = "In Launcher",
            //            Timestamps = {
            //            Start = Convert.ToInt64(DateHelper.ConvertToUnixTimestamp(DateTime.Now))
            //        },
            //            Assets = new ActivityAssets()
            //        };
            //        activity.Assets.SmallImage = "sma_icon";
            //        activity.Assets.LargeImage = "sma_icon";
            //        activity.Assets.LargeText = "In Launcher";
            //        Client.GetActivityManager().UpdateActivity(activity, r => { });
            //    }
            //    catch (Exception ex)
            //    {
            //        Logger.LogError(new Exception("DiscordManager::SetInLauncher: Discord exception", ex));
            //    }
            //}
            //else
            //{
            //    Logger.LogWarning("DiscordManager::SetInLauncher: Discord not running");
            //}
        }

        public static void SetIngame(string branch)
        {
            //if (Initialized)
            //{
            //    try
            //    {
            //        Activity activity = new Activity()
            //        {
            //            State = "In " + branch,
            //            Timestamps = {
            //            Start = Convert.ToInt64(DateHelper.ConvertToUnixTimestamp(DateTime.Now))
            //        },
            //            Assets = new ActivityAssets()
            //        };
            //        activity.Assets.SmallImage = "sma_icon";
            //        activity.Assets.LargeImage = "sma_icon";
            //        activity.Assets.LargeText = "In " + branch;
            //        Client.GetActivityManager().UpdateActivity(activity, r => { });
            //    }
            //    catch (Exception ex)
            //    {
            //        Logger.LogError(new Exception("DiscordManager::SetIngame: Discord exception", ex));
            //    }
            //}
            //else
            //{
            //    Logger.LogWarning("DiscordManager::SetIngame: Discord not running");
            //}
        }

        public static bool Initialize()
        {
      return false;
            //try
            //{
            //    Client = new Discord.Discord(973279648467267615L, 1UL);

            //    Task.Run(() =>
            //    {
            //        while (true)
            //        {
            //            try
            //            {
            //                Client.RunCallbacks();
            //            }
            //            catch (Exception ex)
            //            {

            //            }

            //            Thread.Sleep(TimeSpan.FromSeconds(1.0));
            //        }
            //    });

            //    Initialized = true;
            //}
            //catch (Exception ex)
            //{
            //    Logger.LogError(new Exception("Discord init failed", ex));
            //}

            //return Initialized;
        }
    }
}
