// Decompiled with JetBrains decompiler
// Type: SMClient.Data.Managers.DataManagers.UnpackManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Models.Exceptions;
using SMClient.Models;
using SMClient.Models.Exceptions;
using SMClient.Utils;
using System;
using System.Collections.Generic;

namespace SMClient.Data.Managers.DataManagers
{
    public class UnpackManager
    {
        private static void AddPackageToQueue(Queue<Package> queue, Package package)
        {
            foreach (Package dependency in package.Dependencies)
            {
                if (!queue.Contains(dependency))
                    UnpackManager.AddPackageToQueue(queue, dependency);
            }
            queue.Enqueue(package);
        }

        public static void UnpackData(
          Package branch,
          Package profileBranch,
          List<Package> packages,
          bool oldWay)
        {
            PrepareManager.PrepareData(oldWay);
            Logger.LogInfo("UnpackData: Unpacking archives");
            Queue<Package> packageQueue = new Queue<Package>();
            if ((profileBranch?.Name ?? "") != "Exterminatus-test")
                UnpackManager.AddPackageToQueue(packageQueue, branch);
            foreach (Package package in packages)
                UnpackManager.AddPackageToQueue(packageQueue, package);
            try
            {
                ArchiveManager.UnpackWithDeps(packageQueue);
                if (!oldWay)
                    return;
                PrepareManager.BackupData();
                FileHelper.CopyDirectory(Settings.Instance.GetLocalePathByBranch(profileBranch), Settings.Instance.LocalePath);
                FileHelper.CopyDirectory(Settings.Instance.ModfigPath, Settings.Instance.ConfigPath);
            }
            catch (Exception ex)
            {
                try
                {
                    UnpackManager.RestoreData();
                }
                catch (Exception ex1)
                {
                    Logger.LogError(ex1);
                }

                throw new UnableToUnpackException("Unable to unpack packages", ex);
            }
        }

        public static void RestoreData() => PrepareManager.RestoreData();
    }
}
