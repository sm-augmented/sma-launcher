// Decompiled with JetBrains decompiler
// Type: SMClient.Unlocker
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Data.Managers;
using SMClient.Data.Managers.DataManagers;
using SMClient.Data.Tasks;
using SMClient.Models;
using SMClient.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace SMClient
{
    internal static class Unlocker
    {
        private static Process spaceMarine;
        private static FileSystemWatcher fsWatch;

        public static event EventHandler OnGameExit;

        public static event EventHandler OnGameStarted;

        public static event EventHandler OnException;

        public static bool IsSteamRunning => ((IEnumerable<Process>)Process.GetProcessesByName("Steam")).FirstOrDefault<Process>() != null;

        private static void ExceptionHandler(Exception ex)
        {
            Logger.LogError(ex);
            //RegisterIngameTask.RegisterPlayerIngame("Launcher");
            EventHandler onException = Unlocker.OnException;
            if (onException == null)
                return;
            onException((object)ex, (EventArgs)null);
        }

        public static void StartGame(Package branch, Package profileBranch, List<Package> packages)
        {
            Logger.LogInfo("StartGame: Starting the game in " + branch.Name + " as " + profileBranch.Name + " with: " + string.Join(", ", packages.Select<Package, string>((Func<Package, string>)(x => x.Name))) + ".");
            bool oldLaunchWay = Settings.Instance.OldLaunchWay;
            try
            {
                if (!Unlocker.IsSteamRunning)
                    throw new SteamNotRunningException();
                UnpackManager.UnpackData(branch, profileBranch, packages, oldLaunchWay);
                Unlocker.LaunchProcess(profileBranch, oldLaunchWay);
            }
            catch (Exception ex)
            {
                try
                {
                    PrepareManager.RestoreData();
                }
                catch (Exception ex1)
                {
                    Logger.LogError(ex1);
                }

                Unlocker.ExceptionHandler(ex);
            }
        }

        private static void LaunchProcess(Package profileBranch, bool oldWay)
        {
            try
            {
                string str1 = Settings.Instance.StartWindowed ? "-window" : "";
                string str2 = oldWay ? "-oldWay" : "";
                string str3 = Settings.Instance.OldLaunchWay ? Settings.Instance.SpaceMarineEXEPath : Settings.Instance.SMAEXEPath;
                Unlocker.spaceMarine = Process.Start(new ProcessStartInfo(str3, "-usepreview " + str1 + " " + str2 + " -mod " + profileBranch.Name + " -utoken " + Settings.Instance.AccessToken)
                {
                    WorkingDirectory = Path.GetDirectoryName(str3)
                });
                Unlocker.spaceMarine.EnableRaisingEvents = true;
                Unlocker.spaceMarine.Exited += new EventHandler(Unlocker.SpaceMarine_Exited);
                Logger.LogInfo("LaunchProcess: Process started");
                EventHandler onGameStarted = Unlocker.OnGameStarted;
                if (onGameStarted != null)
                    onGameStarted((object)null, (EventArgs)null);
                RegisterIngameTask.RegisterPlayerIngame(profileBranch.Name, (Action)(() => OnlineManager.Initialize()));
            }
            catch (Exception ex)
            {
                if (Unlocker.fsWatch != null)
                {
                    Unlocker.fsWatch.EnableRaisingEvents = false;
                    Unlocker.fsWatch.Dispose();
                    Unlocker.fsWatch = (FileSystemWatcher)null;
                }
                Thread.Sleep(3000);
                Unlocker.Close();
                UnpackManager.RestoreData();
                throw new SMException("Unable to start game process", ex);
            }
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

        private static void SpaceMarine_Exited(object sender, EventArgs e)
        {
            if (Unlocker.spaceMarine != null)
            {
                Unlocker.spaceMarine.EnableRaisingEvents = false;
                Unlocker.spaceMarine = (Process)null;
            }
            if (Unlocker.fsWatch != null)
            {
                Unlocker.fsWatch.EnableRaisingEvents = false;
                Unlocker.fsWatch.Dispose();
                Unlocker.fsWatch = (FileSystemWatcher)null;
            }

            try
            {
                UnpackManager.RestoreData();                
            }
            catch (Exception ex)
            {
                Unlocker.ExceptionHandler((Exception)new UnableToRestoreException("Unable to restore backup data", ex));
            }
            finally
            {
                EventHandler onGameExit = Unlocker.OnGameExit;
                if (onGameExit != null)
                {
                    onGameExit((object)null, (EventArgs)null);
                }
            }
        }

        private static void RunSMWatcher()
        {
            try
            {
                DateTime now = DateTime.Now;
                while (Process.GetProcessesByName("SpaceMarine").Length == 0)
                {
                    if (DateTime.Now.Subtract(now).Seconds > 30)
                        throw new SMException("Space Marine process link timeout");
                }
                Unlocker.spaceMarine = ((IEnumerable<Process>)Process.GetProcessesByName("SpaceMarine")).FirstOrDefault<Process>();
                Unlocker.spaceMarine.EnableRaisingEvents = true;
                Unlocker.spaceMarine.Exited += new EventHandler(Unlocker.SpaceMarine_Exited);
                Logger.LogInfo("LaunchProcess: Process linked");
                EventHandler onGameStarted = Unlocker.OnGameStarted;
                if (onGameStarted == null)
                    return;
                onGameStarted((object)null, (EventArgs)null);
            }
            catch (Exception ex)
            {
                if (Unlocker.fsWatch != null)
                {
                    Unlocker.fsWatch.EnableRaisingEvents = false;
                    Unlocker.fsWatch.Dispose();
                    Unlocker.fsWatch = (FileSystemWatcher)null;
                }
                Unlocker.Close();
                throw new SMException("Unable to link Space Marine process", ex);
            }
        }

        private static void WatchPreview()
        {
            if (Unlocker.fsWatch != null)
            {
                Unlocker.fsWatch.EnableRaisingEvents = true;
            }
            else
            {
                Unlocker.fsWatch = new FileSystemWatcher();
                Unlocker.fsWatch.Path = Settings.Instance.PreviewPath;
                Unlocker.fsWatch.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite;
                Unlocker.fsWatch.Filter = "*.*";
                Unlocker.fsWatch.Changed += new FileSystemEventHandler(Unlocker.PreviewChanged);
                Unlocker.fsWatch.Created += new FileSystemEventHandler(Unlocker.PreviewChanged);
                Unlocker.fsWatch.Deleted += new FileSystemEventHandler(Unlocker.PreviewChanged);
                Unlocker.fsWatch.Renamed += new RenamedEventHandler(Unlocker.FsWatch_Renamed);
                Unlocker.fsWatch.EnableRaisingEvents = true;
            }
        }

        private static void PreviewChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed && (e.Name == "art" || e.Name == "ui" || e.Name == "sim"))
                return;
            try
            {
                Unlocker.spaceMarine?.Kill();
            }
            catch
            {
            }
            if (Unlocker.fsWatch == null)
                return;
            Unlocker.fsWatch.EnableRaisingEvents = false;
            Unlocker.fsWatch.Dispose();
            Unlocker.fsWatch = (FileSystemWatcher)null;
        }

        private static void FsWatch_Renamed(object sender, RenamedEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed && (e.Name == "art" || e.Name == "ui" || e.Name == "sim"))
                return;
            try
            {
                Unlocker.spaceMarine?.Kill();
            }
            catch
            {
            }
            if (Unlocker.fsWatch == null)
                return;
            Unlocker.fsWatch.EnableRaisingEvents = false;
            Unlocker.fsWatch.Dispose();
            Unlocker.fsWatch = (FileSystemWatcher)null;
        }
    }
}
