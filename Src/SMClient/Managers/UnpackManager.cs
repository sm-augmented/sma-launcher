using Models.Exceptions;
using SMClient.Models;
using SMClient.Utils;
using System;
using System.Collections.Generic;

namespace SMClient.Managers
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
            Logger.LogInfo("UnpackData: Unpacking archives");

            PrepareManager.PrepareData();

            Queue<Package> packageQueue = new Queue<Package>();
            if ((profileBranch?.Name ?? "") != "Exterminatus-test")
                AddPackageToQueue(packageQueue, branch);
            foreach (Package package in packages)
                AddPackageToQueue(packageQueue, package);

            try
            {
                ArchiveManager.UnpackWithDeps(packageQueue);
            }
            catch (Exception ex)
            {
                PrepareManager.ClearStaticData();
                throw ex;
            }

            if (oldWay)
            {
                try
                {
                    PrepareManager.BackupData(profileBranch);
                }
                catch (Exception ex)
                {
                    PrepareManager.ClearStaticData();
                    throw ex;
                }
            }
        }
    }
}
