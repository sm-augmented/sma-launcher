using Dropbox.Api.Files;
using Dropbox.Api.Stone;
using SMClient.Models;
using SMClient.Models.Dropbox;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace SMClient.Api
{
    public class DataApi
    {
        public static async Task<bool> DownloadPackage(
          Package branch,
          ProgressChangedEventHandler onDownloadProgress,
          EventHandler onDownloadComplete)
        {
            bool result = true;
            string str = "/";
            try
            {
                IDownloadResponse<FileMetadata> betaMeta = null;
                if (Settings.Instance.BetaChannel)
                {
                    try
                    {
                        betaMeta = await BaseApi.GetFileMetadata(str + branch.Name + "_Beta.sma");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                IDownloadResponse<FileMetadata> fileMeta = betaMeta != null ? betaMeta : await BaseApi.GetFileMetadata(str + Settings.Instance.GetArchiveNameByBranch(branch));
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
