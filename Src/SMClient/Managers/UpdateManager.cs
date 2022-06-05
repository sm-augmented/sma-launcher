using Dropbox.Api.Files;
using Dropbox.Api.Stone;
using SMClient;
using SMClient.Api;
using SMClient.Models.Dropbox;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Managers
{
    public class UpdateManager
    {
        private static async Task DownloadAutoupdater(IDownloadResponse<FileMetadata> fileMetadata)
        {
            DownloadedFile downloadedFile = await BaseApi.DownloadFile(fileMetadata);
            if (downloadedFile == null)
                return;
            System.IO.File.WriteAllBytes("SMClientUpdater.exe", downloadedFile.File);
        }

        public static async Task CheckAutoupdater()
        {
#if DEBUG
            return;
#else
            try
            {
                IDownloadResponse<FileMetadata> updaterMeta = await BaseApi.GetFileMetadata("/SMClientUpdater.exe");
                if (!HashApi.CheckUpdaterHash(updaterMeta.Response.ContentHash))
                    await DownloadAutoupdater(updaterMeta);
                updaterMeta.Dispose();
            }
            catch (Exception ex)
            {
                Logger.LogError(new Exception("Updater download exception", ex));
            }
#endif
        }

        public static async Task<bool> CheckUpdate()
        {
            var hasUpdate = false;
#if DEBUG
            
#else
            try
            {
                hasUpdate = await HashApi.CheckHashByFile("/SMClient.exe", Assembly.GetEntryAssembly().Location);
            }
            catch (Exception ex)
            {
                Logger.LogError(new Exception("Update check exception", ex));
            }
#endif
            return hasUpdate;
        }
    }
}
