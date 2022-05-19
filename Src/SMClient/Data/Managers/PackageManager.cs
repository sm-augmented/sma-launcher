// Decompiled with JetBrains decompiler
// Type: SMClient.Data.Managers.PackageManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Api;
using SMClient.Data.Managers.DataManagers;
using SMClient.Models;
using SMClient.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SMClient.Data.Managers
{
    public static class PackageManager
    {
        public static bool IsPackagesDownloaded;

        public static List<Package> Packages { get; set; }

        public static event EventHandler PackagesDownloaded;

        public static async Task Initialize() => PackageManager.Packages = await MetadataApi.LoadMetadata();

        public static void ReUnpackStaticArchives(ProgressChangedEventHandler onDownloadProgress = null,
          EventHandler onDownloadComplete = null)
        {
            IEnumerable<Package> packages = PackageManager.Packages.Where(x => x.IsStatic);
            PrepareManager.RemoveStaticData();
            foreach (Package package in packages)
                ArchiveManager.UnpackStaticWithDeps(package, onDownloadProgress, onDownloadComplete);
        }

        public static async Task DownloadPackages(
          ProgressChangedEventHandler onDownloadProgress,
          EventHandler onDownloadComplete)
        {
            try
            {
                foreach (Package package in PackageManager.Packages)
                {
                    bool flag = false;
                    while (!flag)
                    {
                        flag = await DataApi.DownloadPackage(package, onDownloadProgress, onDownloadComplete);
                        if (!flag)
                            Logger.LogWarning("Downloaded branch hash mismatch, redownloading...");
                    }
                }
                foreach (Package package1 in PackageManager.Packages)
                {
                    Package package = package1;
                    if (package.DependenciesRaw != null)
                    {
                        package.Dependencies = new List<Package>();
                        string dependenciesRaw = package.DependenciesRaw;
                        char[] chArray = new char[1] { ',' };
                        foreach (string str in dependenciesRaw.Split(chArray))
                        {
                            string dep = str;
                            package.Dependencies.Add(PackageManager.Packages.FirstOrDefault<Package>((Func<Package, bool>)(x => x.Name == dep && x != package)));
                        }
                    }
                }
                List<Package> list = PackageManager.Packages.Where<Package>((Func<Package, bool>)(x => !x.IsLoaded)).ToList<Package>();
                if (list.Count > 0)
                    throw new SMException("Failed to load packages: " + string.Join(",", list.Select<Package, string>((Func<Package, string>)(x => x.Name))));
                if (PackageManager.Packages.Where<Package>((Func<Package, bool>)(x => x.IsStatic)).Any<Package>((Func<Package, bool>)(x => x.IsUpdated)) || !Directory.Exists(Settings.Instance.ModPreviewPath) || !Directory.Exists(Path.Combine(Settings.Instance.ModPreviewPath, "art")))
                    PackageManager.ReUnpackStaticArchives(onDownloadProgress, onDownloadComplete);
                PackageManager.IsPackagesDownloaded = true;
                EventHandler packagesDownloaded = PackageManager.PackagesDownloaded;
                if (packagesDownloaded == null)
                    return;
                packagesDownloaded((object)null, (EventArgs)null);
            }
            catch (Exception ex)
            {
                throw new SMException("Unable to download packages", ex);
            }
        }
    }
}
