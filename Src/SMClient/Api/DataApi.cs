// Decompiled with JetBrains decompiler
// Type: SMClient.Api.DataApi
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Dropbox.Api.Files;
using Dropbox.Api.Stone;
using Newtonsoft.Json;
using SMClient.Data.Dropbox;
using SMClient.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SMClient.Api
{
    public class DataApi
    {
        private static async Task DownloadAutoupdater(IDownloadResponse<FileMetadata> fileMetadata)
        {
            DownloadedFile downloadedFile = await BaseApi.DownloadFile(fileMetadata);
            if (downloadedFile == null)
                return;
            System.IO.File.WriteAllBytes("SMClientUpdater.exe", downloadedFile.File);
        }

        private static async Task CheckAutoupdater()
        {
            try
            {
                IDownloadResponse<FileMetadata> updaterMeta = await BaseApi.GetFileMetadata("/SMClientUpdater.exe");
                if (!HashApi.CheckUpdaterHash(updaterMeta.Response.ContentHash))
                    await DataApi.DownloadAutoupdater(updaterMeta);
                updaterMeta.Dispose();
            }
            catch (Exception ex)
            {

            }            
        }

        public static async Task<bool> UpdateClient()
        {
#if DEBUG
            return true;
#else
            await DataApi.CheckAutoupdater();
            int num = await HashApi.CheckClientHash() ? 1 : 0;
            if (num == 0)
                Process.Start("SMClientUpdater.exe", "");
            return num != 0;
#endif

        }

        public static async Task<bool> DownloadPackage(
          Package branch,
          ProgressChangedEventHandler onDownloadProgress,
          EventHandler onDownloadComplete)
        {
            bool result = true;
            string str = "/";
            try
            {
                IDownloadResponse<FileMetadata> fileMeta = await BaseApi.GetFileMetadata(str + Settings.Instance.GetArchiveNameByBranch(branch));
                if (!HashApi.CheckDataHash(branch, fileMeta.Response.ContentHash))
                {
                    Logger.LogInfo("Data update - " + branch.Name + ":" + branch.Version);
                    DownloadedFile downloadedFile = await BaseApi.DownloadFile(fileMeta, onDownloadProgress, onDownloadComplete);
                    if (downloadedFile != null)
                    {
                        Directory.CreateDirectory(Settings.Instance.DataPath);
                        System.IO.File.WriteAllBytes(Settings.Instance.GetArchivePathByBranch(branch), downloadedFile.File);
                        branch.IsUpdated = true;
                    }
                    result = HashApi.CheckDataHash(branch, fileMeta.Response.ContentHash);
                }
                branch.IsLoaded = true;
                fileMeta.Dispose();
            }
            catch (Exception ex)
            {
                branch.IsLoaded = false;
                Logger.LogError(ex);
            }
            return result;
        }
    }
}
