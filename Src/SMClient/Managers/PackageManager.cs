using Newtonsoft.Json;
using SMClient.Api;
using SMClient.Models;
using SMClient.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMClient.Managers
{
    public static class PackageManager
    {
        public static List<Package> Packages { get; set; }

        public static async Task<bool> Initialize(ProgressChangedEventHandler onDownloadProgress = null,
          EventHandler onDownloadComplete = null)
        {
            var initialized = false;
            try
            {
                await LoadMetadata();
                await DownloadPackages(onDownloadProgress, onDownloadComplete);
                initialized = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(new Exception("Package download exception", ex));
            }

            return initialized;
        }

        public static async Task LoadMetadata()
        {
            Packages = new List<Package>();

            try
            {
                var fileInfo = await BaseApi.GetFileMetadata("/branchMetadata.json");
                var file = await BaseApi.DownloadFile(fileInfo);
                var metadata = Encoding.UTF8.GetString(file.File);
                Packages = string.IsNullOrEmpty(metadata) ? null : JsonConvert.DeserializeObject<List<Package>>(metadata);
                Packages.Add(new Package()
                {
                    Name = "Exterminatus-test",
                    UserfriendlyName = "Testing version",
                    IsVisible = true,
                    CountedAsBranch = true,
                    DependenciesRaw = "BaseArtSim",
                    Roles = new List<string>() { "tester" }
                });

                var user = AuthManager.Account;
                Packages = Packages.Where(x => x.Roles == null || (x.Roles?.Contains(user?.Role ?? "user") ?? false)).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError(new Exception("Unable to get packages metadata", ex));
                throw ex;
            }
        }

        public static async Task DownloadPackages(
          ProgressChangedEventHandler onDownloadProgress,
          EventHandler onDownloadComplete)
        {
            try
            {
                while (!Settings.Instance.GamePathFine)
                    Thread.Sleep(5000);

                foreach (Package package in Packages)
                {
                    bool flag = false;
                    while (!flag)
                    {
                        flag = await DataApi.DownloadPackage(package, onDownloadProgress, onDownloadComplete);
                        if (!flag)
                            Logger.LogWarning("Downloaded branch hash mismatch, redownloading...");
                    }
                }

                foreach (Package package1 in Packages)
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
            }
            catch (Exception ex)
            {
                throw new SMException("Unable to download packages", ex);
            }
        }

        public static void ReUnpackStaticArchives(ProgressChangedEventHandler onDownloadProgress = null,
          EventHandler onDownloadComplete = null)
        {
            IEnumerable<Package> packages = PackageManager.Packages.Where(x => x.IsStatic);
            PrepareManager.RemoveStaticData();
            foreach (Package package in packages)
                ArchiveManager.UnpackStaticWithDeps(package, onDownloadProgress, onDownloadComplete);
        }
    }
}
