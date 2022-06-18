using SMClient.Managers;
using SMClient.Models;
using SMClient.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SMClient
{
    internal static class Unlocker
    {
        private static Process spaceMarine;

        public static bool IsSteamRunning => Process.GetProcessesByName("Steam").FirstOrDefault() != null;

        public static void StartGame(Package branch, Package profileBranch, List<Package> packages)
        {
            Logger.LogInfo("StartGame: Starting the game in " + branch.Name + " as " + profileBranch.Name + " with: " + string.Join(", ", packages.Select(x => x.Name)) + ".");

            if (!IsSteamRunning)
                throw new SteamNotRunningException();

            bool oldLaunchWay = Settings.Instance.OldLaunchWay;
            UnpackManager.UnpackData(branch, profileBranch, packages, oldLaunchWay);
            LaunchProcess(profileBranch, oldLaunchWay);
        }

        private static void LaunchProcess(Package profileBranch, bool oldWay)
        {
            try
            {
                var firstTime = !ProfileInfoManager.RecreateModProfile(profileBranch.Name);
                var ftParam = firstTime ? "-firstTime" : "";
                string str1 = Settings.Instance.StartWindowed ? "-window" : "";
                string str2 = oldWay ? "-oldWay" : "";
                string str3 = Settings.Instance.OldLaunchWay ? Settings.Instance.SpaceMarineEXEPath : Settings.Instance.SMAEXEPath;
                Unlocker.spaceMarine = Process.Start(new ProcessStartInfo(str3, "-usepreview " + ftParam + " " + str1 + " " + str2 + " -mod " + profileBranch.Name + " -utoken " + Settings.Instance.AccessToken)
                {
                    WorkingDirectory = Path.GetDirectoryName(str3)
                });
                Unlocker.spaceMarine.EnableRaisingEvents = true;
                Logger.LogInfo("LaunchProcess: Process started");
            }
            catch (Exception ex)
            {
                try
                {
                    Thread.Sleep(3000);
                    Unlocker.Close();
                }
                catch (Exception exInn)
                {
                    Logger.LogError(new Exception("Unable to restore", exInn));
                }
                
                throw new SMException("Unable to start game process", ex);
            }
        }

        public static void CloseNoWait()
        {
            Unlocker.spaceMarine?.Kill();
        }

        internal static void Close()
        {
            Unlocker.spaceMarine?.Kill();
            Process spaceMarine;
            do
            {
                spaceMarine = Unlocker.spaceMarine;
            }
            while ((spaceMarine != null ? (!spaceMarine.HasExited ? 1 : 0) : 0) != 0);
        }

        public static Task WaitForGame()
        {
            return Task.Run(() =>
            {
                if (spaceMarine != null && !spaceMarine.HasExited)
                {
                    spaceMarine.WaitForExit();
                }

                if (spaceMarine != null)
                {
                    spaceMarine.EnableRaisingEvents = false;
                    spaceMarine = null;
                }

                try
                {
                    PrepareManager.RestoreData();
                }
                catch (Exception ex)
                {
                    throw new UnableToRestoreException("Unable to restore backup data", ex);
                }
            });
        }
    }
}
